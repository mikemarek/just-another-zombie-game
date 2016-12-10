/**
**/

using UnityEngine;
using System.Collections;

public class ActorState
{
    protected bool exit = false;

    /**
    **/
    public virtual ActorState HandleInput(GameObject parent)
    {
        return null;
    }

    /**
    **/
    public virtual void Update(GameObject parent)
    {
    }

    /**
    **/
    public virtual void Initialize(GameObject parent)
    {
    }

    /**
    **/
    public virtual void OnEnter(GameObject parent)
    {
    }

    /**
    **/
    public virtual void OnExit(GameObject parent)
    {
    }

    /**
    **/
    public virtual void OnReturn(GameObject parent)
    {
        OnEnter(parent);
    }

    /**
    **/
    public void Exit()
    {
        exit = true;
    }

    /**
    **/
    public bool TimeToExit()
    {
        return exit;
    }
}
