using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCheeseState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public bool gotCheese;
    public int numOfCheese;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (gotCheese) return fleeState;
        else return this;
    }

    private void Update()
    {
        if (numOfCheese == 2)
        {
            Debug.Log("Hider collected all cheese. Hider won");
            numOfCheese = 0;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cheese")
        {
            hiderCol = (int)collision.transform.position.x;
            hiderRow = (int)collision.transform.position.y;
            gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cheese")
        {
            fleeState.canSeeCheese = false;
            fleeState.FleeFromSeeker();
            gotCheese = true;
        }
    }
}
