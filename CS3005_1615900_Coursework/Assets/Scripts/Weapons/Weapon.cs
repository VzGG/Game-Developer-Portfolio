using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public RuntimeAnimatorController AirComboAnimController;
    [SerializeField] public RuntimeAnimatorController GroundedComboAnimController;
    [TextArea(1, 20)]
    [SerializeField] private string _airComboList;
    [TextArea(1, 20)]
    [SerializeField] private string _groundedComboList;

    [SerializeField] private float _weaponDamage;
    [SerializeField] private float _weaponEnergy;
    [SerializeField] public Weapon SecondaryWeapon;         // This is your bow or sword or handspell - can be added at runtime or can have its preset.


}
