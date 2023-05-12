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
    [SerializeField] private float _damageReduction = 0f;
    public float DamageReduction 
    { 
        get
        {
            return _damageReduction;
        } 
        set
        {
            _damageReduction = value;

            // Prevent going over the limit
            if (_damageReduction > _damageReductionCap)
            {
                _damageReduction = _damageReductionCap;
            }

            // Turn on/off damage reduction
            if (_damageReduction <= 0f)
                canDamageReduction = false;
            else
                canDamageReduction = true;
        } 
    }
    // 90 = 90%
    private float _damageReductionCap = 90f;

    public float GetMaxArmour() { return this._MaxArmour; }
    public float GetArmour() { return this._armour; }
    public float GetArmourPercentage() { return _armour / _MaxArmour; }
    public void SetMaxArmour(float maxArmour) { this._MaxArmour = maxArmour; }
    public void SetArmour(float armour) { this._armour = armour; }
    public bool IsArmourZero() { return _armour <= 0; }

    public void TakeArmourDamage(float damage) 
    {
        //this._armour -= damage;
        _armour = Mathf.Max(_armour - damage, 0f);
    }

    // Change the given damage and don't make a copy via "Ref" keyword
    public float ReduceDamage(float damage)
    {
        float reducedDamage = damage * (1f - (DamageReduction / 100f));
        return reducedDamage;
    }
}