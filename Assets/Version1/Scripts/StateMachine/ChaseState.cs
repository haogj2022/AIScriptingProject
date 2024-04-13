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
        if (transform.position == gridVisualize.hiderAI.transform.position)
        {
            //Debug.Log("catch");
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

        if (gridVisualize.GetGridCell(hiderCol, hiderRow).IsHidingSpot || gridVisualize.GetGridCell(hiderCol, hiderRow).IsRestricted)
        {
            if (gridVisualize.customGrid)
            {
                seekState.canSeeHider = false;
                seekState.hiderIsHiding = true;
                seekState.FindSeekingSpot();
            }
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
