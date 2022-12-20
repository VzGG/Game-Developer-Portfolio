using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;

namespace Oswald.Enemy
{
    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] DialogueManager dialogueManager;
        [SerializeField] GameObject bossGameObject;
        [SerializeField] Vector3 bossSpawnLocation;

        private void OnTriggerEnter2D(Collider2D collision)
        {

            // this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // Turn off the box collider to stop it from running any collisions again

            int dialogueNumber = 18;
            // Play dialogue
            Instantiate(dialogueManager.InstantiateDialogue(dialogueNumber));

            // Then spawn the boss
            Instantiate(bossGameObject, bossSpawnLocation, Quaternion.identity);


            // Turn off this gameObject
            this.gameObject.SetActive(false);
        }
    }
}

