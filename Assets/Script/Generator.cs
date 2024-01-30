using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [Range(1, 25)]   public int numberOfRessources;
    [Range(0, 100)] public float spawnChances;

    [SerializeField] GameObject[] spawnPoints;
    public GameObject ressource;
    public GameObject playerSpawn;

    private void Start()
    {
        for (int i = 0; i < numberOfRessources; i++)
        {
            //Decides if the ressource spawns or not
            if(DoesSpawn(spawnChances))
            {
                GameObject ressource_ = GameObject.Instantiate(ressource, spawnPoints[i].transform);

                //Obstruct the closest path to go to the ressource with obstacles
                Physics2D.Raycast(ressource_.transform.position, GetPlayerSpawn());
                Debug.DrawLine(ressource_.transform.position, GetPlayerSpawn(), Color.red, 60f);
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
}
