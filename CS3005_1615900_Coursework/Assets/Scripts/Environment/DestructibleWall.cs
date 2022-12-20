using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;
using Oswald.Story;

/*
 * Concept of IEnumerator and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */

namespace Oswald.Environment
{
    public class DestructibleWall : MonoBehaviour
    {
        [SerializeField] float timeToShowVFX = 3f;
        [SerializeField] Vector2 spawnPosition;
        [SerializeField] GameObject explosionVFX;


        [Space]
        [Header("Dialogue Properties")]
        [SerializeField] DialogueManager dialogueManager;
        [SerializeField] int dialogueIndex;                         // Specified in the editor for more customisation

        [SerializeField] Dialogue dialogue;
        // Start is called before the first frame update
        void Start()
        {
            // Debug.Log("I am running1");
            StartCoroutine(ShowVFX());       // When loading the scene Level 1 from Story Intro scene, this coroutine doesn't run
                                             // Debug.Log("I am running2");
        }


        IEnumerator ShowVFX()
        {
            yield return new WaitForSeconds(timeToShowVFX);
            // Debug.Log("EXPLOSION");
            // Show VFX
            Instantiate(explosionVFX, spawnPosition, transform.rotation);

            // Works
            /*        dialogue.SetTimeManager(this.timeManager);
                    Instantiate(dialogue.gameObject);*/
            Instantiate(dialogueManager.InstantiateDialogue(dialogueIndex));


            gameObject.SetActive(false);                                    // Fix problem of above by not destroying the gameobject but setting it inactive instead

            // dialogueManager.DialoguePlay(dialogueIndex);                    // Plays the dialogue
        }
    }
}



