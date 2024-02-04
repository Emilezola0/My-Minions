using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConsumeManager : MonoBehaviour
{
    #region Singleton

    public static ConsumeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null && Instance != this)
            Destroy(this);
        DontDestroyOnLoad(Instance);

        Debug.Log(Instance);
    }

    #endregion

    public Tilemap ConsumeMap;
    public float  maxConsume;
    [SerializeField] Color minConsumeColor, maxConsumeColor, clearColor;

    Dictionary<Vector3Int, float> consumedTiles = new Dictionary<Vector3Int, float>();

    public void AddConsume(Vector2 worldPosition, float consumeAmount, int radius)
    {
        Vector3Int gridPosition = ConsumeMap.WorldToCell(worldPosition);

        for(int x = -radius; x<= radius; x++)
        {
            for(int y = -radius; y<= radius; y++)
            {
                float distanceFromCenter = Mathf.Abs(x) + Mathf.Abs(y);

                if (distanceFromCenter <= radius)
                {
                    Vector3Int nextTilePosition = new Vector3Int(gridPosition.x + x, gridPosition.y + y, 0);
                    ChangeConsume(nextTilePosition, consumeAmount);
                }
            }
        }


        ChangeConsume(gridPosition, consumeAmount);
        //Visualize the changes, can be suppr from this place, but it is easier to let it her
        VisualizeConsume();
    }

    void ChangeConsume(Vector3Int gridPosition, float changeBy)
    {
        if(!consumedTiles.ContainsKey(gridPosition))
            consumedTiles.Add(gridPosition, 0f);

        float newValue = consumedTiles[gridPosition] + changeBy;

        if(newValue <= 0f)
        {
            consumedTiles.Remove(gridPosition);

            ConsumeMap.SetTileFlags(gridPosition, TileFlags.None);
            ConsumeMap.SetColor(gridPosition, clearColor);
            ConsumeMap.SetTileFlags(gridPosition, TileFlags.LockColor);
        }
        else
            consumedTiles[gridPosition] = Mathf.Clamp(newValue, 0f, maxConsume);
    }

    void VisualizeConsume()
    {
        foreach(var entry  in consumedTiles)
        {
            float consumePercent = entry.Value / maxConsume;
            Color newTileColor = maxConsumeColor * consumePercent + minConsumeColor * (1 - consumePercent);

            ConsumeMap.SetTileFlags(entry.Key, TileFlags.None);
            ConsumeMap.SetColor(entry.Key, newTileColor);
            ConsumeMap.SetTileFlags(entry.Key, TileFlags.LockColor);
        }
    }
}
