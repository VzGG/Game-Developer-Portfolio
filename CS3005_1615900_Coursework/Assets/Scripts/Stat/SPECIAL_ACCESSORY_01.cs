using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_ACCESSORY_01 : SPECIAL
{
    private float _damageReductionBonus = 20f;
    private float _evasionRateBonus = 12.5f;
    private float _healthRegenBonus = 8f;
    private float _criticalChanceBonus = 12.5f;
    private float _criticalDamageMultiplierBonus = 0.75f;

    public SPECIAL_ACCESSORY_01()
    {
        this.Value = 10f;                                   // +10 atk bonus
        this.Description = "SPECIAL: LEGENDARY ACCESSORY";
        this.RatingPerStat = 3f;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(object obj)
    {
        base.SpecialEffect(obj);

    }

    public override void SpecialEffect(params object[] obj)
    {
        base.SpecialEffect(obj);

        Debug.Log("Component count: " + obj.Length);

        // Apply all around legendary stat but weaker versions
        bool foundAttackComponent = false;
        bool foundHealthComponent = false;

        foreach (Object cObj in obj)
        {
            Debug.Log("Obj type: " + cObj.GetType());

            // 2 Legendary effects for PlayerAttack: Bonus damage, and crit chance
            if (cObj.GetType() == typeof(Oswald.Player.PlayerAttack))
            {
                if (foundAttackComponent) { continue; }


                Oswald.Player.PlayerAttack playerAttack = (Oswald.Player.PlayerAttack)cObj;

                // Weaker version of Legendary Glove
                playerAttack.canCritical = true;
                playerAttack.criticalChance += this._criticalChanceBonus;
                playerAttack.criticalDamageMultiplier += this._criticalDamageMultiplierBonus;

                // Legendary effect of Accessory
                playerAttack.SetMyDamage(playerAttack.GetMyDamage() + this.Value);
                playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() + this.Value);

                // Stop running this if statement twice
                foundAttackComponent = true;
            }
            // 2 Legendary effects for Health: Health regen and Evasion
            else if (cObj.GetType() == typeof(Health))
            {
                if (foundHealthComponent) { continue; }

                // THIS IS CALLED TWICE!!! -> make it so that it is called once
                Debug.Log("Health comp if statement called");

                Health health = (Health)cObj;

                // Weaker version of Legendary Helmet
                health.canRegen = true;
                health.healthRegen += this._healthRegenBonus;

                // Weaker version of Legendary Boots
                health.canEvadeDamage = true;
                health.evasionRate += this._evasionRateBonus;

                foundHealthComponent = true;
            }
            // 1 Legendary effect for Armour: Damage reduction
            else if (cObj.GetType() == typeof(Armour))
            {
                // Weaker version of Legendary Plate
                Armour armour = (Armour)cObj;

                armour.canDamageReduction = true;
                armour.damageReduction += this._damageReductionBonus;
            }
        }
    }

    
    public override void RemoveSpecialEffect(params object[] obj)
    {
        base.RemoveSpecialEffect(obj);

        bool foundAttackComponent = false;
        bool foundHealthComponent = false;

        foreach (Object cObj in obj)
        {
            Debug.Log("Obj type: " + cObj.GetType());

            // 2 Legendary effects for PlayerAttack: Bonus damage, and crit chance
            if (cObj.GetType() == typeof(Oswald.Player.PlayerAttack))
            {
                if (foundAttackComponent) { continue; }


                Oswald.Player.PlayerAttack playerAttack = (Oswald.Player.PlayerAttack)cObj;

                playerAttack.criticalChance -= this._criticalChanceBonus;
                playerAttack.criticalDamageMultiplier -= this._criticalDamageMultiplierBonus;

                // Legendary effect of Accessory
                playerAttack.SetMyDamage(playerAttack.GetMyDamage() - this.Value);
                playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() - this.Value);

                if (playerAttack.criticalChance <= 0)
                {
                    playerAttack.canCritical = false;
                }


                // Stop running this if statement twice
                foundAttackComponent = true;
            }
            // 2 Legendary effects for Health: Health regen and Evasion
            else if (cObj.GetType() == typeof(Health))
            {
                if (foundHealthComponent) { continue; }

                // THIS IS CALLED TWICE!!! -> make it so that it is called once
                Debug.Log("Health comp if statement called");

                Health health = (Health)cObj;

                // Weaker version of Legendary Helmet
                health.healthRegen -= this._healthRegenBonus;

                // Weaker version of Legendary Boots
                health.evasionRate -= this._evasionRateBonus;

                if (health.healthRegen <= 0f)
                    health.canRegen = false;

                if (health.evasionRate <= 0f)
                    health.canEvadeDamage = false;

                foundHealthComponent = true;
            }
            // 1 Legendary effect for Armour: Damage reduction
            else if (cObj.GetType() == typeof(Armour))
            {
                // Weaker version of Legendary Plate
                Armour armour = (Armour)cObj;

                armour.damageReduction -= this._damageReductionBonus;

                if (armour.damageReduction <= 0f)
                    armour.canDamageReduction = false;
            }
        }
    }
}
