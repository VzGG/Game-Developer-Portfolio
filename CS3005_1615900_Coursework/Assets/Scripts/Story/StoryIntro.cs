using System;
using System.Collections;
using System.Collections.Generic;
// Unity Package (N/A) 'TextMeshPro' [Scripting API]. Available at: https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
using TMPro;
using UnityEngine;

public class StoryIntro : MonoBehaviour
{
    [SerializeField] GameObject[] storyTexts;
    [SerializeField] TMP_Text currentText;
    [SerializeField] private int index = 0;
    [Space]
    [SerializeField] LevelsManager levelsManager;
    [SerializeField] string sceneName;
    private void Start()
    {
        // Show first part of the text
        currentText = storyTexts[index].GetComponent<TMP_Text>();
        currentText.alpha = 255f;
    }

    private void Update()
    {
        // Press enter - to move forward to the intro
        if (Input.GetKeyDown(KeyCode.Return) && index < storyTexts.Length)
        {
            currentText.alpha = 0f;                                         // Set the previous text's alpha to 0
            index += 1;
            try
            {
                currentText = storyTexts[index].GetComponent<TMP_Text>();
                currentText.alpha = 255f;                                       // Set the new one's text's alpha to 255
            }
            catch(IndexOutOfRangeException e)
            {
                Debug.LogError("Error Found!!!: " + e.Message);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) && index == 3)
        {
            // Show next scene
            // Debug.Log("Show next scene!!!");
            levelsManager.LoadScene(sceneName);
        }


    }



}
