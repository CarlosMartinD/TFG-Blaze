using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    private bool hasMoved = false;
    public bool isPlayer = true;
    public int movementCapacity = 2;
    private int moveSpeed = 2;

    public bool CanMove()
    {
        GameMaster master = FindObjectOfType<GameMaster>();
        return !master.IsEnemyTurn() == isPlayer;
    }

    public void Move(Tile tile)
    {
        if(!MovementEngine.GetInstance().highlitedTiles.Contains(tile))
        {
            return;
        }
        StartCoroutine(StartMovement(tile));
    }

    IEnumerator StartMovement(Tile tilePos)
    {
        Vector3 position = tilePos.transform.TransformPoint(Vector3.zero);
        position.z = -2;
        while (transform.position.x != position.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(position.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(position.x, position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
