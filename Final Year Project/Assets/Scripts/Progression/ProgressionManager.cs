using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The purpose of this class to monitor the player's progression in the game (level 1/2/3).
/// 
/// To progress to the higher levels 2 and 3, the player must defeat all enemies that are in the labyrinth rooms (although there can be found outside).
/// 
/// </summary>
public class ProgressionManager : MonoBehaviour
{
    [SerializeField] bool isInstantLevelClearOn = false;            // Turn on in the editor to instant clear enemies in this level 
    [SerializeField] string enemyLevelNumber = "";                  // For instant clear, this is defined in each scenes in the editor

    [SerializeField] int currentDestroyed = 0;
    [SerializeField] int numberOfEnemiesToDestroy = 15;

    [Space]
    [SerializeField] int weaponACTs = 0;                    // Player's weapon augment currency
    [SerializeField] int armourACTs = 0;                    // Player's armour a.. c..
    [SerializeField] int boosterACTs = 0;
    [SerializeField] int frameACTs = 0;
    [SerializeField] int coreACTs = 0;

    bool isRewardScreenOpen = false;
    bool hasPlayerSpentAllACTs = false;

    [Space]
    [SerializeField] List<Augment> chosenAugments = new List<Augment>();

    private void Awake()
    {
        // Singleton and Dont destroy on loading other scenes
        int numberOfProgressionManager = FindObjectsOfType<ProgressionManager>().Length;
        if (numberOfProgressionManager > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);

        ResetCurrentDestroyed();

        

    }

    public int GetWeaponACTs() { return this.weaponACTs; }
    public int GetArmourACTs() { return this.armourACTs; }
    public int GetBoosterACTs() { return this.boosterACTs; }
    public int GetFrameACTs() { return this.frameACTs; }
    public int GetCoreACTs() { return this.coreACTs; }
    public int GetCurrentDestroyed() { return this.currentDestroyed; }
    public int GetNumberOfEnemiesToDestroy() { return this.numberOfEnemiesToDestroy; }
    public void IncreaseCurrentDestroyed() 
    {
        Debug.Log("Increase target count by 1");
        this.currentDestroyed += 1; 
    }
    public void ResetCurrentDestroyed() 
    {
        Debug.Log("Resetting target count");
        this.currentDestroyed = 0; 
    }

    private void Update()
    {
        //Debug.Log("Currently destroyed: " + currentDestroyed + " / " + numberOfEnemiesToDestroy);
        // "esc" button - quit to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetEverything("Main Menu");
        }
        // "i" button - show player status screen
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Show player status
            DisplayPlayerStatus();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            // Hide player status
            HidePlayerStatus();
        }
        // TESTING ONLY - instant clear of that level - tick the bool to true in the Editor/Inspector to grant allowing instant level clear
        // Press "5" to instant level clear
        if (isInstantLevelClearOn)
        {
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                Debug.Log("TESTING ONLY - DELETE THIS CALLER LATER - pressed 5 and now displaying rewards");
                // Remove all enemy gameobjects
                
                // Finds the current enemies in the level
                GameObject enemyLevelContainer = GameObject.Find("ENEMY-LEVEL-" + enemyLevelNumber).gameObject;

                for (int i = 0; i < enemyLevelContainer.transform.childCount; i++)
                {
                    // Check each "ENEMY" gameobject under the container object
                    GameObject enemy = enemyLevelContainer.transform.GetChild(i).gameObject;
                    if (enemy.activeInHierarchy)
                    {
                        // Remove in the game to instant clear level
                        Destroy(enemy);
                        this.currentDestroyed = 15;
                    }
                }
                // Set the total destroyed to 15 to proceed to next level
                this.currentDestroyed = 15;
            }
        }
        // Check if 15 enemies are destroyed - if so make isRewardScreenOpen to true and grant the player augments
        HasTargetReached();
        // Have player choose augments
        if (isRewardScreenOpen == true)
        {
            // PAUSE THE GAME
            PauseTime();
            // Turn on the Reward Screen
            GameObject mainCanvasGameObj = GameObject.Find("Main Canvas");
            GameObject rewardScreenGameObj = mainCanvasGameObj.transform.GetChild(5).gameObject;
            rewardScreenGameObj.SetActive(true);
            // Update the player's currency in all types
            DisplayPlayerAugmentCurrencyType(rewardScreenGameObj, this.weaponACTs, this.armourACTs, this.boosterACTs, this.frameACTs,
                this.coreACTs);
            // Also update the player's current augment slots
            DisplayPlayerCurrentAugmentSlotsAttached(rewardScreenGameObj);

            // Checks player current ACTs and changes the "hasPlayerSpentAllACTs" bool to true if player has spent them all
            CheckPlayerACTs();

            if (hasPlayerSpentAllACTs == true)
            {
                // THEN close the screen
                // AND Reset all bool that was changed
                Debug.Log("Closing Screen!!!");
                isRewardScreenOpen = false;
                rewardScreenGameObj.SetActive(false);
                hasPlayerSpentAllACTs = false;

                // Unpause the game
                ContinueTime();

                // Then Re-Apply augments to Weapon, Armour, Booster, Frame, Core of the Player
                FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledWeapons()[0].ApplyAugments();
                FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledArmour().ApplyAugments();
                FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledBooster().ApplyAugments();
                FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledFrame().ApplyAugments();
                FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledCore().ApplyAugments();

                // Then go to the next level
                FindObjectOfType<LevelManager>().LoadNextScene();
            }
            

        }
    }

    public void HasTargetReached()
    {
        if (currentDestroyed >= 15)
        {
            Debug.Log("ProgressionManager: loading next scene");
            // Reset AND Move to the next level!
            ResetCurrentDestroyed();            // Has to be at 0 because if it stays at 15 when it gets to 15, it will cause this method HasTargetReached() to run over and over

            // CHECK IF we next level scene is Level 2 and 3, otherwise, do not open the reward screen and go to the next level
            if (FindObjectOfType<LevelManager>().GetLevelName() == "Level 2" || 
                FindObjectOfType<LevelManager>().GetLevelName() == "Level 3")
            {
                // Reward player with augments currency type (A.C.T)
                List<string> playerACTs = AugmentLibrary.RewardAugmentCurrencyType();
                // Convert the player's currency string to integer
                this.weaponACTs = this.GetHowManyASingleTypeOfACT(playerACTs, "Weapon");
                this.armourACTs = this.GetHowManyASingleTypeOfACT(playerACTs, "Armour");
                this.boosterACTs = this.GetHowManyASingleTypeOfACT(playerACTs, "Booster");
                this.frameACTs = this.GetHowManyASingleTypeOfACT(playerACTs, "Frame");
                this.coreACTs = this.GetHowManyASingleTypeOfACT(playerACTs, "Core");

                // Turn on the Reward Screen
                isRewardScreenOpen = true;
            }
            else
            {
                // Otherwise SKIP open reward screen and go to next level (Main menu) AND DESTROY ALL DON'T DESTROY ON LOAD including this

                // Destroy all enemies in the dont destroy section

                ResetEverything(FindObjectOfType<LevelManager>().GetLevelName());
            } 

        }
    }

    public void ResetEverything(string sceneName)
    {
        // Return to normal time scale
        Time.timeScale = 1f;
        isRewardScreenOpen = false;
        hasPlayerSpentAllACTs = false;

        FindObjectOfType<EnemyOnLoad>().DestroyAllEnemiesAndSelf();
        // Destroy player controller to reset
        Destroy(FindObjectOfType<PlayerController>().gameObject.transform.parent.gameObject);
        // Destroy all projectiles (player and enemy owners)
        Projectile[] allProjectilesGameObj = FindObjectsOfType<Projectile>();
        for (int i = 0; i < allProjectilesGameObj.Length; i++)
        {
            Destroy(allProjectilesGameObj[i].gameObject);
        }
        // Load next scene - this means we go to Main Menu or something else not level 1, 2 or 3
        FindObjectOfType<LevelManager>().LoadScene(sceneName);

        Time.timeScale = 1f;
        // Then destroy THIS gameobject
        Destroy(this.gameObject);

    }

    
    private void DisplayPlayerStatus()
    {
        GameObject mainCanvasGameObj = FindObjectOfType<MainCanvas>().gameObject;
        GameObject playerStatsGameObj = mainCanvasGameObj.transform.GetChild(6).gameObject;

        playerStatsGameObj.SetActive(true);

        // Pause time?

        // Display the stats
        PlayerController playerController = FindObjectOfType<PlayerController>();

        // Get the "Player Component" gameobject
        GameObject playerComponentGameObj = playerStatsGameObj.transform.GetChild(0).gameObject;
        GameObject weaponStatsGameObj = playerComponentGameObj.transform.GetChild(0).gameObject;
            // Sets the name of the weapon name status
        weaponStatsGameObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetName();
        // Sets the value of the weapon power status
        weaponStatsGameObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =
            "POWER: " +
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetPower().ToString();

        weaponStatsGameObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text =
            "FIRE RATE: " +
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetFireRate().ToString();

        weaponStatsGameObj.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text =
            "CONSUMPTION: " +
            (playerController.GetPlayerShip().GetInstalledWeapons()[0].GetEnergyConsumption() * 10f).ToString() +
            "%";
        // Gets the Type number of the weapon and displays it
        string bonusText = "";
        int weaponType = (int) playerController.GetPlayerShip().GetInstalledWeapons()[0].GetValues()[3];
        if (weaponType == 0)
        {
            bonusText = " (NON HOMING PROJECTILE - DEALS 100% MORE DAMAGE)";
        }
        weaponStatsGameObj.transform.GetChild(5).gameObject.GetComponent<TMP_Text>().text =
            "TYPE: " + 
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetValues()[3].ToString()
            + bonusText;


        // For armour
        GameObject armourStatsGameObj = playerComponentGameObj.transform.GetChild(1).gameObject;
            // Display name
        armourStatsGameObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledArmour().GetName();
            // Display defense
        armourStatsGameObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =
            "DEFENSE: " +
            playerController.GetPlayerShip().GetInstalledArmour().GetMaxArmour().ToString();

        armourStatsGameObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text =
            "DAMAGE REDUCTION: " +
            (playerController.GetPlayerShip().GetInstalledArmour().GetCurrentDamageReduction() * 100f).ToString() +
            "%";

        // For evasion
        GameObject boosterStatsGameObj = playerComponentGameObj.transform.GetChild(2).gameObject;

        boosterStatsGameObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledBooster().GetName();

        boosterStatsGameObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =
            "BOOST SPEED: " +
            playerController.GetPlayerShip().GetInstalledBooster().GetBoostSpeed().ToString() +
            "x";

        boosterStatsGameObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text =
            "EVASION: " +
            (playerController.GetPlayerShip().GetInstalledBooster().GetEvasion() * 100f).ToString() +
            "%";

        boosterStatsGameObj.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text =
            "NORMAL SPEED: " +
            playerController.GetPlayerShip().GetInstalledBooster().GetNormalSpeed().ToString();

        // For Frame(HP)
        GameObject frameStatsGameObj = playerComponentGameObj.transform.GetChild(3).gameObject;
        frameStatsGameObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledFrame().GetName();

        frameStatsGameObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =
            "FRAME (HP): " +
            playerController.GetPlayerShip().GetInstalledFrame().GetMaxHull().ToString();

        frameStatsGameObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text =
            "FRAME (HP) REGEN: " +
            playerController.GetPlayerShip().GetInstalledFrame().GetCurrentHullRegen().ToString();

        // For Core(MP)
        GameObject coreStatsGameObj = playerComponentGameObj.transform.GetChild(4).gameObject;
        coreStatsGameObj.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledCore().GetName();

        coreStatsGameObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text =
            "CORE (MP): " +
            playerController.GetPlayerShip().GetInstalledCore().GetMaxEnergy().ToString();

        coreStatsGameObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text =
            "CORE (MP) REGEN: " +
            playerController.GetPlayerShip().GetInstalledCore().GetEnergyRegen().ToString();

    }

    private void HidePlayerStatus()
    {
        GameObject mainCanvasGameObj = FindObjectOfType<MainCanvas>().gameObject;
        GameObject playerStatsGameObj = mainCanvasGameObj.transform.GetChild(6).gameObject;

        playerStatsGameObj.SetActive(false);

        // Unpause time?
    }

    #region Augment Reward System

    /// <summary>
    /// Attached to the BUTTONS!!! DECIDES if we can add it to the the list of chosen augments
    /// </summary>
    /// <param name="index"></param>
    public void AddAugmentToChosenAugments(Augment toAddAugment)
    {
        // Get currency if we have enough
        int currencyRequired = toAddAugment.GetSlotTaken();
        //Debug.Log("Required currency: " + currencyRequired);

        string notEnoughCurrency = "Not enough currency. Cannot buy it";
        string componentType = toAddAugment.GetComponentType();
        // Deduct your current currency and add to your augment
        if (componentType == "Weapon")
        {
            if (this.weaponACTs - currencyRequired < 0)
            {
                Debug.Log(notEnoughCurrency);
                return;
            }
            // Take away from weaponACT currency
            this.weaponACTs -= currencyRequired;

            // Add to the player's weapon augment
            FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledWeapons()[0].AddAugment(toAddAugment);
        }
        else if (componentType == "Armour")
        {
            if (this.armourACTs - currencyRequired < 0) { Debug.Log(notEnoughCurrency); return; }
            this.armourACTs -= currencyRequired;

            // Add armour augment to player
            FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledArmour().AddAugment(toAddAugment);
        }
        else if (componentType == "Booster")
        {
            if (this.boosterACTs - currencyRequired < 0) { Debug.Log(notEnoughCurrency); return; }
            this.boosterACTs -= currencyRequired;

            FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledBooster().AddAugment(toAddAugment);
        }
        else if (componentType == "Frame")
        {
            if (this.frameACTs - currencyRequired < 0) { Debug.Log(notEnoughCurrency); return; }
            this.frameACTs -= currencyRequired;

            FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledFrame().AddAugment(toAddAugment);
        }
        else if (componentType == "Core")
        {
            if (this.coreACTs - currencyRequired < 0) { Debug.Log(notEnoughCurrency); return; }
            this.coreACTs -= currencyRequired;

            FindObjectOfType<PlayerController>().GetPlayerShip().GetInstalledCore().AddAugment(toAddAugment);
        }
        

        this.chosenAugments.Add(toAddAugment);
        // After adding to the list, now add this to the player -> look at the if statements where we add it to the player
       
    }

    private void DisplayPlayerAugmentCurrencyType(GameObject rewardScreenGameObj, int wACT, int aACT, int bACT,
        int fACT, int cACT)
    {
        // Gets the A.C.T gameobject under BackgroundImage of the MainCanvas prefab
        GameObject ACTGameObj = rewardScreenGameObj.transform.GetChild(0).gameObject;
        for (int i = 0; i < ACTGameObj.transform.childCount; i++)
        {

            GameObject textGameObj = ACTGameObj.transform.GetChild(i).transform.GetChild(1).gameObject;
            TMP_Text textComponent = textGameObj.GetComponent<TMP_Text>();

            string wepACTs = "Weapon";
            string armACTs = "Armour";
            string bstACTs = "Booster";
            string frmACTs = "Frame";
            string creACTs = "Core";

            // Display player's augment currency types (A.C.T)
            if (textGameObj.name.StartsWith(wepACTs))
                textComponent.text = wACT.ToString();
            else if (textGameObj.name.StartsWith(armACTs))
                textComponent.text = aACT.ToString();
            else if (textGameObj.name.StartsWith(bstACTs))
                textComponent.text = bACT.ToString();
            else if (textGameObj.name.StartsWith(frmACTs))
                textComponent.text = fACT.ToString();
            else if (textGameObj.name.StartsWith(creACTs))
                textComponent.text = cACT.ToString();


        }
    }

    private int GetHowManyASingleTypeOfACT(List<string> playerACTs, string actType)
    {
        // Augment Currency Type
        int numberOfSingleTypeACT = 0;

        for (int i = 0; i < playerACTs.Count; i++)
        {
            if (actType.Equals(playerACTs[i]))
            {
                //Debug.Log("They are equal. Add 1 to ACT count");
                numberOfSingleTypeACT += 1;
            }
        }

        return numberOfSingleTypeACT;

    }

    private void DisplayPlayerCurrentAugmentSlotsAttached(GameObject rewardScreenGameObj)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        // Get "Augment Slots" Gameobject under the Background Image under the Main Canvas prefab game object
        GameObject augmentSlotsGameObj = rewardScreenGameObj.transform.GetChild(7).gameObject;

        // Display player's current augment slot taken for WEAPON
        GameObject weaponAugSlotGameObjText = augmentSlotsGameObj.transform.GetChild(1).transform.GetChild(1).gameObject;
        weaponAugSlotGameObjText.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetCurrentAugmentSlots().ToString() +
            "/" +
            playerController.GetPlayerShip().GetInstalledWeapons()[0].GetMaxAugmentSlots().ToString();


        // Display player's current augment slot taken for ARMOUR
        GameObject armourAugSlotGameObjText = augmentSlotsGameObj.transform.GetChild(2).transform.GetChild(1).gameObject;
        armourAugSlotGameObjText.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledArmour().GetCurrentAugmentSlots().ToString() +
            "/" +
            playerController.GetPlayerShip().GetInstalledArmour().GetMaxAugmentSlots().ToString();


        GameObject boosterAugSlotGameObjText = augmentSlotsGameObj.transform.GetChild(3).transform.GetChild(1).gameObject;
        boosterAugSlotGameObjText.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledBooster().GetCurrentAugmentSlots().ToString() +
            "/" +
            playerController.GetPlayerShip().GetInstalledBooster().GetMaxAugmentSlots().ToString();


        GameObject frameAugSlotGameObjText = augmentSlotsGameObj.transform.GetChild(4).transform.GetChild(1).gameObject;
        frameAugSlotGameObjText.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledFrame().GetCurrentAugmentSlots().ToString() +
            "/" +
            playerController.GetPlayerShip().GetInstalledFrame().GetMaxAugmentSlots().ToString();


        GameObject coreAugSlotGameObjText = augmentSlotsGameObj.transform.GetChild(5).transform.GetChild(1).gameObject;
        coreAugSlotGameObjText.GetComponent<TMP_Text>().text =
            playerController.GetPlayerShip().GetInstalledCore().GetCurrentAugmentSlots().ToString() +
            "/" +
            playerController.GetPlayerShip().GetInstalledCore().GetMaxAugmentSlots().ToString();


    }

    private void CheckPlayerACTs()
    {
        List<bool> areAllACTsEmpty = new List<bool>();

        if (this.weaponACTs > 0)
            areAllACTsEmpty.Add(true);

        if (this.armourACTs > 0)
            areAllACTsEmpty.Add(true);

        if (this.boosterACTs > 0)
            areAllACTsEmpty.Add(true);

        if (this.frameACTs > 0)
            areAllACTsEmpty.Add(true);

        if (this.coreACTs > 0)
            areAllACTsEmpty.Add(true);

        // Now check if there is at least 1 true in the list if so, all our ACTs is not empty yet
        // We know that if the list has more than 1 count then we have not spent all our acts yet
        if (areAllACTsEmpty.Count > 0)
        {
            this.hasPlayerSpentAllACTs = false;
        }
        else if (areAllACTsEmpty.Count <= 0)
        {
            this.hasPlayerSpentAllACTs = true;
        }

    }

    private void PauseTime()
    {
        Time.timeScale = 0f;
    }

    private void ContinueTime()
    {
        Time.timeScale = 1f;
    }

    #endregion


}
