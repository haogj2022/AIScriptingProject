using UnityEngine;

public class ChaseState : State
{
    public GridVisualize gridVisualize;
    public SeekState seekState;
    public CatchState catchState;
    public bool canCatchHider;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (!seekState.canSeeHider) return seekState;
        if (canCatchHider) return catchState;
        else return this;
    }

    private void Update()
    {        
        // seeker and hider are at the same position
        if (gridVisualize.seekerAI.transform.position == gridVisualize.hiderAI.transform.position)
        {
            canCatchHider = true;
        }

        if (transform.position == gridVisualize.gridCellGameObjects[hiderCol, hiderRow].transform.position && seekState.canSeeHider)
        {
            ChaseHider();
        }
    }

    public void ChaseHider()
    {
        hiderCol = (int)gridVisualize.hiderAI.transform.position.x;
        hiderRow = (int)gridVisualize.hiderAI.transform.position.y;

        if (gridVisualize.GetGridCell(hiderCol, hiderRow).IsHidingSpot)
        {
            Debug.Log("Hider is hiding at " + hiderCol + ", " + hiderRow);
            gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(0, 0));
        }
        else
        {
            LocateDestination();
        }
    }

    public void LocateDestination()
    {
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
    }
}
