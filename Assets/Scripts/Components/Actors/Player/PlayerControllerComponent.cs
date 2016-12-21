/**
* PlayerControllerComponent.cs
* Created by Michael Marek (2016)
*
* State controller for main players. Initialize starting state for the player.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControllerComponent : ActorControllerComponent
{
    public PlayerControllerComponent()
    {
        startingState = new PlayerBaseState();
    }
}
