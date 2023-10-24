using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour
{
    private Grid<PathNode> grid;

    public void SetGrid(Grid<PathNode> grid)
    {
        this.grid = grid;
        UpdateVisual();
    }

    private void UpdateVisual()
    {

    }
}
