using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent of Augment, Ability and AbilityUpgrade Classes
/// </summary>
[System.Serializable]
public abstract class Mastery
{
    [SerializeField] protected string name = "";
    [SerializeField] protected List<float> values = new List<float>();
    [SerializeField] protected string description = "";
    [SerializeField] protected int slotTaken = 0;

    public string GetName() { return this.name; }
    public List<float> GetValues() { return this.values; }
    public string GetDescription() { return this.description; }
    public int GetSlotTaken() { return this.slotTaken; }
}
