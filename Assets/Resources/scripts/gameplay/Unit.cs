using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    private bool hasMoved = false;
    public bool hasStartedMovementAction;
    public bool isPlayer = true;
    private int moveSpeed = 2;
    public int movementCapacity = 3;
    public int rangeAttack = 1;
    public Tile placedTile;
    private ISet<Tile> movementCandidates;
    private List<Unit> enemiesInRange = new List<Unit>();

    public bool CanMove()
    {
        GameMaster master = FindObjectOfType<GameMaster>();
        return !master.IsEnemyTurn() == isPlayer;
    }

    public void ShowMovementCadidates()
    {
        movementCandidates = MovementEngine.GetInstance().GetTilesOnRange(placedTile, movementCapacity);
        foreach (Tile item in movementCandidates)
        {
            item.Highlight(Color.red);
        }

        hasStartedMovementAction = true;
    }

    public void Move(Tile to)
    {
        if(!movementCandidates.Contains(to))
        {
            return;
        }

        PathFinder pathFinder = new PathFinder();
        Stack<Tile> tilesToMove = pathFinder.findShortestPath(placedTile, to, movementCandidates);
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

        foreach (Tile movementCandidate in movementCandidates)
        {
            movementCandidate.CleanHighLight();
        }

        movementCandidates.Clear();

        ISet<Tile> candidateTilesWithEnemies = MovementEngine.GetInstance().GetTilesOnRange(placedTile, rangeAttack);
        foreach (Tile candidateTileWithEnemy in candidateTilesWithEnemies)
        {
            if (candidateTileWithEnemy.unitPlaced == null)
            {
                continue;
            }
            candidateTileWithEnemy.Highlight(Color.red);
            yield return null;
        }
    }
}
