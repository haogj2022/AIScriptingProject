using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchState : State
{
    bool caughtHider;

    public override State RunCurrentState()
    {
        if (caughtHider)
        {
            Debug.Log("Seeker caught Hider. Seeker won");
            caughtHider = false;
        }

        Time.timeScale = 0;
        return this;
    }

    private void Start()
    {
        caughtHider = true;
    }
}
