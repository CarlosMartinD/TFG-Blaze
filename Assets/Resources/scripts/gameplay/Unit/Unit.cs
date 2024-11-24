using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    
    protected bool hasMoved = false;
    protected Color highlightColor;
    public ISet<Tile> movementCandidates;

    public List<Tile> enemiesInRange = new List<Tile>();

    public Stats stats;
    public bool isPlayer = true;
    public int movementCapacity = 3;
    public int rangeAttack = 1;

    public Tile placedTile;
    public DamageIcon damageIcon;

    private int moveSpeed = 2;
    private Animator animator;

    private void Start()
    {
        animator = Camera.main.GetComponent<Animator>();
    }
    public bool CanMove()
    {
        return !hasMoved;
    }

    public void ShowMovementCadidates()
    {
        movementCandidates = MovementEngine.GetInstance().GetTilesOnRange(placedTile, movementCapacity, tile => tile.IsClear());
        foreach (Tile item in movementCandidates)
        {
            item.Highlight(Color.red);
        }
    }

    public List<Tile> detectEnemiesInRange(int range)
    {
        ISet<Tile> candidateTilesWithEnemies = MovementEngine.GetInstance().GetTilesOnRange(placedTile, range);
        List<Tile> unitsInRange = new List<Tile>();

        foreach (Tile candidateTileWithEnemy in candidateTilesWithEnemies)
        {
            if (candidateTileWithEnemy.unitPlaced == null || !unitIsEnemy(candidateTileWithEnemy.unitPlaced))
            {
                continue;
            }

            unitsInRange.Add(candidateTileWithEnemy);
        }

        return unitsInRange;
    }

    public void RemoveMovementCandidates()
    {
        foreach (Tile movementCandidate in movementCandidates)
        {
            movementCandidate.CleanHighLight();
        }

        movementCandidates.Clear();
    }

    public ISet<Tile> MovementCandidates()
    {
        return MovementEngine.GetInstance().GetTilesOnRange(placedTile, movementCapacity);
    }

    public void Attack(Unit toAttack)
    {
        if(toAttack == null || toAttack.Equals(this))
        {
            return;
        }

        foreach (Tile enemyInRange in enemiesInRange)
        {
            enemyInRange.CleanHighLight();
        }

        enemiesInRange.Clear();

        Stats enemyStatus = toAttack.stats;

        int hitChance = Math.Max(0, 100 - Math.Abs(enemyStatus.velocity - this.stats.velocity));
           
        System.Random random = new System.Random();

        if (random.Next(1, 100) >= hitChance)
        {
            return;
        }

        int lifePoints = this.stats.attack - enemyStatus.deffense;
        toAttack.takeDamage(lifePoints);
    }

    public void takeDamage(int damage)
    {
        if (!stats.LifePointsVariation(damage))
        {
            this.highlightColor = Color.gray;
            DamageIcon ins = DamageIcon.Instantiate(damageIcon, transform.position, damage);
            Destroy(gameObject, ins.lifetime);
        }
    }

    public int DamageRealized(Stats rivalStats)
    {
        return Math.Max(0, this.stats.attack - rivalStats.deffense);
    }

    public void Move(Tile to)
    {
        if(!movementCandidates.Contains(to))
        {
            return;
        }

        PathFinder pathFinder = new PathFinder();
        Stack<Tile> tilesToMove = pathFinder.findShortestPath(placedTile, to, movementCandidates);
        StartCoroutine(StartMovement(tilesToMove));

        placedTile.unitPlaced = null;
        placedTile = to;
        placedTile.unitPlaced = this;
        hasMoved = true;
    }

    IEnumerator StartMovement(Stack<Tile> path)
    {
        yield return move(path);
        RemoveMovementCandidates();
        yield return highlightEnemies();
    }

    private IEnumerator move(Stack<Tile> path)
    {
        while (path.Count > 0)
        {
            Tile nextTile = path.Pop();
            Vector3 targetPosition = nextTile.transform.position;
            targetPosition.z = -2;

            while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetPosition;


            yield return new WaitForEndOfFrame();
        }
    }


    private IEnumerator highlightEnemies()
    {
        foreach (Tile tileWithEnemy in detectEnemiesInRange(rangeAttack))
        {
            tileWithEnemy.Highlight(Color.red);
            enemiesInRange.Add(tileWithEnemy);
            yield return null;
        }

        if(enemiesInRange.Count == 0)
        {
            GameMaster.getInstance().selectedUnit = null;
        }
    }

    private bool unitIsEnemy(Unit unit)
    {
        return unit.isPlayer == isPlayer;
    }
}
