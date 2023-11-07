using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool canSeeTheEnemy;

    public override State RunCurrentState()
    {
        if (canSeeTheEnemy) return chaseState;          
        else return this;
    }
}
