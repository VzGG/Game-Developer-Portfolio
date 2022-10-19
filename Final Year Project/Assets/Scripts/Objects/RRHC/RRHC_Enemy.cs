using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRHC_Enemy : MonoBehaviour
{
    //[SerializeField] ShipSpawner shipSpawner;
    [SerializeField] int fitnessCalls = 1000;
    [SerializeField] int hillClimbCalls = 10;
    [SerializeField] float baseLowerLimit = 0.5f;               // Min multiplier of a ship's component multiplier - incremental at each higher level
    [SerializeField] float baseUpperLimit = 0.75f;              // Max multiplier of a ship's component multiplier

    [SerializeField] float limitIncreaser = 0.25f;              // Increase both lower and upper limit at each call of the RRHC -> rrhc is called 7 times
    [SerializeField] float currentLowerLimit = 0.5f;            // This value is changed
    [SerializeField] float currentUpperLimit = 0.75f;           // This values is changed

    // Create/instantiate set of enemies only instantiate the enemies 30 times AT ANY POINT IN THE GAME
    // Set gameobject active to false

    private List<EnemyShips> enemiesPerLevel = new List<EnemyShips>();

    [SerializeField] List<EnemyShips> localOptimaListOfEnemyShips = new List<EnemyShips>(); // Contains many set of enemyships that have optimized stat multiplier for that level
    [SerializeField] EnemyShips globalOptimaEnemyShips = null;                              // The best of the global optima enemies - this is what the ShipSpawner should get
    

    public void RandomRestartHillClimb(List<EnemyShip> givenEnemyShips)
    {
        // Debug.LogError("CURRENT LIMIT: (lower) " + currentLowerLimit + "\n | (upper): " + currentUpperLimit); // WORKING AS INTENDED!!!

        // Store and get the local optima - local best solutions
        for (int i = 0; i < this.hillClimbCalls; i++)
        {
            EnemyShips localoptimaEnemies = HillClimb(givenEnemyShips, this.fitnessCalls, this.currentLowerLimit, this.currentUpperLimit);
            localOptimaListOfEnemyShips.Add(localoptimaEnemies);
        }

        // Set current best global optima to the first index element
        EnemyShips currentBestGlobalOptimaEnemies = localOptimaListOfEnemyShips[0];
        // Find the best local optima - to get the global optima
        for (int i = 0; i < localOptimaListOfEnemyShips.Count; i++)
        {
            if (localOptimaListOfEnemyShips[i].GetFitness() > currentBestGlobalOptimaEnemies.GetFitness())
            {
                // Set to the improved one
                currentBestGlobalOptimaEnemies = localOptimaListOfEnemyShips[i];
            }
        }
        // Debug.Log("The best enemy has fitness: " + currentBestGlobalOptimaEnemies.GetFitness());
        globalOptimaEnemyShips = currentBestGlobalOptimaEnemies;

        Debug.Log("Set of enemy's level fitness: " + globalOptimaEnemyShips.GetFitness());

        // Increase the range of multipliers of a ship component - used for making stronger enemies in higher levels
        currentLowerLimit += limitIncreaser;
        currentUpperLimit += limitIncreaser;

        // Debug.LogError("LIMIT INCREASE!!!");
    }

    private EnemyShips HillClimb(List<EnemyShip> givenEnemyShips, int fitnessCalls, float givenLowerLimit, float givenUpperLimit)
    {
        // Hill Climb
        #region hill climb optimized
        // Rand start - give each enemy ship's component a new value stat multiplier (from 1 to a range of 0.5-0.75 or incremental)
        //Enemies enemiesTest = new Enemies(enemyCount, givenLowerLimit, givenUpperLimit, givenLowerLimitSpeed, givenUpperLimitSpeed);
        EnemyShips enemyShips = new EnemyShips(0, givenEnemyShips, givenLowerLimit, givenUpperLimit);

        for (int i = 0; i < fitnessCalls - 1; i++)
        {
            // Rand start
            EnemyShips currentEnemyShips = new EnemyShips(0, givenEnemyShips, givenLowerLimit, givenUpperLimit);

            // Small Change
            float smallchangeValuePower = Random.Range(-0.03f, 0.03f);
            float smallchangeValueArmour = Random.Range(-0.03f, 0.03f);
            float smallchangeValueSpeed = Random.Range(-0.03f, 0.03f);
            float smallchangeValueHull = Random.Range(-0.03f, 0.03f);
            float smallchangeValueEnergy = Random.Range(-0.03f, 0.03f);
            for (int j = 0; j < currentEnemyShips.GetEnemyShips().Count; j++)
            {
                // Each enemy within Enemies has its attack value changed by a small amount
                // Each enemy ship's component's value multiplier is changed

                // FOR WEAPONS - for now, just set to 1 weapon component
                currentEnemyShips.GetEnemyShips()[j].GetInstalledWeapons()[0].
                    SetValueMultiplier(currentEnemyShips.GetEnemyShips()[j].GetInstalledWeapons()[0].GetValueMultiplier() + smallchangeValuePower);

                currentEnemyShips.GetEnemyShips()[j].GetInstalledArmour().
                    SetValueMultiplier(currentEnemyShips.GetEnemyShips()[j].GetInstalledArmour().GetValueMultiplier() + smallchangeValueArmour);

                currentEnemyShips.GetEnemyShips()[j].GetInstalledBooster().
                    SetValueMultiplier(currentEnemyShips.GetEnemyShips()[j].GetInstalledBooster().GetValueMultiplier() + smallchangeValueSpeed);

                currentEnemyShips.GetEnemyShips()[j].GetInstalledFrame().
                    SetValueMultiplier(currentEnemyShips.GetEnemyShips()[j].GetInstalledFrame().GetValueMultiplier() + smallchangeValueHull);

                currentEnemyShips.GetEnemyShips()[j].GetInstalledCore().
                    SetValueMultiplier(currentEnemyShips.GetEnemyShips()[j].GetInstalledCore().GetValueMultiplier() + smallchangeValueEnergy);
            }
            // Need to do fitness evaluation after small change to update its fitness
            currentEnemyShips.FitnessEvaluation();
            // Debug.Log("Current fitness: " + currentEnemyShips.GetFitness());
            // Decider - as the iteration reachs the final fitness calls, the fitness should be higher value
            if (currentEnemyShips.GetFitness() > enemyShips.GetFitness())
            {
                // enemiesTest = currentEnemiesTest;
                enemyShips = currentEnemyShips;
                // Debug.Log("FOUND BETTER" + enemyShips.GetFitness());
            }
        }
        // At the end of fitness increase loop above, add the local optima enemiesTest to the list
        // Debug.Log("Best fitness (local optima): " + enemyShips.GetFitness());
        return enemyShips;
        #endregion
    }
}
