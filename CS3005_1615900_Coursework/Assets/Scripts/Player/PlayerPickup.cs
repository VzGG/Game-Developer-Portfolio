using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;
using Oswald.Items;

namespace Oswald.Player
{
    public class PlayerPickup : MonoBehaviour
    {
        [Header("My Bow")]
        [SerializeField] GameObject myForgottenBow;

        [SerializeField] DialogueManager dialogueManager;

        [Space]
        [Header("Pickup sound effects")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip itemPickupSFX;

        [Space]
        [Header("Keys")]
        [SerializeField] private int numberOfKeys = 0;

        public int GetNumberOfKeys() { return numberOfKeys; }

        public void SetMyDialogueManager(DialogueManager dialogueManager) { this.dialogueManager = dialogueManager; }


        // The player's body (body and feet) collides with the item and the item can only touch these layers (player)
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Debug.Log("collided: " + collision.gameObject.name + " | " + collision.gameObject.layer + " | " + LayerMask.NameToLayer("Item"));
            if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                Item item = collision.gameObject.GetComponent<Item>();
                item.Effect(dialogueManager, GetComponent<PlayerController>(), audioSource, itemPickupSFX);
            }

            /*if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                Item pickedUpItem = collision.gameObject.GetComponent<Item>();
                if (pickedUpItem.GetItemName() == "Oswald's Bow")
                {
                    // Set the bow to our player
                    myForgottenBow = collision.gameObject;          // Now we have an item - skill
                                                                    // Debug.Log("PLAY BOW DIALOGUE");
                    int dialogueNumber = 5;
                    Instantiate(dialogueManager.InstantiateDialogue(dialogueNumber));         // Display the bow dialogue - instantiate it
                                                                                              // Unlock new player attack animation and set the bow damage to the value
                    playerAttack.SetHasBow(true);
                    playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() + collision.gameObject.GetComponent<Item>().GetItemValue());    // Pass the damage value to the player attack, then the player attack passes the value to the arrow projectile on instatiation

                    collision.gameObject.SetActive(false);          // Hide the physical item
                }
                else if (pickedUpItem.GetItemName() == "Quarter Key")
                {
                    int dialogueNumber = 0;

                    if (numberOfKeys == 0)
                        dialogueNumber = 9;
                    else if (numberOfKeys == 1)
                        dialogueNumber = 10;
                    else if (numberOfKeys == 2)
                        dialogueNumber = 11;
                    else if (numberOfKeys == 3)
                        dialogueNumber = 12;

                    numberOfKeys += 1;                                      // Increase key number 
                    Instantiate(dialogueManager.InstantiateDialogue(dialogueNumber));   // Play dialogue
                    collision.gameObject.SetActive(false);                  // Deactivate the key object
                }
                audioSource.PlayOneShot(itemPickupSFX);

            }*/

        }

    }
}