using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Contains a list of all augments possible. Call this static class from other classes to get the augments.
/// 
/// </summary>
public static class AugmentLibrary
{
    public static Augment GetAugment(int index)
    {
        return allAugments[index];
    }

    //public static Augment GetAugmentByName(string name)
    //{
        
    //}

    public readonly static List<Augment> allAugments = new List<Augment>()
    {
        // Weapon augments
            // Tier 1 - Weakest
        new Augment("Increase Power", new List<float>() { 100f }, "+100 Power", 1, "Power", "Weapon"),
        new Augment("Increase Fire Rate", new List<float>() { 0.05f }, "+5% Fire rate", 1, "Fire Rate", "Weapon"),
        new Augment("Reduce Energy Consumption", new List <float>() { 0.05f } , "-5% Energy consumption", 1, "Energy Consumption", "Weapon"),
            // Tier 2 - Good
        new Augment("Greatly Increase Power ", new List<float>() { 300f }, "+300 Power", 2, "Power", "Weapon"),
        new Augment("Greatly Increase Fire Rate", new List<float>() { 0.15f }, "+15% Fire rate", 2, "Fire Rate", "Weapon"),
        new Augment("Greatly Reduce Energy Consumption", new List <float>() { 0.15f } , "-15% Energy consumption", 2, "Energy Consumption", "Weapon"),
            // Tier 3 - Strongest
        new Augment("Massively Increase Power ", new List<float>() { 600f }, "+600 Power", 3, "Power", "Weapon"),
        new Augment("Massively Increase Fire Rate", new List<float>() { 0.4f }, "+40% Fire rate", 3, "Fire Rate", "Weapon"),
        new Augment("Massively Reduce Energy Consumption", new List <float>() { 0.4f } , "-40% Energy consumption", 3, "Energy Consumption", "Weapon"),
        // 0-8

        // Armour augments
            // Tier 1 - Weakest
        new Augment("Increase Armour", new List<float>(){ 1500f}, "+1500 Armour", 1, "Armour", "Armour"), // Player should regain his armour back on taking this augment
        new Augment("Increase Damage Reduction", new List<float>() { 0.05f }, "+5% Damage Reduction (Multiplicative)", 1, "Damage Reduction", "Armour"),
            // Tier 2 - Good
        new Augment("Greatly Increase Armour", new List<float>(){ 5000f }, "+5000 Armour", 2, "Armour", "Armour"),
        new Augment("Greatly Increase Damage Reduction", new List<float>() { 0.15f }, "+15% Damage Reduction (Multiplicative)", 2, "Damage Reduction", "Armour"),
            // Tier 3 - Strongest
        new Augment("Massively Increase Armour", new List<float>(){ 12000f }, "+12000 Armour", 3, "Armour", "Armour"),
        new Augment("Massively Increase Damage Reduction", new List<float>() { 0.4f }, "+40% Damage Reduction (Multiplicative)", 3, "Damage Reduction", "Armour"),
        // 9-14

        // Booster augments
            // Tier 1 - Weakest
        new Augment("Increase Boost Speed", new List<float>() { 0.1f }, "+0.1 Boost Speed", 1, "Boost Speed", "Booster"),
        new Augment("Increase Evasion", new List<float>() { 0.05f }, "+5% Evasion (Multiplicative)", 1, "Evasion", "Booster"),
        new Augment("Increase Base Speed", new List<float>() { 0.1f }, "+0.1 Base Speed", 1, "Base Speed", "Booster"),
            // Tier 2 - Good
        new Augment("Greatly Increase Boost Speed", new List<float>() { 0.3f }, "+0.3 Boost Speed", 2, "Boost Speed", "Booster"),
        new Augment("Greatly Increase Evasion", new List<float>() { 0.15f }, "+15% Evasion (Multiplicative)", 2, "Evasion", "Booster"),
        new Augment("Greatly Increase Base Speed", new List<float>() { 0.3f }, "+0.3 Base Speed", 2, "Base Speed", "Booster"),
            // Tier 3 - Strongest
        new Augment("Massively Increase Boost Speed", new List<float>() { 0.7f }, "+0.7 Boost Speed", 3, "Boost Speed", "Booster"),
        new Augment("Massively Increase Evasion", new List<float>() { 0.40f }, "+40% Evasion (Multiplicative)", 3, "Evasion", "Booster"),
        new Augment("Massively Increase Base Speed", new List<float>() { 0.7f }, "+0.7 Base Speed", 3, "Base Speed", "Booster"),
        // 15-23

        // Frame augments
            // Tier 1 - Weakest
        new Augment("Increase Frame (HP) Regen", new List<float>() { 20f }, "+20 Frame (HP) Regen", 1, "Frame Regen", "Frame"),
        new Augment("Increase Frame (HP)", new List<float>() { 1500f }, "+1500 Frame (HP)", 1, "Frame", "Frame"),
            // Tier 2 - Good
        new Augment("Greatly Increase Frame (HP) Regen", new List<float>() { 60f }, "+60 Frame (HP) Regen", 2, "Frame Regen", "Frame"),
        new Augment("Greatly Increase Frame (HP)", new List<float>() { 5000f}, "+5000 Frame (HP)", 2, "Frame", "Frame"),
            // Tier 3 - Strongest
        new Augment("Massively Increase Frame (HP) Regen", new List<float>() { 150f }, "+150 Frame (HP) Regen", 3, "Frame Regen", "Frame"),
        new Augment("Massively Increase Frame (HP)", new List<float>() { 12000f }, "+12000 Frame (HP)", 2, "Frame", "Frame"),
        // 24-29

        // Core augments - since energy consumption are % based, there is no point for increase of MP as regen is better overall
            // Tier 1 - Weakest
        new Augment("Increase Core (MP) Regen - Flat", new List<float>() { 2f }, "+2 Core (MP) Regen", 1, "Core Regen Flat", "Core"),
        new Augment("Increase Core (MP) Regen - Percentage", new List<float>() { 0.01f }, "+1% Core (MP) Regen", 1, "Core Regen Percentage", "Core"),
            // Tier 2 - Good
        new Augment("Greatly Increase Core (MP) Regen - Flat", new List<float>() { 6f }, "+6 Core (MP) Regen", 2, "Core Regen Flat", "Core"),
        new Augment("Greatly Increase Core (MP) Regen - Percentage", new List<float>() { 0.025f }, "+2.5% Core (MP) Regen", 2, "Core Regen Percentage", "Core"),
            // Tier 3 - Strongest
        new Augment("Massively Increase Core (MP) Regen - Flat", new List<float>() { 15f }, "+15 Core (MP) Regen", 3, "Core Regen Flat", "Core"),
        new Augment("Massively Increase Core (MP) Regen - Percentage", new List<float>() { 0.05f }, "+5% Core (MP) Regen", 3, "Core Regen Percentage", "Core"),
        // 30-35
    };

    #region Unusued Augments - No time to work on

    public readonly static List<Augment> allAugmentsUnused = new List<Augment>()
    {
        // Weapon Augments
        new Augment("Increase Power", new List<float>() { 100f }, "+100 Power", 1, "Power", "Weapon"),
        new Augment("Increase Fire Rate", new List<float>() { 0.1f }, "+10% Fire rate", 1, "Fire Rate", "Weapon"),
        new Augment("Reduce Energy Consumption", new List <float>() { 0.13f } , "-13% Energy consumption", 1, "Energy Consumption", "Weapon"),
        new Augment("Increase Weight", new List <float>(){ 100f }, "+100 Weight", 1, "Weight", "Weapon"),
        new Augment("Decrease Weight", new List<float>(){ -100f }, "-100 Weight", 1, "Weight", "Weapon"),
        new Augment("Increase Projectile Count (Spread)", new List<float>() { 1f }, "+1 Projectile count (Spread Type Only)", 2, "Projectile Count Spread", "Weapon"),
        new Augment("Increase Projectile Count (Single)", new List<float>() { 0.30f }, "+30% Chance to fire another shot that deals 50% damage (Stacks additively)", 2, "Projectile Count Single", "Weapon"),
        new Augment("Increase Frame Damage",  new List<float>() { 0.2f }, "+20% More damage to Frame (Armour destroyed)", 2, "Frame Damage", "Weapon"),
        new Augment("Pierce Armour", new List <float>() { 0.2f }, "20% Chance to ignore Armour and directly damage Frame", 2, "Pierce Armour", "Weapon"),
        new Augment("Random Modifier", new List<float>() {
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f) },
            "-15% to 30% Added bonus stat to Power, Fire rate, and Energy Consumption",
            3,
            "Random",
            "Weapon"),
        new Augment("Critical Strike Chance", new List<float>() { 0.2f, 1.75f }, "+20% Chance to deal 175% damage (chance stacks additively).", 3, "Critical", "Weapon")
        ,
        // Armour Augments
        new Augment("Increase Armour", new List<float>(){ 100f}, "+100 Armour", 1, "Armour", "Armour"),
        new Augment("Increase Damage Reduction", new List<float>() { 0.05f }, "+5% Damage Reduction (Stacks additively)", 1, "Damage Reduction", "Armour"),
        new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight", "Armour"),
        new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight", "Armour"),
        new Augment("Reactive Armour", new List<float>() { 100f }, "+20% Chance on damage taken to drop an object that deals 100/200/300 + Max Armour + Damage Reduction % of Max Armour", 2, "Reactive Armour", "Armour"),
        new Augment("Random Modifier", new List<float>() {
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f) },
            "-15% to 30% Added bonus stat to Armour, and Damage Reduction",
            2,
            "Random",
            "Armour"),
        new Augment("Ignore Incoming Damage", new List<float>() {0.05f}, "+5% To ignore incoming damage (Stacks additively)", 3, "Ignore Incoming Damage", "Armour"),
        new Augment("Armour Absorb", new List<float>(){ 0.1f, 300f, 10f }, "+10% Chance on damage taken to grant +300 Armour (10 second cooldown) (Stacks additively for both values)", 3, "Armour Absorb", "Armour"),
        new Augment("Armour Regen", new List<float>() {0.005f }, "+0.5% Armour Regen per second (Stacks additively)", 3, "Armour Regen", "Armour")
        ,
        // Booster Augments
        new Augment("Increasae Base Speed", new List<float>() { 1f }, "+1 Base Speed", 1, "Base Speed", "Booster"),
        new Augment("Decrease Base Speed", new List<float>() {-1f }, "-1 Base Speed", 1, "Base Speed", "Booster"),
        new Augment("Increase Boost Speed", new List<float>() { 0.1f }, "+0.1 Boost Speed", 1, "Boost Speed", "Booster"),
        new Augment("Decrease Boost Speed", new List<float>() {-0.1f}, "-0.1 Boost Speed", 1, "Boost Speed", "Booster"),
        new Augment("Increase Evasion", new List<float>() { 0.05f }, "+5% Evasion", 1, "Evasion", "Booster"),
        new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight", "Booster"),
        new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight", "Booster"),
        new Augment("Reactive Booster", new List<float>() { 1f }, "+20% Chance on dodge boost to drop an object that deals 100/200/300 multiplied by a multiplication of base speed and boost speed (Stacks additively)", 2, "Reactive Booster", "Booster"),
        new Augment("Random Modifier", new List<float>() {
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f) },
            "-15% to 30% Added bonus stat to Base Speed, Boost Speed, and Evasion",
            3,
            "Random",
            "Booster"),
        new Augment("Offensive Booster", new List<float>() { 1f }, "+30% Chance on dodge boost to deal damage around it. Deals 250/350 damage multiplied by a multiplication of base speed and boost speed (Stacks additively)", 3, "Offensive Booster", "Booster")
        ,
        // Frame Augments
        new Augment("Increase Base Regen", new List<float>() { 1f}, "+1 Base Regen", 1, "Base Regen", "Frame"),
        new Augment("Increase Regen Bonus", new List<float>() { 0.1f }, "+10% Base Regen Bonus", 1, "Base Regen Bonus", "Frame"),
        new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight", "Frame"),
        new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight", "Frame"),
        new Augment("Reactive Frame", new List<float>() {100f, 0.5f}, "+20% Chance on damage taken to drop an object that deals 100/200/300 + 50% of sum of base regen and base regen bonus (Stacks additively)", 2, "Reactive Frame", "Frame"),
        new Augment("Absorb Damage", new List<float>() { 0.1f, 0.15f }, "+10% Chance on damage taken to absorb 15/30/45% of damage received and be healed by the damage absorbed", 2, "Absorb Damage", "Frame"),
        new Augment("Random Modifier", new List<float>() {
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f) },
            "-15% to 30% Added bonus stat to Base Regen, and Base Regen Bonus",
            2,
            "Random",
            "Frame"),
        new Augment("Frame Regen", new List<float>() {0.01f }, "+1% of Max Frame as regen per second", 3, "Frame Regen", "Frame"),
        new Augment("Ignore Damage", new List<float>() {0.05f}, "+5% Chance to ignore damage taken", 3, "Ignore Damage", "Frame")
        ,
        // Core Augments
        new Augment("Increase Base Regen", new List<float>() { 1f}, "+1 Base Regen", 1, "Base Regen", "Core"),
        new Augment("Increase Regen Bonus", new List<float>() { 0.1f }, "+10% Base Regen Bonus", 1, "Base Regen", "Core"),
        new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight", "Core"),
        new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight", "Core"),
        new Augment("Reactive Core", new List<float>() {100f, 0.5f}, "+20% Chance on damage taken to drop an object that deals 100/200/300 + 50% of sum of base regen and base regen bonus (Stacks additively)", 2, "Reactive Core", "Core"),
        new Augment("Negate Energy Consumption", new List<float>() { 0.1f, 0.15f }, "+10% Chance on energy taken to negate 15/30/45% of energy used (Stacks additively)", 2, "Negate Energy Consumption", "Core"),
        new Augment("Random Modifier", new List<float>() {
            (Random.Range(-15, 31) / 100f),
            (Random.Range(-15, 31) / 100f) },
            "-15% to 30% Added bonus stat to Base Regen, and Base Regen Bonus",
            2,
            "Random",
            "Core"),
        new Augment("Core Regen", new List<float>() {0.01f }, "+1% of Max Frame as regen per second", 3, "Core Regen", "Core"),
        new Augment("Ignore Energy Consumption", new List<float>() {0.05f}, "+5% Chance to ignore energy consumption used", 3, "Ignore Energy Consumption", "Core")


    };

    #endregion




    /// <summary>
    /// Rewards the player with 5 random A.C.T (Augment Currency Type)
    /// 
    /// There are Weapon A.C.T, Armour A.C.T, Booster A.C.T, Frame A.C.T, Core A.C.T. Gaining 1 of each grants the player to buy an augment in an appropriate component with the appropriate slot requirement.
    /// 1 augment slot requirement = 1 <component> A.C.T required.
    /// 2 augment slot requirement = 2 A.C.T required.
    /// 3 augment slot requirement = 3 A.C.T required.
    /// </summary>
    public static List<string> RewardAugmentCurrencyType()
    {
        List<string> playersAugmentsCurrency = new List<string>();

        string[] augmentsCurrencyType = new string[] { "Weapon", "Armour", "Booster",
        "Frame", "Core"};

        int numberOfRewards = 5;

        
        bool hasReachedLimit = true;
        while (hasReachedLimit == true)
        {
            Debug.Log("Running while loop");
            // currentNumberOfRewards = 0;
            playersAugmentsCurrency.Clear();

            for (int i = 0; i < numberOfRewards; i++)
            {
                string chosenACT = augmentsCurrencyType[Random.Range(0, augmentsCurrencyType.Length)];
                playersAugmentsCurrency.Add(chosenACT);
            }
            // Depending on the hasReachedLimit => true => means that we have to loop again
            hasReachedLimit = CountACTs(playersAugmentsCurrency);
            


        }

        return playersAugmentsCurrency;

    }

    private static bool CountACTs(List<string> playerACTs)
    {
        bool hasReachedLimit = false;

        int weaponACT = 0;
        int armourACT = 0;
        int boosterACT = 0;
        int frameACT = 0;
        int coreACT = 0;
        // Check each item in the list and count appearance if its greater than 3, set hasReachedLimit to true
        for (int i = 0; i < playerACTs.Count; i++)
        {
            if (playerACTs[i].Equals("Weapon"))
                weaponACT += 1;
            else if (playerACTs[i].Equals("Armour"))
                armourACT += 1;
            else if (playerACTs[i].Equals("Booster"))
                boosterACT += 1;
            else if (playerACTs[i].Equals("Frame"))
                frameACT += 1;
            else if (playerACTs[i].Equals("Core"))
                coreACT += 1;
        }

        if (weaponACT > 3)
            hasReachedLimit = true;
        else if (armourACT > 3)
            hasReachedLimit = true;
        else if (boosterACT > 3)
            hasReachedLimit = true;
        else if (frameACT > 3)
            hasReachedLimit = true;
        else if (coreACT > 3)
            hasReachedLimit = true;

        if (hasReachedLimit == true)
        {
            Debug.LogError("REPEAT RANDOM!!!");
        }

        return hasReachedLimit;
    }



    #region Weapon Augments - Unsused
    // MIGHT NEED TO PUT THESE IN ONE LIST and ADD another var ComponentType to it with value as either its component name or just a number
    // Initializied Augments - just call these to get the augments
    //public readonly static List<Augment> weaponAugments = new List<Augment> {
    //    new Augment("Increase Power", new List<float>() { 100f }, "+100 Power", 1, "Power"),
    //    new Augment("Increase Fire Rate", new List<float>() { 0.1f }, "+10% Fire rate", 1, "Fire Rate"),
    //    new Augment("Reduce Energy Consumption", new List <float>() { 0.13f } , "-13% Energy consumption", 1, "Energy Consumption"),
    //    new Augment("Increase Weight", new List <float>(){ 100f }, "+100 Weight", 1, "Weight"),
    //    new Augment("Decrease Weight", new List<float>(){ -100f }, "-100 Weight", 1, "Weight"),
    //    new Augment("Increase Projectile Count (Spread)", new List<float>() { 1f }, "+1 Projectile count (Spread Type Only)", 2, "Projectile Count Spread"),
    //    new Augment("Increase Projectile Count (Single)", new List<float>() { 0.30f }, "+30% Chance to fire another shot that deals 50% damage (Stacks additively)", 2, "Projectile Count Single"),
    //    new Augment("Increase Frame Damage",  new List<float>() { 0.2f }, "+20% More damage to Frame (Armour destroyed)", 2, "Frame Damage"),
    //    new Augment("Pierce Armour", new List <float>() { 0.2f }, "20% Chance to ignore Armour and directly damage Frame", 2, "Pierce Armour"),
    //    new Augment("Random Modifier", new List<float>() { 
    //        (Random.Range(-15, 31) / 100f), 
    //        (Random.Range(-15, 31) / 100f), 
    //        (Random.Range(-15, 31) / 100f) }, 
    //        "-15% to 30% Added bonus stat to Power, Fire rate, and Energy Consumption", 
    //        3,
    //        "Random"),
    //    new Augment("Critical Strike Chance", new List<float>() { 0.2f, 1.75f }, "+20% Chance to deal 175% damage (chance stacks additively).", 3, "Critical")

    //};
    #endregion

    #region Armour Augments - Unused
    //public readonly static List<Augment> armourAugments = new List<Augment>
    //{
    //    new Augment("Increase Armour", new List<float>(){ 100f}, "+100 Armour", 1, "Armour"),
    //    new Augment("Increase Damage Reduction", new List<float>() { 0.05f }, "+5% Damage Reduction (Stacks additively)", 1, "Damage Reduction"),
    //    new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight"),
    //    new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight"),
    //    new Augment("Reactive Armour", new List<float>() { 100f }, "+20% Chance on damage taken to drop an object that deals 100/200/300 + Max Armour + Damage Reduction % of Max Armour", 2, "Reactive Armour"),
    //    new Augment("Random Modifier", new List<float>() {
    //        (Random.Range(-15, 31) / 100f),
    //        (Random.Range(-15, 31) / 100f) },
    //        "-15% to 30% Added bonus stat to Armour, and Damage Reduction",
    //        2,
    //        "Random"),
    //    new Augment("Ignore Incoming Damage", new List<float>() {0.05f}, "+5% To ignore incoming damage (Stacks additively)", 3, "Ignore Incoming Damage"),
    //    new Augment("Armour Absorb", new List<float>(){ 0.1f, 300f, 10f }, "+10% Chance on damage taken to grant +300 Armour (10 second cooldown) (Stacks additively for both values)", 3, "Armour Absorb"),
    //    new Augment("Armour Regen", new List<float>() {0.005f }, "+0.5% Armour Regen per second (Stacks additively)", 3, "Armour Regen")
    //};
    #endregion

    #region Booster Augments - Unused
    //public readonly static List<Augment> boosterAugments = new List<Augment>
    //{
    //    new Augment("Increasae Base Speed", new List<float>() { 1f }, "+1 Base Speed", 1, "Base Speed"),
    //    new Augment("Decrease Base Speed", new List<float>() {-1f }, "-1 Base Speed", 1, "Base Speed"),
    //    new Augment("Increase Boost Speed", new List<float>() { 0.1f }, "+0.1 Boost Speed", 1, "Boost Speed"),
    //    new Augment("Decrease Boost Speed", new List<float>() {-0.1f}, "-0.1 Boost Speed", 1, "Boost Speed"),
    //    new Augment("Increase Evasion", new List<float>() { 0.05f }, "+5% Evasion", 1, "Evasion"),
    //    new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight"),
    //    new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight"),
    //    new Augment("Reactive Booster", new List<float>() { 1f }, "+20% Chance on dodge boost to drop an object that deals 100/200/300 multiplied by a multiplication of base speed and boost speed (Stacks additively)", 2, "Reactive Booster"),
    //    new Augment("Random Modifier", new List<float>() {
    //        (Random.Range(-15, 31) / 100f),
    //        (Random.Range(-15, 31) / 100f),
    //        (Random.Range(-15, 31) / 100f) },
    //        "-15% to 30% Added bonus stat to Base Speed, Boost Speed, and Evasion",
    //        3,
    //        "Random"),
    //    new Augment("Offensive Booster", new List<float>() { 1f }, "+30% Chance on dodge boost to deal damage around it. Deals 250/350 damage multiplied by a multiplication of base speed and boost speed (Stacks additively)", 3, "Offensive Booster"),

    //};
    #endregion

    #region Frame Augments - Unused
    //public readonly static List<Augment> frameAugments = new List<Augment>
    //{
    //    new Augment("Increase Base Regen", new List<float>() { 1f}, "+1 Base Regen", 1, "Base Regen"),
    //    new Augment("Increase Regen Bonus", new List<float>() { 0.1f }, "+10% Base Regen Bonus", 1, "Base Regen Bonus"),
    //    new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight"),
    //    new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight"),
    //    new Augment("Reactive Frame", new List<float>() {100f, 0.5f}, "+20% Chance on damage taken to drop an object that deals 100/200/300 + 50% of sum of base regen and base regen bonus (Stacks additively)", 2, "Reactive Frame"),
    //    new Augment("Absorb Damage", new List<float>() { 0.1f, 0.15f }, "+10% Chance on damage taken to absorb 15/30/45% of damage received and be healed by the damage absorbed", 2, "Absorb Damage"),
    //    new Augment("Random Modifier", new List<float>() {
    //        (Random.Range(-15, 31) / 100f),
    //        (Random.Range(-15, 31) / 100f) },
    //        "-15% to 30% Added bonus stat to Base Regen, and Base Regen Bonus",
    //        2,
    //        "Random"),
    //    new Augment("Frame Regen", new List<float>() {0.01f }, "+1% of Max Frame as regen per second", 3, "Frame Regen"),
    //    new Augment("Ignore Damage", new List<float>() {0.05f}, "+5% Chance to ignore damage taken", 3, "Ignore Damage")
    //};
    #endregion

    #region Core Augments - Unused
    //public readonly static List<Augment> coreAugments = new List<Augment>
    //{
    //    new Augment("Increase Base Regen", new List<float>() { 1f}, "+1 Base Regen", 1, "Base Regen"),
    //    new Augment("Increase Regen Bonus", new List<float>() { 0.1f }, "+10% Base Regen Bonus", 1, "Base Regen"),
    //    new Augment("Increase Weight", new List<float>() { 100f }, "+100 Weight", 1, "Weight"),
    //    new Augment("Decrease Weight", new List<float>() { -100f }, "-100 Weight", 1, "Weight"),
    //    new Augment("Reactive Core", new List<float>() {100f, 0.5f}, "+20% Chance on damage taken to drop an object that deals 100/200/300 + 50% of sum of base regen and base regen bonus (Stacks additively)", 2, "Reactive Core"),
    //    new Augment("Negate Energy Consumption", new List<float>() { 0.1f, 0.15f }, "+10% Chance on energy taken to negate 15/30/45% of energy used (Stacks additively)", 2, "Negate Energy Consumption"),
    //    new Augment("Random Modifier", new List<float>() {
    //        (Random.Range(-15, 31) / 100f),
    //        (Random.Range(-15, 31) / 100f) },
    //        "-15% to 30% Added bonus stat to Base Regen, and Base Regen Bonus",
    //        2,
    //        "Random"),
    //    new Augment("Core Regen", new List<float>() {0.01f }, "+1% of Max Frame as regen per second", 3, "Core Regen"),
    //    new Augment("Ignore Energy Consumption", new List<float>() {0.05f}, "+5% Chance to ignore energy consumption used", 3, "Ignore Energy Consumption")
    //};
    #endregion
}
