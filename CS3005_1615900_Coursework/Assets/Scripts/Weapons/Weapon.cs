using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Secondary
    {
        Bow
    }

    [SerializeField] public RuntimeAnimatorController AirComboAnimController;
    [SerializeField] public RuntimeAnimatorController GroundedComboAnimController;

    [SerializeField] private string[] AirComboList;

    [SerializeField] private string[] GroundedComboList;

    [SerializeField] private float _weaponDamage;
    [SerializeField] private float _weaponEnergy;
    [SerializeField] public Secondary SecondaryWeapon;          // This is your bow or potentially a handspell - can be set at runtime or can have its preset.

    public string[] GetAirComboList() { return AirComboList; }
    public string[] GetGroundedComboList() { return GroundedComboList; }
}