using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oswald.Environment
{
    public class CatchArrowProjectile : MonoBehaviour
    {
        [SerializeField] Tilemap tileMap;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Player") && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Vector3 playerPos = collision.gameObject.transform.position;
                //Debug.Log("Floor is colliding with player" + " \nPlayer pos: " + playerPos + " | and is equivalent to tile pos: " + tileMap.WorldToCell(playerPos));

                //RemoveTile(playerPos);

            }
        }

        private void Start()
        {
            tileMap.SetTile(new Vector3Int(-9, 0, 0), null);
        }

        void RemoveTile(Vector3 playerPos)
        {
            Vector3Int playerPosToTilePos = tileMap.WorldToCell(playerPos);
            playerPosToTilePos = new Vector3Int(playerPosToTilePos.x, playerPosToTilePos.y - 2, playerPosToTilePos.z);

            // Remove tile at player touching feet
            tileMap.SetTile(playerPosToTilePos, null);
            tileMap.SetTile(playerPosToTilePos + Vector3Int.right, null);   // Right of original pos
            tileMap.SetTile(playerPosToTilePos + Vector3Int.left, null);    // Left of original pos
            tileMap.SetTile(playerPosToTilePos + new Vector3Int(1, 1, 0), null);
            tileMap.SetTile(playerPosToTilePos + new Vector3Int(-1, 1, 0), null);
        }
    }
}

