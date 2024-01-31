using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;
    public Tile tile2;
    public Vector3Int location;

    private void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            GetTileAtLocation(mp);
            tilemap.SetTile(location, tile); //Set the tile 
        }

        if (Input.GetMouseButtonDown(1))
        {
            GetTileAtLocation(mp);
        }

        if (Input.GetMouseButtonDown(2))
        {
            GetTileAtLocation(mp);
            
            //Check if the tile hovered is the same as tile2 we preset
            if(tile2 == tilemap.GetTile<Tile>(location))
            {
                Debug.Log("tile2");
            }
        }
    }

    void GetTileAtLocation(Vector3 position)
    {
        location = tilemap.WorldToCell(position); //Register the tile position on the tilemap
    }
}
