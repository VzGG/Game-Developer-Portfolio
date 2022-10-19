using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armour : ShipComponents
{
    float maxDamageReduction = 0.45f;
    // ctrl + e, c = comment
    // ctrl + e, u = uncomment
    public Armour(int givenWeight, List<Sprite> armourSprites)
    {
        ArmourInitialize(givenWeight, armourSprites);
    }

    public override void ComponentActive()
    {
        // Takes damage
        // Check if this armour component is alive, if so, take damage here instead
        if (this.currentArmour <= 0f)
        {
            this.currentArmour = 0f;
            this.isArmourZero = true;
        }
        else if (this.currentArmour > this.maxArmour)
        {
            this.currentArmour = this.maxArmour;
            this.isArmourZero = false;
        }
        else if (this.currentArmour > 0f)
        {
            this.isArmourZero = false;
        }
    }

    // NEEDS TO BE APPLIED EARLIER IN THE SCENE... for Enemy 
    public void ApplyValueStatModifier()
    {
        if (this.values.Count <= 0) { return; }

        float modifiedDefense = this.values[0] * valueMultiplier;
        float modifiedDamageReduction = this.values[1] * valueMultiplier;

        //Debug.Log("original defense: " + this.values[0] + " | modified defense: " + modifiedDefense);
        //Debug.Log("original damage reduction: " + this.values[1] + " | modified damage reduction: " + modifiedDamageReduction);

        this.modifiedValues.Add(modifiedDefense);           // modifiedValues[0]
        this.modifiedValues.Add(modifiedDamageReduction);   // modifiedValues[1]

        // Activate the current armour of the owner
        InitializeArmour();

    }

    public void ApplyAugments()
    {
        if (this.augments.Count <= 0) 
        {
            InitializeArmour();
            return;
        }
        else
        {
            // Apply augments to the player
            float augmentedDefense = 0f;
            float augmentedDamageReduction = 0f;

            // Clear the modified values
            this.modifiedValues.Clear();

            for (int i = 0; i < this.augments.Count; i++)
            {
                if (augments[i].GetAugmentType() == "Armour")
                {
                    // Increase armour
                    augmentedDefense += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Damage Reduction")
                {
                    // Increase damage reduction
                    augmentedDamageReduction += augments[i].GetValues()[0];
                }
            }

            // Now add this augmented values in a new value together with original values then store to the modified values list
            float appliedAugmentedDefense = this.values[0] + augmentedDefense;
            float appliedAugmentedDamageReduction = this.values[1] * (1 + augmentedDamageReduction);

            // Now add to modified values
            this.modifiedValues.Add(appliedAugmentedDefense);
            this.modifiedValues.Add(appliedAugmentedDamageReduction);

            // Reinitialize the armour
            InitializeArmour();
            return;
            // Then call this in the ProgressionManager
        }
    }

    float currentArmour = 0f;
    float maxArmour = 0f;
    float currentDamageReduction = 0f;
    bool isArmourZero = false;

    float baseArmour = 5000f;


    // Creates a random but balanced armour - 5000 + (1000-15000) -> 
    private void ArmourInitialize(int givenWeight, List<Sprite> armourSprites)
    {
        // Determine item by the given weight
        // givenWeight is a random number between 100-1501
        float defense = (float) givenWeight * 10f;                                              // Random between 1,000 to 15,000
        float damageReduction =  maxDamageReduction - ((float) givenWeight * 0.0003f);          // 0.45 - (100 to 1500) * 0.0003 -> Random between 0.00 (0%) to 0.42 (42%) damage reduction
        string armourName = "A";
        string armourNameBehaviour = "";
        int numberName = givenWeight / 100;

        // Determine armour's name
        if (givenWeight <= 500)
            armourNameBehaviour = "-LIGHT";
        else if (givenWeight >= 501 && givenWeight <= 1000)
            armourNameBehaviour = "-MIDDLE";
        else if (givenWeight >= 1001 && givenWeight <= 1500)
            armourNameBehaviour = "-HEAVY";
        armourName += armourNameBehaviour + "-" + numberName.ToString();

        // Add the determined values
        this.values.Add(defense + baseArmour);               // Values[0]
        this.values.Add(damageReduction);                    // Values[1]

        this.weight = givenWeight;
        this.name = armourName;

        // Determine the sprite
        int randomIndex = Random.Range(0, armourSprites.Count); // USEFUL FOR SAVING TO HARD DRIVE - LOOK AT THIS LATER ON
        Sprite randomSprite = armourSprites[randomIndex];
        this.sprite = randomSprite;


    }

    //}
    private void InitializeArmour()
    {
        if (this.modifiedValues.Count <= 0)
        {
            // Setup armour at the start of the battle
            currentArmour = this.values[0];
            maxArmour = currentArmour;

            currentDamageReduction = this.values[1];

            //Debug.Log("Armour initialized WITHOUT MODIFIERS");
        }
        else
        {
            //Debug.Log("Armour is initialized");
            // If there is modifiers, (stat value multiplier - for enemys) or (augments - for players)
            // Use modified value instead

            // For enemy AND player
            currentArmour = this.modifiedValues[0];
            maxArmour = currentArmour;

            currentDamageReduction = this.modifiedValues[1];

            // For player
        }


    }

    public void TakeArmourDamage(float damage)
    {
        // Everytime we take damage, the damage is reduced
        float reducedDamage = damage - (damage * currentDamageReduction);
        // Now take damage
        this.currentArmour -= reducedDamage;
    }

    public float GetCurrentArmour() { return this.currentArmour; }
    public float GetCurrentArmourPercentage() { return this.currentArmour / this.maxArmour; }
    public float GetMaxArmour() { return this.maxArmour; }
    public float GetCurrentDamageReduction() { return this.currentDamageReduction; }
}
