using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player controller class should be the one doing all the logic of all the Player related class like PlayerAttack, PlayerMovement, Energy, etc.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Reference
    PlayerAttack myAttack;
    PlayerMovement myMovement;
    Health myHealth;
    Energy myEnergy;
    TimeManager timeManager;

    // This bool is changed to true at the start of any animation like Attack, Jump, and Slide
    // It is then set to false when we return to Idle
    private bool isUsingActionAnim = false;

    private void Awake()
    {
        Singleton();
        SetReferences();
    }


    // Update is called once per frame - all the logic and input events here
    void Update()
    {
        if (myHealth.GetHealth() <= 0f) { return; }         // Stops any logic below when player dies
        if (timeManager == null) { return; }                // Stop null reference error
        if (timeManager.GetIsTimeStopped()) { return; }     // Pause time should stop any physics movement - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause


        myEnergy.EnergyRegen();                             // Regen my energy


        // If there is an animation going on, cannot do any pressing
        if (isUsingActionAnim) { return; }


        // 3 IF Statement, only one should run

        
        if (myMovement.SlidePressed(myEnergy))              // When we press slide, activate the slide animation in the fixedupdate
        {
            // Can only slide, cannot attack at the same time or buffer
        }
        else if (myAttack.Attack(myEnergy))                 // When we press attack, show the animation for attacking
        {
            // can only attack, cannot slide at the same time or buffer
        }
        else if (myAttack.BowAttack(myEnergy))              // When we press the other attack (right click), show bow animation attack
        {

        }
         
    }

    // All physics related movement here
    private void FixedUpdate()
    {
        if (myHealth.GetHealth() <= 0f) { return; }         // Stops any logic below when player dies
        if (timeManager == null) { return; }                // Stop null reference error
        if (timeManager.GetIsTimeStopped()) { return; }     // Pause time should stop any physics movement - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause

        // If there is an animation going on, cannot do any pressing, or any animation in the fixed update
        if (isUsingActionAnim) { return; }

       

        myMovement.Run();                                   // Run either left or right with a run animation
        myMovement.Jump(myEnergy);                          // Makes the player jump up with jump animation
        myMovement.SlideNew();                              // Makes the player slide with slide animation
    }

    private void Singleton()
    {
        // Singleton - only one class should exist
        int player = FindObjectsOfType<PlayerController>().Length;
        if (player > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
    private void SetReferences()
    {
        myAttack = GetComponent<PlayerAttack>();
        myMovement = GetComponent<PlayerMovement>();
        myHealth = GetComponent<Health>();
        myEnergy = GetComponent<Energy>();

        // Not sure about time manager for now
        timeManager = FindObjectOfType<TimeManager>();
    }

    // Called in Animation Event
    public void IsUsingActionAnimTrue() { this.isUsingActionAnim = true; }
    // Called in Animation Event
    public void IsUsingActionAnimFalse() { this.isUsingActionAnim = false; }

   

}
