using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    protected bool hasMoved = false;
    protected Color highlightColor;
    protected ISet<Tile> movementCandidates;
    public List<Tile> enemiesInRange = new List<Tile>();

    public Stats stats;
    public bool isPlayer = true;
    private int moveSpeed = 2;
    public int movementCapacity = 3;
    public int rangeAttack = 1;
    public Tile placedTile;

    public bool CanMove()
    {
        return true;
    }

    public void ShowMovementCadidates()
    {
        movementCandidates = MovementEngine.GetInstance().GetTilesOnRange(placedTile, movementCapacity);
        foreach (Tile item in movementCandidates)
        {
            item.Highlight(highlightColor);
        }
    }
    public void Attack(Unit toAttack)
    {
        if(toAttack == null || toAttack.Equals(this))
        {
            return;
        }

        Stats enemyStatus = toAttack.stats;

        int hitChance = Math.Min(0, 100 - Math.Abs(enemyStatus.velocity - this.stats.velocity));
           
        System.Random random = new System.Random();

        if (random.Next(1, 100) >= hitChance)
        {
            return;
        }


        int lifePoints = this.stats.attack - enemyStatus.deffense;
        stats.LifePointsVariation(lifePoints);

        foreach(Tile enemyInRange in enemiesInRange)
        {
            enemyInRange.CleanHighLight();
        }

        enemiesInRange.Clear();
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
        foreach (Tile movementCandidate in movementCandidates)
        {
            movementCandidate.CleanHighLight();
        }

        movementCandidates.Clear();

        yield return detectEnemies();
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

    private List<Tile> detectEnemiesInRange()
    {
        ISet<Tile> candidateTilesWithEnemies = MovementEngine.GetInstance().GetTilesOnRange(placedTile, rangeAttack);
        List<Tile> unitsInRange = new List<Tile>();

        foreach (Tile candidateTileWithEnemy in candidateTilesWithEnemies)
        {
            if (candidateTileWithEnemy.unitPlaced == null)
            {
                continue;
            }

            candidateTileWithEnemy.Highlight(Color.red);
            unitsInRange.Add(candidateTileWithEnemy);
        }

        return unitsInRange;
    }
    private IEnumerator detectEnemies()
    {
        ISet<Tile> candidateTilesWithEnemies = MovementEngine.GetInstance().GetTilesOnRange(placedTile, rangeAttack);
        foreach (Tile candidateTileWithEnemy in candidateTilesWithEnemies)
        {
            if (candidateTileWithEnemy.unitPlaced == null)
            {
                continue;
            }
            candidateTileWithEnemy.Highlight(Color.red);
            enemiesInRange.Add(candidateTileWithEnemy);
            yield return null;
        }

        if(enemiesInRange.Count == 0)
        {
            GameMaster.getInstance().selectedUnit = null;
        }
    }

}
