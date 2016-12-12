/**
* PlayerReloadState.cs
* Created by Michael Marek (2016)
*
* Reloads the player's current weapon after a set delay (weapon reload time).
**/

using UnityEngine;
using System.Collections;

public class PlayerReloadState : ActorState
{
    private float                       reloadTimer = 0f;
    private float                       reloadDelay = 1f;
    private bool                        reload      = false;

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private ProgressComponent           progress;

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (reload)
        {
            equipment.ReloadWeapon();
            Exit();
            if (equipment.CanReload())
                return new PlayerReloadState();
            return null;
        }

        //cancel the reload and transition to appropriate state
        if (input._Reload)
        {
            equipment.CancelReload();
            Exit();
            return null;
        }
        else if (input._Sprint)
        {
            equipment.CancelReload();
            Exit();
            return new PlayerSprintingState();
        }
        /*else if (input._Melee)
        {
            Exit();
            return PlayerMeleeState();
        }*/

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        reloadTimer += (1f / reloadDelay) * Time.deltaTime;
        progress.SetProgress(reloadTimer);

        if (reloadTimer >= 1f)
            reload = true;
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
        reloadDelay = equipment.ReloadTime();

        if (Mathf.Approximately(reloadDelay, 0f))
        {
            Exit();
            return;
        }

        equipment.StartReload();

        interact.allowInteraction = false;
        movement.reloading = true;
        movement.reloadingSpeed = (equipment.equipped as Weapon).reloadMoveSpeedRatio;

        progress.SetColour(Color.blue);
        progress.SetProgress(0f);
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
        movement.reloading = false;
    }
}
