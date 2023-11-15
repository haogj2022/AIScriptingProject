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

    public override State RunCurrentState()
    {
        if (canSeeTheEnemy) return chaseState;          
        else return this;
    }
   
    private void Update()
    {
        MoveToDestination();
    }

    void MoveToDestination()
    {
        if (isSeeker)
        {
            if (gridVisualize.seekerAI.transform.position == gridVisualize.gridCellGameObjects[seekerCol, seekerRow].transform.position)
            {                
                NextDestination();
            }
        }
    }

    void NextDestination()
    {
        if (gridVisualize.gridConstructed)
        {
            if (gridVisualize.gridCellArray[seekerCol, seekerRow].IsWalkable)
            {
                LocateDestination();
            }
            else
            {
                RelocateDestination();
            }
        }
    }

    void LocateDestination()
    {
        Debug.Log("Seeker position is " + seekerCol + ", " + seekerRow);
        seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
        seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
        Debug.Log("Seeker destination is " + seekerCol + ", " + seekerRow);
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[seekerCol, seekerRow]);
    }

    void RelocateDestination()
    {
        Debug.Log("Seeker destination is unwalkable");
        seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
        seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
        Debug.Log("Seeker destination changes to " + seekerCol + ", " + seekerRow);
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[seekerCol, seekerRow]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            seekerCol = (int)collision.transform.position.x;
            seekerRow = (int)collision.transform.position.y;
            Debug.Log("Coin found at " + seekerCol + ", " + seekerRow);
            gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.gridCellArray[seekerCol, seekerRow]);
        }
    }
}
