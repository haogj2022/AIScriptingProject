using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : State
{
    public bool caughtBySeeker;

    public override State RunCurrentState()
    {
        caughtBySeeker = true;
        return this;
    }
}
