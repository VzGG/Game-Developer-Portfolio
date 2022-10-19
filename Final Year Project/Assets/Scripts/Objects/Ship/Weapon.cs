using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : ShipComponents
{
    float power = 0f;
    float fireRate = 0f;
    float energyConsumption = 0f;

    // Constructor
    public Weapon(int givenWeight, List<Sprite> weaponRangeSprites, List<Sprite> weaponMeleeSprites)
    {
        // Give weight, then the FireRate, Power, Consumption, Name, random the sprite and weapon type
        WeaponInitialize(givenWeight, weaponRangeSprites, weaponMeleeSprites);
    }
    // Unused for this class
    public override void ComponentActive() { }
    // The enemy's (ONLY) value stat modifier - called by the enemy controller only
    public void ApplyValueStatModifier()
    {
        if (this.values.Count <= 0) { return; }

        float modifiedPower = this.values[0] * valueMultiplier;
        float modifiedFireRate = this.values[1] / valueMultiplier;
        float modifiedEnergyConsumption = this.values[2] / valueMultiplier;

        //Debug.Log("original power: " + this.values[0] + " | modified power: " + modifiedPower);
        //Debug.Log("original fireRate: " + this.values[1] + " | modified fireRate: " + modifiedFireRate);
        //Debug.Log("original consumption: " + this.values[2] + " | modified consumption: " + modifiedEnergyConsumption);

        this.modifiedValues.Add(modifiedPower);             // modifiedValues[0]
        this.modifiedValues.Add(modifiedFireRate);          // modifiedValues[1]
        this.modifiedValues.Add(modifiedEnergyConsumption); // modifiedValues[2]

        InitializeWeapon();
    }
    // Called by the player controller only
    public void ApplyAugments()
    {
        if (this.augments.Count <= 0)
        {
            // Use original values instead - no initializations
            InitializeWeapon();
            // USE INITIALIZATION
            return;
        }
        else
        {
            // Apply augments to the player
            float augmentedPower = 0f;
            float augmentedFireRate = 0f;
            float augmentedEnergyConsumption = 0f;

            // Clear the modified values
            this.modifiedValues.Clear();
            
            for (int i = 0; i < this.augments.Count; i++)
            {
                if (augments[i].GetAugmentType() == "Power")
                {
                    // Increase power
                    augmentedPower += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Fire Rate")
                {
                    // Increase fireRate
                    augmentedFireRate += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Energy Consumption")
                {
                    // Increase energy consumption
                    augmentedEnergyConsumption += augments[i].GetValues()[0];
                }
            }

            // Now add this augmented values together with original values in a new value and store it in the modified values list
            float appliedAugmentedPower = this.values[0] + augmentedPower;
            float appliedAugmentedFireRate = this.values[1] - (this.values[1] * augmentedFireRate);
            float appliedAugmentedEnergyConsumption = this.values[2] - (this.values[2] * augmentedEnergyConsumption);

            // Now add to modified values (for player only)
            this.modifiedValues.Add(appliedAugmentedPower);
            this.modifiedValues.Add(appliedAugmentedFireRate);
            this.modifiedValues.Add(appliedAugmentedEnergyConsumption);

            // Reinitialize weapon
            InitializeWeapon();
            return;

            // Then call this method in the progression manager
        }
    }
    private void WeaponInitialize(int givenWeight, List<Sprite> weaponRangeSprites, List<Sprite> weaponMeleeSprites)
    {
        // Initializing weapon type and appropriate sprite
        int weaponType = -1;
        weaponType = Random.Range(0, 2); // Rand between 0 and 1

        // Determine weapon attributes according to the given weight
        float power = (float)givenWeight;                             
        float fireRate = givenWeight / 1000f;                       // the random weight determines the fire rate between 0.1-1.5
        float energyConsumption = givenWeight / 10000f;             // random between 0.01 to 0.15 (1% to 15%) and gets reduced by the Core component to 10% of it -> 0.001 to 0.015 (0.1% to 1.5%)
        string weaponName = "W";
        string weaponNameBehaviour = "";
        int numberName = givenWeight / 100;

        // Determine the name depending on the weight
        if (givenWeight <= 500)
            weaponNameBehaviour = "-FAST";
        else if (givenWeight >= 501 && givenWeight <= 1000)
            weaponNameBehaviour = "-NORMAL";
        else if (givenWeight >= 1001 && givenWeight <= 1500)
            weaponNameBehaviour = "-SLOW";
        weaponName += weaponNameBehaviour + "-" + numberName.ToString();

        // Add the determined values to the list
        this.values.Add(power);                 // Values[0]
        this.values.Add(fireRate);              // Values[1]
        this.values.Add(energyConsumption);     // Values[2]
        this.values.Add(weaponType);            // Values[3]

        this.weight = givenWeight;
        this.name = weaponName;

        // Determine sprite part
        int randomIndex = Random.Range(0, weaponRangeSprites.Count);        
        Sprite randomSprite = weaponRangeSprites[randomIndex];
        this.sprite = randomSprite;

    }

    private void InitializeWeapon()
    {
        if (this.modifiedValues.Count <= 0)
        {
            // Where there's no modifiers i.e., stat multiplier (enemy) or augment (player), just use original values
            power = this.values[0];
            fireRate = this.values[1];
            energyConsumption = this.values[2];


        }
        else
        {
            // If there are modifiers either from stat multiplier or augment, use them instead

            power = this.modifiedValues[0];
            fireRate = this.modifiedValues[1];
            energyConsumption = this.modifiedValues[2];

        }
    }

    public float GetPower() { return this.power; }
    public float GetFireRate() { return this.fireRate; }
    public float GetEnergyConsumption() { return this.energyConsumption; }

    

}
