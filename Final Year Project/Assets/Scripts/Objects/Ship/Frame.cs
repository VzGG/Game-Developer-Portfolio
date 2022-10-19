using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Frame : ShipComponents
{
    // HP
    // Regen
    // Take damage
    // Has:
    // hull (hp)
    // regen bonus (additive)
    // base regen
    float maxRegenBonus = 15f;
    float baseRegen = 3f;       // Added in the list


    float baseHull = 5000f;
    float baseHullRegen = 50f;
    

    // In battle/scene properties
    bool isRegenOn = false;
    float currentHull = 0f;
    float maxHull = 0f;
    float hullRegen = 0f;
    bool isFrameDestroyed = false;
    

    public Frame(int givenWeight, List<Sprite> frameSprites)
    {
        FrameInitialize(givenWeight, frameSprites);
    }

    public override void ComponentActive()
    {
        // Your health per second

        //float regenBonus = this.values[1];
        //float baseRegenBonus = this.values[2];

        // Only regen when regen on is true
        //if (isRegenOn)
        //{
        //    this.currentHull += (regenBonus + baseRegenBonus) * Time.deltaTime;
        //}
        if (isRegenOn)
        {
            this.currentHull += (hullRegen) * Time.deltaTime;
        }


        if (this.currentHull > maxHull)
            this.currentHull = this.maxHull;
        else if (currentHull <= 0f)
        {
            this.currentHull = 0f;
            // Perform die
            Debug.LogError("PERFORM DYING ACTION");
            isFrameDestroyed = true;
        }

        // throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        this.currentHull -= damage;
    }

    public void ApplyValueStatModifier()
    {
        if (this.values.Count <= 0) { return; }

        float modifiedHull = this.values[0] * valueMultiplier;
        float modifiedHullRegen = (this.values[1] + this.values[2]) * valueMultiplier;

        //Debug.Log("original hull: " + this.values[0] + " | modified hull: " + modifiedHull);
        //Debug.Log("original hull regen: " + (this.values[1] + this.values[2]) + " | modified hull regen: " + modifiedHullRegen);

        this.modifiedValues.Add(modifiedHull);
        this.modifiedValues.Add(modifiedHullRegen);

        InitializeHull();
    }

    public void ApplyAugments()
    {
        if (this.augments.Count <= 0)
        {
            InitializeHull();
            return;
        }
        else
        {
            // Apply augments to the player
            float augmentedFrame = 0f;
            float augmentedFrameRegen = 0f;

            // Clear the modified values
            this.modifiedValues.Clear();

            for (int i = 0; i < this.augments.Count; i++)
            {
                if (augments[i].GetAugmentType() == "Frame")
                {
                    augmentedFrame += augments[i].GetValues()[0];
                }
                else if (augments[i].GetAugmentType() == "Frame Regen")
                {
                    // Increase frame regen
                    augmentedFrameRegen += augments[i].GetValues()[0];
                }
            }

            // Now add this augmented values in a new value together with original values then store to modified values list
            float appliedAugmentedFrame = this.values[0] + augmentedFrame;
            float appliedAugmentedFrameRegen = (this.values[1] + this.values[2]) + augmentedFrameRegen;

            // Now add to modified values
            this.modifiedValues.Add(appliedAugmentedFrame);
            this.modifiedValues.Add(appliedAugmentedFrameRegen);

            // Reinitialize the armour
            InitializeHull();
            return;
            // Then call this in the ProgressionManager
        }
    }

    private void FrameInitialize(int givenWeight, List<Sprite> frameSprites)
    {
        // Determine frame by given weight
        float hullHP = (float)givenWeight * 10f;                            // 100 to 1500 * 10 -> 1000 to 15000 bonus hp
        //float regenBonus = maxRegenBonus - ((float)givenWeight * 0.01f);
        float regenBonus = 150f - ((float)givenWeight / 10f);               // 150 - (( 100 to 1500)/ 10) -> 150 - (10 to 150)
        string frameName = "F";
        string frameNameBehaviour = "";
        int numberName = givenWeight / 100;

        // Determine frame name
        if (givenWeight <= 500)
            frameNameBehaviour = "-LIGHT-STRONG";
        else if (givenWeight >= 501 && givenWeight <= 1000)
            frameNameBehaviour = "-MIDDLE-NORMAL";
        else if (givenWeight >= 1001 && givenWeight <= 1500)
            frameNameBehaviour = "-HEAVY-WEAK";
        frameName += frameNameBehaviour + "-" + numberName.ToString();

        //float newValue = hullHP + 15000f;
        //float newValue2 = baseRegen + 500f;
        // Now add the determined values
        this.values.Add(hullHP + baseHull);         // values[0]
        this.values.Add(regenBonus);                // values[1]
        this.values.Add(baseHullRegen);             // values[2]

        this.weight = givenWeight;
        this.name = frameName;

        // Determine the sprite
        int randomIndex = Random.Range(0, frameSprites.Count); // USED FOR SAVING - not implemented yet but make this a field in the ship componetns abstract class
        Sprite randomSprite = frameSprites[randomIndex];
        this.sprite = randomSprite;


        //InitializeHull(this.values[0]);
        //isFrameDestroyed = false;
    }

    //private void InitializeHull(float hullhp)
    //{
    //    // set the current hp/hull in battle
    //    currentHull = hullhp;
    //    maxHull = currentHull;
    //}

    private void InitializeHull()
    {
        isFrameDestroyed = false;

        if (this.modifiedValues.Count <= 0)
        {
            // Set the current hp/hull in battle
            currentHull = this.values[0];
            maxHull = currentHull;

            hullRegen = this.values[1] + this.values[2]; // HullRegen is a combination of baseRegen + regenBonus

            //Debug.Log("Initialize hull without modifiers");

        }
        else
        {
           // Debug.Log("Initialize hull WITH MODIFIERS");

            currentHull = this.modifiedValues[0];
            maxHull = currentHull;

            hullRegen = this.modifiedValues[1]; 

        }
    }


    public void SetIsRegenOn(bool status) { this.isRegenOn = status; }

    public float GetCurrentHull() { return this.currentHull; }
    public float GetCurrentHullPercentage() { return this.currentHull / this.maxHull; }
    public bool GetIsFrameDestroyed() { return this.isFrameDestroyed; }
    public bool GetIsRegenOn() { return this.isRegenOn; }
    public float GetMaxHull() { return this.maxHull; }
    public float GetCurrentHullRegen() { return this.hullRegen; }
}
