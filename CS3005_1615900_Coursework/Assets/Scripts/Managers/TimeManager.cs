using System.Collections;
using System.Collections.Generic;

// Unity Package (N/A) ‘TextMeshPro’. [Scripting API]. https://docs.unity3d.com/Manual/com.unity.textmeshpro.html
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TMP_Text tmp_timeText;
    [SerializeField] float currentTime = 0f;


    [SerializeField] private bool isTimeStopped = false;
    [SerializeField] GameObject dialoguePanel;                  // Reference to the UI dialogue panel - used by timemanager to hide it after it is shown.

    public float GetCurrentTime() { return currentTime; }
    public bool GetIsTimeStopped() { return isTimeStopped; }


    // Update is called once per frame
    void Update()
    {
        RecordTime();

        // checks if we stopped time
        if (isTimeStopped)
            StopTime();
        else if (!isTimeStopped)
            ContinueTime();

        // Press enter key ONCE to stop time
        /*        if (Input.GetKeyDown(KeyCode.Return))
                {
                    ChangeIsTimeStopped();
                    if (isTimeStopped)
                        StopTime();
                    else if (!isTimeStopped)
                        ContinueTime();
                }*/

        // Unpause
        if (Input.GetKeyDown(KeyCode.Return) && isTimeStopped)
        {
            // Usually the timeis stopped so we just changeIsTimeStopped();
            Debug.Log("Unpausing!!!");
            dialoguePanel.SetActive(false);                 // Hide the dialogue panel
            isTimeStopped = false;
        }
    }

    private void RecordTime()
    {
        // Records time
        currentTime += Time.deltaTime;

        // Print to decimal place - source - https://answers.unity.com/questions/192977/print-to-only-two-decimal-places.html
        tmp_timeText.text = currentTime.ToString("F2");
    }

    private void StopTime()
    {
        Time.timeScale = 0f;
    }

    private void ContinueTime()
    {
        Time.timeScale = 1f;
    }

    public void ChangeIsTimeStopped()
    {
        isTimeStopped = !isTimeStopped;
    }
}
