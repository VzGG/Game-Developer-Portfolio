using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] string levelName = "";

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + this.levelName);
        SceneManager.LoadScene(this.levelName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ControlPage()
    {
        SceneManager.LoadScene("Controls");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public string GetLevelName() {

        //Debug.LogError("next level name: ");  
        return this.levelName; 
    }

}
