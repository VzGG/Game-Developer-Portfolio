using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// This class is within a ship class
/// </summary>
public class Ability : Mastery
{
    private List<AbilityUpgrade> abilityUpgrades = new List<AbilityUpgrade>();


    // Constructor
    public Ability(string givenName, List<float> givenValues, string givenDescription, int givenSlotTaken, 
        List<AbilityUpgrade> givenAbilityUpgrades)
    {
        this.name = givenName;
        this.values = givenValues;
        this.description = givenDescription;
        this.slotTaken = givenSlotTaken;

        this.abilityUpgrades = givenAbilityUpgrades;
    }

    // The ability's active effect
    public void Active()
    {

    }


}
