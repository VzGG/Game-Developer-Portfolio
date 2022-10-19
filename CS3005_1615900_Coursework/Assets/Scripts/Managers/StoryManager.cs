using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose of this manager is to TRACK the total time taken from level 1 to 3. This means the player has beaten the game and the Game Over screen has not been shown
/// Add all times from TimeManager in here and if it is less than a specified total time taken, then a different story is provided in the Ending Story scene
/// </summary>
public class StoryManager : MonoBehaviour
{
    [SerializeField] private string goodEnding;
    [SerializeField] private string badEnding;

    
    /*
     * Level 1 finished => time taken recorded
     * Level 2 finished => time taken recorded
     * Level 3 finsihed => time taken recorded
     * 
     * if at any point we lose time taken recorded is restarted
     */
    [Space]
    [Header("Time taken")]
    [SerializeField] private float totalTimeTaken = 0f;



    /* 
     * Singleton and don't destroy on load concept and inspired by:
     * 
     * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Laser Defender‘ ***2019 Course. 2021 course provides different learning materials*** [Course] 
     * Available at: https://www.udemy.com/course/unitycourse/
     */
    private void Awake()
    {
        int numberOfStoryManagers = FindObjectsOfType<StoryManager>().Length;
        if (numberOfStoryManagers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }


    public void SetTotalTimeTaken(float time) { this.totalTimeTaken = time; }

    public float GetTotalTimeTaken() { return totalTimeTaken; }

    public void ResetTotalTimeTaken() { this.totalTimeTaken = 0f; }

    public string GetGoodEnding() { return this.goodEnding; }
    public string GetBadEnding() { return this.badEnding; }
}
