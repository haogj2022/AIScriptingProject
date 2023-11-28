using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public GridVisualize gridVisualize;
    public CatchState catchState;
    public bool canCatchHider;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (canCatchHider) return catchState;
        else return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hider")
        {
            Debug.Log("Seeker found Hider at " + hiderCol + ", " + hiderRow);
            ChaseHider();            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Hider")
        {
            Debug.Log("Hider has fleed away. Seeker chased Hider");
            ChaseHider();
        }
    }

    private void Update()
    {        
        if (gridVisualize.seekerAI.transform.position == gridVisualize.hiderAI.transform.position)
        {
            canCatchHider = true;
        }
    }

    void ChaseHider()
    {
        hiderCol = (int)gridVisualize.hiderAI.transform.position.x;
        hiderRow = (int)gridVisualize.hiderAI.transform.position.y;
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
    }
}
