using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEquipment : MonoBehaviour
{
    [SerializeField] public List<Equipment> myEquipment;

    private const int _equipmentLimit = 5;

    #region Adding Equipment

    public bool AddEquipment(Equipment equipment, Oswald.Player.PlayerController playerController)
    {
        if (myEquipment.Count > (_equipmentLimit - 1) ) { return false; }

        // Might be good to have Array instead of list?
        myEquipment.Add(equipment);
        ApplyStat(playerController.GetMyStat(), equipment);
        UpdateStat(playerController.GetMyStat(),
            playerController.gameObject.GetComponent<Health>(),
            playerController.gameObject.GetComponent<Energy>(),
            playerController.gameObject.GetComponent<Armour>(),
            playerController.gameObject.GetComponent<Oswald.Player.PlayerAttack>());

        equipment.gameObject.SetActive(false);

        return true;
    }

    /// <summary>
    /// Applies stats positively
    /// </summary>
    /// <param name="myStat"></param>
    /// <param name="equipment"></param>
    private void ApplyStat(MyStat myStat, Equipment equipment)
    {
        int numberOfStats = equipment.stats.Count;

        // An equipment has N amount of stats, apply those to the character stat
        for (int i = 0; i < numberOfStats; i++)
        {
            Stat stat = equipment.stats[i];

            if (i == 3)
            {
                // Apply Epic stat
                if (stat.GetType() == typeof(HP))
                    myStat.HPPercent += stat.Value;
                else if (stat.GetType() == typeof(EN))
                    myStat.ENPercent += stat.Value;
                else if (stat.GetType() == typeof(ATK))
                    myStat.ATKPercent += stat.Value;
                else if (stat.GetType() == typeof(DEF))
                    myStat.DEFPercent += stat.Value;
                else if (stat.GetType() == typeof(ENRGN))
                    myStat.ENRGNPercent += stat.Value;
                else if (stat.GetType() == typeof(ATKSPD))
                    myStat.ATKSPDPercent += stat.Value;

                continue;
            }


            if (stat.GetType() == typeof(HP))
            {
                myStat.HP += stat.Value;
            }
            else if (stat.GetType() == typeof(EN))
            {
                myStat.EN += stat.Value;
            }
            else if (stat.GetType() == typeof(ATK))
            {
                myStat.ATK += stat.Value;
            }
            else if (stat.GetType() == typeof(DEF))
            {
                myStat.DEF += stat.Value;
            }
            else if (stat.GetType() == typeof(ENRGN))
            {
                myStat.ENRGN += stat.Value;
            }
            else if (stat.GetType() == typeof(ATKSPD))
            {
                myStat.ATKSPD += stat.Value;
            }

            else if (stat.GetType() == typeof(SPECIAL_HELMET_01))
            {
                // This is the legendary stat index (usually the last index = 4)
                equipment.stats[i].SpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_PLATE_01))
            {
                equipment.stats[i].SpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_GLOVE_01))
            {
                equipment.stats[i].SpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_BOOTS_01))
            {
                equipment.stats[i].SpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_ACCESSORY_01))
            {
                equipment.stats[i].SpecialEffect(myStat);
            }
        }
    }

    #endregion

    #region Removing Equipment

    public void RemoveEquipment(int index, Oswald.Player.PlayerController playerController, Oswald.Manager.TimeManager timeManager)
    {
        ApplyStat(index, playerController);
        UpdateStat(playerController.GetMyStat(),
            playerController.gameObject.GetComponent<Health>(),
            playerController.gameObject.GetComponent<Energy>(),
            playerController.gameObject.GetComponent<Armour>(),
            playerController.gameObject.GetComponent<Oswald.Player.PlayerAttack>());

        GameObject equipmentGameObj = myEquipment[index].gameObject;

        //GameObject equipmentGameObj = myEquipment[index].gameObject;
        //equipmentGameObj.SetActive(true);
        //equipmentGameObj.transform.position = playerController.transform.position + new Vector3(0f, 0.35f);

        myEquipment.RemoveAt(index);

        StartCoroutine(WaitForUnpauseToShowItem(index, playerController, timeManager, equipmentGameObj));
    }

    private IEnumerator WaitForUnpauseToShowItem(int index, Oswald.Player.PlayerController playerController, Oswald.Manager.TimeManager timeManager, GameObject equipmentGameObj)
    {
        yield return new WaitUntil(() => timeManager.GetIsTimeStopped() == false);

        equipmentGameObj.SetActive(true);
        equipmentGameObj.transform.position = playerController.transform.position + new Vector3(0f, 0.35f);
    }

    /// <summary>
    /// Apply stat negatively
    /// </summary>
    private void ApplyStat(int index, Oswald.Player.PlayerController playerController)
    {
        Equipment equipment = myEquipment[index];
        MyStat myStat = playerController.GetMyStat();

        int numberOfStats = equipment.stats.Count;

        // An equipment has N amount of stats, apply those to the character stat
        for (int i = 0; i < numberOfStats; i++)
        {
            Stat stat = equipment.stats[i];

            if (i == 3)
            {
                // Apply Epic stat
                if (stat.GetType() == typeof(HP))
                    myStat.HPPercent -= stat.Value;
                else if (stat.GetType() == typeof(EN))
                    myStat.ENPercent -= stat.Value;
                else if (stat.GetType() == typeof(ATK))
                    myStat.ATKPercent -= stat.Value;
                else if (stat.GetType() == typeof(DEF))
                    myStat.DEFPercent -= stat.Value;
                else if (stat.GetType() == typeof(ENRGN))
                    myStat.ENRGNPercent -= stat.Value;
                else if (stat.GetType() == typeof(ATKSPD))
                    myStat.ATKSPDPercent -= stat.Value;

                continue;
            }


            if (stat.GetType() == typeof(HP))
            {
                myStat.HP -= stat.Value;
            }
            else if (stat.GetType() == typeof(EN))
            {
                myStat.EN -= stat.Value;
            }
            else if (stat.GetType() == typeof(ATK))
            {
                myStat.ATK -= stat.Value;
            }
            else if (stat.GetType() == typeof(DEF))
            {
                myStat.DEF -= stat.Value;
            }
            else if (stat.GetType() == typeof(ENRGN))
            {
                myStat.ENRGN -= stat.Value;
            }
            else if (stat.GetType() == typeof(ATKSPD))
            {
                myStat.ATKSPD -= stat.Value;
            }

            else if (stat.GetType() == typeof(SPECIAL_HELMET_01))
            {
                // This is the legendary stat index (usually the last index = 4)
                equipment.stats[i].RemoveSpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_PLATE_01))
            {
                equipment.stats[i].RemoveSpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_GLOVE_01))
            {
                equipment.stats[i].RemoveSpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_BOOTS_01))
            {
                equipment.stats[i].RemoveSpecialEffect(myStat);
            }
            else if (stat.GetType() == typeof(SPECIAL_ACCESSORY_01))
            {
                equipment.stats[i].RemoveSpecialEffect(myStat);
            }
        }
    }

    #endregion

    public void UpdateStat(MyStat myStat, Health health, Energy energy, Armour armour, Oswald.Player.PlayerAttack playerAttack)
    {
        // For HP stat
        health.SetMaxHealth(myStat.HP);
        health.SetHealth(health.GetMaxHealth());

        // For EN stat
        energy.SetMaxEnergy(myStat.EN);
        energy.SetEnergy(energy.GetMaxEnergy());

        // For ENRGN stat
        energy.SetEnergyRegen(myStat.ENRGN);

        // For DEF stat
        armour.SetMaxArmour(myStat.DEF);
        armour.SetArmour(armour.GetMaxArmour());

        // For ATK stat
        playerAttack.SetMyDamage(myStat.ATK);
        playerAttack.SetBowDamage(myStat.ATK);

        // For ATKSPD stat
        playerAttack.AttackSpeed = myStat.ATKSPD;



        // For EPIC STATS
        float hpBonus = myStat.HP * (myStat.HPPercent / 100f);
        health.SetMaxHealth(hpBonus);
        health.SetHealth(health.GetMaxHealth());

        float enBonus = myStat.EN * (myStat.ENPercent / 100f);
        energy.SetMaxEnergy(enBonus);
        energy.SetEnergy(energy.GetMaxEnergy());

        float enrgnBonus = myStat.ENRGN * (myStat.ENRGNPercent / 100f);
        energy.SetEnergyRegen(enrgnBonus);

        float defBonus = myStat.DEF * (myStat.DEFPercent / 100f);
        armour.SetMaxArmour(defBonus);
        armour.SetArmour(armour.GetMaxArmour());

        float atkBonus = myStat.ATK * (myStat.ATKPercent / 100f);
        playerAttack.SetMyDamage(atkBonus);
        playerAttack.SetBowDamage(atkBonus);

        float atkspdBonus = myStat.ATKSPD * (myStat.ATKSPDPercent / 100f);
        playerAttack.AttackSpeed = atkspdBonus;

        // For SPECIAL STATS
        health.HealthRegen = myStat.HealthRegen;

        armour.DamageReduction = myStat.DamageReduction;

        playerAttack.CriticalChance = myStat.CriticalChance;
        playerAttack.CriticalDamage = myStat.CriticalDamage;

        health.EvasionRate = myStat.EvasionRate;
    }
}