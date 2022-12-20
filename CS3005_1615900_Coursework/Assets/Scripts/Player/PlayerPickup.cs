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
        [Space]
        [Header("My scroll of strengths")]
        // [SerializeField] int numberOfScrollOfStrengths = 0;
        [SerializeField] List<GameObject> myScrollOfStrengths = new List<GameObject>();
        [SerializeField] DialogueManager dialogueManager;

        [Space]
        [Header("Attack properties")]
        [SerializeField] PlayerAttack playerAttack;

        [Space]
        [Header("Health and Energy Properties")]
        [SerializeField] Health myHealth;
        [SerializeField] Energy myEnergy;

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
                else if (pickedUpItem.GetItemName() == "Scroll Of Strength")
                {
                    // Increase scroll of strength number and this number times by the Item's value is added to the player's total damage, also add a dialogue here
                    myScrollOfStrengths.Add(collision.gameObject);
                    // Debug.Log("PLAY SCROLL OF STRENGTH DIALOGUE");
                    int dialogueNumber = 4;                                     // Play the scroll of strength dialogue
                    Instantiate(dialogueManager.InstantiateDialogue(dialogueNumber));
                    // Update player attack damage
                    float itemAttackValue = collision.gameObject.GetComponent<Item>().GetItemValue();
                    playerAttack.SetMyDamage(playerAttack.GetMyDamage() + itemAttackValue);
                    playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() + itemAttackValue);

                    collision.gameObject.SetActive(false);                      // When they are inactive, the script is also inactive - maybe not inactive in for these - also think about changing in new scenes later 
                }
                else if (pickedUpItem.GetItemName() == "Healing Potion")
                {
                    myHealth.SetHealth(myHealth.GetHealth() + pickedUpItem.GetItemValue());
                    myHealth.SetMaxHealth(myHealth.GetMaxHealth() + pickedUpItem.GetItemValue());   // Also update the max health
                    collision.gameObject.SetActive(false);
                }
                else if (pickedUpItem.GetItemName() == "Energy Potion")
                {
                    myEnergy.SetEnergy(myEnergy.GetEnergy() + pickedUpItem.GetItemValue());
                    myEnergy.SetMaxEnergy(myEnergy.GetMaxEnergy() + pickedUpItem.GetItemValue());   // Also update the max energy
                    collision.gameObject.SetActive(false);
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

            }
        }

    }
}