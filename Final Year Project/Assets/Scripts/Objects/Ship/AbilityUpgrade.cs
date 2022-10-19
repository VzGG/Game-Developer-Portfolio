using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityUpgrade : Mastery
{
    protected bool upgradeActive = false;
    public AbilityUpgrade(string givenName, List<float> givenValues, string givenDescription, int givenSlotTaken, bool givenUpgradeActive)
    {
        this.name = givenName;
        this.values = givenValues;
        this.description = givenDescription;
        this.slotTaken = givenSlotTaken;

        this.upgradeActive = givenUpgradeActive;
    }

    public bool GetUpgradeActive() { return this.upgradeActive; }
}
