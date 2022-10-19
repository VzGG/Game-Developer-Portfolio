using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Almost all living objects have energy except traps, they use energy to attack, run, etc.
/// </summary>
public class Energy : MonoBehaviour
{
    [SerializeField] private const float maxEnergy = 100f;
    [SerializeField] private float energy = maxEnergy;
    [SerializeField] private float energyRegeneration = 5f;
    [SerializeField] private bool isEnergyBeingUsed = false;
    [SerializeField] float time = 0f;
    [SerializeField] float regenWaitTime = 3f;
    public bool GetIsEnergyBeingUsed() { return isEnergyBeingUsed; }
    public void SetIsEnergyBeingUsed(bool status) { isEnergyBeingUsed = status; }

    public void UseEnergy(float energy) 
    {
        isEnergyBeingUsed = true;

        this.energy = Mathf.Max(this.energy - energy, 0f);
        // this.energy -= energy; 
    }
    public float GetEnergy() { return this.energy; }
    public float GetEnergyPercentage() { return energy / maxEnergy; }

    public bool CanUseEnergy(float energyToUse) 
    {
        if (energy - energyToUse < 0)
            return false;
        else
            return true;
    }


    public void SetEnergy(float energy) { this.energy = energy; }
    
    private void Update()
    {
        if (isEnergyBeingUsed == false)
        {
            // Continuously increase your energy until max
            energy = Mathf.Min(energy + energyRegeneration * Time.deltaTime, maxEnergy);
        }

        // If we all energy then we wait for a few seconds to charge our energy
        if (isEnergyBeingUsed == true && energy <= 1)
        {
            time += Time.deltaTime;
            if (time > regenWaitTime)
            {
                time = 0f;
                isEnergyBeingUsed = false;
            }
        }

    }
}
