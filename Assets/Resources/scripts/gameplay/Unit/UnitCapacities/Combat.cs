using System;
using System.Collections.Generic;

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
        damage = CalculateTriangleWeaponVariation(damage, target);
        target.takeDamage(damage);
    }

    private int CalculateTriangleWeaponVariation(int damage, Unit target)
    {
        const float advantageBonus = 1.2f;
        const float disadvantagePenalty = 0.8f;
        Weapon attackerWeapon = unit.weapon;
        Weapon targetWeapon = target.weapon;

        if ((attackerWeapon == Weapon.SWORD && targetWeapon == Weapon.SWORD) ||
            (attackerWeapon == Weapon.AXE && targetWeapon == Weapon.AXE) ||
            (attackerWeapon == Weapon.SPIKE && targetWeapon == Weapon.SPIKE))
        {
            return (int)(damage * advantageBonus);
        }
        else if ((attackerWeapon == Weapon.SWORD && targetWeapon == Weapon.SPIKE) ||
                 (attackerWeapon == Weapon.AXE && targetWeapon == Weapon.SWORD) ||
                 (attackerWeapon == Weapon.SPIKE && targetWeapon == Weapon.AXE))
        {
            return (int)(damage * disadvantagePenalty);
        }

        return damage;
    }

    private bool IsEnemy(Unit otherUnit)
    {
        return unit.isPlayer != otherUnit.isPlayer;
    }
}

