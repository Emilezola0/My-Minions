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
    [Range(1, 25)]  public int   numberOfRessources;
    [Range(0, 100)] public float ressourceSpawnChances;
    [Range(0, 100)] public float obstacleSpawnChances;

    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject playerSpawn;
    [SerializeField] GameObject ressource;
    [SerializeField] GameObject obstacle;

    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap obstacleTilemap;
    [SerializeField] Tilemap ressourcesTilemap;

    [SerializeField] Tile groundTile;
    [SerializeField] Tile obstacleTile;
    [SerializeField] Tile ressourceTile;
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

    void GetTileAtLocation(Vector3 position)
    {
        tileLocation = groundTilemap.WorldToCell(position); //Register the tile position on the tilemap
    }

    #region Spawns

    void SpawnObstacles(float SpawnChances_)
    {
        if (DoesSpawn(SpawnChances_)) //Decides if the obstacle spawns or not
        {
            GameObject obstacle_ = GameObject.Instantiate(obstacle/*, .transform*/);
        }
    }

    void SpawnRessources(float SpawnChances_)
    {

        for (int i = 0; i < numberOfRessources; i++)
        {
            if (DoesSpawn(SpawnChances_)) //Decides if the ressource spawns or not
            {
                tileLocation = groundTilemap.WorldToCell(spawnPoints[i].transform.position);

                GameObject ressource_ = GameObject.Instantiate(ressource, spawnPoints[i].transform);
                ressource_.transform.position = tileLocation;

                //Need to be generated only inside the groundTilemap
            }
        }
    }

    bool DoesSpawn(float spawnChances)
    {
        float result_ = Random.Range(0, 100);
        if (result_ < spawnChances)
            return true;
        else return false;
    }

    Vector2 GetPlayerSpawn()
    {
        return playerSpawn.transform.position;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector2(sizeX, sizeY));
    }
}
