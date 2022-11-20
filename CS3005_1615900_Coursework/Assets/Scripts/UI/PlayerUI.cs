using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Unity Package (N/A) 'TextMeshPro' [Scripting API]. Available at: https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
using TMPro;            // To use the text mesh pro class - 
public class PlayerUI : MonoBehaviour
{
    #region Player properties
    // These are all defined in the editor - drag the components to the respective fields here
    [Header("Health Components")]
    [SerializeField] private Health myHealth;
    [SerializeField] private Image healthImageBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Image healthImageBackgroundBar;
    [Space]
    [Header("Energy Components")]
    [SerializeField] private Energy myEnergy;
    [SerializeField] private Image energyImageBar;
    [SerializeField] private Text energyText;
    [SerializeField] private Image energyImageBackgroundBar;

    // Set by the PlayerArriveLocation - for changing levels
    public void SetMyHealth(Health health) { this.myHealth = health; }
    public void SetMyEnergy(Energy energy) { this.myEnergy = energy; }
    #endregion

    #region Boss properties
    [Header("Boss Health Components")]
    [SerializeField] GameObject bossPanelObj;
    [SerializeField] private Health bossHealth;
    [SerializeField] private Image bossHealthImageBar;
    [SerializeField] private TMP_Text bossHealthText;

    public void SetBossHealth(Health bossHealth) { this.bossHealth = bossHealth; }
    #endregion


    private void Update()
    {
        if (myHealth == null || myEnergy == null) { return; }

        // Update my image's fill amount by my player's health and energy percentages
        float currentHealth = myHealth.GetHealthPercentage();
        float currentEnergy = myEnergy.GetEnergyPercentage();
        healthImageBar.fillAmount = currentHealth;
        energyImageBar.fillAmount = currentEnergy;
        // Same as above but applies on the number text instead and are rounded to nearest int
        healthText.text = Mathf.RoundToInt(myHealth.GetHealth()).ToString();
        energyText.text = Mathf.RoundToInt(myEnergy.GetEnergy()).ToString();

        healthImageBackgroundBar.fillAmount = currentHealth + 0.05f;
        energyImageBackgroundBar.fillAmount = currentEnergy + 0.05f;

        // Stop null reference error
        if (bossPanelObj == null) { return; }

        // if active get the boss' ui
        if (bossPanelObj.activeInHierarchy)
        {
            float bossCurrentHealthPercentage = bossHealth.GetHealthPercentage();
            bossHealthImageBar.fillAmount = bossCurrentHealthPercentage;

            float bossCurrentHealth = bossHealth.GetHealth();

            // Same as above but applies on the number text instead and are rounded to nearest int
            bossHealthText.text = Mathf.RoundToInt(bossCurrentHealth).ToString();
            // Turn of boss health UI on boss' death
            if (bossHealth == null) { SetBossPanelUI(false); }
        }
    }


    public void SetBossPanelUI(bool status)
    {
        bossPanelObj.SetActive(status);
    }

}
