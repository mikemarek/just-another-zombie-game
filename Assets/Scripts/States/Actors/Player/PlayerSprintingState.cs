/**
* PlayerSprintingState.cs
* Created by Michael Marek (2016)
*
* Handles the control scheme for the player as they sprint.
**/

using UnityEngine;
using System.Collections;

public class PlayerSprintingState : ActorState
{
    private float                       shakeAmount = 50f;      //amount of camera shake per step
    private float                       stepTime    = 0.25f;    //time between steps [camera shake] (sec)
    private float                       stepCounter = 0f;

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private PlayerCameraComponent       camera;
    private ProgressComponent           progress;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (input.Move.magnitude == 0f)
            Exit();

        if (!movement.sprinting)
            Exit();

        if (input._Sprint)
            Exit();

        if (movement.currentVelocity.magnitude < 0.1f)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        progress.SetProgress(movement.sprintingStamina);

        stepCounter += (1f / stepTime) * Time.deltaTime;
        if (stepCounter >= 1f)
        {
            stepCounter = 0f;
            camera.Shake(shakeAmount);
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
        camera = parent.GetComponent<PlayerCameraComponent>();
        progress = parent.GetComponent<ProgressComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        stepCounter = 0f;

        movement.allowMovement = true;
        movement.allowAiming = false;
        movement.sprinting = true;

        interact.allowInteraction = false;

        equipment.allowItemUse = false;

        progress.SetColour(Color.green);
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        movement.sprinting = false;
    }
}
