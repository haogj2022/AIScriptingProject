using UnityEngine;

public class GetCheeseState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public GameObject jerryWins;
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
            jerryWins.SetActive(true);
            //Debug.Log("Hider collected all cheese. Hider won");
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
            gotCheese = true;
            fleeState.canSeeCheese = false;
            fleeState.FleeFromSeeker();
        }
    }
}
