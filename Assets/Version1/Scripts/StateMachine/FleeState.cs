using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public GridVisualize gridVisualize;
    public GetCheeseState getCheeseState;
    public bool canSeeCheese;

    int hiderCol;
    int hiderRow;

    bool gotCaught;

    public override State RunCurrentState()
    {
        if (canSeeCheese) return getCheeseState;
        else return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seeker")
        {
            FleeFromSeeker();
        }       

        if (collision.tag == "Cheese")
        {
            canSeeCheese = true;
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

    public void FleeFromSeeker()
    {
        if (!gotCaught && !canSeeCheese)
        {
            hiderCol = Random.Range(0, gridVisualize.maxCol - 1);
            hiderRow = Random.Range(0, gridVisualize.maxRow - 1);
            gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
        }        
    }
}
