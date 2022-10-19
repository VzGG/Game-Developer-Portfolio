using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image playerHullImage;             // UI hull image circle
    [SerializeField] Image playerEnergyImage;           // UI energy image circle
    [SerializeField] Image playerArmourImage;           // UI armour image circle
    [SerializeField] TMP_Text tmp_text;

    [SerializeField] PlayerController playerController;

    [SerializeField] ProgressionManager progressionManager;

    private void Start()
    {
        // Find progressionManager gameobject and its class at EACH LEVEL (1/2/3)
        progressionManager = FindObjectOfType<ProgressionManager>();
    }


    // Set this during runtime
    public void SetPlayerController(PlayerController player) { this.playerController = player; }
    public PlayerController GetPlayerController() { return this.playerController; }
    // Update is called once per frame
    void Update()
    {
        if (playerController == null) { return; }
        //Debug.Log("Updating UI");

        // Update the hull, and energy bar
        playerHullImage.fillAmount = playerController.GetPlayerShip().GetInstalledFrame().GetCurrentHullPercentage();
        playerEnergyImage.fillAmount = playerController.GetPlayerShip().GetInstalledCore().GetCurrentEnergyPerecentage();
        playerArmourImage.fillAmount = playerController.GetPlayerShip().GetInstalledArmour().GetCurrentArmourPercentage();

        int destroyCount = progressionManager.GetCurrentDestroyed();
        int totalToDestroy = progressionManager.GetNumberOfEnemiesToDestroy();
        // Update the destroy count in the UI
        tmp_text.text = "Destroyed: " + destroyCount.ToString() + "/" + totalToDestroy.ToString();
        
        //playerController.GetPlayerShip().GetInstalledCore().GetCurrentRegenPerecentage();

    }
}
