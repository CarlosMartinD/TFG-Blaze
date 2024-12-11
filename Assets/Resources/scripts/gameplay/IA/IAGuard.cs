using System;
using System.Collections;
using System.Collections.Generic;

public class IAGuard : IAModule
{
    public override IEnumerator[] generateBehavior(Unit selectedUnit)
    {
        List<Tile> unitsAtRange = selectedUnit.combat.DetectEnemiesInRange(selectedUnit.movementCapacity + selectedUnit.rangeAttack);
        if (unitsAtRange.Count == 0) return new IEnumerator[0];

        Unit unitToAttack = GetUnitToAttackBasedOnDamage(selectedUnit, unitsAtRange);
        Tile tileToMove = GetTileToMove(selectedUnit, unitToAttack);

        IEnumerator[] actions = new IEnumerator[4];
        actions[0] = selectedUnit.ShowMovementCadidatesAsync();
        actions[1] = selectedUnit.Move(tileToMove);
        actions[2] = selectedUnit.Attack(unitToAttack);

        return actions;
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

            if (percentDamage > maxDamagePercent)
            {
                maxDamagePercent = percentDamage;
                selectedUnit = possibleTarget;
            }
        }

        return selectedUnit;
    }

    private Tile GetTileToMove(Unit selected, Unit toAttack)
    {

        System.Collections.Generic.ISet<Tile> tileToMove = selected.unitMovement.GetMovementCandidates(selected.placedTile);
        Tile placedTaleAttacked = toAttack.placedTile;
        Tile placedTaleAttSelected = toAttack.placedTile;

        int distanceSelect = int.MaxValue;

        Tile selectedTile = null;
        foreach (Tile tile in tileToMove)
        {
            if (!tile.IsClear())
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
