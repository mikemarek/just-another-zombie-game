/**
**/

using UnityEngine;
using System.Collections;

public class WeaponState
{

    /**
    **/
    public virtual WeaponState HandleInput(Weapon weapon, uint mode)
    {
        return null;
    }

    /**
    **/
    public virtual void Update(Weapon weapon, uint mode)
    {
    }

    /**
    **/
    public virtual void Initialize(Weapon weapon, uint mode)
    {
    }

    /**
    **/
    public virtual void OnEnter(Weapon weapon, uint mode)
    {
    }

    /**
    **/
    public virtual void OnExit(Weapon weapon, uint mode)
    {
    }
}
