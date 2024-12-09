using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private AllyUnit target;

    private EnemyUnit selected;

    private GameMaster gameMaster;

    private MapEngine mapEngine;

    public IAQueueExecution iaQueueExecution;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private State state;

    void Start()
    {
        gameMaster = EngineDependencyInjector.getInstance().Resolve<GameMaster>();
        mapEngine = EngineDependencyInjector.getInstance().Resolve<MapEngine>();
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                TakeTurn();
                break;

            case State.Busy:
                break;
        }
    }

    public void OnTurnEnemyStarted()
    {
        state = State.TakingTurn;
    }

    private void TakeTurn()
    {
        state = State.Busy;
        List<Unit> selectableUnits = mapEngine.enemyUnits;
        
        foreach (Unit selectedUnit in selectableUnits)
        {
            List<Tile> unitsAtRange = selectedUnit.combat.DetectEnemiesInRange(selectedUnit.movementCapacity + selectedUnit.rangeAttack);

            if (unitsAtRange.Count == 0) continue;

            Unit unitToAttack = GetUnitToAttackBasedOnDamage(selectedUnit, unitsAtRange);
            Tile tileToMove = GetTileToMove(selectedUnit, unitToAttack);

            iaQueueExecution.Enqueue(selectedUnit.ShowMovementCadidatesAsync());
            iaQueueExecution.Enqueue(selectedUnit.Move(tileToMove));
            iaQueueExecution.Enqueue(selectedUnit.Attack(unitToAttack));
        }
        state = State.WaitingForEnemyTurn;
    }

    private Unit GetUnitToAttackBasedOnDamage(Unit unit, List<Tile> unitsAtRange)
    {
        int maxDamagePercent = -1;
        Unit selectedUnit = null;

        foreach (Tile tileAtRange in unitsAtRange)
        {
            Unit possibleTarget = tileAtRange.unitPlaced;
            int damateToRealize = unit.DamageRealized(possibleTarget.stats);
            int percentDamage = (damateToRealize * 100 / possibleTarget.stats.life); 

            if(percentDamage > maxDamagePercent)
            {
                maxDamagePercent = percentDamage;
                selectedUnit = possibleTarget;
            }
        }

        return selectedUnit;
    }

    private Tile GetTileToMove(Unit selected, Unit toAttack)
    {

        ISet<Tile> tileToMove = selected.unitMovement.GetMovementCandidates(selected.placedTile);
        Tile placedTaleAttacked = toAttack.placedTile;
        Tile placedTaleAttSelected = toAttack.placedTile;

        int distanceSelect = int.MaxValue;

        Tile selectedTile = null;
        foreach (Tile tile in tileToMove)
        {
            if(!tile.IsClear())
            {
                continue;
            }
            int distanceAttacked = Math.Abs(placedTaleAttacked.x - tile.x) + Math.Abs(placedTaleAttSelected.y - tile.y);
            int distanceSelected = Math.Abs(placedTaleAttSelected.x - tile.x) + Math.Abs(placedTaleAttSelected.y - tile.y);

            if (distanceAttacked <= selected.rangeAttack && distanceSelected <= distanceSelect)
            {
                selectedTile = tile;
            }
        }

        return selectedTile;
    }
}
