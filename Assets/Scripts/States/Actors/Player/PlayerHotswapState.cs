/**
* PlayerHotswapState.cs
* Created by Michael Marek (2016)
*
* Transitional state as player switches between weapons.
**/

using UnityEngine;
using System.Collections;

public class PlayerHotswapState : ActorState
{
    private float                       swapDelay   = 0.25f;
    private float                       swapTime    = 0f;
    private bool                        swapped     = false;

    private PlayerInputComponent        input;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private ProgressComponent           progress;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (swapped)
        {
            equipment.HotswapItems();
            Exit();
            return null;
        }

        if (!input.Hotswap)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        swapTime += (1f / swapDelay) * Time.deltaTime;
        progress.SetProgress(swapTime);

        if (swapTime >= 1f)
            swapped = true;
    }

    /**
    **/
    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        interact = parent.GetComponent<PlayerInteractableComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        progress = parent.GetComponent<ProgressComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        interact.allowInteraction = false;
        equipment.allowItemUse = false;

        progress.SetColour(Color.red);
        progress.SetProgress(0f);
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        progress.Reset();
    }
}
