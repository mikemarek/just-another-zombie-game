/**
* ActorState.cs
* Created by Michael Marek (2016)
*
* A template for creating transitional states for controlling actors. States allow for the
* transition of functionality for game entities. A state can essentially turn on or off the various
* components that are attached to the actor, enabling or disabling functionality for that state
* (ie. turning off weapons functionality while the player runs so they cannot run-and-gun). States
* also set properties and call upon methods belonging to these components, allowing the actor to
* utilize the functionality they provide.
**/

using UnityEngine;
using System.Collections;

public class ActorState
{
    //a flag to exit out of the current state prematurely
    protected bool exit = false;


    /**
    * Main branching point for the state. We can perform actor input or property checks here, and
    * determine if we wish to transition out of the current state. By returning a new instance of
    * a state, we can exit and transition to that state immediately. By returning null, we can
    * stay in this current state and continue to update.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   ActorState  the new state we wish to transition to; null if we wish to stay
    **/
    public virtual ActorState HandleInput(GameObject parent)
    {
        return null;
    }


    /**
    * Update the current state.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public virtual void Update(GameObject parent)
    {
    }


    /**
    * Initialize the state when we enter it for the first time. Set up any initial properties here.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public virtual void Initialize(GameObject parent)
    {
    }


    /**
    * Called when we enter this state for the first time or when we transition back to this state
    * from a previous one.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public virtual void OnEnter(GameObject parent)
    {
    }


    /**
    * Called when we exit this state and transition to a new one.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public virtual void OnExit(GameObject parent)
    {
    }


    /**
    * Called when we return back to this state from one that was just exited.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public virtual void OnReturn(GameObject parent)
    {
        OnEnter(parent);
    }


    /**
    * Exit this state prematurely.
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   null
    **/
    public void Exit()
    {
        exit = true;
    }


    /**
    * Are we ready to exit this state?
    *
    * @param    GameObject  a reference to the actor that the state machine is operating on
    * @return   bool        flag determining if we should exit the state prematurely
    **/
    public bool TimeToExit()
    {
        return exit;
    }
}
