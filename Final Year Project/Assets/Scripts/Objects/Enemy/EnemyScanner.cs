using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleCol2d;
    [SerializeField] EnemyController enemyController;

    public void SetEnemyController(EnemyController enemyController) { this.enemyController = enemyController; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Update the enemy controller's target
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && 
            collision.gameObject.name == "SHIP")
        {

            Debug.Log("Player found!!!: " + collision.gameObject.name);
            //Debug.Log("Found player in range");
            // Add the enemy's target
            enemyController.SetPlayerController(collision.gameObject.GetComponent<PlayerController>());

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") &&
    collision.gameObject.name == "SHIP")
        {
            //Debug.Log("Found player have escaped the range");
            // Remove the enemy's target
            enemyController.SetPlayerController(null);

        }
    }
}
