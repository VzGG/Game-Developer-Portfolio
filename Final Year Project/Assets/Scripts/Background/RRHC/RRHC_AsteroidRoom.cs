using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRHC_AsteroidRoom : MonoBehaviour
{
    #region Unsure about usage


    [SerializeField] List<GameObject> runtimeRandomPermOfAsteroidsCenterPoints;
    // An NxN 2d distance matrix of asteroid rooms, its length depends on the number of gameobjects that have the name "CENTER"
    private float[][] asteroidRoomDistanceMatrix;

    #endregion

    [SerializeField] private int hillclimbCalls = 10;


    [SerializeField] List<GameObject> startingHP = new List<GameObject>();
    [SerializeField] List<GameObject> bestofbestHP = new List<GameObject>();

    #region Finding the Optimal Path from room 1 to room n

    /// <summary>
    /// Find the best of the best the shortest hamiltonian path - that is each room is only visited once. The output provides you a list of "CENTER" gameobjects where the first index is the first room and the last index is the last room.
    /// 
    /// </summary>
    /// <param name="asteroidCenterPoints"></param>
    /// <param name="hillclimbCalls"></param>
    /// <returns></returns>
    public List<GameObject> RRHC(List<GameObject> asteroidCenterPoints)
    {
        CreateAsteroidDistanceMatrix(asteroidCenterPoints);
        FillAsteroidDistanceMatrix(asteroidCenterPoints);
        // this.CheckDistanceMatrix();

        // List of local best solutions 
        List<List<GameObject>> hamiltonianPaths = new List<List<GameObject>>();

        // Perform many hill climbs and store all the local best solutions
        for (int i = 0; i < this.hillclimbCalls; i++)
        {
            List<GameObject> localOptimaHP = new List<GameObject>();
            localOptimaHP.AddRange(HillClimb(asteroidCenterPoints, 100));
            // Debug.Log("best fitness call 2: " + FitnessFunction(localOptimaHP));
            // Add to list
            hamiltonianPaths.Add(localOptimaHP);
        }

        // Set current best as the first index 
        List<GameObject> globalOptimaHP = hamiltonianPaths[0];
        float globalOptimaFitness = FitnessFunction(globalOptimaHP);

        // Find the best local optima to become the global optima
        for (int i = 0; i < hamiltonianPaths.Count; i++)
        {
            // Get fitness of current local optima
            float currentFitness = FitnessFunction(hamiltonianPaths[i]);

            if (currentFitness > globalOptimaFitness)
            {
                // Update to the current found best 
                globalOptimaHP = hamiltonianPaths[i];
                globalOptimaFitness = currentFitness;
            }
        }

        Debug.Log("Best of best fitness: " + globalOptimaFitness);

        bestofbestHP = globalOptimaHP;          // DELETE

        return globalOptimaHP;
    }

    /// <summary>
    /// Creates a local optima (local best) hamiltonian path - where each rooms are visited only once. This should provide you one of the shortest distance.
    /// </summary>
    /// <param name="asteroidRoomCenterPoints"></param>
    /// 
    public List<GameObject> HillClimb(List<GameObject> asteroidRoomCenterPoints, int fitnessCalls)
    {
        // Rand Start old - Find a random order of asteroid center points in which all rooms are visited once (Hamiltonian Path)
        List<GameObject> oldHamiltonianPath = new List<GameObject>();
        oldHamiltonianPath.AddRange(RandomHamiltonianPathPerm(asteroidRoomCenterPoints));

        startingHP = oldHamiltonianPath;        // DELETE

        // Fitness Function
        float oldFitness = FitnessFunction(oldHamiltonianPath);
        // Debug.Log("starting fitness: " + oldFitness);

        for (int i = 0; i < fitnessCalls; i++)
        {
            // Rand Start current new
            List<GameObject> currentHamiltonianPath = new List<GameObject>();
            currentHamiltonianPath.AddRange(RandomHamiltonianPathPerm(asteroidRoomCenterPoints));

            // Fitness Function
            float curFitnessRegular = FitnessFunction(currentHamiltonianPath);

            // Small Change
            currentHamiltonianPath = SmallChange(currentHamiltonianPath);

            // Fitness Function
            float curFitnessSmallChange = FitnessFunction(currentHamiltonianPath);

            // Smaller fitness - smaller total distance the better, therefore we take that HP instead
            if (curFitnessSmallChange < oldFitness)
            {
                // Update to the better solution
                oldHamiltonianPath = currentHamiltonianPath;
                oldFitness = curFitnessSmallChange;
                
            }
        }
        // Debug.Log("ending Best fitness: " + oldFitness);
        return oldHamiltonianPath;

    }

    // Find a permutation of an hamiltonian path (HP)
    private List<GameObject> RandomHamiltonianPathPerm(List<GameObject> asteroidCenterPoints)
    {
        // Find a random order of asteroid center points
        // Original: [Room-0, Room-1, Room-2, Room-3]
        // Now with random: [Room-1, Room-3, Room-2, Room-0]
        // Then in other methods Calculate its distance

        List<GameObject> randomOrderOfAsteroidCenterGameObjs = new List<GameObject>();

        List<GameObject> copyOfOriginalAsteroidCenterPoints = new List<GameObject>();
        copyOfOriginalAsteroidCenterPoints.AddRange(asteroidCenterPoints);          // Copy

        while (copyOfOriginalAsteroidCenterPoints.Count > 0)
        {
            // Pick one of the asteroid from the list, add that to the random order list
            int randomIndex = Random.Range(0, copyOfOriginalAsteroidCenterPoints.Count);
            
            GameObject randomAsteroid = copyOfOriginalAsteroidCenterPoints[randomIndex];
            // Add the chosen random gameobject as the first or nth room
            randomOrderOfAsteroidCenterGameObjs.Add(randomAsteroid);
            // Delete the random index chosen from the fake asteroid center points list
            copyOfOriginalAsteroidCenterPoints.RemoveAt(randomIndex);
        }

       // Debug.Log("count of asteroids center points: " + asteroidCenterPoints.Count + "\ncount of copylist: " + copyOfOriginalAsteroidCenterPoints.Count);

        return randomOrderOfAsteroidCenterGameObjs;
    }

    /// <summary>
    /// The fitness is the distance from Room-0 to Room-1, to Room-2, to Room-3, to Room-N, ...
    /// The shorter the distance the better. It is a minimisation problem.
    /// Use this method to pass a hamiltonian path of gameobjects to give you the total distance.
    /// </summary>
    /// <param name="givenHamiltonianPath"></param>
    /// <returns></returns>
    private float FitnessFunction(List<GameObject> givenHamiltonianPath)
    {
        float fitness = 0f;

        int numberOfRooms = givenHamiltonianPath.Count;
        // When we have 1 room or less, it means it has 0 distance as there is no other rooms to compare with
        if (numberOfRooms <= 1) { return fitness = 0f; }

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            Vector2 roomPos = givenHamiltonianPath[i].transform.position;
            Vector2 otherRoomPos = givenHamiltonianPath[i + 1].transform.position;

            float distance = Vector2.Distance(roomPos, otherRoomPos);

            fitness = fitness + distance;
        }

        return fitness;
    }

    /// <summary>
    /// Swap the position of the rooms in the hopes of finding a more optimal HP.
    /// Pick two index numbers from the list, then swap the elements.
    /// For example:
    /// [Room-0, Room-1, Room-2, Room-3]
    /// 
    /// 2 random num: 2 and 3
    /// Now swap room-1's index of 2 to 3 then swap room-2's index of 3 to 2
    /// Now after swapping we have:
    /// [Room-0, Room-2, Room-1, Room-3]
    /// 
    /// </summary>
    /// <param name="givenHamiltonianPath"></param>
    private List<GameObject> SmallChange(List<GameObject> givenHamiltonianPath)
    {
        List<GameObject> copyHP = new List<GameObject>();
        copyHP.AddRange(givenHamiltonianPath);

        int i = 0;
        int j = 0;

        // Ensure two numbers are different - used to get the index of the object in the list, then swap the indexes together
        while (i == j)
        {
            i = Random.Range(0, givenHamiltonianPath.Count);
            j = Random.Range(0, givenHamiltonianPath.Count);
        }

        //Debug.Log("Swap at: " + i + ", " + j);

        // Performing swapping
        // Get the gameobject from the list before swapping
        GameObject i_room = copyHP[i];
        GameObject j_room = copyHP[j];

        //Debug.Log("Swap: " + i_room.transform.parent.name + " at i: " + i + " and " + j_room.transform.parent.name+ " at j: " + j);
        // Overwrite the elements at the given index of i and j with the new rooms - and finally swaps them
        copyHP[i] = j_room;
        copyHP[j] = i_room;

        //Debug.Log("Now it is: " + copyHP[i].transform.parent.name + " at i: " + i + " and " + copyHP[j].transform.parent.name + " at j: " + j);


        return copyHP;

    }


    #endregion

    #region Setting up the 2D Matrix Before any finding optimal path is done

    // Called above RRHC or in RRHC but before the loops
    private void CreateAsteroidDistanceMatrix(List<GameObject> asteroidCenterPoints)
    {
        int distanceMatrixLength = asteroidCenterPoints.Count;

        // Initialize the 2d float array
        asteroidRoomDistanceMatrix = new float[distanceMatrixLength][];

        // Debug.Log("Count of 2d arraY: " + distanceMatrixLength);

        // need to initialize each element with an array
        // Create an NxN 2d array
        for (int i = 0; i < asteroidRoomDistanceMatrix.Length; i++)
        {
            asteroidRoomDistanceMatrix[i] = new float[distanceMatrixLength];
            //Debug.Log("Length of element's array: " + asteroidRoomDistanceMatrix[i].Length);
        }

    }

    // Working
    private float[][] FillAsteroidDistanceMatrix(List<GameObject> asteroidCenterPoints)
    {
        // Find the distance from each center point game objects

        // Find distance matrix of first room to the other rooms
        // Each asteroid
        for (int i = 0; i < asteroidCenterPoints.Count; i++)
        {
            // 
            Vector2 currentAsteroidCenterPos = asteroidCenterPoints[i].transform.position;

            // Find the other asteroids position and find the distance between this current asteroid and the found asteroid

            int currentAsteroidCenterIndex = i;

            // Add to the 2d array the distance between itself
            this.asteroidRoomDistanceMatrix[i][i] = 0f;


            // Each asteroid center needs to find its distance from ALL THE ROOMS
            // we only iterate to the total - 1 as we do not need to know the distance between room A and room A for example
            for (int j = 0; j < asteroidCenterPoints.Count-1; j++)
            {
                /* Asteroids
                 * 1 2 3 4 5
                 * Find distance (number below are the index/ number of asteroid)
                 * 2 3 4 5 1
                 * 3 4 5 1 2
                 * 4 5 1 2 3
                 * 5 1 2 3 4
                 * 
                 * if end is reached go to starting index and go iterate again
                 */
                currentAsteroidCenterIndex += 1;

                // See the coment above in this for loop
                if (currentAsteroidCenterIndex >= asteroidCenterPoints.Count)
                {
                    currentAsteroidCenterIndex = 0;
                }

                // Get position of the other asteroid "CENTER" gameobject
                Vector2 otherAsteroidCenterPos = asteroidCenterPoints[currentAsteroidCenterIndex].transform.position;
                float distanceFromCurrentToOtherRoom = Vector2.Distance(currentAsteroidCenterPos, otherAsteroidCenterPos); 

                // Get more info on who is the parent of that gameobject
                //Debug.LogError("Distance: " + distanceFromCurrentToOtherRoom + 
                //    "\nasteroid parent: " + asteroidCenterPoints[i].gameObject.transform.parent.name + 
                //    "\nasteroid parent2: " + asteroidCenterPoints[currentAsteroidCenterIndex].gameObject.transform.parent.name);

                // Add to the distance 2d matrix
                this.asteroidRoomDistanceMatrix[i][currentAsteroidCenterIndex] = distanceFromCurrentToOtherRoom; 
            } 
        }

        return null;

    }

    // Just for checking
    private void CheckDistanceMatrix()
    {
        // each row
        for (int i = 0; i < this.asteroidRoomDistanceMatrix.Length; i++)
        {
            // each column
            for (int j = 0; j < this.asteroidRoomDistanceMatrix[i].Length; j++)
            {
                float distanceBetween2Rooms = this.asteroidRoomDistanceMatrix[i][j];
                Debug.Log("Distance: " + distanceBetween2Rooms + " at ij: " + i + ", " + j); //  Works
            }
        }
    }

    #endregion
}
