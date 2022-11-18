using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] Tile tile;

    [SerializeField] Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        //tilemap.InsertCells(Vector3Int.zero, 3, 3, 1);
        // tilemap.SetTile(Vector3Int.zero, tile);

        RandomGenLevel();
    }


    // Create random gen tile platform/stage
    private void RandomGenLevel()
    {
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

}
