using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State<T> where T : unmanaged
{
    State<T> HandleInput();

    void Enter();
    void Execute();
    void Exit();
    State<T> OnMessaged(T pmsg);
}


public class FSM<T> where T : unmanaged
{
    private State<T> currState;
    private State<T> nextState;

    public void Start(State<T> currState)
    {
        this.currState = currState;
        currState.Enter();
    }

    public void ManageState(State<T> state = null)
    {
        if (state != null)
            nextState = state;
        else
            nextState = currState.HandleInput();

        if (nextState != null)
        {
            currState.Exit();
            currState = nextState;
            currState.Enter();
        }
        currState.Execute();
    }

    public void PrintLog()
    {
        Debug.Log(currState.GetType().Name);
    }

    public void SendMessage(T msg)
    {
        ManageState(currState.OnMessaged(msg));
    }
}
