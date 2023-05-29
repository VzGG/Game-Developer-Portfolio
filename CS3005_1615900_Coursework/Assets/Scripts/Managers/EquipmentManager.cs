using Oswald.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private GameObject _equipmentPanel;

    #region Chosen Equipment Components

    [SerializeField] private GameObject _selectedEquipment;
    [SerializeField] private GameObject _removeEquipmentIcon;
    [SerializeField] private Text _statMetaText;
    [SerializeField] private Text _statText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _ratingText;
    [SerializeField] private Text _descriptionText;

    #endregion

    public Oswald.Player.PlayerController PlayerController { get; set; } = null;
    private int _equimentSelected = -1;
    private int EquipmentSelected 
    {
        get
        {
            return _equimentSelected;
        }
        set 
        {
            _equimentSelected = value;

            if (EquipmentSelected < 0)
                _equimentSelected = _equipmentSlots.Length - 1;
            else if (EquipmentSelected > _equipmentSlots.Length - 1)
                _equimentSelected = 0;
        }
     }

    #region Stats
    [Space]
    [SerializeField] private GameObject _hpStat;
    [SerializeField] private GameObject _enStat;
    [SerializeField] private GameObject _atkStat;
    [SerializeField] private GameObject _defStat;
    [SerializeField] private GameObject _enrgnStat;
    [SerializeField] private GameObject _atkspdStat;

    [SerializeField] private GameObject _hpPercentStat;
    [SerializeField] private GameObject _enPercentStat;
    [SerializeField] private GameObject _atkPercentStat;
    [SerializeField] private GameObject _defPercentStat;
    [SerializeField] private GameObject _enrgnPercentStat;
    [SerializeField] private GameObject _atkspdPercentStat;

    [SerializeField] private GameObject _healthregenStat;
    [SerializeField] private GameObject _damagereductionStat;
    [SerializeField] private GameObject _criticalchanceStat;
    [SerializeField] private GameObject _criticaldamageStat;
    [SerializeField] private GameObject _evasionrateStat;
    #endregion

    #region Equipment Slots

    [SerializeField] private GameObject[] _equipmentSlots = new GameObject[5];

    #endregion

    #region Actual Max HP EN AMR

    [SerializeField] private Text _maxHealthText;
    [SerializeField] private Text _maxEnergyText;
    [SerializeField] private Text _maxArmourText;

    #endregion

    private void Update()
    {
        if (PlayerController == null) { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowOrHideEquipment();
        }

        // Do not allow any UI controls when time is not stopped
        if (_timeManager.GetIsTimeStopped() == false) { return; }

        SelectingEquipment();
    }

    public void UpdateEquipmentUI(MyStat myStat, MyEquipment myEquipment)
    {
        UpdateAllUIStat(myStat);
        UpdateEquipmentSlot(myEquipment);
    }

    private void UpdateAllUIStat(MyStat myStat)
    {
        UpdateUIStat(_hpStat, nameof(myStat.HP).ToString(), myStat.HP.ToString());
        UpdateUIStat(_enStat, nameof(myStat.EN).ToString(), myStat.EN.ToString());
        UpdateUIStat(_atkStat, nameof(myStat.ATK).ToString(), myStat.ATK.ToString());
        UpdateUIStat(_defStat, nameof(myStat.DEF).ToString(), myStat.DEF.ToString());
        UpdateUIStat(_enrgnStat, nameof(myStat.ENRGN).ToString(), myStat.ENRGN.ToString());
        UpdateUIStat(_atkspdStat, nameof(myStat.ATKSPD).ToString(), myStat.ATKSPD.ToString());

        UpdateUIStat(_hpPercentStat, nameof(myStat.HPPercent).ToString(), myStat.HPPercent.ToString() + "%");
        UpdateUIStat(_enPercentStat, nameof(myStat.ENPercent).ToString(), myStat.ENPercent.ToString() + "%");
        UpdateUIStat(_atkPercentStat, nameof(myStat.ATKPercent).ToString(), myStat.ATKPercent.ToString() + "%");
        UpdateUIStat(_defPercentStat, nameof(myStat.DEFPercent).ToString(), myStat.DEFPercent.ToString() + "%");
        UpdateUIStat(_enrgnPercentStat, nameof(myStat.ENRGNPercent).ToString(), myStat.ENRGNPercent.ToString() + "%");
        UpdateUIStat(_atkspdPercentStat, nameof(myStat.ATKSPDPercent).ToString(), myStat.ATKSPDPercent.ToString() + "%");

        UpdateUIStat(_healthregenStat, nameof(myStat.HealthRegen).ToString(), myStat.HealthRegen.ToString());
        UpdateUIStat(_damagereductionStat, nameof(myStat.DamageReduction).ToString(), myStat.DisplayDamageReduction().ToString() + "%");
        UpdateUIStat(_criticalchanceStat, nameof(myStat.CriticalChance).ToString(), myStat.DisplayCriticalChance().ToString() + "%");
        UpdateUIStat(_criticaldamageStat, nameof(myStat.CriticalDamage).ToString(), (myStat.CriticalDamage * 100f).ToString() + "%");
        UpdateUIStat(_evasionrateStat, nameof(myStat.EvasionRate).ToString(), myStat.DisplyEvasionRate().ToString() + "%");
    }

    private void UpdateUIStat(GameObject statGameObj, string metaValue, string statValue)
    {
        Text statTextComponent = statGameObj.transform.GetChild(1).gameObject.GetComponent<Text>();
        statTextComponent.text = statValue;
    }

    private void UpdateEquipmentSlot(MyEquipment myEquipment)
    {
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            Debug.Log("Equipment is empty, unable to update the UI!");
            Image image = _equipmentSlots[i].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();

            if (i >= myEquipment.myEquipment.Count)
            {
                image.sprite = null;
                image.color = new Color(0, 0, 0, 0);
                continue;
            }

            Sprite equipmentSprite = myEquipment.myEquipment[i].sprite;

            image.sprite = equipmentSprite;
            image.color = Color.white;
        }
    }

    private void SelectingEquipment()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            // Move left
            SelectLeft();
            ShowCurrenteEquipmentSelected();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            // Move right
            SelectRight();
            ShowCurrenteEquipmentSelected();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // Remove equipment
            PlayerController.GetMyEquipment().RemoveEquipment(_equimentSelected, PlayerController, _timeManager);
            UpdateEquipmentUI(PlayerController.GetMyStat(), PlayerController.GetMyEquipment());
            ShowCurrenteEquipmentSelected();
        }
    }

    private void ShowOrHideEquipment()
    {
        EquipmentSelected = -1;
        HideAllEquipmentSelected();

        _equipmentPanel.SetActive(!_equipmentPanel.activeInHierarchy);
        UpdateEquipmentUI(PlayerController.GetMyStat(), PlayerController.GetMyEquipment());

        // Show max hp, en, amr
        UpdateActualMaxStat(_maxHealthText, Mathf.RoundToInt(PlayerController.GetHealth().GetMaxHealth()).ToString());
        UpdateActualMaxStat(_maxEnergyText, Mathf.RoundToInt(PlayerController.GetEnergy().GetMaxEnergy()).ToString());
        UpdateActualMaxStat(_maxArmourText, Mathf.RoundToInt(PlayerController.GetArmour().GetMaxArmour()).ToString());

        _timeManager.ChangeIsTimeStopped();
    }

    private void UpdateActualMaxStat(Text textComponent, string statValue)
    {
        textComponent.text = statValue;
    }

    #region Selection methods

    private void SelectLeft()
    {
        EquipmentSelected--;
    }

    private void SelectRight()
    {
        EquipmentSelected++;
    }

    private void ShowCurrenteEquipmentSelected()
    {
        HideAllEquipmentSelected();
        _equipmentSlots[EquipmentSelected].GetComponent<Image>().enabled = true;

        ShowEquipmentDetails();
    }

    private void HideAllEquipmentSelected()
    {
        foreach (GameObject equipmentGameObj in _equipmentSlots)
        {
            equipmentGameObj.GetComponent<Image>().enabled = false;
        }
        HideEquimentDetails();
    }

    private void ShowEquipmentDetails()
    {
        _selectedEquipment.SetActive(true);
        _removeEquipmentIcon.SetActive(true);

        MyEquipment equipmentComponent = this.PlayerController.GetMyEquipment();

        // Do not show any equipment details when there is no equipment in that index
        if (EquipmentSelected >= equipmentComponent.myEquipment.Count) 
        {
            HideEquimentDetails();
            return;
        };

        Equipment equipment = equipmentComponent.myEquipment[EquipmentSelected];

        _statMetaText.text = "";
        _statText.text = "";

        for (int i = 0; i < equipment.stats.Count; i++)
        {
            _statMetaText.text += $"{equipment.stats[i].GetName()}\n";

            if (i == 3)
                _statText.text += $"{equipment.stats[i].Value}%\n";
            else
                _statText.text += $"{equipment.stats[i].Value}\n";
        }

        _nameText.text = $"{equipment.name}";                            
        _nameText.color = equipment._nameTextColour;
        _ratingText.text = $"{Mathf.RoundToInt(equipment.rating)}";

        _descriptionText.text = equipment._description;
    }

    private void HideEquimentDetails()
    {
        _selectedEquipment.SetActive(false);
        _removeEquipmentIcon.SetActive(false);
    }

    #endregion
}