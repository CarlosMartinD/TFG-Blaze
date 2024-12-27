using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAModule : MonoBehaviour
{
    public abstract IEnumerator[] generateBehavior(Unit unit);

    protected ISet<Tile> DetectEnemiesInRange(ISet<Tile> tiles, Unit unit)
    {
        ISet<Tile> enemies = new HashSet<Tile>();   
        foreach(Tile tile in tiles)
        {
            List<Tile> enemiesPlaced =  unit.combat.DetectEnemiesInRangeFromTile(tile);
            foreach(Tile enemyPlace in enemiesPlaced)
            {
                enemies.Add(enemyPlace);
            }
        }

        return enemies;
    }
}
