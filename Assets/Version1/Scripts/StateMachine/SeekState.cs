using System.Collections;
using UnityEngine;

public class SeekState : State
{
    public GridVisualize gridVisualize;
    public ChaseState chaseState;
    public bool canSeeHider;
    public bool hiderIsHiding;
    public GameObject body;

    int seekerCol;
    int seekerRow;

    public override State RunCurrentState()
    {
        if (canSeeHider) return chaseState;          
        else return this;
    }

    private void Update()
    {
        if (!hiderIsHiding)
        {
            MoveToDestination();
        }
    }

    public void FindSeekingSpot()
    {
        seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
        seekerRow = Random.Range(0, gridVisualize.maxRow - 1);

        if (gridVisualize.GetGridCell(seekerCol, seekerRow).IsSeekingSpot)
        {
            //Debug.Log("Seeking at " + seekerCol + ", " + seekerRow);
            LocateDestination();
            StartCoroutine(WaitUntilHiderLeaves());
        }
        else
        {
            RelocateDestination();
            FindSeekingSpot();
        }
    }

    IEnumerator WaitUntilHiderLeaves()
    {
        //Debug.Log("wait");
        yield return new WaitForSeconds(2f);
        //Debug.Log("chase");
        canSeeHider = true;
        hiderIsHiding = false;
        chaseState.ChaseHider();
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
        if (gridVisualize.GetGridCell(seekerCol, seekerRow).IsWalkable && !gridVisualize.GetGridCell(seekerCol, seekerRow).IsRestricted)
        {
            LocateDestination();
        }
        else
        {
            RelocateDestination();
            NextDestination();
        }      
    }

    void LocateDestination()
    {        
        gridVisualize.seekerAI.SetDestination(gridVisualize, gridVisualize.GetGridCell(seekerCol, seekerRow));

        if (seekerCol < gridVisualize.seekerAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (seekerCol > gridVisualize.seekerAI.transform.position.x)
        {
            body.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void RelocateDestination()
    {
        //Debug.Log("Seeker destination is unwalkable");
        seekerCol = Random.Range(0, gridVisualize.maxCol - 1);
        seekerRow = Random.Range(0, gridVisualize.maxRow - 1);
        //Debug.Log("Seeker destination changes to " + seekerCol + ", " + seekerRow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hider")
        {            
            canSeeHider = true;
            hiderIsHiding = false;
            chaseState.ChaseHider();
        }
    }
}
