using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;
using Oswald.Player;

namespace Oswald.Environment
{
    public class GateKey : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                int numberOfKeys = collision.gameObject.GetComponent<PlayerPickup>().GetNumberOfKeys();

                int dialogueNumber = 0;
                if (numberOfKeys == 0)
                    dialogueNumber = 13;
                else if (numberOfKeys == 1)
                    dialogueNumber = 14;
                else if (numberOfKeys == 2)
                    dialogueNumber = 15;
                else if (numberOfKeys == 3)
                    dialogueNumber = 16;
                else if (numberOfKeys == 4)
                {
                    dialogueNumber = 17;
                    // Then open the gate
                    gameObject.transform.parent.gameObject.SetActive(false);

                    //gameObject.SetActive(false);
                }


                Instantiate(dialogueManager.InstantiateDialogue(dialogueNumber));


            }
        }
    }
}


