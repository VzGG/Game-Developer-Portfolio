using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Almost all living objects have energy except traps, they use energy to attack, run, etc.
/// </summary>
public class Energy : MonoBehaviour
{
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energy = 0f;
    [SerializeField] private float energyRegeneration = 5f;
    [SerializeField] private bool isEnergyBeingUsed = false;
    [SerializeField] float time = 0f;
    [SerializeField] float regenWaitTime = 3f;
    [SerializeField] bool isStartWait = false;
    public float GetEnergy() { return this.energy; }
    public float GetEnergyPercentage() { return energy / maxEnergy; }
    public float GetMaxEnergy() { return this.maxEnergy; }
    public void SetEnergy(float energy) { this.energy = energy; }
    public void SetMaxEnergy(float maxEnergy) { this.maxEnergy = maxEnergy; }
    public void SetEnergyRegen(float energyRegen) { this.energyRegeneration = energyRegen; }
    public void UseEnergy(float energy) 
    {
        isEnergyBeingUsed = true;
        isStartWait = false;

        this.energy = Mathf.Max(this.energy - energy, 0f); 
    }

    /// <summary>
    /// Called over and over per frame in the update method. It should regenerate the player's energy per frame.
    /// It should stop regenerating when the energy is being used.
    /// </summary>
    public void EnergyRegen()
    {
        if (isEnergyBeingUsed == false)
        {
            // Continuously increase your energy until max
            energy = Mathf.Min(energy + energyRegeneration * Time.deltaTime, maxEnergy);
        }

        // Everytime energy is used, it refreshes the time needed for reaching the wait time.
        // This means that the player can only start regenerating energy when they stop using energy for the given regen wait time.
        if ((isEnergyBeingUsed || energy <= 1) && isStartWait == false)
        {
            time = 0f;
            isStartWait = true;
        }

        // Start to wait for the regen time to reach.
        // Once the regen time is reached, replenish the player's energy.
        if (isStartWait)
        {
            time += Time.deltaTime;
            if (time > regenWaitTime)
            {
                time = 0f;
                isEnergyBeingUsed = false;
                isStartWait = false;
            }
        }
    }
}
