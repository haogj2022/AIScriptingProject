using UnityEngine;

public class ChaseState : State
{
    public GridVisualize gridVisualize;
    public SeekState seekState;
    public CatchState catchState;
    public bool canCatchHider;
    public GameObject body;

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
            //Debug.Log("catch");
            canCatchHider = true;
        }

        if (gridVisualize.seekerAI.transform.position == gridVisualize.gridCellGameObjects[hiderCol, hiderRow].transform.position && seekState.canSeeHider)
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

        if (hiderCol < gridVisualize.seekerAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (hiderCol > gridVisualize.seekerAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
