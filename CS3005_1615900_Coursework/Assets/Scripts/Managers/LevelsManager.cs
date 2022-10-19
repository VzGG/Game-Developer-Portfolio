using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity Library (N/A) ‘SceneManager’. [Scripting API]. https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html  
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    private MusicManager musicManager;
    [SerializeField] int musicManagerMusicIndex = -1;

    private TimeManager timeManager;
    private StoryManager storyManager;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
        storyManager = FindObjectOfType<StoryManager>();
    }

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        MusicChangeOnLevelChange();

        executionCounter = 0;                           // Reset counter for EXECUTION of the time taken for a level

        if (SceneManager.GetActiveScene().name == "Ending Story")
        {
            // Destroy the character gameObject to restart when played again.
            Destroy(FindObjectOfType<PlayerMovement>().gameObject);
        }
    }

    // All levels have a level manager, they will all have different musicManagermusicIndex
    private void MusicChangeOnLevelChange()
    {
        musicManager.PlayMusic(musicManagerMusicIndex);
    }



    // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
    public void LoadScene(string sceneName)
    {
        if (sceneName == "Quit") { Application.Quit(); return; } // https://docs.unity3d.com/ScriptReference/Application.Quit.html

        // Before loading to the next levels
        // If loading to next scene are these scenes, add them all up
        if (sceneName == "Level 2" || sceneName == "Level 3" || sceneName == "Ending Story")
        {
            RecordTimeForLevel1_2_3();
        }
        else
        {
            // if are next scene to go to are: credits, start screen, level 1, and Game Over (, and maybe Story Intro but this scene is only accessed ONCE)
            storyManager.ResetTotalTimeTaken();
            Debug.Log("RESETTING THE TIME taken!!!");
        }

        
        SceneManager.LoadScene(sceneName);


    }

    int executionCounter = 0;
    void RecordTimeForLevel1_2_3()
    {
        executionCounter++;
        if (executionCounter == 1)
        {
            float timeTakenForCurrentLevel = timeManager.GetCurrentTime();
            // Add the times
            storyManager.SetTotalTimeTaken(storyManager.GetTotalTimeTaken() + timeTakenForCurrentLevel);
            Debug.Log("RECORDING THE TIME taken: " + timeTakenForCurrentLevel.ToString("F2"));
        }

    }

}
