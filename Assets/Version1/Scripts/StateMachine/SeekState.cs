using UnityEngine;

public class SeekState : State
{
    public GridVisualize gridVisualize;
    public ChaseState chaseState;
    public bool canSeeHider;

    int seekerCol;
    int seekerRow;

    public override State RunCurrentState()
    {
        if (canSeeHider) return chaseState;          
        else return this;
    }

    private void Update()
    {
        MoveToDestination();
    }

    void MoveToDestination()
    {
        if (transform.position == gridVisualize.gridCellGameObjects[seekerCol, seekerRow].transform.position && !canSeeHider)
        {
            //Debug.Log("Seeker position is " + seekerCol + ", " + seekerRow);
            seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
            seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
            //Debug.Log("Seeker destination is " + seekerCol + ", " + seekerRow);
            NextDestination();
        }
    }

    void NextDestination()
    {
        if (gridVisualize.GetGridCell(seekerCol, seekerRow).IsWalkable)
        {
            LocateDestination();
        }
        else
        {
            RelocateDestination();
        }
    }

    void LocateDestination()
    {        
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(seekerCol, seekerRow));
    }

    void RelocateDestination()
    {
        //Debug.Log("Seeker destination is unwalkable");
        seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
        seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
        //Debug.Log("Seeker destination changes to " + seekerCol + ", " + seekerRow);
        NextDestination();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hider")
        {            
            canSeeHider = true;
        }
    }
}
