/**
* ActorControllerComponent.cs
* Created by Michael Marek (2016)
*
* Responsible for updating the current state of an actor and transitioning to new states.
*
* This component is modelled after a pushdown automaton. States are stored in a stack; when
* transitioning to a new state, we simply push it onto the stack, and we pop off the stack to
* transition back to a previous state. This is the crucial feature of the automaton - since we
* store states in a stack, it provides us with a means of tracking order of operations on states.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorControllerComponent : MonoBehaviour
{
    protected   ActorState          startingState;
    private     Stack<ActorState>   states;


    /**
    * Add the actor's starting state to the stack.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        states = new Stack<ActorState>(10);

        if (startingState == null)
        {
            Debug.Log("ActorControllerComponent has been initialized without a starting state!");
            return;
        }

        GotoState(startingState);
    }


    /**
    * Update the actor's current state. The state will perform an initial input validation, and if
    * it doesn't return null, will return a new object instead for the state we wish to transition
    * to. We also check if we need to exit the current state and transition to the previous one.
    *
    * @param    null
    * @return   null
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
    * Push a new state onto the stack and transition to it.
    *
    * @param    ActorState  the new state we wish to transition to
    * @return   null
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
    * Pop our current state off the stack and transition to the previous one (if any).
    *
    * @param    null
    * @return   null
    **/
    public void ExitState()
    {
        states.Peek().OnExit(gameObject);
        states.Pop();

        if (states.Count > 0)
            states.Peek().OnReturn(gameObject);
    }


    public ActorState state { get { return states.Peek(); } }
}
