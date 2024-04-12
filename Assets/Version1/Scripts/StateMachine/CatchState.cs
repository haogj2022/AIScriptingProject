using UnityEngine;

public class CatchState : State
{
    public GameObject tomWins;
    bool caughtHider;

    public override State RunCurrentState()
    {
        if (caughtHider)
        {
            tomWins.SetActive(true);
            //Debug.Log("Seeker caught Hider. Seeker won");
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
