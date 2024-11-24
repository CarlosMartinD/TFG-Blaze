using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat
{
    private Unit unit;
    private MovementEngine movementEngine;


    public Combat(Unit unit, MovementEngine movementEngine)
    {
        this.unit = unit;
        this.movementEngine = movementEngine;
    }

    public List<Tile> DetectEnemiesInRange(int range)
    {
        ISet<Tile> tilesWithEnemies = movementEngine.GetTilesOnRange(unit.placedTile, range);
        List<Tile> enemiesInRange = new();

        foreach (Tile tile in tilesWithEnemies)
        {
            if (tile.unitPlaced != null && IsEnemy(tile.unitPlaced))
            {
                enemiesInRange.Add(tile);
            }
        }

        return enemiesInRange;
    }

    public void Attack(Unit target)
    {
        if (target == null || target.Equals(unit))
        {
            return;
        }

        int damage = Math.Max(0, unit.stats.attack - target.stats.deffense);
        target.takeDamage(damage);
    }

    private bool IsEnemy(Unit otherUnit)
    {
        return unit.isPlayer != otherUnit.isPlayer;
    }
}

