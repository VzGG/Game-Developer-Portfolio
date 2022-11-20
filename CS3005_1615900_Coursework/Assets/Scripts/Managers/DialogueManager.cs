using System.Collections;
using System.Collections.Generic;

// Unity Package (N/A) ‘TextMeshPro’. [Scripting API]. https://docs.unity3d.com/Manual/com.unity.textmeshpro.html
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] string[] dialogues;
    [SerializeField] GameObject dialoguesObj;
    [SerializeField] TimeManager timeManager;


    [Space]
    [Header("UI Dialogue Properties")]
    [SerializeField] GameObject dialoguePanel;
    //[SerializeField] TMP_Text dialogueSpeakerTextObj;
    //[SerializeField] TMP_Text dialogueTextObj;
    [SerializeField] Text dialogueSpeakerTextObj;
    [SerializeField] Text dialogueTextObj;

    // Dialogue manager is used to instantiate a dialogue with specified speaker and dialogue itself, the communicating objects just call the dialogue manager to pick the index they want
    // A dialogue prefab gameObject with attributes is then created BUT not instantiated for the caller to instantiate themselves

    public GameObject InstantiateDialogue(int dialogueIndex)
    {
        GameObject newDialogueObj = dialoguesObj;                                           // Make reference to the dialogue gameObject
        newDialogueObj.GetComponent<Dialogue>().SetDialogue(dialogues[dialogueIndex]);      // Set the new dialogue gameobject's attributes to these
        newDialogueObj.GetComponent<Dialogue>().SetDialogueSpeaker("Oswald");
        newDialogueObj.GetComponent<Dialogue>().SetTimeManager(this.timeManager);

        newDialogueObj.GetComponent<Dialogue>().SetDialoguePanel(dialoguePanel);                           // Set the UI Dialogue panel and its text obj
        newDialogueObj.GetComponent<Dialogue>().SetDialogueSpeakerTextObj(dialogueSpeakerTextObj);
        newDialogueObj.GetComponent<Dialogue>().SetSetDialogueTextObj(dialogueTextObj);

        return newDialogueObj;
    }
}
