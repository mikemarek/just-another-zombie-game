/**
* PlayerInteractionState.cs
* Created by Michael Marek (2016)
*
* Transitional state as player interacts with an item or object. When returning to this state we
* perform several checks in case the item we interacted with is no longer present, and take actions
* accordingly.
**/

using UnityEngine;
using System.Collections;

public class PlayerInteractionState : ActorState
{
    private Interactable                    interactable;
    private float                           interactionTime;

    private PlayerInputComponent            input;
    private PlayerMovementComponent         movement;
    private PlayerInteractableComponent     interact;
    private PlayerEquipmentComponent        equipment;
    private ProgressComponent               progress;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (interactable == null)
        {
            Exit();
            return null;
        }

        if (!input.Interact)
        {
            interactable.StopInteracting();
            Exit();
            return null;
        }

        //if (interactionTime >= 1f)
        //{
            interactable.Interact(parent);
            Exit();
        //}

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        if (interactable != null)
        {
            interactionTime += (1f / interactable.interactionTime) * Time.deltaTime;
            progress.SetProgress(interactionTime);
        }
    }

    /**
    **/
    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        movement = parent.GetComponent<PlayerMovementComponent>();
        interact = parent.GetComponent<PlayerInteractableComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        progress = parent.GetComponent<ProgressComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        interactionTime = 0f;

        movement.allowMovement = false;
        movement.allowAiming = false;

        equipment.allowItemUse = false;

        interactable = interact.ClosestInteraction();

        if (interactable == null || !interactable.Usable())
        {
            Exit();
            return;
        }

        if (interactable.interactionTime == 0f)
        {
            interactable.Interact(parent);
            Exit();
            return;
        }

        interactable.StartInteracting();

        //progress.SetColour(new Color(1f, 0.6f, 0f, 1f));
        progress.SetProgress(0f);
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        if (interactable != null)
            interactable.StopInteracting();
        progress.Reset();
    }

    /**
    **/
    public override void OnReturn(GameObject parent)
    {
        interactionTime = 0f;
        if (interactable != null)
            interactable.StopInteracting();
        Exit();
    }
}
