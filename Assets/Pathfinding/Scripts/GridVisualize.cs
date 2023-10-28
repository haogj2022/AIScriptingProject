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

    GameObject[,] gridCellGameObjects;

    // the 2d array of Vecto2Int.
    // This structure stores the 2d indices of the grid cells.
    protected Vector2Int[,] indices;

    // the 2d array of the GridCell.
    protected GridCell[,] gridCells;

    public Color walkableCell = new Color(42 / 255.0f, 99 / 255.0f, 164 / 255.0f, 1.0f);
    public Color unwalkableCell = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    public Transform aiDestination;
    public AIMovement aiMovement;

    private void Start()
    {       
        // Construct the grid and the cell game objects.
        Construct(maxCol, maxRow);

        // Reset the camera to a proper size and position.
        ResetCamera();
    }

    void ResetCamera()
    {
        Camera.main.orthographicSize = maxRow / 2.0f + 1.0f;
        Camera.main.transform.position = new Vector3(maxCol / 2.0f - 0.5f, maxRow / 2.0f - 0.5f, -10.0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayCastAndToggleWalkable();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RayCastAndSetDestination();
        }
    }

    // toggling of walkable/non-walkable cells.
    public void RayCastAndToggleWalkable()
    {
        Vector2 rayPos = new Vector2(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            GameObject obj = hit.transform.gameObject;
            GridCellVisualize selectedCell = obj.GetComponent<GridCellVisualize>();
            ToggleWalkable(selectedCell);            
        }
    }

    public void ToggleWalkable(GridCellVisualize selectedCell)
    {
        if (selectedCell == null)
        {
            Debug.Log(selectedCell + " is null");
            return;
        }           

        int x = selectedCell.gridCell.Value.x;
        int y = selectedCell.gridCell.Value.y;

        selectedCell.gridCell.IsWalkable = !selectedCell.gridCell.IsWalkable;

        if (selectedCell.gridCell.IsWalkable)
        {
            Debug.Log(selectedCell + " is now walkable");
            selectedCell.SetInnerColor(walkableCell);
        }
        else
        {
            Debug.Log(selectedCell + " is now unwalkable");
            selectedCell.SetInnerColor(unwalkableCell);
        }
    }

    void RayCastAndSetDestination()
    {
        Vector2 rayPos = new Vector2(
            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            GameObject obj = hit.transform.gameObject;
            GridCellVisualize selectedCell = obj.GetComponent<GridCellVisualize>();
            if (selectedCell == null) return;

            Vector3 pos = aiDestination.position;
            pos.x = selectedCell.gridCell.Value.x;
            pos.y = selectedCell.gridCell.Value.y;
            aiDestination.position = pos;

            // Set the destination to the NPC.
            aiMovement.SetDestination(this, selectedCell.gridCell);
        }
    }

    // Construct a grid with the max cols and rows.
    protected void Construct(int numCol, int numRow)
    {
        maxCol = numCol;
        maxRow = numRow;

        indices = new Vector2Int[maxCol, maxRow];
        gridCellGameObjects = new GameObject[maxCol, maxRow];
        gridCells = new GridCell[maxCol, maxRow];

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
                gridCells[i, j] = new GridCell(this, indices[i, j]);

                // set a reference to the GridCellVisualize
                GridCellVisualize gridCellVisualize =
                  gridCellGameObjects[i, j].GetComponent<GridCellVisualize>();
                if (gridCellVisualize != null)
                {
                    gridCellVisualize.gridCell = gridCells[i, j];
                }
            }
        }
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

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check top-right
        if (y < maxRow - 1 && x < maxCol - 1)
        {
            int i = x + 1;
            int j = y + 1;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check right
        if (x < maxRow - 1)
        {
            int i = x + 1;
            int j = y;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check right-down
        if (x < maxRow - 1 && y > 0)
        {
            int i = x + 1;
            int j = y - 1;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check down
        if (y > 0)
        {
            int i = x;
            int j = y - 1;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check down-left
        if (y > 0 && x > 0)
        {
            int i = x - 1;
            int j = y - 1;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check left
        if (x > 0)
        {
            int i = x - 1;
            int j = y;

            Vector2Int v = indices[i, j];

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
            }
        }
        // Check left-top
        if (x > 0 && y < maxRow - 1)
        {
            int i = x - 1;
            int j = y + 1;

            if (gridCells[i, j].IsWalkable)
            {
                neighbours.Add(gridCells[i, j]);
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
            return gridCells[x, y];
        }
        return null;
    }
}
