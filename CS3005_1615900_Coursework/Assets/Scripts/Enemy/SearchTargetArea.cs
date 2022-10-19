using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchTargetArea : MonoBehaviour
{
    [SerializeField] GobliController gobliController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gobliController.SetEnemyTarget(collision.gameObject.GetComponent<PlayerMovement>());
           // Debug.Log("Character detected!!!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gobliController.SetEnemyTarget(null);
    }
}
