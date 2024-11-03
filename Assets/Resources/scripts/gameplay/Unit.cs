using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    private bool hasMoved = false;
    public bool isPlayer = true;
    private int moveSpeed = 2;
    public int movementCapacity = 2;
    public Tile placedTile;

    public bool CanMove()
    {
        GameMaster master = FindObjectOfType<GameMaster>();
        return !master.IsEnemyTurn() == isPlayer;
    }

    public void Move(Tile to)
    {
        if(!MovementEngine.GetInstance().highlitedTiles.Contains(to))
        {
            return;
        }

        PathFinder pathFinder = new PathFinder();
        Stack<Tile> tilesToMove = pathFinder.findShortestPath(placedTile, to, MovementEngine.GetInstance().highlitedTiles);
        StartCoroutine(StartMovement(tilesToMove));
        placedTile.unitPlaced = null;
        placedTile = to;
        placedTile.unitPlaced = this;
    }

    IEnumerator StartMovement(Stack<Tile> path)
    {
        while(path.Count > 0)
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
}
