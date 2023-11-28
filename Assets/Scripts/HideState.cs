using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideState : State
{
    public GridVisualize gridVisualize;
    public FleeState fleeState;
    public bool canSeeSeeker;

    public override State RunCurrentState()
    {
        if (canSeeSeeker) return fleeState;
        else return this;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Seeker")
        {
            canSeeSeeker = true;
        }
    }
}
