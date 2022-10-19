using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyShip : Ship
{
    public EnemyShip(ShipOperator givenShipOperator, int givenWeightCapacity, List<Weapon> givenInstalledWeapons,
    Armour givenInstalledArmour, Booster givenInstalledBooster, Frame givenInstalledFrame, Core givenInstalledCore)
    {
        this.shipOperator = givenShipOperator;
        this.weightCapacity = givenWeightCapacity;

        this.installedWeapons = givenInstalledWeapons;
        this.installedArmour = givenInstalledArmour;
        this.installedBooster = givenInstalledBooster;
        this.installedFrame = givenInstalledFrame;
        this.installedCore = givenInstalledCore;
    }

    // Fitness is the average value multiplier of all value multipliers from each component in a ship
    /// <summary>
    /// Fitness is the average value multiplier 
    /// </summary>
    /// <returns></returns>
    public float GetShipAverageFitness()
    {
        // Only use the first weapon, if we use average of all number of weapons' value multiplier then it will be optimized to have as much weapons as possible
        float installedWeapons = this.installedWeapons[0].GetValueMultiplier();
        float installedArmour = this.installedArmour.GetValueMultiplier();
        float installedBooster = this.installedCore.GetValueMultiplier();
        float installedCore = this.installedCore.GetValueMultiplier();
        float installedFrame = this.installedFrame.GetValueMultiplier();

        float allShipFitness = (installedWeapons + installedArmour + installedBooster + installedCore + installedFrame) / 5f;

        return allShipFitness;

    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Dodge()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 Move()
    {
        throw new System.NotImplementedException();
    }
    public override void MoveFaster()
    {
        throw new System.NotImplementedException();
    }
}
