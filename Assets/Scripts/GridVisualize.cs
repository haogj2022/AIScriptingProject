using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;

public class GridVisualize : MonoBehaviour
{
    // the max number of columns in the grid.
    public int maxCol;
    // the max number of rows in the grid
    public int maxRow;

    // The prefab for representing a grid cell. We will 
    // use the prefab to show/visualize the status of the cell
    // as we proceed with our pathfinding.
    [SerializeField]
    GameObject gridCell;

    [SerializeField]
    GameObject coin;

    public GameObject[,] gridCellGameObjects;

    // the 2d array of Vecto2Int.
    // This structure stores the 2d indices of the grid cells.
    protected Vector2Int[,] indices;

    // the 2d array of the GridCell.
    protected GridCell[,] gridCellArray;

    private Color walkableCell = new Color(42 / 255.0f, 99 / 255.0f, 164 / 255.0f, 1.0f);
    private Color unwalkableCell = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    //public Transform aiDestination;
    //public AIMovement aiMovement;

    public AIMovement seekerAI;
    public AIMovement hiderAI;
    public GameObject cheese;

    private void Awake()
    {
        // Define the size of the grid
        int randomCol = Random.Range(10, 20);
        int randomRow = Random.Range(10, 20);

        maxCol = randomCol;
        maxRow = randomRow;

        // Set the default position of the AI
        seekerAI.transform.position = Vector2.zero;
        hiderAI.transform.position = new Vector2(maxCol - 1, maxRow - 1);

        // Construct the grid and the cell game objects.
        Construct(maxCol, maxRow);

        // Reset the camera to a proper size and position.
        ResetCamera();       
    }

    void ResetCamera()
    {
        Camera.main.orthographicSize = maxRow / 2.0f + 1.0f;
        if (maxCol > maxRow)
            Camera.main.transform.position = new Vector3(maxCol / 2.0f - 0.5f, maxRow / 2.0f - 0.5f, -maxCol + 2.0f);   
        else if (maxCol < maxRow)
            Camera.main.transform.position = new Vector3(maxCol / 2.0f - 0.5f, maxRow / 2.0f - 0.5f, -maxRow + 2.0f);
        else
            Camera.main.transform.position = new Vector3(maxCol / 2.0f - 0.5f, maxRow / 2.0f - 0.5f, -(maxCol + maxRow) / 2.0f);   
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RayCastAndToggleWalkable();
    //    }

    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        RayCastAndSetDestination();
    //    }
    //}

    //// toggling of walkable/non-walkable cells.
    //public void RayCastAndToggleWalkable()
    //{

    //    Debug.Log(Input.mousePosition);


    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //    if (hit.collider != null)
    //    {
    //        GameObject obj = hit.transform.gameObject;
    //        GridCellVisualize selectedCell = obj.GetComponent<GridCellVisualize>();
    //        ToggleWalkable(selectedCell);            
    //    }
    //}

    //public void ToggleWalkable(GridCellVisualize selectedCell)
    //{
    //    if (selectedCell == null)
    //        return;     

    //    int x = selectedCell.gridCell.Value.x;
    //    int y = selectedCell.gridCell.Value.y;

    //    selectedCell.gridCell.IsWalkable = !selectedCell.gridCell.IsWalkable;

    //    if (selectedCell.gridCell.IsWalkable)
    //    {
    //        Debug.Log(selectedCell + " is now walkable");
    //        selectedCell.SetInnerColor(walkableCell);
    //    }
    //    else
    //    {
    //        Debug.Log(selectedCell + " is now unwalkable");
    //        selectedCell.SetInnerColor(unwalkableCell);
    //    }
    //}

    //void RayCastAndSetDestination()
    //{
    //    Vector2 rayPos = new Vector2(
    //        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
    //        Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    //    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

    //    if (hit)
    //    {
    //        GameObject obj = hit.transform.gameObject;
    //        GridCellVisualize selectedCell = obj.GetComponent<GridCellVisualize>();
    //        if (selectedCell == null) return;

    //        Vector3 pos = aiDestination.position;
    //        pos.x = selectedCell.gridCell.Value.x;
    //        pos.y = selectedCell.gridCell.Value.y;
    //        aiDestination.position = pos;

    //        // Set the destination to the NPC.
    //        aiMovement.SetDestination(this, selectedCell.gridCell);
    //    }
    //}

    // Construct a grid with the max cols and rows.
    protected void Construct(int numCol, int numRow)
    {       
        maxCol = numCol;
        maxRow = numRow;

        indices = new Vector2Int[maxCol, maxRow];
        gridCellGameObjects = new GameObject[maxCol, maxRow];
        gridCellArray = new GridCell[maxCol, maxRow];

        // create all the grid cells (Index data) with default values.
        // also create the grid cell game ibjects from the prefab.
        for (int i = 0; i < maxCol; ++i)
        {
            for (int j = 0; j < maxRow; ++j)
            {
                indices[i, j] = new Vector2Int(i, j);
                gridCellGameObjects[i, j] = Instantiate(
                  gridCell,
                  new Vector3(i, j, 0.0f),
                  Quaternion.identity);
               
                // Set the parent for the grid cell to this transform.
                gridCellGameObjects[i, j].transform.SetParent(transform);

                // set a name to the instantiated cell.
                gridCellGameObjects[i, j].name = "Cell_" + i + "_" + j;

                // create the GridCells
                gridCellArray[i, j] = new GridCell(this, indices[i, j]);

                // set a number of grid cell to unwalkable
                bool walkableCell = Random.value > 0.1f;
                gridCellArray[i, j].IsWalkable = walkableCell;                               

                bool spawnCheese = Random.value > 0.9f;

                if (gridCellArray[i, j].IsWalkable == false)
                {
                    GridCellVisualize selectedCell = gridCellGameObjects[i, j].GetComponent<GridCellVisualize>();
                    selectedCell.SetInnerColor(unwalkableCell);
                }

                if (gridCellArray[i, j].IsWalkable == true && spawnCheese)
                {
                    Instantiate(cheese, gridCellGameObjects[i, j].transform.position, Quaternion.identity);
                }

                // set a reference to the GridCellVisualize
                GridCellVisualize gridCellVisualize =
                  gridCellGameObjects[i, j].GetComponent<GridCellVisualize>();

                if (gridCellVisualize != null)
                {
                    gridCellVisualize.gridCell = gridCellArray[i, j];
                }               
            }
        }

        // set the default grid cell to walkable
        gridCellArray[0, 0].IsWalkable = true;       
        gridCellArray[maxCol - 1, maxRow - 1].IsWalkable = true;
        gridCellArray[0, maxRow - 1].IsWalkable = true;
        gridCellArray[maxCol - 1, 0].IsWalkable = true;

        GridCellVisualize seekerAI = gridCellGameObjects[0, 0].GetComponent<GridCellVisualize>();
        seekerAI.SetInnerColor(walkableCell);

        GridCellVisualize hiderAI = gridCellGameObjects[maxCol - 1, maxRow - 1].GetComponent<GridCellVisualize>();
        hiderAI.SetInnerColor(walkableCell);        
    }

    // get neighbour cells for a given cell.
    public List<Node<Vector2Int>> GetNeighbourCells(Node<Vector2Int> location)
    {
        List<Node<Vector2Int>> neighbours = new List<Node<Vector2Int>>();

        int x = location.Value.x;
        int y = location.Value.y;

        // Check up.
        if (y < maxRow - 1)
        {
            int i = x;
            int j = y + 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check top-right
        if (y < maxRow - 1 && x < maxCol - 1)
        {
            int i = x + 1;
            int j = y + 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check right
        if (x < maxCol - 1)
        {
            int i = x + 1;
            int j = y;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check right-down
        if (x < maxCol - 1 && y > 0)
        {
            int i = x + 1;
            int j = y - 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check down
        if (y > 0)
        {
            int i = x;
            int j = y - 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check down-left
        if (y > 0 && x > 0)
        {
            int i = x - 1;
            int j = y - 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check left
        if (x > 0)
        {
            int i = x - 1;
            int j = y;

            Vector2Int v = indices[i, j];

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }
        // Check left-top
        if (x > 0 && y < maxRow - 1)
        {
            int i = x - 1;
            int j = y + 1;

            if (gridCellArray[i, j].IsWalkable)
            {
                neighbours.Add(gridCellArray[i, j]);
            }
        }

        return neighbours;
    }

    public static float GetManhattanCost(
    Vector2Int a,
    Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public static float GetEuclideanCost(
      Vector2Int a,
      Vector2Int b)
    {
        return GetCostBetweenTwoCells(a, b);
    }

    public static float GetCostBetweenTwoCells(
      Vector2Int a,
      Vector2Int b)
    {
        return Mathf.Sqrt(
                (a.x - b.x) * (a.x - b.x) +
                (a.y - b.y) * (a.y - b.y)
            );
    }

    public GridCell GetGridCell(int x, int y)
    {
        if (x >= 0 && x < maxCol && y >= 0 && y < maxRow)
        {
            return gridCellArray[x, y];
        }
        return null;
    }
}
