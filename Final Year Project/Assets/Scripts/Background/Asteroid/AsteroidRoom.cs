using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each asteroid room has asteroid points, these are a list of vector2 (x and y) that counts as the 2d array that is used for 
/// checking if another asteroid room is trying to fill a point that has this asteroid room's name. It is just a class holder,
/// 
/// no need to instantiate it via Gameobject, just instantiate as a regular c# object
/// </summary>
public class AsteroidRoom
{

    private string name = "";
    private string asteroidRoomEdges = "";
    private string asteroidRoomFill = "FILLED-HIDDEN";
    private int widthlength = 0;


    List<Vector2> asteroidRoomMarkedPoints = new List<Vector2>();
    private static int asteroidRoomNumber = 0;

    private bool canCreate = false;
    public AsteroidRoom(string[][] grid, string name, string edgesName, int widthlength)
    {
        CreateAsteroidRoom(grid);

        this.name = name;
        this.asteroidRoomEdges = edgesName;
        this.widthlength = widthlength;

    }

    public List<Vector2> GetAsteroidRoomMarkedPoints() { return this.asteroidRoomMarkedPoints; }
    public int GetWidthLength() { return widthlength; }
    public bool GetCanCreate() { return this.canCreate; }

    // Attempts to create an asateroid room by putting a point anywhere in the 2d grid and then checking if this index position is valid by checking the point given is empty and the point's surroudnings (+-5 index range) are also empty.
    private void CreateAsteroidRoom(string[][] grid)
    {
        int randomCenterIndexX = Random.Range(6, 55);
        int randomCenterIndexY = Random.Range(6, 55);


        bool isGridEmpty = true;

        int roomWidthLength = 11;

        // Check if grid is empty
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                string gridElement = grid[i][j];

                // Debug.LogError("grid element: " + gridElement); // Working as intended
                // CHECK EACH ELEMENT ARE EMPTY- IF NOT WE HAVE 2 decisions
                if (!string.IsNullOrEmpty(gridElement))
                {
                    // Debug.LogError("EMPTY GRID");
                    isGridEmpty = false;
                    break;
                }
            }
        }


        // If grid is empty, populate given point and its surroundings to create an asteroid room in the 2d array (not visualized yet)
        if (isGridEmpty == true)
        {
            //Debug.LogError("Try to fill the grid");

            

            // Fill the centre
            grid[randomCenterIndexX][randomCenterIndexY] = "CENTER-" + asteroidRoomNumber.ToString();
            
            // Everytime we set an index with "CENTER" we must add the asteroid room number - this is used in processing the corridors for connecting the rooms
            asteroidRoomNumber += 1;

            // Add to the vector points as this is the list that visualises the marked point in the grid
            this.asteroidRoomMarkedPoints.Add(new Vector2(randomCenterIndexX, randomCenterIndexY));

            // Then populate around the center in the grid
            for (int i = 0; i < roomWidthLength; i++)
            {
                int row = (randomCenterIndexX - 5) + i;
                for (int j = 0; j < roomWidthLength; j++)
                {
                    int col = (randomCenterIndexY - 5) + j;

                    // if centre is found, skip current iteration
                    if (row == randomCenterIndexX && col == randomCenterIndexY) { continue; }

                    // Only name the edges of the room as filled, otherwise everything is hidden
                    if (i == 0 || i == 10 || j == 0 || j == 10)
                        grid[row][col] = "FILLED";
                    else
                        grid[row][col] = "FILLED-HIDDEN";

                    // grid[row][col] = "FILLED";

                    this.asteroidRoomMarkedPoints.Add(new Vector2(row, col));

                }
            }

            this.canCreate = true;
        }
        else
        {
            string element = grid[randomCenterIndexX][randomCenterIndexY];
            // Check the grid if new random is touching an element in the grid
            if (string.IsNullOrEmpty(element))
            {
                // If empty, go place it in that position
                //Debug.LogError("Found empties");
                // Search/scan in 8 directions from the given point, scan 6 index wide/long and find if there are
                // elements that are not empty, if so, we cannot place this asteroid room.
                bool canPlace = IsCenterPointAllowed(grid, randomCenterIndexX, randomCenterIndexY);
                if (canPlace)
                {
                    grid[randomCenterIndexX][randomCenterIndexY] = "CENTER-" + asteroidRoomNumber.ToString();
                    asteroidRoomNumber += 1;
                    this.asteroidRoomMarkedPoints.Add(new Vector2(randomCenterIndexX, randomCenterIndexY));
                    // THen place around it...
                    for (int i = 0; i < roomWidthLength; i++)
                    {
                        int row = (randomCenterIndexX - 5) + i;
                        for (int j = 0; j < roomWidthLength; j++)
                        {
                            int col = (randomCenterIndexY - 5) + j;

                            // if centre is found, skip current iteration
                            if (row == randomCenterIndexX && col == randomCenterIndexY) { continue; }

                            // Only name the edges of the room as filled, otherwise everything is hidden
                            if (i == 0 || i == 10 || j == 0 || j == 10)
                                grid[row][col] = "FILLED";
                            else
                                grid[row][col] = "FILLED-HIDDEN";

                            //grid[row][col] = "FILLED";

                            this.asteroidRoomMarkedPoints.Add(new Vector2(row, col));

                        }
                    }
                    //Debug.LogError("Actually placing");
                    this.canCreate = true;
                }
                else
                {
                    this.canCreate = false;
                   // Debug.LogError("The element is filled...");
                }
            }
            else
            {
                this.canCreate = false;
                //Debug.LogError("The element is filled...");
            }
        }

    }

    private bool IsCenterPointAllowed(string[][] grid, int xCentre, int yCentre)
    {
        bool canPlace = false;
        int asteroidRoomRadius = 5 + 1; // 5 is the width/length and + 1 for space between 2 asteroid room if they are close
        int roomWidthLength = (asteroidRoomRadius * 2) + 1; // double the radius + 1 as 1 is the center
        List<bool> markedPoints = new List<bool>();

        Debug.Log("marked points count: " + markedPoints);

        #region Top Part Range Check

        // Check top left corner - go through this 6 times
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            // Go the to top left corner then at each iteration go closer to the given center point
            int row = (xCentre - asteroidRoomRadius) + i;
            int col = (yCentre - asteroidRoomRadius) + i;

            //Debug.Log("x: " + row + "\ny: " + col);
            if ( !(string.IsNullOrEmpty(grid[row][col]) ))
            {
                markedPoints.Add(true);
               // Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        // Check top corner 
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            // Row changes, col does not
            int row = (xCentre - asteroidRoomRadius) + i;
            int col = (yCentre);
            //Debug.Log("x: " + row + "\ny: " + col);

            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                // Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }

        }

        // Check top right corner
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            int row = (xCentre - asteroidRoomRadius) + i;
            int col = (yCentre + asteroidRoomRadius) - i;
            //Debug.Log("x: " + row + "\ny: " + col);

            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                //Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        #endregion

        #region Middle Part Change Check

        // Check middle right
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            // Same row, but column changes
            int row = (xCentre);
            int col = (yCentre + i + 1); // Go to its right

            //Debug.Log("x: " + row + "\ny: " + col);
            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                //Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        // Check middle left
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            int row = (xCentre);
            int col = (yCentre - asteroidRoomRadius) + i;   // Start to the left 

            //Debug.Log("x: " + row + "\ny: " + col);
            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                //Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        #endregion
        #region Bottom Part Range Check

        // Check bottom left corner
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            int row = (xCentre + asteroidRoomRadius) - i; // Start bottom left corner and go closer to the centre
            int col = (yCentre - asteroidRoomRadius) + i; // Start bottom left

            // Debug.Log("x: " + row + "\ny: " + col);
            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                //Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        // Check bottom corner
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            // column does not change but the row does, gets closer to the centre at each iteration
            int row = (xCentre + asteroidRoomRadius) - i;
            int col = (yCentre);

            // Debug.Log("x: " + row + "\ny: " + col);
            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                //Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }

        // Check bottom right corner
        for (int i = 0; i < asteroidRoomRadius; i++)
        {
            // starts bottom right corner and gets closer to the center at each iteration
            int row = (xCentre + asteroidRoomRadius) - i;
            int col = (yCentre + asteroidRoomRadius) - i;

            // Debug.Log("x: " + row + "\ny: " + col);
            if (!(string.IsNullOrEmpty(grid[row][col])))
            {
                markedPoints.Add(true);
                // Debug.LogError("Cannot place asteroid. Need to find another place.");
                break;
            }
        }


        #endregion


        if (markedPoints.Count > 0)
        {
            // Cannot place
            canPlace = false;
        }
        else if (markedPoints.Count <= 0)
        {
            canPlace = true;
        }

        return canPlace;
    }

}
