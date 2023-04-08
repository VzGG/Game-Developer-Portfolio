using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEquipment : MonoBehaviour
{
    [SerializeField] public List<Equipment> myEquipment;

    // Use this class to handle the equipment
    public void ActivateLegendaryEffect(System.Type legendaryType, System.Object obj)
    {
        foreach (Equipment equipment in myEquipment)
        {
            if (equipment.rarity == Rarity.Legendary)
            {
                if (equipment.stats[4].GetType() == legendaryType)
                {
                    equipment.stats[4].SpecialEffect(obj);
                    break;
                }
            }
        }
    }

    public void ApplyStat(MyStat myStat)
    {
        //Health health = GetComponent<Health>();
        // For now add all stats to HP

        for (int i = 0; i < myEquipment.Count; i++)
        {
            if (myEquipment[i] == null) { continue; }

            int statsCount = myEquipment[i].stats.Count;
            for (int j = 0; j < statsCount; j++)
            {
                // Check each equipment's stats
                Stat currentStat = myEquipment[i].stats[j];
                if (currentStat.GetType() == typeof(HP))
                {
                    myStat.HP += currentStat.Value;
                }
                else if (currentStat.GetType() == typeof(EN))
                {
                    myStat.EN += currentStat.Value;
                }
                else if (currentStat.GetType() == typeof(ATK))
                {
                    myStat.ATK += currentStat.Value;
                }
                else if (currentStat.GetType() == typeof(DEF))
                {
                    myStat.DEF += currentStat.Value;
                }
                else if (currentStat.GetType() == typeof(ENRGN))
                {
                    myStat.ENRGN += currentStat.Value;
                }
                else if (currentStat.GetType() == typeof(ATKSPD))
                {
                    myStat.ATKSPD += currentStat.Value;
                }

                // How to apply stat for legendary? assumming all legendaries are different
            }
        }

        //foreach(Equipment equipment in myEquipment)
        //{
        //    foreach(Stat stat in equipment.stats)
        //    {
        //        if (stat.GetType().Equals(typeof(HP)))
        //        {
        //            health.SetHealth(health.GetHealth() + stat.Value);
        //        }
        //    }
        //}
    }

    // When we make changes to the list, we update the stat
}
