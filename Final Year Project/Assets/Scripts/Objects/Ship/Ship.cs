using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ship
{
    // Ship has 5 components
    /*
     * Weapon - to fire
     * Armour - to absorb damage before taking Frame Damage
     * Booster - to move
     * Frame - to take damage
     * Core - to use attacks, special movements
     * 
     * Each components are summed up. There is a weight capacity. Going over means reduction in stats for all components, does not reduce the actual ITEMS value but the referenced one
     * 
     */

    // SHOULD HAVE MULTIPLIER - a global multiplier of all parts? or each part have multipliers?
    // Each PART MAKES MORE SENSE CODE WISE AND BETTER TOO, each enemy have average multiplier. 
    // 0.5-1.0 for level 1
    // 0.75-1.25 for level 2
    // 0.1-1.5 for level 3
    // 1.25-1.5 for level 4
    // 1.5-1.75 for level 5
    // 1.75-2.0 for level 6
    // 2.0-2.25 for level 7


    [SerializeField] protected ShipOperator shipOperator = null;
    [SerializeField] protected int weightCapacity = 0;
    #region Ship Components
    // Ship components
    //[SerializeField] protected Weapon installedWeapon = null;
    [SerializeField] protected List<Weapon> installedWeapons = new List<Weapon>();
    [SerializeField] protected Armour installedArmour = null;
    [SerializeField] protected Booster installedBooster = null;
    [SerializeField] protected Frame installedFrame = null;
    [SerializeField] protected Core installedCore = null;

    // List of weapon installed weapon and spawn many weapon
    [SerializeField] protected List<Ability> abilities = new List<Ability>();
    // Player has two slot at the first level but can be leveled to 4
    // 2 slot = 2 support abilities OR 1 Offensive
    [SerializeField] protected int maxAbilitySlot = 2;
    [SerializeField] protected int currentAbilitySlotTaken = 0;
    #endregion

    public void SetShipOperator(ShipOperator givenShipOperator) { this.shipOperator = givenShipOperator; }

    #region Getter and Setter of Components
    //public void SetInstalledWeapon(Weapon givenWeapon) { this.installedWeapons = givenWeapon; }
    public void SetInstalledWeapon(List<Weapon> givenWeapons) { this.installedWeapons = givenWeapons; }
    public void SetInstalledArmour(Armour givenArmour) { this.installedArmour = givenArmour; }
    public void SetInstalledBooster(Booster givenBooster) { this.installedBooster = givenBooster; }
    public void SetInstalledFrame(Frame givenFrame) { this.installedFrame = givenFrame; }
    public void SetInstalledCore(Core givenCore) { this.installedCore = givenCore; }
    public void SetMaxAbilitySlot(int givenMaxAbilitySlot) { this.maxAbilitySlot = givenMaxAbilitySlot; }
    public void SetCurrentAbilitySlotTaken(int givenCurrentAbilitySlotTaken) { this.currentAbilitySlotTaken = givenCurrentAbilitySlotTaken; }

    // Getters
    public Frame GetInstalledFrame() { return this.installedFrame; }
    public Booster GetInstalledBooster() { return this.installedBooster; }
    public Armour GetInstalledArmour() { return this.installedArmour; }
    public Core GetInstalledCore() { return this.installedCore; }
    public List<Weapon> GetInstalledWeapons() { return this.installedWeapons; }
    public List<Ability> GetAbilities() { return this.abilities; }
    public int GetMaxAbilitySlot() { return this.maxAbilitySlot; }
    public int GetCurrentAbilitySlotTaken() { return this.currentAbilitySlotTaken; }


    #endregion

    #region Abstract methods
    // Abstract Methods - define these methods in the playership or enemyship or bossship
    public abstract void Attack();
    public abstract Vector2 Move();
    public abstract void MoveFaster();
    public abstract void Dodge();

    #endregion
}
