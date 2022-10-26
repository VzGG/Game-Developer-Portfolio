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

    float time2En = 0f;
    // Called over and over per frame in the update method
    public void EnergyRegen()
    {
        if (isEnergyBeingUsed == false)
        {
            // Continuously increase your energy until max
            energy = Mathf.Min(energy + energyRegeneration * Time.deltaTime, maxEnergy);
        }

        // When energy is spent up, wait a few seconds and then start regenerating energy
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

    // When using energy, we need to set is energy to true, but after that should be setting to false
    // Jump requires energy -> the moment we press jump we require
    // Slide requires energy
    // Attack requires energy
    public void WhenEnergyIsUsed()
    {

    }

    private void Update()
    {
        //if (isEnergyBeingUsed == false)
        //{
        //    // Continuously increase your energy until max
        //    energy = Mathf.Min(energy + energyRegeneration * Time.deltaTime, maxEnergy);
        //}

        //// If we all energy then we wait for a few seconds to charge our energy
        //if (isEnergyBeingUsed == true && energy <= 1)
        //{
        //    time += Time.deltaTime;
        //    if (time > regenWaitTime)
        //    {
        //        time = 0f;
        //        isEnergyBeingUsed = false;
        //    }
        //}

    }
}
