using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;

namespace Oswald.Story
{
    public class DialogueSpot : MonoBehaviour
    {
        [SerializeField] int dialogueIndex;
        [SerializeField] DialogueManager dialogueManager;
        [SerializeField] Collider2D thisCollider2d;
        // Start is called before the first frame update
        private void Awake()
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
            thisCollider2d = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.thisCollider2d.IsTouching(collision.gameObject.GetComponent<CapsuleCollider2D>()))
            {
                // Create the dialogie
                Instantiate(dialogueManager.InstantiateDialogue(dialogueIndex));
                gameObject.SetActive(false);                                        // Prevents this script from occuring again
            }


        }

    }
}