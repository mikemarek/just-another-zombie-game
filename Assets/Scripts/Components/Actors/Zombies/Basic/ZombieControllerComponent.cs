/**
* ZombieControllerComponent.cs
* Created by Michael Marek (2016)
*
* State controller for basic zombies. Initialize starting state for the zombie.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieControllerComponent : ActorControllerComponent
{
    public ZombieControllerComponent()
    {
        startingState = new ZombieWanderingState();
    }
}
