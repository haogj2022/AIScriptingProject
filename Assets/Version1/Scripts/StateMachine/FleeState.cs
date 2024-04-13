using UnityEngine;

public class FleeState : State
{
    public GridVisualize gridVisualize;
    public HideState hideState;
    public GetCheeseState getCheeseState;
    public bool canSeeSeeker;
    public bool canSeeCheese;

    int hiderCol;
    int hiderRow;

    bool gotCaught;

    public override State RunCurrentState()
    {
        if (canSeeSeeker) return hideState;
        if (canSeeCheese) return getCheeseState;
        else return this;
    }

    private void Start()
    {
        hiderCol = ((int)transform.position.x);
        hiderRow = ((int)transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (collision.tag == "Cheese")
        {
            canSeeCheese = true;
        }

        if (collision.tag == "Seeker")
        {
            Debug.Log("Hider saw Seeker");           
            canSeeSeeker = true;
            hideState.canFlee = false;
            hideState.FindHidingSpot();
        }
    }

    private void Update()
    {
        // seeker and hider are at the same position
        if (gridVisualize.seekerAI.transform.position == gridVisualize.hiderAI.transform.position)
        {
            gotCaught = true;
        }

        if (transform.position == gridVisualize.gridCellGameObjects[hiderCol, hiderRow].transform.position && !canSeeSeeker)
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

            if (gridVisualize.GetGridCell(hiderCol, hiderRow).IsWalkable)
            {
                LocateDestination();
            }
            else
            {
                RelocateDestination();
            }           
        }        
    }

    public void LocateDestination()
    {
        gridVisualize.hiderAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(hiderCol, hiderRow));
    }

    public void RelocateDestination()
    {
        FleeFromSeeker();
    }
}
