using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Core : ShipComponents
{
    float maxRegenBonus = 15f;
    float baseRegen = 3f;               // Add to the list
    public Core(int givenWeight, List<Sprite> coreSprites)
    {
        CoreInitialize(givenWeight, coreSprites);
    }

    float currentEnergy = 0f;               // The current energy - copies from values[0]
    float maxEnergy = 0f;                   // The max energy that does not change
    bool isEnergyUsed = false;              // Prevents regen when boosting and such
    float currentTime = 0f;                 
    float waitDuration = 3.5f;                // Cooldown of regeneration being active again - more than 0 energy regen per second
    bool isEnergyZero = true;            // When our energy is zero, we must activate or stop the regeneration for a few seconds
    float energyRegen = 0f;
    float innateRegenPercentage = 0.025f;    // Buffing the core regen by providing 2.5% energy regen per second

    // Increase energy per second - called once every call in the update method
    public override void ComponentActive()
    {
        //Debug.Log("Regenerating energy");

        //if (isEnergyUsed == false && isEnergyZero == false)
        if (isEnergyUsed == false)
        {
            // Increase regen per second
            float regenBonus = this.values[1];
            float baseRegenBonus = this.values[2];

            //this.currentEnergy += (regenBonus + baseRegenBonus) * Time.deltaTime;
            this.currentEnergy += (energyRegen) * Time.deltaTime;

            // Setting the limits
            if (this.currentEnergy > this.maxEnergy)
            {
                this.currentEnergy = this.maxEnergy;
            }

            if (this.currentEnergy <= 0f)
            {
                this.currentEnergy = 0f;
                Debug.Log("Energy is zero");
                //isEnergyZero = true;
            }
        }

    }

    public void ApplyValueStatModifier()
    {
        if (this.values.Count <= 0) { return; }

        float modifiedEnergy = this.values[0] * valueMultiplier;
        float modifiedEnergyRegen = (this.values[1] + this.values[2]) * valueMultiplier;

        //Debug.Log("original energy: " + this.values[0] + " | modified energy: " + modifiedEnergy);
        //Debug.Log("original energy regen: " + (this.values[1] + this.values[2]) + " | modified energy regen: " + modifiedEnergyRegen);

        this.modifiedValues.Add(modifiedEnergy);
        this.modifiedValues.Add(modifiedEnergyRegen);

        InitializeEnergy();
    }

    public void ApplyAugments()
    {
        if (this.augments.Count <= 0)
        {
            //InitializeHull();
            InitializeEnergy();
            return;
        }
        else
        {
            // Apply augments to the player 
            // Does not augment the current/max ENERGY itself, just the regen!
            float notAugmentedEnergy = this.values[0];

            float augmentedEnergyRegen_Flat = 0f;
            float augmentedEnergyRegen_Percentage = 0f;

            // Clear the modified values
            this.modifiedValues.Clear();

            for (int i = 0; i < this.augments.Count; i++)
            {
                if (augments[i].GetAugmentType() == "Core Regen Flat")
                {
                    // Increase regen flat
                    augmentedEnergyRegen_Flat += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Core Regen Percentage")
                {
                    augmentedEnergyRegen_Percentage += augments[i].GetValues()[0];
                }
            }

            // Now add this augmented values in a new value together with orginal value and store to modifiedvalue list
            // Flat increase of REGEN
            float appliedAugmentedEnergyRegen_Flat = (this.values[1] + this.values[2]) + augmentedEnergyRegen_Flat;
            // Percentage increase of REGEN based of max energy
            float appliedAugmentedEnergyRegen_Perecentage = (this.values[0]) * augmentedEnergyRegen_Percentage;

            // Another bonus - 2.5% of max energy per second
            float bonusInnateRegen = this.values[0] * innateRegenPercentage;

            // Combine them 2 together - if we get 0 for percentage increase then we cna still benefit from flat increase
            float appliedFinalAugmentedEnergyRegen =
                appliedAugmentedEnergyRegen_Flat + appliedAugmentedEnergyRegen_Perecentage
                + bonusInnateRegen;

            

            // Now add to modified values
            this.modifiedValues.Add(notAugmentedEnergy);
            this.modifiedValues.Add(appliedFinalAugmentedEnergyRegen);

            // Reinitialize the core
            InitializeEnergy();
            return;
            // Then call this in the progression manager
        }
    }

    private void CoreInitialize(int givenWeight, List<Sprite> coreSprites)
    {
        // Determine core by given weight
        float energyMP = (float)givenWeight;                                // Random between 100-1500 (your energy/MP)
        float regenBonus = maxRegenBonus - ((float)givenWeight * 0.01f);    // regen bonus provides between 0 to 14
        string coreName = "C";
        string coreNameBehaviour = "";
        int numberName = givenWeight / 100;

        // Determine the core's name
        if (givenWeight <= 500)
            coreNameBehaviour = "-LIGHT-STRONG";
        else if (givenWeight >= 501 && givenWeight <= 1000)
            coreNameBehaviour = "-MIDDLE-NORMAL";
        else if (givenWeight >= 1001 && givenWeight <= 1500)
            coreNameBehaviour = "-HEAVY-WEAK";
        coreName += coreNameBehaviour + "-" + numberName.ToString();

        // Now add the determined values
        this.values.Add(energyMP);              // Values[0]
        this.values.Add(regenBonus);            // Values[1]
        this.values.Add(baseRegen);             // Values[2]                // Base regen of 3 (3 + 0 to 14 -> up to 17 mp/s)

        this.weight = givenWeight;
        this.name = coreName;

        // Determine the sprite
        int randomIndex = Random.Range(0, coreSprites.Count); // USED for save - not yet implemented - make this an abstract field
        Sprite randomSprite = coreSprites[randomIndex];
        this.sprite = randomSprite;


        //InitializeEnergy(energyMP);
    }

    //private void InitializeEnergy(float energyMP)
    //{
    //    // Set the current energy in battle
    //    currentEnergy = energyMP;
    //    maxEnergy = currentEnergy;
    //}

    private void InitializeEnergy()
    {
        if (this.modifiedValues.Count <= 0)
        {
            // WHEN THERE IS NO MODIFIERS - STAT MULTIPLIER OR AUGMENT, just use original values

            // Set the current energy in battle
            currentEnergy = this.values[0];
            maxEnergy = currentEnergy;

            float bonusRegenPercentage = this.maxEnergy * this.innateRegenPercentage;
            energyRegen = this.values[1] + this.values[2] + bonusRegenPercentage;

            //Debug.Log("Initialize energy without modifiers");
        }
        else
        {
            currentEnergy = this.modifiedValues[0];
            maxEnergy = currentEnergy;

            energyRegen = this.modifiedValues[1];

            //Debug.Log("Initialize energy WITH MODIFIERS!!!");
        }

    }

    // Take energy method
    public void EnergyBoost()
    {
        // Consumption is 1% + 5 of max energy
        float consumption = (this.maxEnergy / 100f) + 5f;
        this.currentEnergy -= consumption * Time.deltaTime;

        // isEnergyUsed = true;
    }
    // Take energy method
    public void DodgeBoost()
    {
        // About 2% of max energy + 5 as consumption - per call of this method
        //float consumption = ((this.maxEnergy / 10f) / 5f) + 5f;

        // NEW - 0.25% of max energy per call of this method
        float consumption = (this.maxEnergy * 0.0025f);
        this.currentEnergy -= consumption;

        // isEnergyUsed = true;
    }

    public bool CanDodgeBoost()
    {
        float consumption = ((this.maxEnergy / 10f) / 5f) +5f;

        if (this.currentEnergy - consumption > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ConsumeEnergy(float consumption)
    {
        // percentage based consumption - depending on the random it is roughly around 0.07% of max energy per consumption
        // 100% -> 1.0
        // 10% -> 0.1
        // 1% -> 0.01
        // 0.1 -> 0.001
        // 0.01 -> 0.0001
        float energyConsumption = this.maxEnergy * (consumption / 10f);

        this.currentEnergy -= energyConsumption;
    }

    public bool CanConsumeEnergy(float consumption)
    {
        float energyConsumption = this.maxEnergy * (consumption / 10f);

        if (this.currentEnergy - energyConsumption > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetCurrentEnergy() { return this.currentEnergy; }
    public float GetCurrentEnergyPerecentage() { return this.currentEnergy / this.maxEnergy; }
    public float GetTotalRegen() { return this.values[1] + this.values[2]; }
    public bool GetIsEnergyZero() { return this.isEnergyZero; }
    public bool GetIsEnergyUsed() { return this.isEnergyUsed; }
    public void SetIsEnergyUsed(bool status) { this.isEnergyUsed = status; }
    public float GetEnergyRegen() { return this.energyRegen; }
    public float GetMaxEnergy() { return this.maxEnergy; }

}
