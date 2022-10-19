using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
/// <summary>
/// Each ship component contains augments and have no more than 7 augments (7 slots)
/// Some augments may take more than 1 slot i.e., powerful augments
/// </summary>
public class Augment : Mastery
{
    //[SerializeField] string name = "";
    //[SerializeField] List<float> values = new List<float>();
    //[SerializeField] string description = "";
    //[SerializeField] int slotTaken = 0;
    [SerializeField] string augmentType = "";
    [SerializeField] string componentType = "";

    // Constructor
    public Augment(string givenName, List<float> givenValues, string givenDescription, int givenSlotTaken, string givenAugmentType
        ,string givenComponentType)
    {
        this.name = givenName;
        this.values = givenValues;
        this.description = givenDescription;
        this.slotTaken = givenSlotTaken;
        this.augmentType = givenAugmentType;
        this.componentType = givenComponentType;
    }
    

    // Getters
    //public string GetName() { return this.name; }
    //public List<float> GetValue() { return this.values; }
    //public string GetDescription() { return this.description; }
    //public int GetSlotTaken() { return this.slotTaken; }
    public string GetAugmentType() { return this.augmentType; }
    public string GetComponentType() { return this.componentType; }
}
