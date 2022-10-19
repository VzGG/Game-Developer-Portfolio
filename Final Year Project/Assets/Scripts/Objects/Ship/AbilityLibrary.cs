using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A library or list of instantiated Abilities, call this class to get the abilities and set them to the ship
/// </summary>
public static class AbilityLibrary
{
    // For Shield and Booster (Armour Shield and Scurry) Abilities
    public static AbilityUpgrade increaseDuration_ShieldBooster_Level_1 = new AbilityUpgrade("Increase Duration", new List<float>() { 0.5f }, "+0.5 second duration", 1, false);
    public static AbilityUpgrade decreaseCooldown_ShieldBooster_Level_1 = new AbilityUpgrade("Decrease Cooldown", new List<float>() { -2.5f }, "-2.5 second cooldown", 1, false);
    public static AbilityUpgrade increaseDuration_ShieldBooster_Level_2 = new AbilityUpgrade("More Increase Duration", new List<float>() { 1.0f }, "+1 second duration", 1, false);
    public static AbilityUpgrade decreaseCooldown_ShieldBooster_Level_2 = new AbilityUpgrade("More Decrease Cooldown", new List<float>() { -3.0f }, "-3 second cooldown", 1, false);

    // For FrameCore (Overtuned) Ability
    public static AbilityUpgrade increaseDuration_FrameCore_Level_1 = new AbilityUpgrade("Increase Duration", new List<float>() { 0.25f }, "+0.25 second duration", 1, false);
    public static AbilityUpgrade decreaseCooldown_FrameCore_Level_1 = new AbilityUpgrade("Decrease Cooldown", new List<float>() { -2.5f }, "-2.5 second cooldown", 1, false);
    public static AbilityUpgrade increaseDuration_FrameCoreLevel_2 = new AbilityUpgrade("More Increase Duration", new List<float>() { 0.75f }, "+0.75 second duration", 1, false);
    public static AbilityUpgrade decreaseCooldown_FrameCore_Level_2 = new AbilityUpgrade("More Decrease Cooldown", new List<float>() { -5f }, "-5 second cooldown", 1, false);

    // For Weapon (Laser Fire) Ability
    public static AbilityUpgrade increaseContactDamage_WeaponLaserFire_Level_1 = new AbilityUpgrade("Increase Contact Damage", new List<float>() { 250f }, "+250 Contact Damage", 1, false);
    public static AbilityUpgrade increaseAdditionalDamage_WeaponLaserFire_Level_1 = new AbilityUpgrade("Increase Additional Damage", new List<float>() { +100f }, "+100 Additional Damage per second", 1, false);
    public static AbilityUpgrade increaseContactDamage_WeaponLaserFire_Level_2 = new AbilityUpgrade("More Increase Contact Damage", new List<float>() { 500f }, "+500 Contact Damage", 1, false);
    public static AbilityUpgrade increaseAdditionalDamage_WeaponLaserFire_Level_2 = new AbilityUpgrade("More Increase Additional Damage", new List<float>() { +100f }, "+200 Additional Damage per second", 1, false);

    // For Weapon (Drag Shot) Ability
    public static AbilityUpgrade increaseContactDamage_WeaponDragShot_Level_1 = new AbilityUpgrade("Increase Contact Damage", new List<float>() { 200f }, "+200 Contact Damage", 1, false);
    public static AbilityUpgrade increaseAdditionalDamage_WeaponDragShot_Level_1 = new AbilityUpgrade("Increase Additional Damage", new List<float>() { 75f }, "+75 Additional Damage", 1, false);
    public static AbilityUpgrade increaseContactDamage_WeaponDragShot_Level_2 = new AbilityUpgrade("More Increase Contact Damage", new List<float>() { 350f }, "+350 Contact Damage", 1, false);
    public static AbilityUpgrade increaseAdditionalDamage_WeaponDragShot_Level_2 = new AbilityUpgrade("More Increase Additional Damage", new List<float>() { 75f }, "+150 Additional Damage", 1, false);

    // For Weapon (Burst Shot) Ability
    public static AbilityUpgrade increaseDamage_WeaponBurstShot_Level_1 = new AbilityUpgrade("Increase Damage", new List<float>() { 10f }, "+10 damage", 1, false);
    public static AbilityUpgrade increaseProjectiles_WeaponBurstShot_Level_1 = new AbilityUpgrade("Increase Projectiles", new List<float>() { 5f }, "+5 Projectiles", 1, false);
    public static AbilityUpgrade increaseDamage_WeaponBurstShot_Level_2 = new AbilityUpgrade("More Increase Damage", new List<float>() { 15f }, "+15 damage", 1, false);
    public static AbilityUpgrade increaseProjectiles_WeaponBurstShot_Level_2 = new AbilityUpgrade("More Increase Projectiles", new List<float>() { 10f }, "+10 projectiles", 1, false);

    // NOT SURE IF THESE WILL PROVIDE different values if its saved???
    public static List<Ability> allAbilities = new List<Ability>()
    {
        // The description variable should not need actual numbers to describe
        // (as the description might change when ability is upgraded) or add SET DESCRIPTION? - just make it vague

        #region Support Abilities - these are instantiated here and are fitted into the Ship
        // Support Abilities
        new Ability(
            // Name
            "Armour Shield",
            // Ability Values
            new List<float>() { 3f, 35f}, 
            // Description
            "Summons a shield around the ship that negates all incoming attacks for a few seconds.",
            // Slot Taken
            1,
            // List of Ability Upgrade for this ability - all will be inactive (bool = false) by default
            new List<AbilityUpgrade>()
            {
                increaseDuration_ShieldBooster_Level_1,
                decreaseCooldown_ShieldBooster_Level_1,
                increaseDuration_ShieldBooster_Level_1,
                decreaseCooldown_ShieldBooster_Level_1,
                // Special Ability Upgrade 1 - Choice 1
                new AbilityUpgrade("Attack Conversion - Duration", new List<float>() {0.5f, 5f}, "Every hit while the shield is active increases the shield duration by 0.5 second (max 5 seconds) and is applied after the ability is finished.", 1, false),
                // Special Ability Upgrade 1 - Choice 2
                new AbilityUpgrade("Attack Conversion - Power", new List<float>() {100f, 1500f, 5f}, "Each attacks absorbed are converted to power of 100 and cannot exceed 1500 damage increase, this takes effect for 5 seconds after the ability is finished.", 1, false),
                increaseDuration_ShieldBooster_Level_2,
                decreaseCooldown_ShieldBooster_Level_2,
                increaseDuration_ShieldBooster_Level_2,
                decreaseCooldown_ShieldBooster_Level_2,
                // Special Ability Upgrade 2 Choice 1 - Defense Path
                new AbilityUpgrade("Attack Conversion - Heal", new List<float>() {0.30f}, "30% of total damage absorbed is healed to the Armour", 1, false),
                // Special Ability Upgrade 2 Choice 2 - Offense Path
                new AbilityUpgrade("Attack Conversion - Blast", new List<float>() {0.30f}, "30% of total damage absorbed during the duration of the shield ability is expelled and inflict damage to enemy hit in the vicinity.", 1, false)
            }
            ),

        new Ability(
            "Scurry",
            new List<float>() {2.5f, 0.5f, 1f, 35f},
            "Become faster increasing both base speed and boost speed while avoiding all incoming attacks for a few seconds.",
            1,
            new List<AbilityUpgrade>()
            {
                increaseDuration_ShieldBooster_Level_1,
                decreaseCooldown_ShieldBooster_Level_1,
                increaseDuration_ShieldBooster_Level_1,
                decreaseCooldown_ShieldBooster_Level_1,

                new AbilityUpgrade("Attack Conversion - Duration", new List<float>() {0.5f, 5f }, "Every hit while the ability is active increases the duration by 0.5 second (max 5 seconds) and is applied after the ability is finished.", 1, false),
                new AbilityUpgrade("Double Speed", new List<float>() {2.0f}, "Double the base speed and boost speed", 1, false),

                increaseDuration_ShieldBooster_Level_2,
                decreaseCooldown_ShieldBooster_Level_2,
                increaseDuration_ShieldBooster_Level_2,
                decreaseCooldown_ShieldBooster_Level_2,

                new AbilityUpgrade("Attack Conversion - Evasion", new List<float>() { 100f }, "Absorbed attacks are converted to more boost speed and base speed +0.5 base speed and +0.1 boost (max 2.0 for both) speed per hit. The sum of both current base speed and boost speed / 100 is added to your component's evasion value and takes after this ability is finished. Lasts 3 seconds.", 1, false),
                new AbilityUpgrade("Booster Conversion", new List<float>() { 0.3f }, "Removes your evasion while your ability is active in exchange for increasing your power. 30% of the sum of your base speed and boost speed is the multiplier of your attacks.", 1, false)
            }
            ),

        new Ability(
            "Over-tuned",
            new List<float>() { 2.0f, 5f, 50f},
            "Massively increase your Frame and Core's base regen and base regen bonus for a few seconds.",
            1,
            new List<AbilityUpgrade>()
            {
                increaseDuration_FrameCore_Level_1,
                decreaseCooldown_FrameCore_Level_1,
                increaseDuration_FrameCore_Level_1,
                decreaseCooldown_FrameCore_Level_1,

                new AbilityUpgrade("Increased Multiplier", new List<float>() { 0.5f }, "+0.5 regen multiplier", 1, false),
                new AbilityUpgrade("Super Increased Duration", new List<float>() { 3.0f}, "+3 second duration", 1, false),

                increaseDuration_FrameCoreLevel_2,
                increaseDuration_FrameCoreLevel_2,
                increaseDuration_FrameCoreLevel_2,
                decreaseCooldown_FrameCore_Level_2,

                new AbilityUpgrade("Super Increased Multiplier", new List<float>() { 2.0f }, "+2.0 regen multiplier", 1, false),
                new AbilityUpgrade("Regen Conversion", new List<float>() { 0.3f }, "Sacrifice and reduce your overall current regeneration to 30% and increase your damage by a multiplier of the sum of your base regen and base regen bonus / 100 (from highest regen component).", 1, false)
            }),
        #endregion

        #region Offensive Abilities
        // Offensive Abilities
        new Ability(
            "Laser Fire",
            new List<float>() { 3000f, 300f, 3f, 60f},
            "Shoots a laser beam at a direction and deals massive contact damage to all enemies hit and applies additional damage afterwards for a few seconds.",
            2,
            new List<AbilityUpgrade>()
            {
                increaseContactDamage_WeaponLaserFire_Level_1,
                increaseAdditionalDamage_WeaponLaserFire_Level_1,
                increaseContactDamage_WeaponLaserFire_Level_1,
                increaseAdditionalDamage_WeaponLaserFire_Level_1,

                new AbilityUpgrade("Concentrated Laser Fire", new List<float>() { 0.05f, 0.5f }, "Reduce the width of the laser to half and increase laser fire's contact damage by +5% max hull of enemy", 1, false),
                new AbilityUpgrade("Spread Laser Fire", new List<float>() { 2000f, 0.5f }, "Double the width of the laser and increase the laser fire's contact damage by 2000", 1, false),

                increaseContactDamage_WeaponLaserFire_Level_2,
                increaseAdditionalDamage_WeaponLaserFire_Level_2,
                increaseContactDamage_WeaponLaserFire_Level_2,
                increaseAdditionalDamage_WeaponLaserFire_Level_2,

                new AbilityUpgrade("Super Damage Increase - Percentage", new List<float>() { 0.10f, 0.01f }, "Increase damage by +10% max hull of enemy as contact damage, +1% max hull of enemy as additional damage", 1, false),
                new AbilityUpgrade("Super Damage Increase - Flat", new List<float>() { 7000f, 700f }, "Increase damage by +7000 contact damage and +700 additional damage per second", 1, false)

            }),

        new Ability(
            "Drag Shot",
            new List<float>() { 2000f, 250f, 3f, 50f},
            "Fires a single heavy projectile that deals strong contact damage to all enemies hit and are dragged away by the shot, all enemies dragged take additional damage per second for a few seconds.",
            2,
            new List<AbilityUpgrade>()
            {
                increaseContactDamage_WeaponDragShot_Level_1,
                increaseAdditionalDamage_WeaponDragShot_Level_1,
                increaseContactDamage_WeaponDragShot_Level_1,
                increaseAdditionalDamage_WeaponDragShot_Level_1,

                // Special ability 1 choice 1
                new AbilityUpgrade("Super Contact Damage Increase - Flat", new List<float>() { 2000f }, "+2000 Contact Damage", 1, false),
                // Special ability 1 choice 2
                new AbilityUpgrade("Super Decrease Cooldown", new List<float>() {-20f}, "-20 second cooldown", 1, false),

                increaseContactDamage_WeaponDragShot_Level_2,
                increaseAdditionalDamage_WeaponDragShot_Level_2,
                increaseContactDamage_WeaponDragShot_Level_2,
                increaseAdditionalDamage_WeaponDragShot_Level_2,

                // Special ability 2 choice 1
                new AbilityUpgrade("Armour Weakener", new List<float>() { -0.20f, 0.05f }, "All hits by the drag shot now deals enemy armour reduction. Contact damage reduces armour reduction by 20% and each second being dragged by the projectile reduces it by 5% more.", 1, false),
                // Special ability 2 choice 2
                new AbilityUpgrade("Weapon Weakener", new List<float>() { -0.30f, 0.05f }, "All hits by the drag shot now reduce the targets' power with an initial hit (contact damage) by 30% and additional 5% per second when being dragged.", 1, false)
            }),

        new Ability(
            "Burst Shot",
            new List<float>() { 100f, 50f, 50f },
            "Fires many projectiles in a spread angle and each hit deals damage to only 1 target.",
            2,
            new List<AbilityUpgrade>()
            {
                increaseDamage_WeaponBurstShot_Level_1,
                increaseProjectiles_WeaponBurstShot_Level_1,
                increaseDamage_WeaponBurstShot_Level_1,
                increaseProjectiles_WeaponBurstShot_Level_1,

                new AbilityUpgrade("Link Damage", new List<float>() { 50f }, "Each enemy hit is marked, the number of marked * 50 as damage is applied (after the ability is finished) to all marked enemies", 1, false),
                new AbilityUpgrade("Incremental Damage", new List<float>() { 0.1f }, "Each hit on an enemy increases the ability's projectile damage by 10%. The increase % is applied before the damage.", 1, false),

                increaseDamage_WeaponBurstShot_Level_2,
                increaseProjectiles_WeaponBurstShot_Level_2,
                increaseDamage_WeaponBurstShot_Level_2,
                increaseProjectiles_WeaponBurstShot_Level_2,

                new AbilityUpgrade("Critical Damage", new List<float>() { 2.5f }, "Each projectile hit towards an enemy is now dealt as 250% (2.5x) critical damage.", 1, false),
                new AbilityUpgrade("Pure Damage", new List<float>() { -100000000f }, "All hits ignore armour, armour reduction, and evasion and damages the enemy Frame instead.", 1, false)
            })

        #endregion
    };
}
