using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Unit : MonoBehaviour
{
    
    public ISet<Tile> movementCandidates;
    public List<Tile> enemiesInRange = new List<Tile>();

    public Stats stats;
    public bool isPlayer = true;
    public int movementCapacity = 3;
    public int rangeAttack = 1;

    public Tile placedTile;
    public DamageIcon damageIcon;

    protected bool hasMoved = false;
    protected Color highlightColor;

    protected Animator cameraAnimator;
    protected Animator unitAnimator;
    protected GameMaster gameMaster;
    protected MapEngine mapEngine;

    public UnitMovement unitMovement;
    public Combat combat;
    public Weapon weapon;

    private void Start()
    {

        EngineDependencyInjector engineDependencyInjector = EngineDependencyInjector.getInstance();
        MovementEngine movementEngine = engineDependencyInjector.Resolve<MovementEngine>();
        gameMaster = engineDependencyInjector.Resolve<GameMaster>();

        this.unitAnimator = this.GetComponent<Animator>();
        this.cameraAnimator = Camera.main.GetComponent<Animator>();
        this.unitMovement = new UnitMovement(this , movementCapacity, gameMaster, movementEngine);
        this.combat = new Combat(this, movementEngine);
        this.mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
    }

    public bool CanMove()
    {
        return !hasMoved;
    }

    public void ShowMovementCadidates()
    {
        movementCandidates = unitMovement.GetMovementCandidates(placedTile);
        foreach (Tile item in movementCandidates)
        {
            item.Highlight(Color.gray);
        }
    }
    public IEnumerator ShowMovementCadidatesAsync()
    {
        movementCandidates = unitMovement.GetMovementCandidates(placedTile);
        foreach (Tile item in movementCandidates)
        {
            item.Highlight(Color.gray);
        }

        yield return null;
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

    public IEnumerator Attack(Unit toAttack)
    {
        gameMaster.isSystemBusy = true;
        if(toAttack == null || toAttack.Equals(this))
        {
            yield break;
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
            yield break;
        }

        yield return AttackAnimation();
        yield return combat.Attack(toAttack);
        gameMaster.isSystemBusy = false;
    }

    private IEnumerator AttackAnimation()
    {
        unitAnimator.SetTrigger("attack");

        float waitFrames = Time.deltaTime * 50;
        yield return new WaitForSeconds(waitFrames);

        while (unitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "attack")
        {
            yield return null;
        }
    }

    public IEnumerator takeDamage(int damage)
    {
        bool isAlive = !stats.LifePointsVariation(damage);
        if(!isAlive)
        {
            unitAnimator.SetTrigger("shine");
            Shine();
            this.highlightColor = Color.gray;
            float waitFrames = Time.deltaTime * 10;
            yield return new WaitForSeconds(waitFrames);
        } else
        {
            unitAnimator.SetTrigger("hurt");
            float waitFrames = Time.deltaTime * 20;
            yield return new WaitForSeconds(waitFrames);
        }

        DamageIcon ins = DamageIcon.Instantiate(damageIcon, transform.position, damage);
        Destroy(gameObject, ins.lifetime);

        yield return null;
    }

    public int DamageRealized(Stats rivalStats)
    {
        return Math.Max(0, this.stats.attack - rivalStats.deffense);
    }

    public IEnumerator Move(Tile to)
    {
        if(!movementCandidates.Contains(to))
        {
            yield break;
        }
        gameMaster.isSystemBusy = true;

        unitAnimator.SetBool("running", true);
        PathFinder pathFinder = new PathFinder();
        Stack<Tile> tilesToMove = pathFinder.findShortestPath(placedTile, to, movementCandidates);
        placedTile.unitPlaced = null;
        placedTile = to;
        placedTile.unitPlaced = this;
        hasMoved = true;

        yield return StartMovement(tilesToMove);

        unitAnimator.SetBool("running", false);
        gameMaster.isSystemBusy = false;

    }

    IEnumerator StartMovement(Stack<Tile> path)
    {
        gameMaster.isSystemBusy = true;
        yield return unitMovement.MoveUnit(path);
        RemoveMovementCandidates();
        yield return highlightEnemies();
        gameMaster.isSystemBusy = false;
    }

    private IEnumerator highlightEnemies()
    {
        foreach (Tile tileWithEnemy in combat.DetectEnemiesInRange(rangeAttack))
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

    public void ResetUnit()
    {
        hasMoved = false;
    }

    protected abstract void Shine();
}
