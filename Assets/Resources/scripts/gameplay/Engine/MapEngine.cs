using System;
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
    public List<EnemyUnit> enemyUnits;

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
                    }
                }
                else
                {
                    Debug.LogWarning($"No Tile component found on {tileTransform.name}");
                }
            }
        }

        Unit[] units = FindObjectsOfType<Unit>();
        foreach(Unit unit in units)
        {
            assignUnit(unit);
        }

        Obstacles[] obstacles = FindObjectsOfType<Obstacles>();
        foreach (Obstacles obstacle in obstacles)
        {
            AssignObstacle(obstacle);
        }

    }

    private void assignUnit(Unit unit)
    {
        Vector3 unitPost = unit.transform.position;
        Tile tile = mapMatrix[(int)Math.Truncate(unitPost.x), (int)Math.Truncate(unitPost.y)];
        tile.unitPlaced = unit;
        unit.placedTile = tile;

        if (unit is AllyUnit)
        {
            allyUnits.Add((AllyUnit) unit);  

        }
        else
        {
            enemyUnits.Add((EnemyUnit) unit);
        }
    }

    private void AssignObstacle(Obstacles obstacle)
    {
        Vector3 unitPost = obstacle.transform.position;
        Tile tile = mapMatrix[(int)Math.Truncate(unitPost.x), (int)Math.Truncate(unitPost.y)];
        tile.obstacles = obstacle;
    }
}
