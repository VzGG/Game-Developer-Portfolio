using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    // Only add certain enemies to defeat
    [SerializeField] List<GameObject> enemies = new List<GameObject>();

    [SerializeField] GameObject portal;
    [SerializeField] Transform portalLocation;

    [SerializeField] GameObject instantiatedPortal;
    [SerializeField] int summonPortalCounter = 0;           // Solves multiple portals summoning

    [SerializeField] string sceneNameOfNextLevel;
    // Delete themselves
    public void DeleteEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObject.name == enemy.gameObject.name)
            {
                enemies.RemoveAt(i);
                return;
            }
        }
    }


    public void AddEnemies(GameObject enemy) { enemies.Add(enemy); }

    public void RemoveAll() { enemies.Clear(); }            // Called by boss controller in the animation of death

    private void Update()
    {
        if (enemies.Count <= 0)
        {
/*            summonPortalCounter++;*/
            summonPortalCounter = Mathf.Min(summonPortalCounter + 1, 10);
            if (summonPortalCounter == 1)
            {
                // Debug.Log("OPEN THE PORTAL TO THE NEXT LEVEL");

                instantiatedPortal = Instantiate(portal, portalLocation.position, transform.rotation);
                // Pass the string name of the scene name to be able to load the next level
                instantiatedPortal.GetComponent<Portal>().SetSceneNameOfNextLevel(sceneNameOfNextLevel);
            }
        }
    }



}
