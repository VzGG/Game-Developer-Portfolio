using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// For creation of Weapon, Armour, Booster, Frame, and Core parts/components
public abstract class ShipComponents
{
    // Stores the stats of the component - child class of this class inherits their own "values" and "weights"
    [SerializeField] protected List<float> values = new List<float>();
    [SerializeField] protected int weight = 0;
    [SerializeField] protected string name = "";
    [SerializeField] protected Sprite sprite = null;

    // Augments that can only be added after clearing a level or buying it in a Trader (requires special currency)
    [SerializeField] protected int maxAugmentSlot = 7;                              // This does not change
    [SerializeField] protected int currentAugmentSlot = 0;                          // Changes and cannot exceed maxAugmentSlot
    [SerializeField] protected List<Augment> augments = new List<Augment>();        // Storage of augments

    [SerializeField] protected float valueMultiplier = 1;
    [SerializeField] protected List<float> modifiedValues = new List<float>();      // The modified and upgraded/downgraded values of the "values" properties

    // Each component has an active that does an action, for weapon it fires, for armour it takes armor damage, etc.
    public abstract void ComponentActive();
    public List<float> GetValues() { return this.values; }
    public string GetName() { return this.name; }
    public Sprite GetSprite() { return this.sprite; }
    public List<Augment> GetAugments() { return this.augments; }
    public void AddAugment(Augment augment)
    {
        
        // Check for current slot
        if (currentAugmentSlot + augment.GetSlotTaken() <= maxAugmentSlot)
        {
            // If we can add, add to current slot number counter
            currentAugmentSlot += augment.GetSlotTaken();

            // Add to augments list
            this.augments.Add(augment);
            Debug.Log("successfully added augment in ship component");
        }
        else
        {
            Debug.Log("Player cannot add augment. Insufficient augment slot.");
        }
    }
    public int GetCurrentAugmentSlots() { return this.currentAugmentSlot; }
    public int GetMaxAugmentSlots() { return this.maxAugmentSlot; }
    public float GetValueMultiplier() { return this.valueMultiplier; }
    public void SetValueMultiplier(float givenValueMultiplier) { this.valueMultiplier = givenValueMultiplier; }
    public List<float> GetModifiedValues() { return this.modifiedValues; }
    public int GetWeight() { return this.weight; }

}
