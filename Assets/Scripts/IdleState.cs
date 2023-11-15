using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public GridVisualize gridVisualize;
    public ChaseState chaseState;
    public bool canSeeTheEnemy;
    public bool isSeeker;

    int seekerCol;
    int seekerRow;
    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (canSeeTheEnemy) return chaseState;          
        else return this;
    }

    private void Start()
    {
        MoveToCoinPosition();
    }

    void MoveToCoinPosition()
    {
        if (gridVisualize.gridConstructed)
        {           
            if (gridVisualize.gridCellArray[seekerCol, seekerRow].IsWalkable)
            {
                gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[seekerCol, seekerRow]);
            }
            else
            {
                Debug.Log("Next seeker destination is unwalkable");
                seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
                seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
                Debug.Log("Next seeker destination changes to " + seekerCol + ", " + seekerRow);
                gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[seekerCol, seekerRow]);
            }
        }
    }

    void MoveToHidingSpot()
    {
        if (gridVisualize.gridConstructed)
        {
            if (gridVisualize.gridCellArray[hiderCol, hiderRow].IsWalkable)
            {
                gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[hiderCol, hiderRow]);
            }
            else
            {
                Debug.Log("Next hiding destination is unwalkable");
                hiderCol = Random.Range(0, gridVisualize.maxCol - 1);
                hiderRow = Random.Range(0, gridVisualize.maxRow - 1);
                Debug.Log("Next hiding destination changes to " + hiderCol + ", " + hiderRow);
                gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[hiderCol, hiderRow]);
            }
        }
    }

    private void Update()
    {
        SeekForCoins();
        RunAndHide();
    }

    void SeekForCoins()
    {
        if (isSeeker)
        {           
            if (gridVisualize.seekerAI.transform.position == gridVisualize.gridCellGameObjects[seekerCol, seekerRow].transform.position)
            {
                Debug.Log("Current seeker location is " + seekerCol + ", " + seekerRow);
                seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
                seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
                Debug.Log("Next seeker destination is " + seekerCol + ", " + seekerRow);
                MoveToCoinPosition();
            }
        }
    }

    void RunAndHide()
    {
        if (!isSeeker)
        {
            if (gridVisualize.seekerAI.transform.position == gridVisualize.gridCellGameObjects[hiderCol, hiderRow].transform.position)
            {
                Debug.Log("Current hiding location is " + hiderCol + ", " + hiderRow);
                hiderCol = Random.Range(0, gridVisualize.maxCol - 1);
                hiderRow = Random.Range(0, gridVisualize.maxRow - 1);
                Debug.Log("Next hiding destination is " + hiderCol + ", " + hiderRow);
                MoveToHidingSpot();
            }
        }
    }
}
