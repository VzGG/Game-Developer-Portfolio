using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerShip : Ship
{
    public PlayerShip(ShipOperator givenShipOperator, int givenWeightCapacity, List<Weapon> givenInstalledWeapons,
        Armour givenInstalledArmour, Booster givenInstalledBooster, Frame givenInstalledFrame, Core givenInstalledCore)
    {
        this.shipOperator = givenShipOperator;
        this.weightCapacity = givenWeightCapacity;

        this.installedWeapons = givenInstalledWeapons;
        this.installedArmour = givenInstalledArmour;
        this.installedBooster = givenInstalledBooster;
        this.installedFrame = givenInstalledFrame;
        this.installedCore = givenInstalledCore;

        // SOMETIMES YOU GET A MELEE WITH NO DAMAGE AND VALUES - REMOVE THIS DEBUG FOR NOW
        //float power = givenInstalledWeapons[0].GetValues()[0];
        //float fireRate = givenInstalledWeapons[0].GetValues()[1];
        //float imaginaryTotalDPS_In10Seconds = (10 / fireRate) * power;
        //Debug.Log("Player Power: " + power + " | fireRate: " + fireRate + " | total DPS for 10 seconds: " + imaginaryTotalDPS_In10Seconds);
    }

    // These below are the ship's attacking, movement, etc.
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
        // Move the game object given
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        return new Vector2(xMove, yMove);

        // Take energy

    }
    public override void MoveFaster()
    {
        throw new System.NotImplementedException();
    }
}
