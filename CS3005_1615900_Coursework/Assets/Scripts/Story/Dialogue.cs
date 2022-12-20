using System.Collections;
using System.Collections.Generic;
// Unity Package (N/A) ‘TextMeshPro’. [Scripting API]. https://docs.unity3d.com/Manual/com.unity.textmeshpro.html 
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Oswald.Manager;

namespace Oswald.Story
{
    /// <summary>
    /// Dialogue is attached to a gameObject, when a dialogue is instantiated, they should play the dialogue (in console or on the screen)
    /// </summary>
    public class Dialogue : MonoBehaviour
    {
        [SerializeField] private string dialogueSpeaker;
        [SerializeField] private string dialogue;
        [SerializeField] TimeManager timeManager;
        [SerializeField] float timeToDeactivateDialogue = 3f;

        [Space]
        [Header("UI Dialogue Properties")]
        [SerializeField] GameObject dialoguePanel;
        [SerializeField] Text dialogueSpeakerTextObj;
        [SerializeField] Text dialogueTextObj;

        public void SetDialoguePanel(GameObject dialoguePanel) { this.dialoguePanel = dialoguePanel; }
        public void SetDialogueSpeakerTextObj(Text speakerObj) { this.dialogueSpeakerTextObj = speakerObj; }
        public void SetSetDialogueTextObj(Text textObj) { this.dialogueTextObj = textObj; }


        public void SetTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }

        public void SetDialogueSpeaker(string dialogueSpeaker) { this.dialogueSpeaker = dialogueSpeaker; }

        public void SetDialogue(string dialogue) { this.dialogue = dialogue; }

        // Start is called before the first frame update
        void Start()
        {
            // On start
            // 1. Call pause time
            timeManager.ChangeIsTimeStopped();
            // 2. Show dialogue with speaker's name - Display
            // Debug.Log(dialogueSpeaker + ": " + dialogue);
            dialoguePanel.SetActive(true);                                      // Display the dialogue panel in the screen by setting it active, to deactivate, the Timemanager handles the deactivation
            dialogueSpeakerTextObj.text = dialogueSpeaker;
            dialogueTextObj.text = dialogue;

            // 3. Get Enter Key input to unpause time - Done in the Timemanager's update

            // Destroy dialogue or set inactive gameObject

            StartCoroutine(DestroyDialogue());

        }

        // When time is paused, so is the coroutine such as this below
        IEnumerator DestroyDialogue()
        {
            // Wait n seconds
            yield return new WaitForSeconds(timeToDeactivateDialogue);
            // Deactive the gameObject with dialogue script in it
            gameObject.SetActive(false);
        }
    }
}

