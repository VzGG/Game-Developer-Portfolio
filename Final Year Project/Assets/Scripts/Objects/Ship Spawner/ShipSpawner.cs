using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] bool canCallEnemyOnLoad = false;               // Use to call the canCallEnemyOnLoad method ONLY ONCE IN THE GAME
    [SerializeField] GameObject targetLockOnGameObj;                // Target LockOn prefab - passed to the enemy to reveal who is target locked on! APPLIES FOR WEAPON TYPE 2 ONLY!
    [SerializeField] GameObject playerScanner;
    [SerializeField] GameObject enemyUICanvas;
    [SerializeField] Sprite playerMinimapIcon;
    [SerializeField] Color playerShipColour;
    [SerializeField] Color enemyShipColour;
    [SerializeField] GameObject projectile;                         // Projectile prefab - for enemy
    [SerializeField] GameObject scanner;                            // Scanner prefab for fitting to the enemy
    [SerializeField] GameObject projectilePlayer;

    [SerializeField] ShipBuilder shipBuilder;
    [SerializeField] ShipComponentsCreator shipComponentsCreator;
    [SerializeField] RRHC_Enemy rrhc_Enemy;

    [SerializeField] List<EnemyShip> enemyShips = new List<EnemyShip>();

    [SerializeField] List<EnemyShip> enemyShipsForLevel1 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel2 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel3 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel4 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel5 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel6 = new List<EnemyShip>();
    [SerializeField] List<EnemyShip> enemyShipsForLevel7 = new List<EnemyShip>();

    [SerializeField] List<GameObject> enemyContainersForEachLevel = new List<GameObject>();

    [SerializeField] int currentLevel = 1;

    [SerializeField] AsteroidSpawner asteroidSpawner;

    private int weightCapacity = 12000;


    public void CreateAndActivatePlayer()
    {
        GameObject playerShipGameObj = CreatePlayerVisualShip();

        // Create minimap
        CreateMinimapIcon(playerShipGameObj.transform, this.playerMinimapIcon, this.playerShipColour);
    }

    private void CreateMinimapIcon(Transform parentGameObj, Sprite givenSprite, Color givenColour)
    {
        GameObject minimapIcon = new GameObject("MinimapIcon");
        minimapIcon.transform.SetParent(parentGameObj.transform);
        minimapIcon.transform.localPosition = Vector3.zero;
        minimapIcon.transform.localScale = Vector3.one * 3f;

        minimapIcon.gameObject.layer = LayerMask.NameToLayer("Minimap");
        minimapIcon.AddComponent<SpriteRenderer>().sprite = givenSprite;
        minimapIcon.GetComponent<SpriteRenderer>().color = givenColour;
        minimapIcon.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        minimapIcon.GetComponent<SpriteRenderer>().sortingOrder = -100;
    }

    public void CreateAndActivateEnemies()
    {
        //int numberOfLevels = 7;
        int numberOfLevels = 3;
        // Create the non visual ships - repeat this 7 times - add it to set of enemies per level
        for (int i = 0; i < numberOfLevels; i++)
        {
            // Create set of enemies per level that are optimized - as in the highest average randomly generated stat multiplier of the set of enemies for a level it can get
            CreateLevelsEnemyShips(i);
        }

        // Activate the only set of enemies in that level
        ActivateGameObjectActiveOnLevel(currentLevel);
        SpawnEnemyForThisLevelRandomlyInEachRoom(currentLevel, asteroidSpawner.GetOptimalHamiltonianPath());
        SpawnEnemyOutsideTheRoom(currentLevel);
    }

    // Creates the player ship class value/dataset and the gameobject with the player controller to move the ship in the scene
    private GameObject CreatePlayerVisualShip()
    {
        PlayerShip player = CreatePlayerNonVisualShip();

        // Create the gameobject player ship
        GameObject playerGameObject = shipBuilder.CreateShip(player, -5f, -5f, -5f, -5f, new GameObject("PLAYER-1").transform);
        // Add player controller class to the PLAYER-1's child gameobject
        PlayerController playerController = playerGameObject.AddComponent<PlayerController>();
        // Set the playership controller's reference
        playerController.SetPlayerShip(player);
        // Add rb2d to the gameobject, set to dynamic and have no gravity pulling it downwards
        Rigidbody2D rb2d = playerGameObject.AddComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 0f;
        // Set reference
        playerController.SetRB2D(rb2d);
        // Set projectile prefab to allow spawning of projectiles
        playerController.SetProjectile(projectilePlayer);
        // Enable player HP regen
        playerController.SetPlayerRegenOnStatus(true);
        // Add box collider
        playerGameObject.AddComponent<BoxCollider2D>();
        // Add the layer as Player - now only collides with the appropriate layer - SEE EDIT > Project Settings > Collision Layer Matrix
        playerGameObject.layer = LayerMask.NameToLayer("Player");


        for (int i = 0; i < playerGameObject.transform.childCount; i++)
            playerGameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Player");

        // Spawn player in the first room randomly in it
        playerGameObject.transform.position = asteroidSpawner.GetOptimalHamiltonianPath()[0].transform.position +
            new Vector3(Random.Range(-40f, 40f), Random.Range(-40f, 40f));

        // Add Scanner gameobj to player
        GameObject scannerGameObj = Instantiate(playerScanner, playerGameObject.transform.position, Quaternion.identity);
        // Set this a children of the PLAYER gameobject
        scannerGameObj.transform.SetParent(playerGameObject.transform);
        // Give the player the reference to the instantiated scanner gameobject
        playerController.SetScanner(scannerGameObj);
        // Give the instantiated scanner and its class playerscanner reference to the player controller
        scannerGameObj.GetComponent<PlayerScanner>().SetPlayerController(playerController);


        return playerGameObject;
    }

    private void CreateLevelsEnemyShips(int counter)
    {
        GameObject enemyLevelContainer = new GameObject("ENEMY-LEVEL-" + (counter + 1).ToString());
        enemyLevelContainer.transform.position = Vector2.zero;

        // MAKES THE LEVEL 1/2/3 created stay in the scene when changing scene
        DontDestroyOnLoad(enemyLevelContainer);

        int enemyCount = 15 + 15;

        List<EnemyShip> enemyShipsForThisLevel = CreateEnemyShips(enemyCount);

        // Call the RRHC here and pass in the set of enemys to be OPTIMIZED - high random values between lower limit and upper limit
        //  rrhc_Enemy.RandomRestartHillClimb(enemyShips);
        rrhc_Enemy.RandomRestartHillClimb(enemyShipsForThisLevel);

        // Debug.Log("Count: "+  enemyShipsForThisLevel.Count);

        // Build the ships visually as game object with sprites on
        // Build the ship visually!!!
        for (int i = 0; i < enemyShipsForThisLevel.Count; i++)
        {
            // Create a container for the ship - ENEMY
            GameObject enemy = new GameObject("ENEMY");
            enemy.transform.position = Vector2.zero;

            enemy.transform.SetParent(enemyLevelContainer.transform);

            EnemyShip enemyShip = enemyShipsForThisLevel[i];
            //GameObject enemyShipGameObj = shipBuilder.CreateShip(enemyShip, -3f, 3f, -3f, 3f, enemyLevelContainer.transform);
            GameObject enemyShipGameObj = shipBuilder.CreateShip(enemyShip, -3f, 3f, -3f, 3f, enemy.transform);
            // GameObject enemyShipGameObj = shipBuilder.CreateShip(enemyShipsForThisLevel[i], -3f, 3f, -3f, 3f, enemyLevelContainer.transform);
            // add the value multipliers for each enemy
            enemyShipGameObj.name = enemyShipGameObj.name + "-" + enemyShip.GetShipAverageFitness().ToString("F2").ToUpper();

            EnemyController enemyController = enemyShipGameObj.AddComponent<EnemyController>();

            Rigidbody2D enemyRB2D = enemyShipGameObj.AddComponent<Rigidbody2D>();
            // enemyRB2D.bodyType = RigidbodyType2D.Dynamic;
            enemyRB2D.bodyType = RigidbodyType2D.Kinematic;
            enemyRB2D.gravityScale = 0f;
            enemyShipGameObj.AddComponent<BoxCollider2D>();

            // Set the enemy controller
            enemyController.SetEnemyShip(enemyShip);
            enemyController.SetRB2D(enemyRB2D);
            enemyController.SetProjectile(projectile);
            // Disable enemy hp regen
            enemyController.SetEnemyRegenOnStatus(false);

            // Ensures - Enemy do not collide with another enemy
            enemyShipGameObj.layer = LayerMask.NameToLayer("Enemy");
            // Each component in the enemy gameobject is also set the layer to Enemy
            for (int j = 0; j < enemyShipGameObj.transform.childCount; j++)
            {
                enemyShipGameObj.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Enemy");
            }

            // Adds a ui minimap icon for the enemy
            CreateMinimapIcon(enemyShipGameObj.transform, this.playerMinimapIcon, this.enemyShipColour);
            // Create a scanner gameobj and place it under the ship gameobj - child of it
            GameObject scannerGameObj = Instantiate(scanner, enemyShipGameObj.transform.position, Quaternion.identity);
            scannerGameObj.transform.SetParent(enemyShipGameObj.transform);

            // Set up scanner to the enemy
            enemyController.SetScanner(scannerGameObj);
            // Set up the enemy scanner's enemy controller at the start
            scannerGameObj.GetComponent<EnemyScanner>().SetEnemyController(enemyController);

            // Add to the enemy the target lock on obj and follows the enemy - only to be turned its sprite renderer on when we have a current target selected
            GameObject prefabTargetLock = Instantiate(this.targetLockOnGameObj, enemyShipGameObj.transform.position, Quaternion.identity);
            prefabTargetLock.transform.SetParent(enemyShipGameObj.transform);
            // Turn off sprite renderer
            prefabTargetLock.GetComponent<SpriteRenderer>().enabled = false;

            // Add to enemy a world canvas ui
            GameObject enemyUI = Instantiate(enemyUICanvas, enemy.transform.position, Quaternion.identity);
            enemyUI.transform.SetParent(enemy.transform);

            EnemyUI enemyUIComponent = enemyUI.GetComponent<EnemyUI>();
            enemyUIComponent.SetEnemyController(enemyController);
            enemyUIComponent.ActivateVitalityBar();

        }

        // After creating the enemyLevelContainer, add it to the reference list in EnemyOnLoad
        if (canCallEnemyOnLoad == true)
        {
            FindObjectOfType<EnemyOnLoad>().AddEnemyLevelContainerToList(enemyLevelContainer);
        }

        // Add to the list that contains each level's enemies
        enemyContainersForEachLevel.Add(enemyLevelContainer);

        if (counter == 0)
            enemyShipsForLevel1 = enemyShipsForThisLevel;
        else if (counter == 1)
            enemyShipsForLevel2 = enemyShipsForThisLevel;
        else if (counter == 2)
            enemyShipsForLevel3 = enemyShipsForThisLevel;
        else if (counter == 3)
            enemyShipsForLevel4 = enemyShipsForThisLevel;
        else if (counter == 4)
            enemyShipsForLevel5 = enemyShipsForThisLevel;
        else if (counter == 5)
            enemyShipsForLevel6 = enemyShipsForThisLevel;
        else if (counter == 6)
            enemyShipsForLevel7 = enemyShipsForThisLevel;



    }
    

    private void ActivateGameObjectActiveOnLevel(int currentLevel)
    {
        for (int i = 0; i < enemyContainersForEachLevel.Count; i++)
        {
            // Set the others inactive and only leave the current level's spawn active
            // i = 0, currentlevel = 1 - 1 -> true
            // ...
            // i = 1, current level = 2 - 1 -> true
            if (i == currentLevel-1) { continue; }

            enemyContainersForEachLevel[i].gameObject.SetActive(false);
        }
    }

    private void SpawnEnemyForThisLevelRandomlyInEachRoom(int currentLevel, List<GameObject> optimalHamiltonianPath)
    {

        int numberOfShipsInThisLevel = enemyContainersForEachLevel[currentLevel - 1].transform.childCount;

        int totalEnemiesInRoomsCombined = numberOfShipsInThisLevel / 2;

        // Each ship are placed randomly in one of the rooms
        for (int i = 0; i < totalEnemiesInRoomsCombined; i++)
        {
            string enemyName = enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).name;
            // Debug.Log("E-name: " + enemyName);

            GameObject randomRoomCenter = optimalHamiltonianPath[Random.Range(0, optimalHamiltonianPath.Count)];
            Vector2 randomRoomCenterPosition = randomRoomCenter.transform.position;
            Vector2 randomPositionInRoom = new Vector2(randomRoomCenterPosition.x + Random.Range(-40f, 40f),
                randomRoomCenterPosition.y + Random.Range(-40f, 40f));

            //Debug.Log("room position: " + randomRoomCenter.transform.position);
            //Vector2 randomPositionInRoom = new Vector2(Random.Range(-40))
            // Place each enemy randomly in the given room
            //enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).gameObject.transform.position = randomPositionInRoom;
            //Debug.Log(enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).gameObject.name);

            enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.transform.position = randomPositionInRoom;
        }

        // Spawn the other enemies outside the room
    }

    /// <summary>
    /// Pass the enemyLevelContainer, an empty gameobject, that contains (children) the set of enemies for that level 
    /// </summary>
    /// <param name="enemyLevelContainer"></param>
    public void SpawnEnemyRandomlyInEachRoomForLevel2AndAbove(GameObject enemyLevelContainer)
    {
        // Get number of children - "ENEMY" gameobjects - should be about 30
        int numberOfChildren = enemyLevelContainer.transform.childCount;

        //Debug.Log("Number of children: " + numberOfChildren);
        //Debug.Log("Children name: " + enemyLevelContainer.transform.GetChild(0).name);  // Prints out "ENEMY"
        int totalEnemiesInRoomsCombined = numberOfChildren / 2;

        List<GameObject> allCenterRoomsSortedInHamiltonianPath = this.asteroidSpawner.GetOptimalHamiltonianPath();

        // Half of the total enemies is placed randomly in each room
        for (int i = 0; i < totalEnemiesInRoomsCombined; i++)
        {
            GameObject randomRoomCenter = allCenterRoomsSortedInHamiltonianPath[Random.Range(0, allCenterRoomsSortedInHamiltonianPath.Count)];
            Vector2 randomRoomCenterPosition = randomRoomCenter.transform.position;
            Vector2 randomPositionInRoom = new Vector2(randomRoomCenterPosition.x + Random.Range(-40f, 40f),
                randomRoomCenterPosition.y + Random.Range(-40f, 40f));

            // Place each enemy -> "ENEMY" -> "SHIP", place each "SHIP" in the "randomPositionInRoom"
            enemyLevelContainer.transform.GetChild(i).transform.GetChild(0).gameObject.transform.position = randomPositionInRoom;
        }

    }

    private void SpawnEnemyOutsideTheRoom(int currentLevel)
    {
        // 30 enemies in total in the level, 15 split - in rooms and 15 for outside
        int startingIndex = enemyContainersForEachLevel[currentLevel - 1].transform.childCount / 2;

        for (int i = startingIndex; i < enemyContainersForEachLevel[currentLevel - 1].transform.childCount; i++)
        {

            int direction = Random.Range(0, 4);

            float yPos = 0f;
            float xPos = 0f;

            if (direction == 0)
            {
                // Top - can only random on the top outer of the level space
                yPos = Random.Range(300f, 315f);
                xPos = Random.Range(-315f, 315f);
            }
            else if (direction == 1)
            {
                // Right - can only random on the right outer of the level space
                yPos = Random.Range(-315f, 315f);
                xPos = Random.Range(300f, 315f);
            }
            else if (direction == 2)
            {
                // Bottom - can only random on the bottom outer of the level space
                yPos = Random.Range(-315f, -300f);
                xPos = Random.Range(-315f, 315f);
            }
            else if (direction == 3)
            {
                // Left
                yPos = Random.Range(-315f, 315f);
                xPos = Random.Range(-315f, -300f);
            }

            // Place the remaining 15 other enemies outside the room
            Vector2 randomPositionOutsideTheRoom = new Vector2(xPos, yPos);
            //enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).transform.position = randomPositionOutsideTheRoom;
            enemyContainersForEachLevel[currentLevel - 1].transform.GetChild(i).gameObject.transform.GetChild(0).transform.position = randomPositionOutsideTheRoom;
        }
    }

    public void SpawnEnemyOutsideTheRoomForLevel2AndAbove(GameObject enemyLevelContainer)
    {
        int startingIndex = enemyLevelContainer.transform.childCount / 2;
        int numberOfChildren = enemyLevelContainer.transform.childCount;

        for (int i = startingIndex; i < numberOfChildren; i++)
        {
            int direction = Random.Range(0, 4);

            float yPos = 0f;
            float xPos = 0f;

            if (direction == 0)
            {
                // Top - can only random on the top outer of the level space
                yPos = Random.Range(300f, 315f);
                xPos = Random.Range(-315f, 315f);
            }
            else if (direction == 1)
            {
                // Right - can only random on the right outer of the level space
                yPos = Random.Range(-315f, 315f);
                xPos = Random.Range(300f, 315f);
            }
            else if (direction == 2)
            {
                // Bottom - can only random on the bottom outer of the level space
                yPos = Random.Range(-315f, -300f);
                xPos = Random.Range(-315f, 315f);
            }
            else if (direction == 3)
            {
                // Left
                yPos = Random.Range(-315f, 315f);
                xPos = Random.Range(-315f, -300f);
            }

            Vector2 randomPosOutsideTheRoom = new Vector2(xPos, yPos);
            // Sets the "SHIP" a children of "ENEMY" a children of "ENEMY-LEVEL-n" to one of the random positions outside the room
            enemyLevelContainer.transform.GetChild(i).transform.GetChild(0).gameObject.transform.position = randomPosOutsideTheRoom;
        }
    }

    public List<EnemyShip> GetEnemyShips() { return this.enemyShips; }

    #region Create Player 

    public PlayerShip CreatePlayerNonVisualShip()
    {
        PlayerShip playerShip = new PlayerShip(
            new ShipOperator("Arvel", 100, 0, null),
            weightCapacity,
            shipComponentsCreator.CreateWeaponComponents(),
            shipComponentsCreator.CreateArmourComponent(),
            shipComponentsCreator.CreateBoosterComponent(),
            shipComponentsCreator.CreateFrameComponent(),
            shipComponentsCreator.CreateCoreComponent());

        return playerShip;

    }
    #endregion

    #region Creating Enemies

    private List<EnemyShip> CreateEnemyShips(int enemyCount)
    {
        List<EnemyShip> enShip = new List<EnemyShip>();
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyShip enemyShip = CreateEnemyNonVisualShip();
            enShip.Add(enemyShip);
        }

        return enShip;
    }

    private EnemyShip CreateEnemyNonVisualShip()
    {
        EnemyShip enemyShip = new EnemyShip(
            new ShipOperator("E-" + Random.Range(1, 1001).ToString(), 0, 0, null),
            weightCapacity,
            shipComponentsCreator.CreateWeaponComponents(),
            shipComponentsCreator.CreateArmourComponent(),
            shipComponentsCreator.CreateBoosterComponent(),
            shipComponentsCreator.CreateFrameComponent(),
            shipComponentsCreator.CreateCoreComponent());
        return enemyShip;
    }
    #endregion
}
