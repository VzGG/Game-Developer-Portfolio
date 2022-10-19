using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class which is used only as value container. In the later parts of the project, this should and must be used to attach to
/// Enemy GAMEOBJECTS and its enemy script (different script not this one) and load the values from this class into that enemy script gameobject class
/// </summary>
 
[System.Serializable]
public class EnemyTest
{
    

    //[SerializeField] public int attackPower = 0;
    // Enemy Attributes
    [SerializeField] float power = 0f;           // Attack power                                                    level 1 enemies have about 1-1000 power
    [SerializeField] float armour = 0f;          // "Health" equivalent points, take damage here before the hull    level 1 enemies have about 1-1000 armour
    [SerializeField] float speed = 0f;           // Movement speed                                                  level 1 enemies have about 0.01-1 speed
    [SerializeField] float hull = 0f;            // Health points                                                   level 1 enemies have about 1-1000 hull
    [SerializeField] float energy = 0f;          // "Mana or stamina" equivalent points                             level 1 enemies have about 1-1000 energy

    // Constructor
    public EnemyTest(float givenPower, float givenArmour, float givenSpeed, float givenHull, float givenEnergy)
    {
        this.power = givenPower;
        this.armour = givenArmour;
        this.speed = givenSpeed;
        this.hull = givenHull;
        this.energy = givenEnergy;
    }

    #region Getters and Setters
    // Getters and Setters
    public float GetPower() { return this.power; }
    public void SetPower(float givenPower) { this.power = givenPower; }

    public float GetArmour() { return this.armour; }
    public void SetArmour(float givenArmour) { this.armour = givenArmour; }

    public float GetSpeed() { return this.speed; }
    public void SetSpeed(float givenSpeed) { this.speed = givenSpeed; }

    public float GetHull() { return this.hull; }
    public void SetHull(float givenHull) { this.hull = givenHull; }

    public float GetEnergy() { return this.energy; }
    public void SetEnergy(float givenEnergy) { this.energy = givenEnergy; }

    #endregion
}
