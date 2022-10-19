using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// A class that holds many enemy ships - to get its fitness
/// </summary>
public class EnemyShips
{
    [SerializeField] List<EnemyShip> enemyShips = new List<EnemyShip>();

    // The average fitness of all ships' fitness
    [SerializeField] float allShipFitness = 0f;

    // Constructor
    public EnemyShips(int currentLevel, List<EnemyShip> givenShip, float lowerLimitMultiplier, float upperLimitMultiplier)
    {
        allShipFitness = 0f;

        // Populate each ship with its components random multipliers
        //for (int i = 0; i < enemyCount; i++)
        for (int i = 0; i < givenShip.Count; i++)
        {
            // Each ship
            

            // Each weapon available is given different multiplier
            int weaponCount = givenShip[i].GetInstalledWeapons().Count;
            float weaponFitness = 0f;
            for (int j = 0; j < weaponCount; j++)
            {
                float weaponValue = Random.Range(lowerLimitMultiplier, upperLimitMultiplier);
                // Each weapon is set the same value multiplier
                givenShip[i].GetInstalledWeapons()[j].SetValueMultiplier(weaponValue);

                if (j == 0)
                {
                    // Add the weaponValue to calculate the multiplier of weapon component - for fitness evaluation
                    // ONLY one fitness is counted because doing so will NOT allow the RRHC to optimize the amount of weapon count
                    weaponFitness += weaponValue;
                }

                
            }




            // Set each component to a different multiplier (default limit is 0.5-1)
            float armourValue = Random.Range(lowerLimitMultiplier, upperLimitMultiplier);
            float boosterValue = Random.Range(lowerLimitMultiplier, upperLimitMultiplier);
            float frameValue = Random.Range(lowerLimitMultiplier, upperLimitMultiplier);
            float coreValue = Random.Range(lowerLimitMultiplier, upperLimitMultiplier);

            givenShip[i].GetInstalledArmour().SetValueMultiplier(armourValue);
            givenShip[i].GetInstalledBooster().SetValueMultiplier(boosterValue);
            givenShip[i].GetInstalledFrame().SetValueMultiplier(frameValue);
            givenShip[i].GetInstalledCore().SetValueMultiplier(coreValue);

            // Add to the list
            enemyShips.Add(givenShip[i]);

            // THe fitness evaluation is the avg of all components - the weapon component can range from 1-4 components - basically 1-4 weapons to choose from
            // but we should not add each for fitness - doing that will make the RRHC have as many weapon to OPTIMIZE
            float shipFitness = (weaponFitness + armourValue + boosterValue + frameValue + coreValue) / 5f;


            allShipFitness += shipFitness;
        }

        allShipFitness = allShipFitness / givenShip.Count;
    }

    public List<EnemyShip> GetEnemyShips() { return this.enemyShips; }

    public float GetFitness() { return this.allShipFitness; }

    // Update the fitness - after small change
    public float FitnessEvaluation()
    {
        // Reset
        allShipFitness = 0f;

        for (int i = 0; i < enemyShips.Count; i++)
        {
            // Each ship's fitness is updated
            float weaponFitness = enemyShips[i].GetInstalledWeapons()[0].GetValueMultiplier(); // FOR WEAPON ONLY TAKES IN ONE component out of max of 4
            float armourFitness = enemyShips[i].GetInstalledArmour().GetValueMultiplier();
            float boosterFitness = enemyShips[i].GetInstalledBooster().GetValueMultiplier();
            float frameFitness = enemyShips[i].GetInstalledFrame().GetValueMultiplier();
            float coreFitness = enemyShips[i].GetInstalledCore().GetValueMultiplier();

            // The average of each component is the fitness of A ship
            float shipFitness = (weaponFitness + armourFitness + boosterFitness + frameFitness + coreFitness)/ 5f;

            allShipFitness += shipFitness;
        }

        // All ship's avg is the average
        allShipFitness = allShipFitness / enemyShips.Count;

        return allShipFitness;
    }
}
