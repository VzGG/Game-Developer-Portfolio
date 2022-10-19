using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomRestartHillClimb : MonoBehaviour
{
    [SerializeField] int enemyCount = 0;
    //[SerializeField] int lowerLimit = 0;
   // [SerializeField] int upperLimit = 1000;
    [SerializeField] int fitnessCalls = 1000;
    [SerializeField] int hillclimbCalls = 10;

    [SerializeField] float lowerLimit = 0f;
    [SerializeField] float upperLimit = 1000f;

    [SerializeField] float lowerLimitSpeed = 0f;
    [SerializeField] float upperLimitSpeed = 1f;


    [SerializeField] PlayerShip myPlayer;

    // Start is called before the first frame update
    void Start()
    {
/*        myPlayer = new PlayerShip(6000);
        // Create operator of ship
        ShipOperator player = new ShipOperator("Gregg", 1000, 0);

        // Create components
        Weapon playerWeaponComponent = new Weapon(1000, 0.5f, 1500, "W", null);
        Armour playerArmourComponent = new Armour(500f, 0.05f, 1000, "A", null);
        Booster playerBoosterComponent = new Booster(1f, 0.05f, 1000, "B", null);
        Frame playerFrameComponent = new Frame(1000f, 10f, 1000, "F", null);
        Core playerCoreComponent = new Core(1000f, 10f, 1000, "C", null);

        myPlayer.SetShipOperator(player);
        // Add the components to the ship
        myPlayer.SetInstalledWeapon(playerWeaponComponent);
        myPlayer.SetInstalledArmour(playerArmourComponent);
        myPlayer.SetInstalledBooster(playerBoosterComponent);
        myPlayer.SetInstalledFrame(playerFrameComponent);
        myPlayer.SetInstalledCore(playerCoreComponent);*/



        float startTime = 0f;
        float endTime = 0f;
        float elapsedTime = 0f;
        startTime = Time.realtimeSinceStartup;

        Debug.Log("test start time: " + Time.realtimeSinceStartup);
        startTime = Time.realtimeSinceStartup;
        RRHCOptimized(fitnessCalls, hillclimbCalls);
        // RRHC(fitnessCalls, hillclimbCalls);
        Debug.Log("Test end time: " + Time.realtimeSinceStartup);
        endTime = Time.realtimeSinceStartup;
        elapsedTime = Mathf.Abs(startTime - endTime);
        Debug.Log("Time taken to generate global optima enemies: " + elapsedTime);

        for (int i = 0; i < globalOptimaEnemies.GetEnemies().Count; i++)
        {
            EnemyTest enemyTest = globalOptimaEnemies.GetEnemies()[i];

            float power = enemyTest.GetPower();
            float armour = enemyTest.GetArmour();
            float speed = enemyTest.GetSpeed();
            float hull = enemyTest.GetHull();
            float energy = enemyTest.GetEnergy();

            float avg = (power + armour + speed + hull + energy) / 5f;
/*            Debug.Log("Enemy (avg: " + avg + "\n" +
                "Power: " + power + "\n" +
                "Armour: " + armour + "\n" +
                "Speed: " + speed + "\n" +
                "Hull: " + hull + "\n" +
                "Energy: " + energy);*/
        }
    }


    [SerializeField] Enemies globalOptimaEnemies;
    #region Optimized Test

    /// <summary>
    /// Generates an optimized list of enemies that have about an atk power of 0-1000
    /// </summary>
    /// <param name="fitnessCalls"></param>
    /// <param name="hillclimbCalls"></param>
    private void RRHCOptimized(int fitnessCalls, int hillclimbCalls)
    {
        for (int i = 0; i < hillclimbCalls; i++)
        {
            Enemies localoptimaEnemies = HillClimbTest(fitnessCalls, this.lowerLimit, this.upperLimit, this.lowerLimitSpeed, this.upperLimitSpeed);
            enemiesAllTest.Add(localoptimaEnemies);
        }

        // Set current best global optima to the first index element
        Enemies currentBestGlobalOptimaEnemies = enemiesAllTest[0];
        // Find the best local optima
        for (int i = 0; i < enemiesAllTest.Count; i++)
        {
            if (enemiesAllTest[i].GetFitness() > currentBestGlobalOptimaEnemies.GetFitness())
            {
                // Set to the improved one
                currentBestGlobalOptimaEnemies = enemiesAllTest[i];
            }
        }
        // Debug.Log("The best enemy has fitness: " + currentBestGlobalOptimaEnemies.GetFitness());
        globalOptimaEnemies = currentBestGlobalOptimaEnemies;
        //Debug.Log("finished RRHC... check time");
    }

   // private Enemies HillClimbTest(int fitnessCalls, int lowerLimit, int upperLimit)
    private Enemies HillClimbTest(int fitnessCalls, float givenLowerLimit, float givenUpperLimit, float givenLowerLimitSpeed, float givenUpperLimitSpeed)
    {
        // Hill Climb
        #region hill climb optimized

        /*        int lowerLimitTest = 0;
                int upperLimitTest = 1000;*/

        // Rand start
        // Enemies enemiesTest = GenerateEnemiesTest(enemyCount, lowerLimitTest, upperLimitTest);  // Should be current best
        //  Enemies enemiesTest = GenerateEnemiesTest(enemyCount, lowerLimit, upperLimit);  // Should be current best - for int type
        Enemies enemiesTest = new Enemies(enemyCount, givenLowerLimit, givenUpperLimit, givenLowerLimitSpeed, givenUpperLimitSpeed);
        // enemiesAllTest.Add(enemiesTest);

        for (int i = 0; i < fitnessCalls - 1; i++)
        {
            // Rand start
            // Enemies currentEnemiesTest = new Enemies(enemyCount, lowerLimitTest, upperLimitTest);
            Enemies currentEnemiesTest = new Enemies(enemyCount, givenLowerLimit, givenUpperLimit, givenLowerLimitSpeed, givenUpperLimitSpeed);

            // Small Change
            // int smallchangeValue = Random.Range(-3, 4);
            float smallchangeValuePower = Random.Range(-3f, 3f);
            float smallchangeValueArmour = Random.Range(-3f, 3f);
            float smallchangeValueSpeed = Random.Range(-0.03f, 0.03f);
            float smallchangeValueHull = Random.Range(-3f, 3f);
            float smallchangeValueEnergy = Random.Range(-3f, 3f);

            for (int j = 0; j < currentEnemiesTest.GetEnemies().Count; j++)
            {
                // Each enemy within Enemies has its attack value changed by a small amount
                // Each enemy has their attributes changed
               // currentEnemiesTest.GetEnemies()[j].SetPower(currentEnemiesTest.GetEnemies()[j].GetPower() + smallchangeValue);
                currentEnemiesTest.GetEnemies()[j].SetPower(currentEnemiesTest.GetEnemies()[j].GetPower() + smallchangeValuePower);
                currentEnemiesTest.GetEnemies()[j].SetArmour(currentEnemiesTest.GetEnemies()[j].GetArmour() + smallchangeValueArmour);
                currentEnemiesTest.GetEnemies()[j].SetSpeed(currentEnemiesTest.GetEnemies()[j].GetSpeed() + smallchangeValueSpeed);
                currentEnemiesTest.GetEnemies()[j].SetHull(currentEnemiesTest.GetEnemies()[j].GetHull() + smallchangeValueHull);
                currentEnemiesTest.GetEnemies()[j].SetEnergy(currentEnemiesTest.GetEnemies()[j].GetEnergy() + smallchangeValueEnergy);
            }

            // NOTE TO SELF - AFTER SMALL CHANGE - NEED TO UPDATE FITNESS FUNCTION!!!

            // Decider - as the iteration reachs the final fitness calls, the fitness should be higher value
            if (currentEnemiesTest.GetFitness() > enemiesTest.GetFitness())
            {
                enemiesTest = currentEnemiesTest;
                //enemiesAllTest.Add(enemiesTest);
            }
        }
        // At the end of fitness increase loop above, add the local optima enemiesTest to the list
        // enemiesAllTest.Add(enemiesTest); // This is the list of all local optimas - to find the best one, global optima

        return enemiesTest;

        #endregion
    }


    // To make this visible in the inspector: https://answers.unity.com/questions/49667/display-non-monobehaviour-array-in-inspector-c.html
    [SerializeField] public List<Enemies> enemiesAllTest = new List<Enemies>();
/*    private Enemies GenerateEnemiesTest(int enemyCount, int lowerLimit, int upperLimit)
    {
        Enemies enemies = new Enemies(enemyCount, lowerLimit, upperLimit);

        return enemies;
    }
*/

    #endregion
}
