using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
    [Header("Map")]
    [Range(0, 100)] public int sizeX;
    [Range(0, 100)] public int sizeY;

    [Header("Ressources")]
    [Range(1, 25)]  public int   minRessources;
    [Range(1, 25)]  public int   maxRessources;
    int ressourceNbr = 0;
    [Range(0, 100)] public float ressourceSpawnChances;
    [Range(0, 100)] public float obstacleSpawnChances;

    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject playerSpawn;
    [SerializeField] GameObject ressource;
    [SerializeField] GameObject obstacle;

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap ressourcesTilemap;

    [SerializeField] Tile groundTile;
    [SerializeField] Tile goldTile;
    [SerializeField] Vector3Int tileLocation;

    private void Start()
    {
        GenerateMap(sizeX, sizeY);
        SpawnRessources(ressourceSpawnChances);
        SpawnObstacles(obstacleSpawnChances);
    }

    private void GenerateMap(int X, int Y)
    {
        for (int i = 0; i < X; i++) 
        {
            for(int j = 0; j < Y; j++)
            {
                tileLocation = new Vector3Int(i, j, 0);
                groundTilemap.SetTile(tileLocation, groundTile); //Set the tile
            }
        }
    }

    #region Spawns

    void SpawnObstacles(float SpawnChances_)
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {

                if (DoesSpawn(SpawnChances_)) //Decides if the obstacle spawns or not
                {
                    tileLocation = groundTilemap.WorldToCell(new Vector2(i + 0.5f ,j + 0.5f));
                    
                    GameObject obstacle_ = GameObject.Instantiate(obstacle);
                    obstacle_.transform.position = groundTilemap.WorldToCell(tileLocation);

                    if (groundTilemap.GetTile(tileLocation) == goldTile)
                    {
                        Destroy(obstacle_);
                    }


                    //Need to be destroyed if collide with a ressource
                }
            }
        }
    }

    void SpawnRessources(float SpawnChances_)
    {

        for (int i = 0; i < maxRessources; i++)
        {
            if (DoesSpawn(SpawnChances_)) //Decides if the ressource spawns or not
            {
                ressourceNbr++;
                tileLocation = groundTilemap.WorldToCell(spawnPoints[i].transform.position);

                GameObject ressource_ = GameObject.Instantiate(ressource, spawnPoints[i].transform);
                ressource_.transform.position = tileLocation;

                groundTilemap.SetTile(tileLocation, goldTile);
            }
        }

        //if (ressourceNbr < minRessources)
        //{
        //    Debug.Log("ressourceNbr :" + ressourceNbr);
        //    SpawnRessources(SpawnChances_);
        //}
    }

    bool DoesSpawn(float spawnChances)
    {
        float result_ = Random.Range(0, 100);
        if (result_ < spawnChances)
            return true;
        else return false;
    }

    #endregion
}
