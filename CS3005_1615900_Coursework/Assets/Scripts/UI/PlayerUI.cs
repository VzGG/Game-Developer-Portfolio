using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Unity Package (N/A) 'TextMeshPro' [Scripting API]. Available at: https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/manual/index.html
using TMPro;            // To use the text mesh pro class - 
using Oswald.Player;
using Oswald.Manager;

namespace Oswald.UI
{
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

        [Header("Armour Components")]
        [SerializeField] private Armour _armour;
        [SerializeField] private Image _armourImageBar;
        [SerializeField] private Text _armourText;
        [SerializeField] private Image _armourImageBackgroundBar;

        [Header("Skill Components")]
        [SerializeField] private Image _skillImage1;
        [SerializeField] private Image _skillImage1Cooldown;
        [SerializeField] private PlayerAttack playerAttack;
        [SerializeField] private Image _skillImage2;
        [SerializeField] private Image _skillImage2Cooldown;

        [Header("Combo Components")]
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private Text _comboList;
        [SerializeField] private Text _comboParentName;
        [SerializeField] private GameObject _comboPanel;
        private bool _isActive = false;

        private float barOffset = 0.05f;

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
            if (timeManager.GetIsTimeStopped()) { return; }

            if (myHealth == null || myEnergy == null) { return; }

            // Update my image's fill amount by my player's health and energy percentages
            float currentHealth = myHealth.GetHealthPercentage();
            float currentEnergy = myEnergy.GetEnergyPercentage();
            float currentArmour = _armour.GetArmourPercentage();

            healthImageBar.fillAmount = currentHealth;
            energyImageBar.fillAmount = currentEnergy;
            _armourImageBar.fillAmount = currentArmour;

            // Same as above but applies on the number text instead and are rounded to nearest int
            healthText.text = Mathf.RoundToInt(myHealth.GetHealth()).ToString();
            energyText.text = Mathf.RoundToInt(myEnergy.GetEnergy()).ToString();
            _armourText.text = Mathf.RoundToInt(_armour.GetArmour()).ToString();

            healthImageBackgroundBar.fillAmount = currentHealth + barOffset;
            energyImageBackgroundBar.fillAmount = currentEnergy + barOffset;
            _armourImageBackgroundBar.fillAmount = currentArmour + barOffset;

            // Show skill 1's cooldown when on use.
            _skillImage1.sprite = playerAttack.skills[0].GetSkillIcon();
            _skillImage1Cooldown.fillAmount = (playerAttack.skills[0].GetTimer() / playerAttack.skills[0].GetCooldown() );
            // Show skill 2 cooldown
            _skillImage2.sprite = playerAttack.skills[1].GetSkillIcon();
            _skillImage2Cooldown.fillAmount = (playerAttack.skills[1].GetTimer() / playerAttack.skills[1].GetCooldown() );

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Display the player's currently equipped text.
                PlayerAttack playerAttack = myHealth.GetComponent<PlayerAttack>();
                string airCombo = playerAttack.MyWeapon.AirComboList;
                string groundedCombo = playerAttack.MyWeapon.GroundedComboList;
                _comboList.text = airCombo + "\n\n" + groundedCombo;
                // Show combo UI - use this "!" for the bool display.
                // Initial value is false - so reverse it means it will be true.
                _isActive = !_isActive;     
                _comboPanel.SetActive(_isActive);
            }


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

}

