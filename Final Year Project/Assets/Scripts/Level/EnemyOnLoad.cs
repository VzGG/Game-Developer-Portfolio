using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached on a gameobject called EnemyOnLoad.
/// 
/// This creates a reference to the non-destroy-on-load-scenes "enemy-level-n" gameobjects to allow setting them to active and inactive
/// </summary>
public class EnemyOnLoad : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyLevelContainers;

    private void Awake()
    {
        // Singleton AND Don't destroy on next load level scene
        int numOfThisGameObj = FindObjectsOfType<EnemyOnLoad>().Length;
        if (numOfThisGameObj > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update

    public void AddEnemyLevelContainerToList(GameObject enemyLevelContainer)
    {
        //Debug.Log("Adding to the list... NOTE: SHOULD ONLY BE CALLED EXACTLY 3 TIMES OR depending on the number of levels");
        enemyLevelContainers.Add(enemyLevelContainer);
    }

    public List<GameObject> GetEnemyLevelContainers() { return this.enemyLevelContainers; }

    // Called by progressionManager
    public void DestroyAllEnemiesAndSelf()
    {
        for (int i = 0; i < enemyLevelContainers.Count; i++)
        {
            Destroy(enemyLevelContainers[i]);
        }

        Destroy(this.gameObject);
    }

}
