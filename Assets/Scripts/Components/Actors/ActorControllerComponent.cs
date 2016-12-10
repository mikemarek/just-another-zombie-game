/**
**/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ActorControllerComponent : MonoBehaviour
{
    [Header("Actor Starting State")]
    public  MonoScript          startingState;

    private Stack<ActorState>   states          = new Stack<ActorState>(10);

    /**
    **/
    void Awake()
    {
        if (startingState == null)
            return;

        GotoState( (ActorState)System.Activator.CreateInstance(startingState.GetClass()) );
    }

    /**
    **/
    void Start()
    {
    }

    /**
    **/
    void Update()
    {
        if (states.Count == 0)
            return;

        ActorState state = states.Peek().HandleInput(gameObject);

        if (states.Peek().TimeToExit())
            ExitState();
        if (state != null)
            GotoState(state);
        if (states.Peek().TimeToExit())
            ExitState();

        states.Peek().Update(gameObject);
    }

    /**
    **/
    public void GotoState(ActorState state)
    {
        if (states.Count != 0)
            states.Peek().OnExit(gameObject);
        states.Push(state);
        states.Peek().Initialize(gameObject);
        states.Peek().OnEnter(gameObject);
    }

    /**
    **/
    public void ExitState()
    {
        states.Peek().OnExit(gameObject);
        states.Pop();
        //states.Peek().Initialize(gameObject);
        //states.Peek().OnEnter(gameObject);
        states.Peek().OnReturn(gameObject);
    }

    /**
    **/
    public ActorState state { get { return states.Peek(); } }
}
