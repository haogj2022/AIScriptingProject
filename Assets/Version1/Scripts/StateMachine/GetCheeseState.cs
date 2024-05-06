using UnityEngine;

public class GetCheeseState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public GameObject jerryWins;
    public bool gotCheese;
    public int numOfCheese;
    public GameObject body;

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
            jerryWins.SetActive(true);
            //Debug.Log("Hider collected all cheese. Hider won");
            numOfCheese = 0;
            Time.timeScale = 0;
        }

        // seeker and hider are at the same position
        if (gridVisualize.hiderAI.transform.position == gridVisualize.seekerAI.transform.position)
        {
            fleeState.gotCaught = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cheese" && !fleeState.canSeeSeeker)
        {
            //Debug.Log("Get Cheese State");
            hiderCol = (int)collision.transform.position.x;
            hiderRow = (int)collision.transform.position.y;
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Cheese" && !fleeState.canSeeSeeker)
        {
            //Debug.Log("got cheese");
            gotCheese = true;
            fleeState.canSeeCheese = false;
            fleeState.FleeFromSeeker();
        }
    }
}
