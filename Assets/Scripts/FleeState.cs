using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public GridVisualize gridVisualize;
    public CaughtState caughtState;
    public bool gotCaught;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (gotCaught) return caughtState;
        else return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seeker")
        {
            FleeFromSeeker();
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Seeker")
        {
            FleeFromSeeker();
        }
    }

    private void Update()
    {
        // seeker and hider are at the same position
        if (gridVisualize.seekerAI.transform.position == gridVisualize.hiderAI.transform.position)
        {
            gotCaught = true;
        }

        // hider reaches the last hiding spot
        if (gridVisualize.hiderAI.transform.position == gridVisualize.gridCellGameObjects[hiderCol, hiderRow].transform.position)
        {
            FleeFromSeeker();
        }
    }

    void FleeFromSeeker()
    {
        hiderCol = Random.Range(0, gridVisualize.maxCol - 1);
        hiderRow = Random.Range(0, gridVisualize.maxRow - 1);
        gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
    }
}
