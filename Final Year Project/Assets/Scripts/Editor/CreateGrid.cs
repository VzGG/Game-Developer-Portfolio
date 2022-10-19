using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Only used in the editor and not in runtime at all. This creates a menu item in the editor to generate many gameobjects in certain position (grid like position or 2d array like position)
/// Essentially automated generation of gameobjects. USE THIS CLASS IN THE EDITOR ONLY ONCE!!!
/// </summary>
public class CreateGrid : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/MenuItem.html
    [MenuItem("Window/GENERATE-GRID-NO-VISUAL", false, 1)]
    static void CreateTheGridWithNoVisual()
    {
        // Useful variables for making the grid
        Vector2 startingPosition = new Vector2(-300f, 300f);
        float xMinus = 0f;
        float yMinus = 0f;

        // Creating container for the rows and columns
        GameObject grid = new GameObject("GRID");
        grid.transform.position = Vector3.zero;

        // Make 61x61 grid in the scene to reach -300 in x and 300 in x
        // -300 in y and 300 in y also
        for (int i = 0; i <61; i++)
        {
            GameObject row = new GameObject("ROW-" + i.ToString());
            row.transform.position = Vector3.zero;

            for (int j = 0; j < 61; j++)
            {
                // Create a point gameobject 
                GameObject point = new GameObject("POINT-" + j.ToString());
                point.transform.position = Vector3.zero;

                // Put to the to the most top left position it can be placed
                point.transform.position = startingPosition + new Vector2(xMinus, yMinus);
                
                // Set the parent to the row gameobject
                point.transform.SetParent(row.transform);

                // Make each new point go to the right
                xMinus = xMinus + 10f;

            }
            // After finishing with one row, reset the xMinus and start spawning each point in the left most again
            xMinus = 0f;
            // Also go down the row
            yMinus = yMinus - 10f;

            row.transform.SetParent(grid.transform);
        }


        // https://docs.unity3d.com/ScriptReference/PrefabUtility.html
        // Saving a gameobject as a prefab NOT IN RUNTIME but in EDITOR - for faster development
        string localPath = "Assets/Generated Prefabs/" + grid.name + ".prefab";

        // ensure path is unique
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Save as prefab and instantiate new saved prefab as clone of it
        PrefabUtility.SaveAsPrefabAssetAndConnect(grid, localPath, InteractionMode.UserAction);
    }

    // https://docs.unity3d.com/ScriptReference/MenuItem.html
    [MenuItem("Window/GENERATE-GRID-VISUAL", false, 1)]
    static void CreateTheGridWithVisual()
    {
        // Get the circle sprite in the assets folder: https://docs.unity3d.com/ScriptReference/AssetDatabase.LoadAssetAtPath.html
        Sprite circleSprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Basic Shapes/Circle.png", typeof(Sprite));

        //Debug.Log("HELLO");

        // Useful variables for making the grid
        Vector2 startingPosition = new Vector2(-300f, 300f);
        float xMinus = 0f;
        float yMinus = 0f;

        // Creating container for the rows and columns
        GameObject grid = new GameObject("GRID");
        grid.transform.position = Vector3.zero;

        // Make 61x61 grid in the scene to reach -300 in x and 300 in x
        // -300 in y and 300 in y also
        for (int i = 0; i < 61; i++)
        {
            GameObject row = new GameObject("ROW-" + i.ToString());
            row.transform.position = Vector3.zero;

            for (int j = 0; j < 61; j++)
            {
                // Create a point gameobject 
                GameObject point = new GameObject("POINT-" + j.ToString());
                point.transform.position = Vector3.zero;

                #region Make the gameobject visual
                // Add each gameobject a sprite renderer and a sprite
                SpriteRenderer spriteRenderer = point.gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = circleSprite;
                // Make the object bigger
                point.transform.localScale = new Vector3(5f, 5f, 5f);

                #endregion

                // Put to the to the most top left position it can be placed
                point.transform.position = startingPosition + new Vector2(xMinus, yMinus);

                // Set the parent to the row gameobject
                point.transform.SetParent(row.transform);

                // Make each new point go to the right
                xMinus = xMinus + 10f;

            }
            // After finishing with one row, reset the xMinus and start spawning each point in the left most again
            xMinus = 0f;
            // Also go down the row
            yMinus = yMinus - 10f;

            row.transform.SetParent(grid.transform);
        }


        // https://docs.unity3d.com/ScriptReference/PrefabUtility.html
        // Saving a gameobject as a prefab NOT IN RUNTIME but in EDITOR - for faster development
        string localPath = "Assets/Generated Prefabs/" + grid.name + ".prefab";

        // ensure path is unique
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        // Save as prefab and instantiate new saved prefab as clone of it
        PrefabUtility.SaveAsPrefabAssetAndConnect(grid, localPath, InteractionMode.UserAction);
    }
}
