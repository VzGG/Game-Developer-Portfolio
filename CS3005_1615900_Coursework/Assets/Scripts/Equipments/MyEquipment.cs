using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEquipment : MonoBehaviour
{
    [SerializeField] public List<Equipment> myEquipment;

    private const int _equipmentLimit = 5;

    public bool AddEquipment(Equipment equipment, Oswald.Player.PlayerController playerController)
    {
        if (myEquipment.Count > (_equipmentLimit - 1) ) { return false; }

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

    public void ApplyStat(MyStat myStat, Equipment equipment)
    {
        int numberOfStats = equipment.stats.Count;

        // An equipment has N amount of stats, apply those to the character stat
        for (int i = 0; i < numberOfStats; i++)
        {
            Stat stat = equipment.stats[i];

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
                Debug.Log("Helmet legendary stat equipped!");


                // This is the legendary stat index (usually the last index = 4)
                equipment.stats[i].SpecialEffect(GetComponent<Health>());
            }
            else if (stat.GetType() == typeof(SPECIAL_PLATE_01))
            {
                Debug.Log("Plate legendary stat equipped!");

                equipment.stats[i].SpecialEffect(GetComponent<Armour>());
            }
            else if (stat.GetType() == typeof(SPECIAL_GLOVE_01))
            {
                Debug.Log("Glove legendary stat equipped!");

                equipment.stats[i].SpecialEffect(GetComponent<Oswald.Player.PlayerAttack>());
            }
            else if (stat.GetType() == typeof(SPECIAL_BOOTS_01))
            {
                Debug.Log("Boot legendary stat equipped!");

                equipment.stats[i].SpecialEffect(GetComponent<Health>());
            }
            else if (stat.GetType() == typeof(SPECIAL_ACCESSORY_01))
            {
                Debug.Log("Accessory legendary stat equipped!");

                equipment.stats[i].SpecialEffect(
                    GetComponent<Oswald.Player.PlayerAttack>(), 
                    GetComponent<Health>(), 
                    GetComponent<Health>(), 
                    GetComponent<Armour>());
            }
        }
    }

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

        // TO-DO: also apply ATK SPD!

        //
    }
}