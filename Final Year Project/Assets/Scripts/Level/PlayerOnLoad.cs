using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On next level load, call this in the player controller to place the player in the starting first room of the generated labyrinth
/// </summary>
public class PlayerOnLoad : MonoBehaviour
{

    public void OnNextLevelLoadPlacePlayerInStartingRoom(PlayerController playerController)
    {
        AsteroidSpawner asteroidSpawner = FindObjectOfType<AsteroidSpawner>();

        // Place player in the first level
        Vector3 centerOfNewGeneratedFirstLabyrinthRoom = asteroidSpawner.GetOptimalHamiltonianPath()[0].transform.position;
        Vector3 randomPosAroundTheRoom = new Vector3(Random.Range(-40f, 40f), Random.Range(-40f, 40f));

        Vector3 playerStartingPosition = centerOfNewGeneratedFirstLabyrinthRoom + randomPosAroundTheRoom;

        // Set the transform of the player
        playerController.gameObject.transform.position = playerStartingPosition;
    }

}
