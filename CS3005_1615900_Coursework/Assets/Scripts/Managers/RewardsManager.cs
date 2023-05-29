using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnWaypoints;
    [SerializeField] private GameObject rewardPrefab;
    public List<GameObject> rewardGameObjs = new List<GameObject>();

    public void SpawnRewards()
    {
        foreach (Transform transform in _spawnWaypoints)
        {
            rewardGameObjs.Add(Instantiate(rewardPrefab, transform.position, Quaternion.identity));
        }
    }

    public void RewardIsSelected(GameObject rewardGameObj)
    {
        rewardGameObjs.Remove(rewardGameObj);

        foreach (GameObject chestGameObj in rewardGameObjs)
        {
            // Hide all the others
            chestGameObj.SetActive(false);

            GameObject equipmentGameObj = chestGameObj.GetComponent<Chest>().spawnedEquipmentGameObj;
            if (equipmentGameObj == null) { continue; }

            Equipment equipment = equipmentGameObj.GetComponent<Equipment>();
            equipment.gameObject.SetActive(false);

            // Play SFX like item shrinks, etc.
        }
    }
}