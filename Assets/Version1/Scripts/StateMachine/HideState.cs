using System.Collections;
using UnityEngine;

public class HideState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public GetCheeseState getCheeseState;
    public bool canFlee;
    public bool canSeeCheese;
    public GameObject body;

    int hiderCol;
    int hiderRow;

    public override State RunCurrentState()
    {
        if (canFlee) return fleeState;
        else return this;
    }

    private void Update()
    {
        // seeker and hider are at the same position
        if (gridVisualize.hiderAI.transform.position == gridVisualize.seekerAI.transform.position)
        {
            fleeState.gotCaught = true;
        }
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

        if (hiderCol < gridVisualize.hiderAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (hiderCol > gridVisualize.hiderAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void RelocateDestination()
    {
        FindHidingSpot();
    }
}
