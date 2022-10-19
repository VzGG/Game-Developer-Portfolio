using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardSearch : MonoBehaviour
{
    public EvilWizardController evilWizardController;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            evilWizardController.target = (collision.gameObject.GetComponent<PlayerMovement>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        evilWizardController.target = (null);
    }
}
