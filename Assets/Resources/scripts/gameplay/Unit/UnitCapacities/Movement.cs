using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{

    private MovementEngine movementEngine;

    private int movementCapacity;

    private GameMaster gameMaster;

    private Unit unit;

    public UnitMovement(Unit unit, int movementCapacity, GameMaster gameMaster)
    {
        this.movementCapacity = movementCapacity;
        this.gameMaster = gameMaster;
        this.unit = unit;
    }

    public ISet<Tile> GetMovementCandidates(Tile placedTile)
    {
        return movementEngine.GetTilesOnRange(placedTile, movementCapacity, tile => tile.IsClear());
    }

    public IEnumerator MoveUnit(Stack<Tile> path)
    {
        while (path.Count > 0)
        {
            Tile nextTile = path.Pop();
            Vector3 targetPosition = nextTile.transform.position;
            targetPosition.z = -2;

            while (Vector2.Distance(unit.transform.position, targetPosition) > 0.01f)
            {
                unit.transform.position = Vector2.MoveTowards(unit.transform.position, targetPosition, Time.deltaTime * 2);
                yield return null;
            }
        }
    }
}
