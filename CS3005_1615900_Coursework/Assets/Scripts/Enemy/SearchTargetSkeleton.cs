using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetSkeleton : MonoBehaviour
{
    [SerializeField] SkeletonController skeletonController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            skeletonController.SetEnemyTarget(collision.gameObject.GetComponent<PlayerMovement>());
            // Debug.Log("Character detected!!!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        skeletonController.SetEnemyTarget(null);
    }
}
