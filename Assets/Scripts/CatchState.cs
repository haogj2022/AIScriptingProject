using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchState : State
{
    public override State RunCurrentState()
    {
        return this;
    }
}
