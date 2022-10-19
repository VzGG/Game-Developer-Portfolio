using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemies
{
    [SerializeField] List<EnemyTest> enemies = new List<EnemyTest>();
    [Space]
    // [SerializeField] int fitness = 0;
    [SerializeField] float fitness = 0f;
    // Constructor 
    //public Enemies(int enemyCount, int lowerLimit, int upperLimit)
    public Enemies(int enemyCount, float givenLowerLimit, float givenUpperLimit, float givenLowerLimitSpeed, float givenUpperLimitSpeed)
    {
        fitness = 0;
        // Populate each enemy with a random value AND perform fitness afterwards
        for (int i = 0; i < enemyCount; i++)
        {
            // int attackValue = Random.Range(lowerLimit, upperLimit);
            float powerValue = Random.Range(givenLowerLimit, givenUpperLimit);
            float armorValue = Random.Range(givenLowerLimit, givenUpperLimit);
            float speedValue = Random.Range(givenLowerLimitSpeed, givenUpperLimitSpeed);
            float hullValue = Random.Range(givenLowerLimit, givenUpperLimit);
            float energyValue = Random.Range(givenLowerLimit, givenUpperLimit);
            // Populate the enemies with random values
            EnemyTest newEnemyTest = new EnemyTest(powerValue, armorValue, speedValue, hullValue, energyValue);

            enemies.Add(newEnemyTest);

            // Fitness of an enemy is average of all of its attributes together
            float singleEnemyAvgF = (powerValue + armorValue + speedValue + hullValue + energyValue) / 5f;
            // Fitness should be average of all attributes of an enemy - this is added to "fitness"
            //fitness += powerValue;
            fitness += singleEnemyAvgF;
        }

        // Get the average as the fitness evaluator 
        fitness = fitness / enemyCount;
    }

    // Getter
    public List<EnemyTest> GetEnemies() { return this.enemies; }

    public float GetFitness() { return this.fitness; }
}
