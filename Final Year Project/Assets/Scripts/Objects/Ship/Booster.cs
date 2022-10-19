using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Booster : ShipComponents
{
    // Booster has:
    // boost speed - this varies and is used to multiply the normal speed
    // normal speed = same for all
    // evasion
    float maxEvasion = 0.45f;
    float baseBoostSpeed = 1.0f; // Requires energy
    float normalSpeed = 5f;      // Does not require energy - need to add in list


    // In game values
    float currentNormalSpeed = 0f;
    float currentEvasion = 0f;
    float currentBoostSpeed = 0f;



    public Booster(int givenWeight, List<Sprite> boosterSprites)
    {
        BoosterInitialize(givenWeight, boosterSprites);
    }

    public override void ComponentActive()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyValueStatModifier()
    {
        if (this.values.Count <= 0) { return; }

        float modifiedNormalSpeed = this.values[0] * valueMultiplier;
        float modifiedEvasion = this.values[1] * valueMultiplier;
        float modifiedBoostSpeed = this.values[2] * valueMultiplier;

        //Debug.Log("original normal speed: " + this.values[0] + " | modified normal speed: " + modifiedNormalSpeed + " | WITH value multiplier: " + valueMultiplier);
        //Debug.Log("original evasion: " + this.values[1] + " | modified evasion: " + modifiedEvasion + " | WITH value multiplier: " + valueMultiplier);
        //Debug.Log("original boost speed: " + this.values[2] + " | modified boost speed: " + modifiedBoostSpeed + " | WITH value multiplier: " + valueMultiplier);

        this.modifiedValues.Add(modifiedBoostSpeed);    // modifiedValues[0]
        this.modifiedValues.Add(modifiedEvasion);       // modifiedValues[1]
        this.modifiedValues.Add(modifiedNormalSpeed);   // modifiedValues[2]

        InitializeBooster();
    }

    public void ApplyAugments()
    {
        if (this.augments.Count <= 0)
        {
            // Use original values instead - no initializations
            InitializeBooster();
            return;
        }
        else
        {
            // Apply augments to the player
            float augmentedBoostSpeed = 0f;
            float augmentedEvasion = 0f;
            float augmentedNormalSpeed = 0f;

            // Clear the modified Values!!!
            this.modifiedValues.Clear();

            for (int i = 0; i < this.augments.Count; i++)
            {
                if (augments[i].GetAugmentType() == "Boost Speed")
                {
                    // Increase boost speed
                    augmentedBoostSpeed += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Evasion")
                {
                    // Increase evasion (stacking them additively) and then using this as a multiplier to the original evasion
                    augmentedEvasion += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Base Speed")
                {
                    augmentedNormalSpeed += augments[i].GetValues()[0];
                }
            }

            // Now add this augmented values together with the orignal values in a new value and store it in
            // the modified values list
            float appliedAugmentedBoostSpeed = this.values[0] + augmentedBoostSpeed;
            float appliedAugmentedEvasion = this.values[1] * (1 + augmentedEvasion);    // Example: 0.45 * (1 + 0.4) -> +40% more evasion
            float appliedAugmentedNormalSpeed = this.values[2] + augmentedNormalSpeed;

            Debug.Log("Augmented value (boost speed): " + appliedAugmentedBoostSpeed + " | Original value: " + this.values[0]
                + "\nAugmented value (evasion): " + appliedAugmentedEvasion + " | Original value: " + this.values[1]
                + "\nAugmented value (normal speed): " + appliedAugmentedNormalSpeed + " | Original value: " + this.values[2]);

            // Now add this to the modified values (for player only)
            this.modifiedValues.Add(appliedAugmentedBoostSpeed);        // modifiedValues[0]
            this.modifiedValues.Add(appliedAugmentedEvasion);           // modifiedValues[1]
            this.modifiedValues.Add(appliedAugmentedNormalSpeed);       // modifiedValues[2]

            // REINITIALIZE THE ARMOUR NOW
            InitializeBooster();
            return;
            // Then call this method in the ProgressionManager
        }
    }

    private void BoosterInitialize(int givenWeight, List<Sprite> boosterSprites)
    {
        //  
        // Determine booster by given weight
        float boostSpeed = baseBoostSpeed + ((float)givenWeight / 1000f); // 1 + (0.1 to 1.5) // This is a multiplier to the boost speed.  2.5 multiplier * 5 = 10.5 would be pretty fast if 
        float evasion = maxEvasion - ((float)givenWeight * 0.0003f);
        string boosterName = "B";
        string boosterNameBehaviour = "";
        int numberName = givenWeight / 100;

        // Determine bosoter name
        if (givenWeight <= 500)
            boosterNameBehaviour = "-SLOW";
        else if (givenWeight >= 501 && givenWeight <= 1000)
            boosterNameBehaviour = "-NORMAL";
        else if (givenWeight >= 1001 && givenWeight <= 1500)
            boosterNameBehaviour = "-FAST";
        boosterName += boosterNameBehaviour + "-" + numberName.ToString();

        // Add the determined values
        this.values.Add(boostSpeed);            // values[0]
        this.values.Add(evasion);               // values[1]
        this.values.Add(normalSpeed);           // values[2]

        this.weight = givenWeight;
        this.name = boosterName;

        // Determine the sprite
        int randomIndex = Random.Range(0, boosterSprites.Count);    // NEED  TO RANDOM INDEX FOR SAVING, make this a field in the abstract class
        Sprite randomSprite = boosterSprites[randomIndex];
        this.sprite = randomSprite;

    }

    private void InitializeBooster()
    {
        if (this.modifiedValues.Count <= 0)
        {
            currentBoostSpeed = this.values[0];

            currentEvasion = this.values[1];

            currentNormalSpeed = this.values[2];
        }
        else
        {
            currentBoostSpeed = this.modifiedValues[0];
            currentEvasion = this.modifiedValues[1];
            currentNormalSpeed = this.modifiedValues[2];
        }
    }

    public float GetBoostSpeed() { return this.currentBoostSpeed; }
    public float GetEvasion() { return this.currentEvasion; }
    public float GetNormalSpeed() { return this.currentNormalSpeed; }
}
