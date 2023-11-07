using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public IdleState idleState;
    public bool haveAttacked;

    public override State RunCurrentState()
    {
        if (haveAttacked) return idleState;
        return this;
    }
}
