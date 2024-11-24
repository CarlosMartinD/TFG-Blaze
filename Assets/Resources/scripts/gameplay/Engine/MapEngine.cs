using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEngine : MonoBehaviour
{
    public GameObject mapParent;
    public int mapWidth;
    public int mapHeight;
    public Tile[,] mapMatrix;

    public GameMaster gameMaster;
    public List<Unit> allyUnits;
    public List<Unit> enemyUnits;

    void Start()
    {
        mapMatrix = new Tile[mapWidth, mapHeight];
        gameMaster = EngineDependencyInjector.getInstance().Resolve<GameMaster>();
        gameMaster.mapEngine = this;
        LoadMapIntoMatrix();
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

                    int x = (int)position.x;
                    int y = (int)position.y;

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

    private void assignUnit(Tile tile)
    {
        Unit unitPlaced = tile.unitPlaced;
        if (unitPlaced == null)
        {
            return;
        }

        List<Unit> unitListToAssign;
        if (unitPlaced is AllyUnit)
        {
            unitListToAssign = allyUnits;
        }
        else
        {
            unitListToAssign = enemyUnits;
        }

        unitListToAssign.Add(unitPlaced);
    }
}
