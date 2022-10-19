using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Follows the targeted enemy and displays the enemy's status in run time
/// </summary>
public class EnemyUI : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
 
    public void SetEnemyController(EnemyController enemyController) { this.enemyController = enemyController; }

    [SerializeField] private List<Image> enemyHullBar;
    [SerializeField] private List<Image> enemyArmourBar;

    private void Update()
    {
        if (enemyController == null) { return; }
        //Debug.Log("Updating enemy UI");

        // The hp and armour bar of enemy follows the player
        this.gameObject.transform.position = enemyController.transform.position;

        // After following, update the hp and armour bar by getting the enemy controller's ship components
        EnemyVitalityBarUpdate();

        // Update enemy ui bar
        float currentEnergy = this.enemyController.GetEnemyShip().GetInstalledCore().GetCurrentEnergyPerecentage();
        this.gameObject.transform.GetChild(0).transform.GetChild(3).gameObject.GetComponent<Image>().fillAmount =
            currentEnergy;


        if (enemyController.GetEnemyShip().GetInstalledFrame().GetCurrentHullPercentage() <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void ActivateVitalityBar()
    {
        GameObject hullBar = this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;

        for (int i = 0; i < hullBar.transform.childCount; i++)
        {
            // Get the child object of each "Image" gameobject that is under of "hullBar" gameobject
            GameObject currentHullObj = hullBar.transform.GetChild(i).transform.GetChild(0).gameObject;
            // Get image of the currentHull gameobject attached to it
            Image currentHullImage = currentHullObj.GetComponent<Image>();

            enemyHullBar.Add(currentHullImage);
        }


        GameObject armourBar = this.gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        //Debug.Log("gameobj name: " + armourBar.name);
        for (int i = 0; i < armourBar.transform.childCount; i++)
        {
            // Get the child object of each "Image" gameobject that is under of "hullBar" gameobject
            GameObject currentArmourObj = armourBar.transform.GetChild(i).transform.GetChild(0).gameObject;
            // Get image of the currentHull gameobject attached to it
            Image currentArmourImage = currentArmourObj.GetComponent<Image>();

            enemyArmourBar.Add(currentArmourImage);
        }


    }

    private void EnemyVitalityBarUpdate()
    {
        float enemyHealthPercentage = enemyController.GetEnemyShip().GetInstalledFrame().GetCurrentHullPercentage();

        if (enemyHealthPercentage >= 1.0f)
            EnemyHullBarFillAmount(enemyHullBar, 1, 1, 1, 1, 1);    // 5 bar = 100%
        else if (enemyHealthPercentage > 0.6f && enemyHealthPercentage <= 0.8f)
            EnemyHullBarFillAmount(enemyHullBar, 0, 1, 1, 1, 1);    // 4 bar = 80%
        else if (enemyHealthPercentage > 0.4f && enemyHealthPercentage <= 0.6)
            EnemyHullBarFillAmount(enemyHullBar, 0, 0, 1, 1, 1);    // 3 bar = 60%
        else if (enemyHealthPercentage > 0.2f && enemyHealthPercentage <= 0.4f)
            EnemyHullBarFillAmount(enemyHullBar, 0, 0, 0, 1, 1);    // 2 bar = 40%
        else if (enemyHealthPercentage > 0f && enemyHealthPercentage <= 0.2f)
            EnemyHullBarFillAmount(enemyHullBar, 0, 0, 0, 0, 1);    // 1 bar = 20%
        else if (enemyHealthPercentage <= 0f)               
            EnemyHullBarFillAmount(enemyHullBar, 0, 0, 0, 0, 0);    // 0 bar = 0%
        /*
         * 00000 = 100%     -> 5 bar    -> 100% and above 
         * O0000 = 80%      -> 4 bar    -> reaches 80-61
         * OO000 = 60%      -> 3 bar    -> reaches 60-41
         * OOO00 = 40%      -> 2 bar    -> reaches 40-21
         * OOOO0 = 20%      -> 1 bar    -> reaches 20-1
         * OOOOO = 0%       -> 0 bar    -> reaches 0
         */

  

        // For enemy armour bar
        float enemyArmourPercentage = enemyController.GetEnemyShip().GetInstalledArmour().GetCurrentArmourPercentage();

        if (enemyArmourPercentage >= 1.0f)
            EnemyHullBarFillAmount(enemyArmourBar, 1, 1, 1, 1, 1);    // 5 bar = 100%
        else if (enemyArmourPercentage > 0.6f && enemyArmourPercentage <= 0.8f)
            EnemyHullBarFillAmount(enemyArmourBar, 0, 1, 1, 1, 1);    // 4 bar = 80%
        else if (enemyArmourPercentage > 0.4f && enemyArmourPercentage <= 0.6)
            EnemyHullBarFillAmount(enemyArmourBar, 0, 0, 1, 1, 1);    // 3 bar = 60%
        else if (enemyArmourPercentage > 0.2f && enemyArmourPercentage <= 0.4f)
            EnemyHullBarFillAmount(enemyArmourBar, 0, 0, 0, 1, 1);    // 2 bar = 40%
        else if (enemyArmourPercentage > 0f && enemyArmourPercentage <= 0.2f)
            EnemyHullBarFillAmount(enemyArmourBar, 0, 0, 0, 0, 1);    // 1 bar = 20%
        else if (enemyArmourPercentage <= 0f)
            EnemyHullBarFillAmount(enemyArmourBar, 0, 0, 0, 0, 0);    // 0 bar = 0%

    }

    private void EnemyHullBarFillAmount(List<Image> enemyBar, int fifthBar, int fourthBar, int thirdBar, int secondBar, int firstBar)
    {
        if (enemyBar.Count <= 0) { Debug.Log("enemyBar image list is empty");  return; }

        enemyBar[4].fillAmount = fifthBar;
        enemyBar[3].fillAmount = fourthBar;
        enemyBar[2].fillAmount = thirdBar;
        enemyBar[1].fillAmount = secondBar;
        enemyBar[0].fillAmount = firstBar;
    }


}
