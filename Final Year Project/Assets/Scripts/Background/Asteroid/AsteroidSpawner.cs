using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Sprite asteroidMovingSprite;                                           // Defined in the inspector/editor
    [SerializeField] Color minimapIconColour;                                               // Defined in the inspector/editor

    [SerializeField] private GameObject grid = null;                                        // The grid gameobject - contains points to spawn the asteroids
    [SerializeField] List<GameObject> asteroidRowSpawnPoints = new List<GameObject>();      // The rows of points of the grid gameobject
    [SerializeField] List<GameObject> asteroids = new List<GameObject>();                   // The asteroids - there are 4 variations

    //[SerializeField] int[][] gridTestPoints = new int[41][];                              // A non visual grid to allow marking of random points/coordinates
    //[SerializeField] List<Vector2> markedPoints = new List<Vector2>();                    // The randomly chosen marked points - used for spawning asteroids randomly

    [SerializeField] Color asteroidFrontColour;
    [SerializeField] Color asteroidMiddleColour;
    [SerializeField] Color asteroidBackColour;

    private int[][] gridPointsFront = new int[41][];                                        // A non visual grid to allow marking of random points/coordinates
    private int[][] gridPointsMiddle = new int[41][];
    private int[][] gridPointsBack = new int[41][];

    private List<Vector2> markedPointsFront = new List<Vector2>();                          // The randomly chosen marked points - used for spawning asteroids randomly
    private List<Vector2> markedPointsMiddle = new List<Vector2>();
    private List<Vector2> markedPointsBack = new List<Vector2>();

    [SerializeField] private RRHC_AsteroidRoom rrhc = null;                                 // RRHC class - needed to process and get you the optimal path of all rooms
    [SerializeField] List<GameObject> asteroidRoomCenterGameObjs;                           // Gameobjects that have the name "CENTER" - used to find optimal path of all rooms

    [SerializeField] List<GameObject> optimalHamiltonianPath = new List<GameObject>();      // The optimal hamiltonian path - contains the center position of each room from the first room to the last room


    public List<GameObject> GetOptimalHamiltonianPath() { return this.optimalHamiltonianPath; }


    public void CreateTheLevelEnvironment()
    {
        SetAsteroidRowSpawnPoints();
        // Spawns asteroids in the front
        RandomlySpawnAsteroids(gridPointsFront, markedPointsFront, 400, new Vector3(0f, 0f, 0f), asteroidFrontColour, false, true, true, true);
        // Spawns in the middle
        RandomlySpawnAsteroids(gridPointsMiddle, markedPointsMiddle, 50, new Vector3(0f, 0f, 30f), asteroidMiddleColour, false, false, false, false);
        // Spawns in the back
        RandomlySpawnAsteroids(gridPointsBack, markedPointsBack, 10, new Vector3(0f, 0f, 100f), asteroidBackColour, false, false, false, false);

        //SpawnAsteroids();

        /* 1. Create asteroid rooms
         * 2. Then find the best hamiltonian path (HP) using RRHC asteroid class
         * 3. With the best HP, create corridors
         * 4. Expose the exit room with 4 direction doors
         * 5. Add asteroids in asteroid rooms as gaps
         */
        GameObject asteroidRooms = SpawnAsteroidRooms();                                            // Create asteroid rooms
        // After spawning, go find the optimal path of the rooms
 
        // Add optimal path in the list
        this.optimalHamiltonianPath.AddRange(this.rrhc.RRHC(this.asteroidRoomCenterGameObjs));


        SpawnCorridors(optimalHamiltonianPath, this.asteroidLandscapeGrid, asteroidRooms);         // Based on the hamiltonian path given, now create many gameobjects to spawn many corridors connecting asteroid rooms together - allows corridors overlapping

        ExposeExitRoom(asteroidRooms, this.asteroidLandscapeGrid, optimalHamiltonianPath);         // In the final room, one of the asteroid room's edge/wall is made hidden to have an exit in the labyrinth/dungeon

        //FillAsteroidRoomGaps(asteroidRooms);
        CreateAllAsteroidRoomGaps(asteroidRooms);           
    }


    /// <summary>
    /// Should be called by a level manager or related in which that class also calls or instantiates the ship spawner class/gameobject.
    /// So all spawning of the major gameobjects is under one class.
    /// </summary>
    private void SetAsteroidRowSpawnPoints()
    {
        int numberOfGridRows = grid.transform.childCount;

        for (int i = 0; i < numberOfGridRows; i++)
        {
            asteroidRowSpawnPoints.Add(grid.transform.GetChild(i).gameObject);
        }
    }

    private void RandomlySpawnAsteroids(int[][] gridPoints, List<Vector2> markedPoints, int numberOfAsteroids, Vector3 vectorDepth, 
        Color asteroidColour, bool isBoxCollider2dEnabled, bool isCircleCollider2dEnabled, bool isIconAllowed, bool doesAsteroidHaveHealth)
    {
        int marked = 1;
        int unmarked = 0;

        // Populate each row
        for (int i = 0; i < gridPoints.Length; i++)
        {
            gridPoints[i] = new int[41];

            // Debug.Log("ROW grid length: " + gridPoints[i].Length);
            //for (int j = 0; j < grid)
        }

        // Randomly choose points
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            int row = Random.Range(0, gridPoints.Length);
            int col = Random.Range(0, gridPoints.Length);

            // If the element is marked, anything other than 0, we keep finding one
            while (gridPoints[row][col] != unmarked)
            {
                row = Random.Range(0, gridPoints.Length);
                col = Random.Range(0, gridPoints.Length);
            }

            // At this point we have found one element that isn't marked
            // Debug.Log("MARKED AN ELEMENT with element: " + row + "," + col);
            gridPoints[row][col] = marked;

            // Add to marked list to be able to spawn them 
            markedPoints.Add(new Vector2(row, col));

        }


        // Create a container for the asteroids
        GameObject asteroidsContainer = new GameObject("ASTEROID-CONTAINER");
        asteroidsContainer.transform.position = Vector3.zero;
        asteroidsContainer.transform.SetParent(this.transform);

        // Spawn all asteroids in the marked points/coordinates
        for (int i = 0; i < markedPoints.Count; i++)
        {
            // Choose random asteroid sprite
            int randNum = Random.Range(0, asteroids.Count);
            // Spawn asteroid
            GameObject asteroid = Instantiate(asteroids[randNum], asteroidRowSpawnPoints[(int)markedPoints[i].x].transform.GetChild((int)markedPoints[i].y).transform.position + vectorDepth, Quaternion.identity);

            // Enable or disable box collider
            asteroid.GetComponent<BoxCollider2D>().enabled = isBoxCollider2dEnabled;
            // Enable or disable circle collider
            asteroid.GetComponent<CircleCollider2D>().enabled = isCircleCollider2dEnabled;
            // Sets the asteroid class to either have a health or no health component to it
            Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
            asteroidComponent.SetHasHealth(doesAsteroidHaveHealth);
            
            // Sets the colour
            // asteroid.GetComponent<SpriteRenderer>().color = asteroidColour;
            // If there is a sprite renderer in that asteroid, set its colour, otherwise, set it on each child sprite renderer
            if (asteroid.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.color = asteroidColour;
            }
            else
            {
                // Debug.Log("NEED TO ADD COLOUR FOR ASTEROID 4");
                // Each child sprite of the asteroid is set to the given colour
                foreach (SpriteRenderer childSprite in asteroid.GetComponentsInChildren<SpriteRenderer>())
                {
                    childSprite.color = asteroidColour;
                }
            }

            // Set the asteroids parent under the container - cleaner to look at in the inspector during run time
            asteroid.transform.SetParent(asteroidsContainer.transform);

            if (isIconAllowed == true)
            {
                CreateMinimapIcon(asteroid.transform, Vector3.one * 0.5f);
            }
            
        }

    }

    private void CreateMinimapIcon(Transform parentGameObj, Vector3 scale)
    {
        GameObject minimapIcon = new GameObject("MinimapIcon");
        minimapIcon.transform.SetParent(parentGameObj);
        minimapIcon.transform.localPosition = Vector3.zero;
        minimapIcon.transform.localScale = scale;
        minimapIcon.AddComponent<SpriteRenderer>().sprite = asteroidMovingSprite;
        minimapIcon.layer = LayerMask.NameToLayer("Minimap");

        minimapIcon.GetComponent<SpriteRenderer>().color = minimapIconColour;
        minimapIcon.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        minimapIcon.GetComponent<SpriteRenderer>().sortingOrder = -100;

    }


    private string[][] asteroidLandscapeGrid = new string[61][];                                    // Used for building all the non visual grid stuff - later used for gameobjects
    [SerializeField] GameObject asteroidLandscapeGameObjectGrid;                                    // A container gameobject that holds a set of rows - 61x61 (2d array essentially). The first position is in -300, -300 and last position is -300, 300 in the scene
    [SerializeField] List<GameObject> asteroidLandscapeSpawnPoints = new List<GameObject>();        // The rows of the 2d grid asteroidLandscapeGameObjectGrid gameobject

    List<Vector2> asteroidLandscapeMarkedPoints = new List<Vector2>();                              // A list of vector2s which represent the marked points in the asteroidLandscapeGrid and asteroidLandscapeGameObjectGrid for detecting already marked indexes and for placing in valid ones (empty)


    private GameObject SpawnAsteroidRooms()
    {
        #region Setting up the GRID gameobject and 2d array of string

        int gridRows = asteroidLandscapeGameObjectGrid.transform.childCount;
        for (int i = 0; i < gridRows; i++)
        {
            // Add EACH ROW-n gameobject of GRID's child gameobject to a list - this list will be used for spawning asteroids
            asteroidLandscapeSpawnPoints.Add(asteroidLandscapeGameObjectGrid.transform.GetChild(i).transform.gameObject);
        }

        // Debug.Log("TEST GRID ROW COunt: " + testGRID.transform.childCount);              // Works as expected

        // Populate each ROW with 61 columns
        for (int i = 0; i < asteroidLandscapeGrid.Length; i++)
        {
            asteroidLandscapeGrid[i] = new string[61];
        }

        //Debug.Log("Number of rows count: " + testGridPoints.Length + "\nNumber of columns count: " + testGridPoints[0].Length);

        #endregion

        // Create a container for asteroid rooms 
        GameObject asteroidRoomsGameObj = new GameObject("ASTEROID-ROOMS");
        asteroidRoomsGameObj.transform.position = Vector3.zero;
        //this.asteroidRoomsGameObj = new GameObject("ASTEROID-ROOMS");
        //this.asteroidRoomsGameObj.transform.position = Vector3.zero;

        // Create container for gap asteroids and set its parent to the asteroid rooms container
        GameObject asteroidGapContainer = new GameObject("GAPS");
        asteroidGapContainer.transform.position = Vector3.zero;
        asteroidGapContainer.transform.SetParent(asteroidRoomsGameObj.transform);

        // Only name the asteroid rooms depending on the actual number of rooms created not the ith iteration
        int asteroidRoomCreated = 0;

        // Create number of asteroid rooms in a 2d array - Attempts to create no more than 7 asteroid rooms
        int numberOfAsteroidRooms = 7;      // Probably at higher stage levels decrease the rooms and put more enemies in each room
        for (int i = 0; i < numberOfAsteroidRooms; i++)
        {
            // Attempts to create n asteroid rooms
            AsteroidRoom asteroidRoom = new AsteroidRoom(asteroidLandscapeGrid, "ROOM-1", "ROOM-1-EDGES", 5);
            // Add all the marked points
            asteroidLandscapeMarkedPoints.AddRange(asteroidRoom.GetAsteroidRoomMarkedPoints());

            //Debug.LogError("can create room?: " + asteroidRoom.GetCanCreate());
            // Skip creating asteroid room container if we cannot create
            if (asteroidRoom.GetCanCreate() == false) { continue; }

            // Create a container for the asteroids - asteroid room
            GameObject asteroidRoomGameObj = new GameObject("ASTEROID-ROOM-" + asteroidRoomCreated);
            asteroidRoomGameObj.transform.position = Vector3.zero;
            asteroidRoomGameObj.transform.SetParent(asteroidRoomsGameObj.transform);

            asteroidRoomCreated += 1;

            // After creating the asteroid room in 2d array, create them in the scene
            for (int j = 0; j < asteroidRoom.GetAsteroidRoomMarkedPoints().Count; j++)
            {

                int randNum = Random.Range(0, asteroids.Count);

                // Spawn asteroid in each marked points
                GameObject asteroid = Instantiate(asteroids[randNum],
                    asteroidLandscapeSpawnPoints[(int)asteroidRoom.GetAsteroidRoomMarkedPoints()[j].x].transform.GetChild((int)asteroidRoom.GetAsteroidRoomMarkedPoints()[j].y).transform.position +
                    new Vector3(0f, 0f, 0f), Quaternion.identity);

                // Set its name to either be FILLED or CENTER according to what what stored in the grid
                asteroid.gameObject.name = asteroidLandscapeGrid[(int)asteroidRoom.GetAsteroidRoomMarkedPoints()[j].x][(int)asteroidRoom.GetAsteroidRoomMarkedPoints()[j].y];

                asteroid.transform.SetParent(asteroidRoomGameObj.transform);

                // Stops it moving 1
                asteroid.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                // Stops it moving 2
                Asteroid asteroid1 = asteroid.gameObject.GetComponent<Asteroid>();
                asteroid1.SetLowerLimit(0f);
                asteroid1.SetUpperLimit(0f);

                // Turn off filled asteroids or the center to make a hollow asteroid room
                //if (asteroid.gameObject.name == "FILLED-HIDDEN" || asteroid.gameObject.name == "CENTER")
                if (asteroid.gameObject.name == "FILLED-HIDDEN" || asteroid.gameObject.name.Substring(0, 6).Equals("CENTER"))
                {
                    asteroid.gameObject.SetActive(false);
                    //if (asteroid.gameObject.name == "CENTER")
                    if (asteroid.gameObject.name.Substring(0, 6).Equals("CENTER"))
                    {
                        this.asteroidRoomCenterGameObjs.Add(asteroid.gameObject);
                    }
                }


                // Set its size
                // asteroid.transform.localScale += new Vector3(40f, 40f, 0f);

                asteroid.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));

                // Create a minimap icon for the rooms
                CreateMinimapIcon(asteroid.transform, Vector3.one * 0.5f);

            }

            // After an asteroid room is completed, add gameobjects around it
            // FillAsteroidRoomEdgeGaps(asteroidRoomGameObj, asteroidGapContainer);

        }

        return asteroidRoomsGameObj;

    }

    #region Corridor Spawning

    private void SpawnCorridors(List<GameObject> hamiltonianPath, string[][] grid, GameObject asteroidRoom)
    {
        this.asteroidLandscapeMarkedPoints.Clear();

        // Need the 2d grid, the list of asteroids in order the path

        GameObject corridorContainer = new GameObject("Corridors");
        corridorContainer.transform.position = Vector3.zero;

        // locate each "CENTER-" gameobjects in the given 2d grid
        int totalCorridors = hamiltonianPath.Count - 1;
        for (int i = 0; i < totalCorridors; i++)
        {
            List<int> roomI_Indexes = FindCenter(hamiltonianPath[i], grid);
            List<int> roomI2_Indexes = FindCenter(hamiltonianPath[i + 1], grid);

            int corridorRowIndex = roomI_Indexes[0];
            int corridorColIndex = roomI_Indexes[1];

            int corridor2RowIndex = roomI2_Indexes[0];
            int corridor2ColIndex = roomI2_Indexes[1];

            int indexColMove = 0;
            int newMoveColIndex = corridorColIndex;             // Set index to the column index
            int totalMoveOfLeftOrRightInGrid = Mathf.Abs(roomI_Indexes[1] - roomI2_Indexes[1]);

            #region Moving in the 2d grid left or right

            for (int z = 0; z < totalMoveOfLeftOrRightInGrid; z++)
            {
                // Compare roomA position to room2 position - to determine whether we move left or right in the 2d array


                if (corridorColIndex < corridor2ColIndex)
                {
                    // Move column to right
                    indexColMove = 1;
                }
                else if (corridorColIndex > corridor2ColIndex)
                {
                    // Move column to the left
                    indexColMove = -1;
                }
                else if (corridorColIndex == corridor2ColIndex)
                {
                    // Stay in the same column
                    indexColMove = 0;
                }

                // Move current roomA's position with the determined col index move
                newMoveColIndex += indexColMove;

                //Debug.Log("Original index col pos: " + roomI_Indexes[1] + "\nNext Room index col pos: " + roomI2_Indexes[1] + "\nNew move index col pos: " + newMoveColIndex);

                //Debug.Log("Element at: " + roomI_Indexes[0] + ", " + newMoveColIndex +
                //    "\nAnd it contains: " + grid[roomI_Indexes[0]][newMoveColIndex]);

                // Placing the corridor-hidden
                /*
                 * Place this X and make it hidden:
                 * 
                 *  __               __
                 * |  X      to     |   
                 * |__              |__
                 */
                // If it's empty or there is a corridor wall in the way (for corridors that overlap each other)
                if (string.IsNullOrEmpty(grid[roomI_Indexes[0]][newMoveColIndex]) ) 
                {
                    // If the grid with given index is empty, then start to fill it with the name CORRIDOR

                    // Mark to the grid
                    // Create asteroid and make it hidden in the scene
                    CreateAsteroid(grid, roomI_Indexes[0], newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform).SetActive(false);

                    // When there is a movement in left or right, fill its surroundings (top row and bottom row) with "CORRIDOR-FILL"
                    // If bottom or top of current column position is empty, then fill it
                    // if at last iteration
                    if (Mathf.Abs(z - totalMoveOfLeftOrRightInGrid) == 1)
                    {
                        // Check if movement is downwards of row move
                        // Movement of row is upwards
                        if (corridorRowIndex > corridor2RowIndex)
                        {
                            /* Removes this X by not placing one in the first place
                             * | X |____________
                             * |                |
                             * |________________|
                             */


                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex, "CORRIDOR-EDGE", "", corridorContainer.transform);
                            }
                        }
                        else if (corridorRowIndex < corridor2RowIndex)
                        {
                            // Movement of row is downwards
                            /* In the comment, it removes the X in the way
                             * 
                             * Removes the X by not placing one in the first place - see else statement where it places both
                             *  ________________
                             * |    ____________|
                             * | X |
                             */

                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex, "CORRIDOR-EDGE", "", corridorContainer.transform);
                            }
                        }
                    } // If not last iteration, then continue to fill both up and down edges
                    else
                    {
                        /*
                         *  X = corridor 
                         *  _ OR | = corridor edges
                         *  
                         *  Step 1: Fill corridor
                         *  
                         *      X
                         *  Step 2: Add corridor edges and make the corridor stay hidden
                         *      _               _
                         *      X       to 
                         *      _               _
                         *  Step 3: Repeat step 2 until we finish filling the column movement
                         *      ____            ____
                         *      XXXX    to 
                         *      ____            ____
                         * 
                         * IN THIS ELSE STATEMENT, we are at STEP 2
                         */

                        if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex]))
                        {
                            CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);

                        }
                        if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex]))
                        {
                            CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);
                        }
                    }

                    // If at last iteration
                    if (Mathf.Abs(z - totalMoveOfLeftOrRightInGrid) == 1)
                    {
                        // Check if index col move is left or right
                        if (indexColMove == -1)
                        {
                            // If left
                            // Populate the surroundings again
                            /*
                             * Before placing new asteroids this is what it would look like for the generated corridor
                             * 
                             * Our indexColMove is negative so it means our last previous move was moving left and the corridor would loo like 
                             * this below:
                             * 
                             * |   |
                             *   _ |_____
                             *           |
                             *   ________|
                             *   
                             * For indexColMove that is 1 (positive) the corridor would be:
                             *       |   |
                             *  _____| _  
                             * |          
                             * |________  
                             *   
                             * We see that the corners do not have walls, the 3 ifs and the else if below solves that
                             */

                            // Fill the top left corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex - 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex - 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }
                            // Fill left corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0]][newMoveColIndex - 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0], newMoveColIndex - 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }
                            // Fill bottom left corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex - 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex - 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }

                        }
                        else if (indexColMove == 1)
                        {
                            // Fill top right corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex + 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex + 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }
                            // Fill right corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0]][newMoveColIndex + 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0], newMoveColIndex + 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }
                            // Fill bottom right corner
                            if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex + 1]))
                            {
                                CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex + 1, "CORRIDOR-CORNER-FILL", "", corridorContainer.transform);
                            }

                        }
                    }
                }

                // FINDING OVERLAPPING ASTEROID - FOR WHEN MOVING LEFT OR RIGHT
                else if (grid[roomI_Indexes[0]][newMoveColIndex] == "CORRIDOR-FILL" || grid[roomI_Indexes[0]][newMoveColIndex] == "CORRIDOR-CORNER-FILL" || grid[roomI_Indexes[0]][newMoveColIndex] == "CORRIDOR-EDGE") 
                {
                    GameObject asteroid = CreateAsteroid(grid, roomI_Indexes[0], newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform);
                    asteroid.SetActive(false);

                    GameObject overlappingAsteroid = FindOverlappingAsteroidCorridor(asteroid, corridorContainer);
                    if (overlappingAsteroid != null)
                    {
                        overlappingAsteroid.SetActive(false);
                    }

                    // NEED TO ADD WALLS AROUND IT IF THEY ARE EMPTY - IF UP OR DOWN OF HIDDEN ASTEROID is empty in GRID, then add it
                    if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex]) )
                    {
                        CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                    if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex] ))
                    {
                        CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                }


                // IF asteroid is FILLED, THEN overwrite it with corridor (then set to false BOTH OVERLAPPING GAMEOBJECTS WITH SAME POSITION),
                        // THEN CHECK FOR ITS SURROUNDINGS WHEN WE OVERWRITE IT WITH "CORRIDOR-", CHECK FOR SURROUNDINGS SUCH AS IS 
                        // the surrounding (is its UP an EMPTY?) or is its DOWN empty? row - 1 or row + 1 - REMEMEMBER THIS IS FOR THE COLUMN MOVEMENT IF SO GO FILL IT
                
                /*   BEFORE (Room is closed)    AFTER (Open the room up)
                 *        |   |                 |   |
                 *  ______|___|           ______| X |  
                 * |          |          |          |
                 * |  ROOM i  |          |  ROOM i  |    
                 * |          |          |          |
                 * |__________|          |__________|
                 * 
                 */

                else if (grid[roomI_Indexes[0]][newMoveColIndex] == "FILLED") // CHECK IF WE CAN REMOVE THE ASTEROID ROOM WALLS BY USING "FILLED"
                {
                    GameObject asteroid = CreateAsteroid(grid, roomI_Indexes[0], newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform);
                    asteroid.SetActive(false);

                    GameObject overlappingAsteroid = FindOverlappingAsteroidRoomFill(asteroid, asteroidRoom);
                    if (overlappingAsteroid != null)
                    {
                        overlappingAsteroid.SetActive(false);
                    }

                    // NEED TO ADD WALLS AROUND IT IF THEY ARE EMPTY - IF UP OR DOWN OF HIDDEN ASTEROID is empty in GRID, then add it
                    if (string.IsNullOrEmpty(grid[roomI_Indexes[0] - 1][newMoveColIndex]))
                    {
                        CreateAsteroid(grid, roomI_Indexes[0] - 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                    if (string.IsNullOrEmpty(grid[roomI_Indexes[0] + 1][newMoveColIndex]))
                    {
                        CreateAsteroid(grid, roomI_Indexes[0] + 1, newMoveColIndex, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                }
            }

            // DON'T DELETE THIS REGION
            #endregion

            int indexRowMove = 0;
            int newMoveRowIndex = corridorRowIndex;
            // Our total move for up and down in the grid for this current corridor built between room i and room i+1
            int totalMoveUpOrDownInGrid = Mathf.Abs(roomI_Indexes[0] - roomI2_Indexes[0]);

            #region Move Up or Down in the grid

            /*
             * At this point we now have a corridor like this:
             *  ________________   
             * |                    |
             * |____________________|
             * 
             * And we want to move up or down, in the generated corridor we move up, so our end goal would be like this:
             *              |  ROOM i+1  |
             *              |___ ___ ____|
             *                  |   |
             *                  |   |
             *  ________________|   |
             * |                    |
             * |____________________|
             * 
             * To know if we move up or down we compare the row index of the room i to the row index of the room i+1
             * 
             * The for loop below achieves complete the corridor between ROOM i and ROOM i+1
             */

            for (int z = 0; z < totalMoveUpOrDownInGrid; z++)
            {
                if (corridorRowIndex < corridor2RowIndex)
                {
                    // Move row downwards
                    indexRowMove = 1;
                }
                else if (corridorRowIndex > corridor2RowIndex)
                {
                    // Move row upwards
                    indexRowMove = -1;
                }
                else if (corridorRowIndex == corridor2RowIndex)
                    indexRowMove = 0; // Stay in the same row

                // Move current grid position with the determiend row index move
                newMoveRowIndex += indexRowMove;

                // Since we already have the newMoveColIndex set up we use that here - PLACEMENT OF X (see comment diagram)
                if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex])) 
                {
                    // Create the X (asteroid) see the comments below
                    // Create asteroid and make the asteroid hidden in the scene
                    CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform).SetActive(false);

                    // If left and right column are empty while placing the row asteroids, go place the edges
                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex + 1]))
                    {
                        /*
                         *  Placing this:
                         *  
                         *   EMPTY          to          X       to       X |       to         |
                         */

                        // Place right
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex + 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }

                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex - 1]))
                    {
                        // Place left
                        /*
                         * Placing this:
                         *  
                         *   EMPTY          to          X       to      | X         to    |
                         */
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex - 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }

                    // if there is "CORRIDOR-FILL" then remove
                    //if ( )

                }

                // FINDING OVERLAPPING ASTEROID - FOR WHEN MOVING UP OR DOWN
                else if (grid[newMoveRowIndex][newMoveColIndex] == "CORRIDOR-FILL" || grid[newMoveRowIndex][newMoveColIndex] == "CORRIDOR-CORNER-FILL" || grid[newMoveRowIndex][newMoveColIndex] == "CORRIDOR-EDGE" )
                {
                    // Find a corridor that has the same position as this and turn that off too
                    GameObject asteroid = CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform);
                    asteroid.SetActive(false);

                    // If we find an overlapping asteroid then make it hidden
                    GameObject overlappingAsteroid = FindOverlappingAsteroidCorridor(asteroid, corridorContainer);
                    if (overlappingAsteroid != null)
                    {
                        overlappingAsteroid.SetActive(false);
                    }

                    // NEED TO CHECK SIDES IF THEY ARE EMPTY IN GRID
                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex - 1]))
                    {   // Check left
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex - 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex + 1]))
                    {   // Check right
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex + 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                }

                // FOR ATTEMPTING TO OPEN THE ASTEROID ROOMS
                else if (grid[newMoveRowIndex][newMoveColIndex] == "FILLED")
                {
                    // Find a corridor that has the same position as this and turn that off too
                    GameObject asteroid = CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex, "CORRIDOR-" + i.ToString(), "", corridorContainer.transform);
                    asteroid.SetActive(false);

                    // If we find an overlapping asteroid room fill then make it hidden
                    GameObject overlappingAsteroid = FindOverlappingAsteroidRoomFill(asteroid, asteroidRoom);
                    if (overlappingAsteroid != null)
                    {
                        overlappingAsteroid.SetActive(false);
                    }

                    // NEED TO CHECK SIDES IF THEY ARE EMPTY IN GRID
                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex - 1]))
                    {   // Check left
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex - 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                    if (string.IsNullOrEmpty(grid[newMoveRowIndex][newMoveColIndex + 1]))
                    {   // Check right
                        CreateAsteroid(grid, newMoveRowIndex, newMoveColIndex + 1, "CORRIDOR-FILL", "", corridorContainer.transform);
                    }
                }
            }


            #endregion
        }
    }

    private GameObject FindOverlappingAsteroidCorridor(GameObject asteroid, GameObject corridorContainer)
    {
        for (int i = 0; i < corridorContainer.transform.childCount; i++)
        {
            if (corridorContainer.transform.GetChild(i).transform.position == asteroid.transform.position)
            {
                return corridorContainer.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }

    // Allows exposing the rooms open
    private GameObject FindOverlappingAsteroidRoomFill(GameObject asteroid, GameObject asteroidRoom)
    {

        //Debug.LogError("asteroid: " + asteroidRoom.transform.GetChild(1).gameObject.name);

        // For each asteroidRooms in this container
        for (int i = 1; i < asteroidRoom.transform.childCount; i++)
        {
            // For each asteroid in each asteroid room
            for (int j = 0; j < asteroidRoom.transform.GetChild(i).transform.childCount; j++)
            {

                // Gets the gameobject "FILLED" in one of the "ASTEROID-ROOM-n"
                if (asteroidRoom.transform.GetChild(i).transform.GetChild(j).transform.position == asteroid.transform.position)
                {
                    return asteroidRoom.transform.GetChild(i).transform.GetChild(j).gameObject;
                }
            }
        }

        return null;
    }

    

    /// <summary>
    /// Find the center game object's index position in the grid using the "CENTER" gameobject's name
    /// </summary>
    /// <param name="center"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private List<int> FindCenter(GameObject center, string[][] grid)
    {
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {

                if (center.name.Equals(grid[i][j]))
                {
                    // Return the index
                    return new List<int>() { i, j };
                }
            }
        }

        return null;
    }


    private GameObject CreateAsteroid(string[][] grid, int rowIndex, int colIndex, string elementName, string movementType, Transform parentTransform)
    {
        grid[rowIndex][colIndex] = elementName;
        this.asteroidLandscapeMarkedPoints.Add(new Vector2(rowIndex, colIndex));

        GameObject asteroid = Instantiate(asteroids[0],
            asteroidLandscapeSpawnPoints[rowIndex].transform.GetChild(colIndex).transform.position,
            Quaternion.identity);
        asteroid.name = elementName;
        asteroid.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        // Set parent
        asteroid.transform.SetParent(parentTransform);

        // Add a minimap icon to this gameobj
        if (elementName == "CORRIDOR-FILL" || elementName == "CORRIDOR-CORNER-FILL" || elementName == "CORRIDOR-EDGE")
        {
            CreateMinimapIcon(asteroid.transform, Vector3.one * 0.5f);
        }
        


        return asteroid;
    }

    #endregion

    #region Exit Door Spawning

    private void ExposeExitRoom(GameObject asteroidRoom, string[][] grid, List<GameObject> hamiltonianPath)
    {
        // In final room, find an asteroid "FILLED" gameobject and in the grid that has no gameobject that has a name of
        // "CORRIDOR-FILL" or "ANY "CORRIDOR" in a 1 index wide
        /*
         * Z = chosen asteroid "FILLED" gameobject
         * C = Check if this index in the 2d grid IS NOT ANY OF "CORRIDOR" AND is empty or FILLED-HIDDEN or FILLED
         *  ____
         * |   CCC 
         * |___CZC
         *     CCC    
         */

        // Spawn, a top column exit asteroid door - the roow stays the same but the column starts at the very top left and moves to the right corner by 1 per iteration
        SpawnExitDoor(asteroidRoom, grid, hamiltonianPath, -5, -5, 0, 0, 0, 1);
        // Spawn, if possible, a bottom column exit asteroid door - here row stays the same, the column starts to the very left and increase by 1
        SpawnExitDoor(asteroidRoom, grid, hamiltonianPath, 5, -5, 0, 0, 0, 1);
        // Spawn a left row exit asteroid door - row does not stay the same and starts and the top left and moves down to the bottom. Column (very left) does not change
        SpawnExitDoor(asteroidRoom, grid, hamiltonianPath, -5, -5, 0, 0, 1, 0);
        // Spawn a right row exit asteroid door - row changes at each iteration to reach the bottom and find a random possible exit. Column (very right) does not change
        SpawnExitDoor(asteroidRoom, grid, hamiltonianPath, -5, 5, 0, 0, 1, 0);
    }

    private void SpawnExitDoor(GameObject asteroidRoom, string[][] grid, List<GameObject> hamiltonianPath, int rowAdd, int colAdd, int rowI, int colI, int rowIIncreaser, int colIIncreaser)
    {
        int roomwidthlength = 11;
        GameObject lastRoom = hamiltonianPath[hamiltonianPath.Count - 1];
        List<int> lastRoomCenterIndexes = FindCenter(lastRoom, grid);

        // Row index of the last room's center
        int rowIndex = lastRoomCenterIndexes[0];
        // Column index of the last room's center
        int columnIndex = lastRoomCenterIndexes[1];

        // For the top row's all possible indexes, then pick in one of them
        List<Vector2> exitTopRowColIndexes = new List<Vector2>();

        // Check all of top row and its columns asteroids
        for (int i = 0; i < roomwidthlength; i++)
        {

            // Change values according to the given parameter
            int newRowIndex = (rowIndex + rowAdd) + rowI;
            //int newColumnIndex = (columnIndex - 5) + i;
            int newColumnIndex = (columnIndex + colAdd) + colI;

            //Debug.Log("top row index: " + newRowIndex + " HELLO Top col index: " + newColumnIndex);

            // SCAN this indexes in the grid if they contain "FILLED"
            if (grid[newRowIndex][newColumnIndex] == "FILLED")
            {
                //Debug.LogError("is filled");

                if (IsPositionAllowed(grid, newRowIndex, newColumnIndex) == true)
                {
                    //Debug.LogError("position IS ALLOWED");
                    exitTopRowColIndexes.Add(new Vector2(newRowIndex, newColumnIndex));
                }
                else { //Debug.Log("not allowed: " + newRowIndex + " | " + newColumnIndex);
                      }

            }

            // Define the increaser in the caller parameter
            rowI = rowI + rowIIncreaser;
            colI = colI + colIIncreaser;

        }

        // Guard
        if (exitTopRowColIndexes.Count <= 0) { return; }

        // FIND ALL POSSIBLE VALID asteroids IN TOP ROW, THEN RANDOMLY PICK ONE OF THEM
        Vector2 chosenRowColIndexes = exitTopRowColIndexes[Random.Range(0, exitTopRowColIndexes.Count)];

        // Spawn asteroid exit door and turn it off
        GameObject exitDoor = new GameObject("EXIT-DOOR-TOP-ROW-0");
        exitDoor.transform.position = asteroidLandscapeSpawnPoints[(int)chosenRowColIndexes[0]].transform.GetChild((int)chosenRowColIndexes[1]).transform.position;
        exitDoor.SetActive(false);

        // Turn off "FILLED" gameobject in the same index as the exit door
        GameObject overlappingAsteroid = FindOverlappingAsteroidRoomFill(exitDoor, asteroidRoom);
        if (overlappingAsteroid != null)
        {
            overlappingAsteroid.SetActive(false);
        }
    }

    private bool IsPositionAllowed(string[][] grid, int rowIndex, int colIndex)
    {
        List<bool> numberOfIssues = new List<bool>();
        bool isValid = false;
        // Check top left corner
        if (string.IsNullOrEmpty(grid[rowIndex - 1][colIndex - 1]) || grid[rowIndex - 1][colIndex - 1] == "FILLED-HIDDEN" || grid[rowIndex - 1][colIndex - 1] == "FILLED")
        {
            // If empty, do nothing
        }
        else
        {
            numberOfIssues.Add(true);
            //Debug.Log("added issue at indexes: " + (rowIndex - 1) + " | " + (colIndex - 1));
        }


        // Check top
        if (string.IsNullOrEmpty(grid[rowIndex - 1][colIndex]) || grid[rowIndex - 1][colIndex] == "FILLED-HIDDEN" || grid[rowIndex - 1][colIndex] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check top right corner
        if (string.IsNullOrEmpty(grid[rowIndex - 1][colIndex + 1]) || grid[rowIndex - 1][colIndex + 1] == "FILLED-HIDDEN" || grid[rowIndex - 1][colIndex + 1] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check left
        if (string.IsNullOrEmpty(grid[rowIndex][colIndex - 1]) || grid[rowIndex][colIndex - 1] == "FILLED-HIDDEN" || grid[rowIndex][colIndex - 1] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check right
        if (string.IsNullOrEmpty(grid[rowIndex][colIndex + 1]) || grid[rowIndex][colIndex + 1] == "FILLED-HIDDEN" || grid[rowIndex][colIndex + 1] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check bottom left corner
        if (string.IsNullOrEmpty(grid[rowIndex + 1][colIndex - 1]) || grid[rowIndex + 1][colIndex - 1] == "FILLED-HIDDEN" || grid[rowIndex + 1][colIndex - 1] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check bottom
        if (string.IsNullOrEmpty(grid[rowIndex + 1][colIndex]) || grid[rowIndex + 1][colIndex] == "FILLED-HIDDEN" || grid[rowIndex + 1][colIndex] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // Check bottom right corner
        if (string.IsNullOrEmpty(grid[rowIndex + 1][colIndex + 1]) || grid[rowIndex + 1][colIndex + 1] == "FILLED-HIDDEN" || grid[rowIndex + 1][colIndex + 1] == "FILLED") { }
        else
            numberOfIssues.Add(true);

        // DECIDER - check if count is more than 0, if so, then this position is not valid
        if (numberOfIssues.Count > 0)
            isValid = false;
        else if (numberOfIssues.Count <= 0)
            isValid = true;

        //Debug.LogError("Is position valid: " + isValid + "\nissue count: " + numberOfIssues.Count);

        return isValid;
    }

    #endregion

    #region Asteroid Room Gap Fill

    private void CreateAllAsteroidRoomGaps(GameObject asteroidRooms)
    {
        for (int i = 1; i < asteroidRooms.transform.childCount; i++)
        {
            FillAsteroidRoomGaps(asteroidRooms, i-1);
        }
    }

    private void FillAsteroidRoomGaps(GameObject asteroidRooms, int index)
    {
        // A list of all ASTEROID-ROOM-n
        List<GameObject> allAsteroidRooms = new List<GameObject>();

        for (int i = 1; i < asteroidRooms.transform.childCount; i++)
        {
            // Start at i = 1 to avoid looking at "GAPS" gameobject container
            // Add ASTEROID-ROOM-n gameobjects to the list
            allAsteroidRooms.Add(asteroidRooms.transform.GetChild(i).gameObject);
        }


        // For the first room, check the top row
        // All rooms have 11x11 asteroids, some are visible some are hidden in the scene
        int asteroidRoomLengthWidth = 11;
        // All ASTEROID-ROOM-n has a child at index 0 a gameobject called "CENTER", we need to ignore this
        int startingIndex = 1;
        int lastIndex = 11 * 11; // 121
        
 

        for (int i = 0; i < asteroidRoomLengthWidth-1; i++)
        {
           // Debug.Log(allAsteroidRooms[0].name);

            // Get all the "FILLED" gameobjects under ASTEROID-ROOM-n container gameobject
            allAsteroidRooms[index].transform.GetChild(i + startingIndex);

            // TOP ROW AND ITS COLUMNS
            // If current gameobject index and next gameobject index is setactive status to true then allow this to add one gameobject TO THE RIGHT
            if (allAsteroidRooms[index].transform.GetChild(i + startingIndex).gameObject.activeSelf == true &&
                allAsteroidRooms[index].transform.GetChild((i + startingIndex) + 1).gameObject.activeSelf == true)
            {
                // If so, populate a gap gameobject from this position +5units in x
                GameObject gap = Instantiate(asteroids[0],
                    allAsteroidRooms[index].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f),
                    Quaternion.identity);
                //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                gap.name = "GAP";
                gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                // Create minimap icon for this gap gameobj
                CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
            }

            // BOTTOM ROW AND ITS COLUMNS - check current asteroid and its neighbouring right if active, if so, place an asteroid in between them, otherwise do not
            if (allAsteroidRooms[index].transform.GetChild( (lastIndex-asteroidRoomLengthWidth) + i).gameObject.activeSelf == true &&
                allAsteroidRooms[index].transform.GetChild( ((lastIndex - asteroidRoomLengthWidth) + i) + 1).gameObject.activeSelf == true)
            {
                // If so, populate a gap gameobject from this position +5units in x
                GameObject gap = Instantiate(asteroids[0],
                    allAsteroidRooms[index].transform.GetChild( (lastIndex-asteroidRoomLengthWidth) + i).transform.position + new Vector3(5f, 0f, 0f),
                    Quaternion.identity);
                //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                gap.name = "GAP";
                gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);
                // Create minimap icon for this gap gameobj
                CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
            }

        }



        int leftRowI = 0;
        int leftRowIAdder = 11;

        int newRowI = 0;
        int newRowIAdder = 11;

        // For Left column and its rows
        for (int i = 0; i < asteroidRoomLengthWidth-1; i++)
        {
            // 1 and 12
            // 13 and 23
            // (Every 11) and so on...
            if (i < 5)
            {
                int newCurrentLeft = (startingIndex + leftRowI);
                int newNextCurrentLeft = (startingIndex + asteroidRoomLengthWidth) + leftRowI;

               //Debug.Log("column: " + newCurrentLeft + " | " + newNextCurrentLeft + " at i: " + i);

                if (allAsteroidRooms[index].transform.GetChild(newCurrentLeft).gameObject.activeSelf == true &&
                     allAsteroidRooms[index].transform.GetChild(newNextCurrentLeft).gameObject.activeSelf == true)
                {
                    //Debug.Log("HELLO");

                    // If so, populate a gap gameobject from this position +5units in x
                    GameObject gap = Instantiate(asteroids[0],
                        allAsteroidRooms[index].transform.GetChild(newCurrentLeft).transform.position + new Vector3(0f, -5f, 0f),
                        Quaternion.identity);
                    //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }


                leftRowI = leftRowI + leftRowIAdder;
            }
            else if (i == 5)
            {
                int newCurrentLeft = (56);
                int newNextCurrentLeft = 66;

                if (allAsteroidRooms[index].transform.GetChild(newCurrentLeft).gameObject.activeSelf == true &&
                    allAsteroidRooms[index].transform.GetChild(newNextCurrentLeft).gameObject.activeSelf == true)
                {
                    //Debug.Log("HELLO");

                    // If so, populate a gap gameobject from this position +5units in x
                    GameObject gap = Instantiate(asteroids[0],
                        allAsteroidRooms[index].transform.GetChild(newCurrentLeft).transform.position + new Vector3(0f, -5f, 0f),
                        Quaternion.identity);
                    //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }

                leftRowI = 66;

            }
            else if (i > 5)
            {
                int newStartIndex = 66;
                int newCurrentLeft = (newStartIndex + newRowI);
                int newNextCurrentLeft = (newStartIndex + asteroidRoomLengthWidth) + newRowI;

                if (allAsteroidRooms[index].transform.GetChild(newCurrentLeft).gameObject.activeSelf == true &&
                    allAsteroidRooms[index].transform.GetChild(newNextCurrentLeft).gameObject.activeSelf == true)
                {
                    // Debug.Log("HELLO");

                    // If so, populate a gap gameobject from this position +5units in x
                    GameObject gap = Instantiate(asteroids[0],
                        allAsteroidRooms[index].transform.GetChild(newCurrentLeft).transform.position + new Vector3(0f, -5f, 0f),
                        Quaternion.identity);
                    //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }

                newRowI = newRowI + newRowIAdder;
            }
        }


        int rightRowI = 0;
        int rightRowIAdder = 11;
        int newRightRowI = 0;
        int rightStartingIndex = 11;
        
        // For Right column and its rows
        for (int i = 0; i < asteroidRoomLengthWidth-1; i++)
        {
            // 11 (count including this number) and 22
            // 23 and 33
            if (i < 4)
            {
                int newCurrentLeft = (rightStartingIndex + rightRowI);
                int newNextCurrentLeft = (rightStartingIndex + asteroidRoomLengthWidth) + rightRowI;

                //Debug.Log("column: " + newCurrentLeft + " | " + newNextCurrentLeft + " at i: " + i);

                if (allAsteroidRooms[index].transform.GetChild(newCurrentLeft).gameObject.activeSelf == true &&
                     allAsteroidRooms[index].transform.GetChild(newNextCurrentLeft).gameObject.activeSelf == true)
                {
                    //Debug.Log("HELLO");

                    // If so, populate a gap gameobject from this position +5units in x
                    GameObject gap = Instantiate(asteroids[0],
                        allAsteroidRooms[index].transform.GetChild(newCurrentLeft).transform.position + new Vector3(0f, -5f, 0f),
                        Quaternion.identity);
                    //gap.transform.position = allAsteroidRooms[0].transform.GetChild(i + startingIndex).transform.position + new Vector3(5f, 0f, 0f);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    // Set parent to "GAPS" gameobject under "ASTEROID-ROOMS"
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }


                rightRowI = rightRowI + leftRowIAdder;
            }
            else if (i == 4)
            {
                int middle = 55;
                int nextAfterMiddle = middle + 10;
                //Debug.Log("rightrowi: " + rightRowI);
                //gapPoints.Add(allAsteroidRooms[0].transform.GetChild(65).gameObject);

                if (allAsteroidRooms[index].transform.GetChild(middle).gameObject.activeSelf == true &&
                    allAsteroidRooms[index].transform.GetChild(nextAfterMiddle).gameObject.activeSelf == true)
                {
                    GameObject gap = Instantiate(asteroids[0],
                         allAsteroidRooms[index].transform.GetChild(middle).transform.position + new Vector3(0f, -5f, 0f),
                            Quaternion.identity);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }
            }
            else if (i > 4)
            {
                int newStartIndexRight = 65;

                //gapPoints.Add(allAsteroidRooms[0].transform.GetChild(newStartIndexRight + newRightRowI).gameObject);

                int newRight = newStartIndexRight + newRightRowI;
                int newNextRight = (newStartIndexRight + rightRowIAdder) + newRightRowI;

                if (allAsteroidRooms[index].transform.GetChild(newRight).gameObject.activeSelf == true &&
                    allAsteroidRooms[index].transform.GetChild(newNextRight).gameObject.activeSelf == true)
                {
                    GameObject gap = Instantiate(asteroids[0],
                allAsteroidRooms[index].transform.GetChild(newRight).transform.position + new Vector3(0f, -5f, 0f),
                Quaternion.identity);
                    gap.name = "GAP";
                    gap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    gap.transform.SetParent(asteroidRooms.transform.GetChild(0).transform);

                    CreateMinimapIcon(gap.transform, Vector3.one * 0.5f);
                }

                newRightRowI = newRightRowI + rightRowIAdder;
            }

        }



    }

    [SerializeField] List<GameObject> gapPoints = new List<GameObject>();
    #endregion

}
