using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;

// This is the main node type for a Rect grid
// based map. We will derive this class from
// GameAI.Pathfinding.Node generic class.
// For a 2D rectangular grid based map we 
// will simply use the Vector2Int class to 
// represent the location of the cell.
public class GridCell : Node<Vector2Int>
{
    // Is this cell walkable?
    public bool IsWalkable { get; set; }

    // Is this cell a hiding spot?
    public bool IsHidingSpot { get; set; }

    // Keep a reference to the grid so that 
    // we can find the neighbours.
    private GridVisualize gridVisualize;

    // construct the node with the grid and the location.
    public GridCell(GridVisualize gridMap, Vector2Int value)
      : base(value)
    {
        gridVisualize = gridMap;

        // by default we set the cell to be walkable.
        IsWalkable = true;

        // by default we set the cell to be not a hiding spot.
        IsHidingSpot = false;
    }

    // get the neighbours for this cell.
    // here will will just throw the responsibility
    // to get the neighbours to the grid.
    public override List<Node<Vector2Int>> GetNeighbours()
    {
        return gridVisualize.GetNeighbourCells(this);
    }
}
