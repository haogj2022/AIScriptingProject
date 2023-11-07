using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if (nextState != null) SwitchToTheNextState(nextState);
    }

    void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
