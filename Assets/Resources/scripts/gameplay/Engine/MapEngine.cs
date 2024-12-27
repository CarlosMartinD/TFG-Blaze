using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MapEngine : MonoBehaviour
{
    [SerializeField]
    private GameObject mapParent;
    public int mapWidth;
    public int mapHeight;
    public Tile[,] mapMatrix;

    public List<Unit> allyUnits;
    public List<EnemyUnit> enemyUnits;
    private static float maxOffsetZ = -0.03694702f * 5;

    void Start()
    {
        mapMatrix = new Tile[mapWidth, mapHeight];
        LoadMapIntoMatrix();
    }

    private void LoadMapIntoMatrix()
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
        for(int i = 0; i < units.Length; i++)
        {
            assignUnit(ref units[i]);
            units[i].transform.position = ResolveZ(units[i].transform.position);
        }

        Obstacles[] obstacles = FindObjectsOfType<Obstacles>();
        for (int i = 0; i < units.Length; i++)
        {
            AssignObstacle(ref obstacles[i]);
            obstacles[i].transform.position = ResolveZ(obstacles[i].transform.position);
        }

    }

    private void assignUnit(ref Unit unit)
    {
        Vector3 unitPost = unit.transform.position;
        unitPost.z = -0.03694702f + (maxOffsetZ + 0.03694702f * unitPost.y);
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

    private void AssignObstacle(ref Obstacles obstacle)
    {
        Vector3 unitPost = obstacle.transform.position;
        Tile tile = mapMatrix[(int)Math.Truncate(unitPost.x), (int)Math.Truncate(unitPost.y)];
        tile.obstacles = obstacle;
    }

    private Vector3 ResolveZ(Vector3 position)
    {
        float z = position.z = -0.03694702f + (maxOffsetZ + 0.03694702f * position.y);
        return new Vector3(position.x, position.y, z);
    }
}
