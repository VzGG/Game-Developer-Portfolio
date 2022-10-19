using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorGameComponentManager : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private ShipSpawner shipSpawner;

    [SerializeField] bool isEnemyAndPlayerCreationOn = true;
    [SerializeField] int enemyLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        /* Call in order:
         * 1. Asteroid Spawner - create all rooms and corridors
         * 2. Ship Spawner - create all enemy ships and spawn in the generated rooms in ship spawner
         * 3. 
         * 
         * 
         * 4. NOTE: this class is not a singleton -> meaning there is another one of this when a new scene loads
         * 5. Bearing #4 in mind, enemies (level 1/2/3 enemies) created in the first instance of this class are kept
         * 6. Depending on the "enemyLevel" class is then the appropriate enemy container is set to active (visible)
         * 7. The rest inactive (hidden)
         */

        asteroidSpawner.CreateTheLevelEnvironment();
        // Only at Level 1 do we create the enemies
        if (isEnemyAndPlayerCreationOn == true)
        {
            
            shipSpawner.CreateAndActivateEnemies();
            shipSpawner.CreateAndActivatePlayer();
        }
        else
        {
            // For level 1 and 2
            //Debug.Log("level 2 and above code!");


            //// Find which level we are in
            List<GameObject> enemyLevelContainers = FindObjectOfType<EnemyOnLoad>().GetEnemyLevelContainers();

            if (enemyLevel == 2)
            {
                // Make the level 2 enemies active
                enemyLevelContainers[1].gameObject.SetActive(true);

                enemyLevelContainers[0].gameObject.SetActive(false);
                enemyLevelContainers[2].gameObject.SetActive(false);

                // Spawn the "ENEMY-LEVEL-2" set of enemies randomly in the generated rooms
                FindObjectOfType<ShipSpawner>().SpawnEnemyRandomlyInEachRoomForLevel2AndAbove(enemyLevelContainers[1]);
                FindObjectOfType<ShipSpawner>().SpawnEnemyOutsideTheRoomForLevel2AndAbove(enemyLevelContainers[1]);
            }
            else if (enemyLevel == 3)
            {
                enemyLevelContainers[2].gameObject.SetActive(true);

                enemyLevelContainers[0].gameObject.SetActive(false);
                enemyLevelContainers[1].gameObject.SetActive(false);

                // Spawn the "ENEMY-LEVEL-3" set of enemies randomly in the generated rooms
                FindObjectOfType<ShipSpawner>().SpawnEnemyRandomlyInEachRoomForLevel2AndAbove(enemyLevelContainers[2]);
                FindObjectOfType<ShipSpawner>().SpawnEnemyOutsideTheRoomForLevel2AndAbove(enemyLevelContainers[2]);
            }


            

            // THEN PLACE THEM IN THE ROOMS RANDOMLY
        }

        
    }


}
