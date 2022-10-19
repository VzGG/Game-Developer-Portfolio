using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A creator of parts for both human player-side faction and human enemy faction.
/// Enemy will generate same parts but they will have multipliers
/// 
/// This creates the non visual ship of player and enemy.
/// 
/// It should, when called, provide only random components
/// </summary>
[System.Serializable]
public class ShipComponentsCreator : MonoBehaviour
{
    [SerializeField] List<PlayerShip> playerShips = new List<PlayerShip>();

    // Read this more on SeralizeReference * BE WARY OF THIS USAGE * - https://forum.unity.com/threads/abstract-class-in-list-only-shows-base-variables.999478/
    [SerializeReference] [SerializeField] List<ShipComponents> shipComponents = new List<ShipComponents>();
    [Space]
    [Header("Weapon Sprites")]
    [SerializeField] List<Sprite> weaponRangeSprites = new List<Sprite>();
    [SerializeField] List<Sprite> weaponMeleeSprites = new List<Sprite>();
    [Space]
    [Header("Armour Sprites")]
    [SerializeField] List<Sprite> armourSprites = new List<Sprite>();
    [Space]
    [Header("Booster Sprites")]
    [SerializeField] List<Sprite> boosterSprites = new List<Sprite>();
    [Space]
    [Header("Frame Sprites")]
    [SerializeField] List<Sprite> frameSprites = new List<Sprite>();
    [Space]
    [Header("Core Sprites")]
    [SerializeField] List<Sprite> coreSprites = new List<Sprite>();

    private int lowerLimit = 100;
    private int upperLimit = 1501;

    #region Create Ship Components - Called by Ship Builder
    // Creates a non visual weapon component
    public List<Weapon> CreateWeaponComponents() 
    {
        int maxWeapons = Random.Range(1, 5);
        List<Weapon> weapons = new List<Weapon>();
        for (int i = 0; i < maxWeapons; i++)
            weapons.Add(new Weapon(Random.Range(lowerLimit, upperLimit), this.weaponRangeSprites, this.weaponMeleeSprites));

        return weapons;
    }

    public Armour CreateArmourComponent()
    {
        return new Armour(Random.Range(lowerLimit, upperLimit), this.armourSprites);   
    }

    public Booster CreateBoosterComponent()
    {
        return new Booster(Random.Range(lowerLimit, upperLimit), this.boosterSprites);
    }

    public Frame CreateFrameComponent()
    {
        return new Frame(Random.Range(lowerLimit, upperLimit), this.frameSprites);
    }

    public Core CreateCoreComponent()
    {
        return new Core(Random.Range(lowerLimit, upperLimit), this.coreSprites);
    }


    #endregion

    // NEED TO ALSO CREATE A SHIP FOR ENEMY CRAFTS
    //public PlayerShip CreatePlayerShip()
    //{

    //    // Rand between 1-4
    //    int maxWeapons = Random.Range(1, 5);

    //    // List of weapons needed to add to playerShip
    //    List<Weapon> weapons = new List<Weapon>();


    //    for (int i = 0; i < maxWeapons; i++)
    //    {
    //        // Create n amount of weapons and add to list
    //        weapons.Add(new Weapon(Random.Range(100, 1501), this.weaponRangeSprites, this.weaponMeleeSprites));
    //    }
    //    weapons[0].GetAugments().Add(AugmentLibrary.allAugments[Random.Range(0,
    //        AugmentLibrary.allAugments.Count)]); // REMOVE

    //    PlayerShip playerShip = new PlayerShip(
    //        null,
    //        8000,
    //        weapons,
    //        new Armour(Random.Range(100, 1501), this.armourSprites),
    //        new Booster(Random.Range(100, 1501), this.boosterSprites),
    //        new Frame(Random.Range(100, 1501), this.frameSprites),
    //        new Core(Random.Range(100, 1501), this.coreSprites)
    //        );

    //    //playerShips.Add(playerShip);
    //    return playerShip;
    //}

    // Create Enemy Ship
    // Create Large Ship - Carrier of enemy ship - basically a spawner
    // Create Boss Ship 1 - use same assets
    // Create Boss Ship 2  - use different assets
}
