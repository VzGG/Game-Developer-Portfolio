using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetArea : MonoBehaviour
{
    [SerializeField] NormalEnemyController normalEnemyController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            normalEnemyController.SetTarget(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        normalEnemyController.SetTarget(null);
    }
}
