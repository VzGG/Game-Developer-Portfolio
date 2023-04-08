using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Armour is just like Energy, and Health. They are components that make the player.
/// The armour is a layer of defense that intercepts the damage first before reducing the health.
/// Quite similar to how a real life armour vest. 
/// </summary>
public class Armour : MonoBehaviour
{
    [Header("Armour properties")]
    [SerializeField] private float _MaxArmour = 0f;
    [SerializeField] private float _armour = 0f;

    public bool canDamageReduction { get; set; } = false;
    public float damageReduction { get; set; } = 0f;

    public float GetMaxArmour() { return this._MaxArmour; }
    public float GetArmour() { return this._armour; }
    public float GetArmourPercentage() { return _armour / _MaxArmour; }
    public void SetMaxArmour(float maxArmour) { this._MaxArmour = maxArmour; }
    public void SetArmour(float armour) { this._armour = armour; }
    public bool IsArmourZero() { return _armour <= 0; }

    public void TakeArmourDamage(float damage) 
    {
        this._armour -= damage;        
    }

    // Change the given damage and don't make a copy via "Ref" keyword
    public void ReduceDamage(ref float damage)
    {
        damage = damage * (1f - ( damageReduction / 100f) );
    }
}