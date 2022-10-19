using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleCol2d;
    [SerializeField] PlayerController playerController;

    public void SetPlayerController(PlayerController playerController) { this.playerController = playerController; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Player scanned: " + collision.gameObject.name + " | other names: " + collision.name);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")
            && collision.gameObject.name.StartsWith("SHIP"))
        {
            //Debug.Log("Player scannnnnnned: " + collision.gameObject.name + " | other names: " + collision.name
            //    + "\n Its parent: " + collision.gameObject.GetComponentInParent<EnemyController>().gameObject.name
            //    + "\n its parent unique id: " + collision.gameObject.GetComponentInParent<EnemyController>().gameObject.GetInstanceID());

            // Get the target gameobject and its class controller
            EnemyController enemyTarget = collision.gameObject.GetComponent<EnemyController>();
            if (enemyTarget == null) { return; }

            // Get the gameobjects of that enemy, the unique ID
            int enemyID = collision.gameObject.GetComponent<EnemyController>().gameObject.GetInstanceID();

            // If its first time adding, go add it to list
            if (playerController.GetTargets().Count <= 0)
            {
                playerController.GetTargets().Add(enemyTarget);
                //return;
            }
            else
            {
                int similarityCounter = 0;
                
                // Check everything in the list, are there elements that have the same ID as this new found ID
                for (int i = 0; i < playerController.GetTargets().Count; i++)
                {
                    //if (playerController.GetTargets()[i] == null) 
                    //{
                    //    Debug.Log("There is a missing enemy in the target");
                    //    return; 
                    //}

                    // if id is equal to the one in the list (current index)
                    if (enemyID == playerController.GetTargets()[i].gameObject.GetInstanceID())
                    {
                        Debug.Log("ID is EQUAL - DO NOT ADD THIS!!!");
                        similarityCounter++;
                        // Do nothing
                    }

                }

                if (similarityCounter >= 1)
                {
                    // Do nothing
                }
                else if (similarityCounter <= 0)
                {
                    // Add to the list this target
                    playerController.GetTargets().Add(enemyTarget);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")
                   && collision.gameObject.name.StartsWith("SHIP"))
        {
            // Unique ID
            int enemyID = collision.gameObject.GetComponent<EnemyController>().gameObject.GetInstanceID();

            for (int i = 0; i < playerController.GetTargets().Count; i++)
            {
                // If we found the ID to be equal to the one in the list, remove the gameobject in the list with the given index
                if (enemyID == playerController.GetTargets()[i].gameObject.GetInstanceID())
                {
                    //Debug.Log("Enemy Target Removed FROM THE LIST!!!");
                    //Remove from the list
                    playerController.GetTargets().RemoveAt(i);
                    // Disable the CIRCLE visualisation of the lock on target
                    playerController.DisableLockOnWhenEnemyLeavesScannerRange(collision.gameObject);
                    break;
                }
            }
        }
    }



}
