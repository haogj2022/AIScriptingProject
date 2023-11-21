using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public GridVisualize gridVisualize;

    int seekerCol;
    int seekerRow;

    int randomCol;
    int randomRow;

    public override State RunCurrentState()
    {
        return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seeker")
        {                     
            if (gridVisualize.seekerAI.transform.position != gridVisualize.hiderAI.transform.position)
            {
                //Debug.Log("Hider saw Seeker at " + seekerCol + ", " + seekerRow);
                FleeFromSeeker();
            }
        }
    }
  
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Seeker")
    //    {
    //        if (gridVisualize.seekerAI.transform.position != gridVisualize.hiderAI.transform.position)
    //        {
    //            StartCoroutine(FleeFromSeeker());
    //        }
    //    }
    //}

    private void Update()
    {
        seekerCol = (int)gridVisualize.seekerAI.transform.position.x;
        seekerRow = (int)gridVisualize.seekerAI.transform.position.y;
    }

    void FleeFromSeeker()
    {
        randomCol = Random.Range(0, gridVisualize.maxCol - 1);
        randomRow = Random.Range(0, gridVisualize.maxRow - 1);
        gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(randomCol, randomRow));
    }
}
