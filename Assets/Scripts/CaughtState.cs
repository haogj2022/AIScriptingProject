using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : State
{
    public override State RunCurrentState()
    {
        return this;
    }
}
