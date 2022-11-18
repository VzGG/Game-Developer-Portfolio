using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy's break point, when it reaches a certain threshold, the enemy becomes "broken". 
/// When broken, all player attacks sends the enemy airborne, good for players minimizing damage while fighting them.
/// </summary>
public class Break : MonoBehaviour
{
    [SerializeField] private float breakPointThreshold = 0f;                // Define in each prefab gameobject - The max threshold needed to "break" an enemy. Breaking them makes them vulnerable by making them be airborne (non-boss enemies) or by being stunned (boss) i.e., longer hurt animations.
    [SerializeField] private float currentBreakPoint = 0f;                  // Track how much the break point the enemy currently has in comparison to the threshold.
    [SerializeField] private bool isBreak = false;
    [SerializeField] private bool canBeBroken = true;
    [SerializeField] private float breakDuration = 5f;
    [SerializeField] private float breakCooldown = 7f;

    public float GetBreakPointThreshold() { return this.breakPointThreshold; }
    public float GetCurrentBreakPoint() { return this.currentBreakPoint; }
    public bool GetIsBreak() { return this.isBreak; }
    public bool GetCanBeBroken() { return this.canBeBroken; }
    public float GetBreakDuration() { return this.breakDuration; }
    public float GetBreakCooldown() { return this.breakCooldown; }
    public float GetCurrentBreakPointPercentage() { return currentBreakPoint / breakPointThreshold; }

    public void SetCurrentBreakPoint(float currentBreakPoint) { this.currentBreakPoint = currentBreakPoint; }
    public void SetIsBreak(bool isBreak) { this.isBreak = isBreak; }
    public void SetCanBeBroken(bool canBeBroken) { this.canBeBroken = canBeBroken; }


    public void TakeBreakPointDamage(float breakPointDamage)
    {
        this.currentBreakPoint += breakPointDamage;
    }
}
