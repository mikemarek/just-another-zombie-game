using UnityEngine;
using System.Collections;

public class PlayerOpenInventoryState : ActorState
{
    private float                       openingDelay    = 0.75f;
    private float                       openingTime     = 0f;
    private bool                        opened          = false;

    private PlayerInputComponent        input;
    private PlayerInteractableComponent interact;
    private PlayerMovementComponent     movement;
    private PlayerEquipmentComponent    equipment;
    private ProgressComponent           progress;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (opened)
            return new PlayerManageInventoryState();

        if (!input.Inventory)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        openingTime += (1f / openingDelay) * Time.deltaTime;
        progress.SetProgress(openingTime);

        if (openingTime >= 1f)
            opened = true;
    }

    /**
    **/
    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        movement = parent.GetComponent<PlayerMovementComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        progress = parent.GetComponent<ProgressComponent>();
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        openingTime = 0f;
        opened = false;

        movement.allowMovement = false;
        movement.allowAiming = false;

        equipment.allowItemUse = false;

        if (!input.Inventory)
            progress.Reset();
        progress.SetColour(Color.white);
        progress.SetProgress(0f);
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        progress.Reset();
    }
}
