using System.Collections;
using System.Collections.Generic;
// Unity Package (N/A) ‘TextMeshPro’. [Scripting API]. https://docs.unity3d.com/Manual/com.unity.textmeshpro.html 
using TMPro;
using UnityEngine;

public class EndingStory : MonoBehaviour
{

    private StoryManager storyManager;

    [SerializeField] TMP_Text endingStoryText;
    [SerializeField] float goodEndingFinishTime = 750f; // 750 seconds / 60 => 12.5 minutes => Good ending story
                                                        // Above 750 seconds => more than 12.5 minutes => Bad endin story


    // Start is called before the first frame update
    void Start()
    {
        storyManager = FindObjectOfType<StoryManager>();


        float timeTaken = storyManager.GetTotalTimeTaken();

        // Show good ending text
        if (timeTaken <= goodEndingFinishTime)
        {
            endingStoryText.text = storyManager.GetGoodEnding();
        }
        else
        {
            // Show bad ending otherwise
            endingStoryText.text = storyManager.GetBadEnding();
        }
    }

}
