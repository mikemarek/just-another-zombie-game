using UnityEngine;
using System.Collections;

public class PlayerHealingState : ActorState
{
    private HealthKit                   healthKit   = null;

    private float                       prepTime    = 1f;   //time taken before healing
    private float                       prepCounter = 0f;

    private float                       healTime    = 0.1f; //time to heal 1 unit of health
    private float                       healCounter = 0f;

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerEquipmentComponent    equipment;
    private PlayerInteractableComponent interact;
    private HealthComponent             health;
    private ProgressComponent           progress;

    public override ActorState HandleInput(GameObject parent)
    {
        if (health.health == health.maxHealth)
        {
            Exit();
            return null;
        }

        if (healthKit.stackSize == 0)
        {
            equipment.DestroyHeldItem();
            Exit();
            return null;
        }

        if (input.Use_)
            Exit();

        return null;
    }

    public override void Update(GameObject parent)
    {
        if (prepCounter < prepTime)
        {
            prepCounter += (1f / prepTime) * Time.deltaTime;
        }
        else
        {
            prepCounter = 1f;
            healCounter += Time.deltaTime;

            if (healCounter > healTime)
            {
                health.health++;
                healthKit.stackSize--;
                healCounter -= healTime;
            }
        }

        progress.SetProgress(prepCounter);
    }

    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        movement = parent.GetComponent<PlayerMovementComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        interact = parent.GetComponent<PlayerInteractableComponent>();
        health = parent.GetComponent<HealthComponent>();
        progress = parent.GetComponent<ProgressComponent>();
    }

    public override void OnEnter(GameObject parent)
    {
        healthKit = equipment.equipped as HealthKit;

        movement.allowMovement = false;
        movement.allowAiming = false;

        interact.allowInteraction = false;

        progress.Reset();
        progress.SetColour(Color.blue);
        progress.SetProgress(0f);
    }

    public override void OnExit(GameObject parent)
    {
    }
}
