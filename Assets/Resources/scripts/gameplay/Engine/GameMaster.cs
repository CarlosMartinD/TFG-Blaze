using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster gameMaster;
    public Unit selectedUnit;   
    public GameObject mapParent;
    public int mapWidth;
    public int mapHeight;
    public Tile[,] mapMatrix;

    public List<Unit> allyUnits;
    public List<Unit> enemyUnits;
    private bool enemyTurn = false;

    public GameMaster()
    {
        gameMaster = this;
    }

    public static GameMaster getInstance()
    {
        return gameMaster;
    }

    void Start()
    {
        gameMaster = this;
        mapMatrix = new Tile[mapWidth, mapHeight];
        LoadMapIntoMatrix();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

   private void EndTurn()
    {
        enemyTurn = !enemyTurn;

        if(selectedUnit != null)
        {
            selectedUnit = null;
        }
    }


    void LoadMapIntoMatrix()
    {
        if (mapParent == null)
        {
            Debug.LogError("Map Parent is not assigned in the GameMaster!");
            return;
        }

  
        foreach (Transform child in mapParent.transform)
        {
            foreach (Transform tileTransform in child)
            {
                Tile tileComponent = tileTransform.GetComponent<Tile>();

                if (tileComponent != null)
                {
                    Vector3 position = tileTransform.position;

                    int x = (int) position.x;
                    int y =(int) position.y;

                    if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                    {
                        mapMatrix[x, y] = tileComponent;
                        tileComponent.x = x;
                        tileComponent.y = y;
                        assignUnit(tileComponent);
                    }
                }
                else
                {
                    Debug.LogWarning($"No Tile component found on {tileTransform.name}");
                }
            }
        }
    }

    public bool IsEnemyTurn()
    {
        return enemyTurn;
    }

    private void assignUnit(Tile tile)
    {
        Unit unitPlaced = tile.unitPlaced;
        if (unitPlaced == null)
        {
            return;
        }

        (unitPlaced.isPlayer ? allyUnits : enemyUnits).Add(unitPlaced);
    }
}
