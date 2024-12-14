using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Unit : MonoBehaviour
{
    
    public ISet<Tile> movementCandidates = new HashSet<Tile>();
    public List<Tile> enemiesInRange = new List<Tile>();

    public Stats stats;
    public bool isPlayer = true;
    public int movementCapacity = 3;
    public int rangeAttack = 1;

    public Tile placedTile;
    public DamageIcon damageIcon;

    protected bool hasAttacked = false;
    protected bool hasMoved = false;
    protected Color highlightColor;

    protected Animator cameraAnimator;
    protected Animator unitAnimator;
    protected SystemOperatorEngine gameMaster;
    protected MapEngine mapEngine;
    protected MovementEngine movementEngine;

    public UnitMovement unitMovement;
    public Combat combat;
    public Weapon weapon;

    private void Start()
    {

        EngineDependencyInjector engineDependencyInjector = EngineDependencyInjector.getInstance();
        movementEngine = engineDependencyInjector.Resolve<MovementEngine>();
        gameMaster = engineDependencyInjector.Resolve<SystemOperatorEngine>();

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

        highlightEnemies();
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
        cleanMovementCandidates();
        cleanEnemiesInRangeTiles();
    }

    public ISet<Tile> MovementCandidates()
    {
        return movementEngine.GetTilesOnRange(placedTile, movementCapacity);
    }

    public IEnumerator Attack(Unit toAttack)
    {
        gameMaster.isSystemBusy = true;
        if(toAttack == null || toAttack.Equals(this))
        {
            yield break;
        }

        cleanEnemiesInRangeTiles();
        cleanMovementCandidates();

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
        yield return new WaitForSecondsRealtime(0.3f);
        gameMaster.isSystemBusy = false;
        hasAttacked = true;
    }

    private IEnumerator AttackAnimation()
    {
        unitAnimator.SetTrigger("attack");

        float waitFrames = Time.deltaTime * 50;
        while (unitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "attack")
        {
            yield return 0;
        }
    }

    public IEnumerator takeDamage(int damage)
    {
        bool isAlive = stats.LifePointsVariation(damage);
        float waitFrames;

        DamageIcon ins = DamageIcon.Instantiate(damageIcon, transform.position, damage);
        if (!isAlive)
        {
            Shine();
            this.highlightColor = Color.gray;
            yield return shineAnimationWithWait();
            ins.Destroy();
            Destroy(gameObject);
        }
        else
        {
            yield return hurtAnimationWithWait();
            ins.Destroy();
        }
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
        while(unitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "running")
        {
            yield return null;
        }
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
        highlightEnemiesWithTurnAssistance();
        gameMaster.isSystemBusy = false;
    }

    private void highlightEnemies()
    {
        if(hasAttacked)
        {
            return;
        }

        foreach (Tile tileWithEnemy in combat.DetectEnemiesInRange(rangeAttack))
        {
            tileWithEnemy.Highlight(Color.red);
            enemiesInRange.Add(tileWithEnemy);
        }
    }

    private void highlightEnemiesWithTurnAssistance()
    {
        highlightEnemies();

        if (enemiesInRange.Count == 0)
        {
            SystemOperatorEngine.getInstance().selectedUnit = null;
        }
    }

    public void ResetUnit()
    {
        hasMoved = false;
        hasAttacked = false;
        cleanEnemiesInRangeTiles();
        cleanMovementCandidates();
    }

    private void cleanEnemiesInRangeTiles()
    {
        foreach (Tile tileWithEnemy in enemiesInRange)
        {
            tileWithEnemy.CleanHighLight();
        }

        enemiesInRange = new List<Tile>();
    }

    private void cleanMovementCandidates()
    {
        foreach (Tile movementCandidate in movementCandidates)
        {
            movementCandidate.CleanHighLight();
        }

        movementCandidates = new HashSet<Tile>();
    }

    private IEnumerator hurtAnimationWithWait()
    {
        unitAnimator.SetTrigger("hurt");
        while (unitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hurt")
        {
            yield return 0;
        }
    }

    private IEnumerator shineAnimationWithWait()
    {
        unitAnimator.SetTrigger("shine");
        while (unitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "shine")
        {
            yield return 0;
        }
    }

    protected abstract void Shine();
}
