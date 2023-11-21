using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : unmanaged
{
    IState<T> HandleInput();

    void Enter();
    void Execute();
    void Exit();
    IState<T> OnMessaged(T pmsg);
}


public class FSM<T> where T : unmanaged
{
    private IState<T> currState;
    private IState<T> nextState;

    public void Start(IState<T> currState)
    {
        this.currState = currState;
        currState.Enter();
    }

    public void ManageState(IState<T> state = null)
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
