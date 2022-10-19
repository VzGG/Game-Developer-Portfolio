using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a visual ship with randomized parts. Builds the ship parts starting from the Frame then to Booster, Armour, Core, and Weapons.
/// 
/// This class should be called a spawner class or gameobject.
/// </summary>
public class ShipBuilder : MonoBehaviour
{

    // Goal: MUST ALLOW THIS TO BE SAVED IN THE FUTURE 
    // Player on start game (new game)
    //      ShipBuiler builds a random ship
    //      Player can decide to randomize 3 times.
    // If player now has gold
    //      They can randomize the ship parts
    // They can save this ship as their loadout
    // Can only have max 3 loadout

    
    [SerializeField] ShipComponentsCreator shipComponentsCreator;   // Set this in the inspector - MIGHT NEED TO BE SET AT EACH SCENE CHANGE
    [SerializeField] PlayerShip playerShip;                         // The playership generated without visuals
    [Space]
    [SerializeField] GameObject shipGameObject;                     // The blueprint
    [SerializeField] GameObject spawnedShipGameObject;              // The spawned blueprint
    [Space]
    [Header("Ships to spawn")]
    [SerializeField] List<PlayerShip> playerShips = new List<PlayerShip>();

    [SerializeField] GameObject shipComponents;

    // Should be called by a spawner gameobject - Creates the visual i.e., sprites of the ship components together
    public GameObject CreateShip(Ship givenShip, float negXPos, float posXPos, float negYPos, float posYPos, Transform levelContainerGameObjects)
    {
        //GameObject newShipGameObject = Instantiate(shipGameObject);

        //int weaponIndex = 0;
        //int armourIndex = 1;
        //int leftWingArmourIndex = 0;
        //int boosterindex = 2;
        //int frameIndex = 3;
        //int coreIndex = 4;

        #region Spawn Components

        // Create frame obj
        GameObject frameGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
        frameGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledFrame().GetSprite();
        frameGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Frame";
        frameGameObject.name = givenShip.GetInstalledFrame().GetName();
        frameGameObject.AddComponent<BoxCollider2D>();


        // Create armour obj - right
        GameObject rightArmourGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
        rightArmourGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledArmour().GetSprite();
        rightArmourGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Armour";
        rightArmourGameObject.name = givenShip.GetInstalledArmour().GetName() + "-RIGHT";
        //rightArmourGameObject.transform.localScale = new Vector2(-1f, rightArmourGameObject.transform.localScale.y);
        //rightArmourGameObject.transform.position = new Vector2(0.25f, rightArmourGameObject.transform.position.y);
        rightArmourGameObject.AddComponent<BoxCollider2D>();
        
        // Create armour obj - left
        GameObject leftArmourGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
        leftArmourGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledArmour().GetSprite();
        leftArmourGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Armour";
        leftArmourGameObject.name = givenShip.GetInstalledArmour().GetName() + "-LEFT";
        //leftArmourGameObject.transform.position = new Vector2(-0.25f, leftArmourGameObject.transform.position.y);
        leftArmourGameObject.AddComponent<BoxCollider2D>();

        // Create booster obj
        GameObject boosterGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
        boosterGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledBooster().GetSprite();
        boosterGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Booster";
        boosterGameObject.name = givenShip.GetInstalledBooster().GetName();
        boosterGameObject.AddComponent<BoxCollider2D>();

        // Create core obj
        GameObject coreGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
        coreGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledCore().GetSprite();
        coreGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Core";
        coreGameObject.name = givenShip.GetInstalledCore().GetName();
        coreGameObject.AddComponent<BoxCollider2D>();

        List<GameObject> weaponGameObjects = new List<GameObject>();
        // Create weapon(s) obj
        for (int i = 0; i < givenShip.GetInstalledWeapons().Count; i++)
        {
            // Create new weapon gameobject and set its sprite and name to the given ship's component properties
            GameObject weaponGameObject = Instantiate(shipComponents, Vector2.zero, Quaternion.identity);
            weaponGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledWeapons()[i].GetSprite();
            weaponGameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ship Weapon";
            weaponGameObject.name = givenShip.GetInstalledWeapons()[i].GetName();
            weaponGameObject.AddComponent<BoxCollider2D>();
            // Add to list to be able to add it as children of the Parent Container
            weaponGameObjects.Add(weaponGameObject);
            // Then add this in the transform parent
        }

        #endregion

        #region Position the Components

        // Then place them properly
        // Frame gameobject not needed to be moved

        // Core not needed to be moved

        // Booster needed to be moved to the bottom of the frame game object - use height of box collider of frame game object
        float frameHeight = frameGameObject.GetComponent<BoxCollider2D>().size.y;
        float spawnBoosterPosY = -(frameHeight / 2);
        boosterGameObject.transform.position = new Vector2(boosterGameObject.transform.position.x, spawnBoosterPosY);

        // Armour needed to be moved away at little bit (1/3 of frame's width) from the frame 
            // Left Armour Wing
        float frameWidth = frameGameObject.GetComponent<BoxCollider2D>().size.x;
        float spawnRightArmourPos = frameWidth / 3f;
        leftArmourGameObject.transform.position = new Vector2(-spawnRightArmourPos, leftArmourGameObject.transform.position.y);
            // Right Armour Wing - reverse/flip the gameobject's scale x
        rightArmourGameObject.transform.localScale = new Vector2(-1f, rightArmourGameObject.transform.localScale.y);
        rightArmourGameObject.transform.position = new Vector2(spawnRightArmourPos, leftArmourGameObject.transform.position.y);

        // Weapon needed to be moved just above left and right armour
        Vector2 wingDimension = rightArmourGameObject.GetComponent<BoxCollider2D>().size;
            // All weapon will have the same y position - y position of wing + wing dimension y
        float weaponYPlacement = rightArmourGameObject.transform.position.y + (wingDimension.y / 2f);
        float weaponXPlacementSliced = wingDimension.x / 4f;
        // float rightArmourPositionX = rightArmourGameObject.transform.position.x;
        for (int i = 0; i < givenShip.GetInstalledWeapons().Count; i++)
        {
            if (i == 0)         // Place weapon 1 to the left but close to the frame game object
                weaponGameObjects[i].transform.position = new Vector2(-weaponXPlacementSliced - rightArmourGameObject.transform.position.x, weaponYPlacement);
            else if (i == 1)    // Place weapon 2 to the right but close to the frame game object
                weaponGameObjects[i].transform.position = new Vector2(weaponXPlacementSliced + rightArmourGameObject.transform.position.x, weaponYPlacement);
            else if (i == 2)    // Place weapon 3 to the far left
                weaponGameObjects[i].transform.position = new Vector2( (-weaponXPlacementSliced * 3f ) - rightArmourGameObject.transform.position.x, weaponYPlacement);
            else if (i == 3)    // Place weapon 4 to the far right
                weaponGameObjects[i].transform.position = new Vector2( (weaponXPlacementSliced * 3f) + rightArmourGameObject.transform.position.x, weaponYPlacement);
        }

        #endregion

        #region Add Components under Parent Empty GameObject Container

        // The parent holder
        GameObject shipComponentContainer = new GameObject("SHIP");
        // Set parent to an empty new game object
        frameGameObject.transform.SetParent(shipComponentContainer.transform);
        rightArmourGameObject.transform.SetParent(shipComponentContainer.transform);
        leftArmourGameObject.transform.SetParent(shipComponentContainer.transform);
        boosterGameObject.transform.SetParent(shipComponentContainer.transform);
        coreGameObject.transform.SetParent(shipComponentContainer.transform);
        // Set each weapon game object under the children of the parent holder
        for (int i = 0; i < weaponGameObjects.Count; i++)
        {
            weaponGameObjects[i].transform.SetParent(shipComponentContainer.transform);
        }
        #endregion

        #region Position the Parent

        float xPos = Random.Range(negXPos, posXPos);
        float yPos = Random.Range(negYPos, posYPos);
        // Randomly position the ship
        shipComponentContainer.transform.position = new Vector2(xPos, yPos);

        #endregion

        #region Set Transform parent of the current parent to the level new empty gameobject holder

        shipComponentContainer.transform.SetParent(levelContainerGameObjects);

        #endregion

        // NOT WORKING PROPERLY - Positioning problems
        //SpawnFrame(givenShip, newShipGameObject, frameIndex);
        //SpawnBooster(givenShip, newShipGameObject, frameIndex, boosterindex);
        //SpawnArmour(givenShip, newShipGameObject, frameIndex, armourIndex);
        //SpawnCore(givenShip, newShipGameObject, frameIndex, coreIndex);
        //SpawnWeapons(givenShip, newShipGameObject, armourIndex, leftWingArmourIndex, weaponIndex);


        return shipComponentContainer.gameObject;
    }


    // TRY TO BUILD WITHOUT the holder of gameobjects then insert it afterwards in them 

    // SPAWNED CORRECTLY
    private void SpawnFrame(Ship givenShip, GameObject givenShipGameObject, int givenFrameIndex)
    {
        // If there are sprites added in the ship (visual) gameobject, then remove them
        foreach (SpriteRenderer childSpriteRenderer in givenShipGameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            childSpriteRenderer.sprite = null;
        }
        // Get the ship's frame gameobject
        GameObject frameGameObject = givenShipGameObject.transform.GetChild(givenFrameIndex).gameObject;
        // Get the frame gameobject's sprite and set to the chosen sprite and add a box collider to it with it
        frameGameObject.GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledFrame().GetSprite();
        frameGameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    // SPAWNED NOT CORRECTLY -
    /*  Problem - gameobjects position not spawning correctly
     *  
     */
    private void SpawnBooster(Ship givenShip, GameObject givenShipGameObject, int givenFrameIndex, int givenBoosterIndex)
    {
        float frameHeight = givenShipGameObject.transform.GetChild(givenFrameIndex).GetComponent<BoxCollider2D>().size.y;
        float spawnBoosterPosY = -(frameHeight / 2);
        // Set the booster gameobject's sprite to the chosen random sprite
        givenShipGameObject.transform.GetChild(givenBoosterIndex).GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledBooster().GetSprite();
        // Set the booster's gameobject's position to be at the bottom position of the frame gameObject

        //givenShipGameObject.transform.GetChild(givenBoosterIndex).transform.position = new Vector2(givenShipGameObject.transform.GetChild(givenBoosterIndex).transform.position.x, 2f);

        //givenShipGameObject.transform.GetChild(givenBoosterIndex).transform.position =
        //    new Vector2(givenShipGameObject.transform.GetChild(givenBoosterIndex).transform.position.x, spawnBoosterPosY);
    }

    
    private void SpawnArmour(Ship givenShip, GameObject givenShipGameObject, int givenFrameIndex, int givenArmourIndex)
    {


        // Get the frame object's width using its BoxCollider size x
        float frameWidth = givenShipGameObject.transform.GetChild(givenFrameIndex).GetComponent<BoxCollider2D>().size.x;
        float spawnLeftWingArmourPosX = frameWidth / 3f;

        float armourWingPosY = givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(0).transform.position.y;
        
        for (int i = 0; i < givenShipGameObject.transform.GetChild(givenArmourIndex).transform.childCount; i++)
        {
            // Set each wing (left/right) gameobject a sprite - same sprite
            givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = givenShip.GetInstalledArmour().GetSprite();
            // Move the wing gameobject to either left or right of the frame gameobject
            if (i == 0)
                givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(i).transform.position = new Vector2(-spawnLeftWingArmourPosX, armourWingPosY);
            else if (i == 1)
                givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(i).transform.position = new Vector2(spawnLeftWingArmourPosX, armourWingPosY);
            // Then add a box collider to the wing/armour gameobject
            givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
        }
    
    }

    private void SpawnCore(Ship givenShip, GameObject givenShipGameObject, int givenFrameIndex, int givenCoreIndex)
    {
        Vector2 spawnCorePosition = givenShipGameObject.transform.GetChild(givenFrameIndex).position;
        // Set the core gameobject's sprite to chosen random sprite
        givenShipGameObject.transform.GetChild(givenCoreIndex).gameObject.GetComponent<SpriteRenderer>().sprite =
            givenShip.GetInstalledCore().GetSprite();
        // Set the core gameobject's position to be on the frame object's position
        givenShipGameObject.transform.GetChild(givenCoreIndex).transform.position = spawnCorePosition;
    }

    // Sets the sprites weapons and are placed depending on the armour(wings)'s height and width
    private void SpawnWeapons(Ship givenShip, GameObject givenShipGameObject, int givenArmourIndex, int givenLeftWingArmourIndex,
        int givenWeaponIndex)
    {
        // Count how many weapons
        // Then place them left right left right
        // Need width of left wing and right wing
        // and need to place it in 1/4 and 3/4 of the width
        // Set sprite
        // Move each weapon gameobject to either left or right at least touching the centre pivot of the leftwing or right wing

        Vector2 wingDimension = givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(givenLeftWingArmourIndex).GetComponent<BoxCollider2D>().size;
        // All weapon will have the same y position
        float weaponYPlacement = givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(givenLeftWingArmourIndex).gameObject.transform.position.y 
            + (wingDimension.y / 2f);
        float weaponXPlacementSliced = wingDimension.x / 4f;
        float armourLeftWingPosition = givenShipGameObject.transform.GetChild(givenArmourIndex).transform.GetChild(0).gameObject.transform.position.x;

        // Placement of weapons on the left wing
        Vector2 weaponOnePlacement = new Vector2(-weaponXPlacementSliced - (Mathf.Abs(armourLeftWingPosition)), weaponYPlacement);   // For weapon one -> should be placed left wing and closer to the frame
        Vector2 weaponThreePlacement = new Vector2(-(weaponXPlacementSliced * 3) - (Mathf.Abs(armourLeftWingPosition)), weaponYPlacement);
        // For placing weapon on the right wing
        Vector2 weaponTwoPlacement = new Vector2(weaponXPlacementSliced + (Mathf.Abs(armourLeftWingPosition)), weaponYPlacement);
        Vector2 weaponFourPlacement = new Vector2(weaponXPlacementSliced * 3 + (Mathf.Abs(armourLeftWingPosition)), weaponYPlacement);
    
        // Each weapon gameobject will have their sprites provided and an appropriate position
        for (int i = 0; i < givenShip.GetInstalledWeapons().Count; i++)
        {
            givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).GetComponent<SpriteRenderer>().sprite =
                givenShip.GetInstalledWeapons()[i].GetSprite();

            givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).gameObject.AddComponent<BoxCollider2D>();

            if (i == 0)
                givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).transform.position = weaponOnePlacement;
            else if (i == 1)
                givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).transform.position = weaponTwoPlacement;
            else if (i == 2)
                givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).transform.position = weaponThreePlacement;
            else if (i == 3)
                givenShipGameObject.transform.GetChild(givenWeaponIndex).GetChild(i).transform.position = weaponFourPlacement;
        }
    }
}
