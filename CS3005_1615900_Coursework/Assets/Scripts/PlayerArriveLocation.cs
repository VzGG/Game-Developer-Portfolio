using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oswald.Camera;
using Oswald.UI;
using Oswald.Manager;
using Oswald.Player;

public class PlayerArriveLocation : MonoBehaviour
{
    [SerializeField] Vector3 playerArriveLocationPoint;

    private void Awake()
    {
        // Find the player in the scene then place him in this position

        // character.transform.position = playerArriveLocationPoint;
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    int counter = 0;
    private void LateUpdate()
    {
        counter++;
        if (counter == 1)
        {
            // FindObjectOfType<PlayerMovement>().gameObject.transform.position = playerArriveLocationPoint;
            GameObject character = FindObjectOfType<PlayerMovement>().gameObject;
            character.transform.position = playerArriveLocationPoint;                           // Set the player's position
            TimeManager timeManager = FindObjectOfType<TimeManager>();                          // Get the time manager
            FindObjectOfType<PlayerController>().SetTimeManager(timeManager);                   // Set the time manager
            FindObjectOfType<PlayerCamera>().SetMyTarget(character.transform);                  // Set the camera

            PlayerUI playerUI = FindObjectOfType<PlayerUI>();
            playerUI.SetMyHealth(character.GetComponent<Health>());                             // Set the playerUI health
            playerUI.SetMyEnergy(character.GetComponent<Energy>());                             // Set the playerUI energy

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            FindObjectOfType<PlayerPickup>().SetMyDialogueManager(dialogueManager);             // Set the dialogue manager for the scroll of strength pickups

            Debug.Log("PLAYER moving to portal position and setting time manager to player movement and attack");
        }
        

    }
}
