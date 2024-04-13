using System.Collections;
using UnityEngine;

public class HideState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public GetCheeseState getCheeseState;
    public bool canFlee;
    public bool canSeeCheese;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (canFlee) return fleeState;
        else return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cheese")
        {
            canSeeCheese = true;
        }
    }

    IEnumerator WaitUntilSeekerLeaves()
    {
        //Debug.Log("wait");
        yield return new WaitForSeconds(5f);
        //Debug.Log("flee");
        canFlee = true;
        fleeState.canSeeSeeker = false;
        fleeState.FleeFromSeeker();
    }

    public void FindHidingSpot()
    {
        hiderCol = Random.Range(0, gridVisualize.maxCol - 1);
        hiderRow = Random.Range(0, gridVisualize.maxRow - 1);

        if (gridVisualize.GetGridCell(hiderCol, hiderRow).IsHidingSpot)
        {
            //Debug.Log("Hiding spot found at " + hiderCol + ", " + hiderRow);
            LocateDestination();
            StartCoroutine(WaitUntilSeekerLeaves());
        }
        else
        {
            RelocateDestination();
        }
    }

    public void LocateDestination()
    {
        gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
    }

    public void RelocateDestination()
    {
        FindHidingSpot();
    }
}
