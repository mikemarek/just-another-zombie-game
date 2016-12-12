/**
* PlayerVehiclePassengerState.cs
* Created by Michael Marek (2016)
*
* Handles the control scheme for the player as they ride around in a vehicle. When entering a
* vehicle, we set the player's attached rigidbody as kinematic so that they won't collide with
* objects while inside of the car, and disable the VehicleEntrance script so that other players
cannot enter the same seat as the player.
**/
/**
using UnityEngine;
using System.Collections;

public class PlayerVehiclePassengerState : ActorState
{
    private Vehicle                 vehicle;
    private VehicleEntrance         entrance;
    private Transform               seat;

    private InputComponent          input;
    private MovementComponent       movement;
    private InteractableComponent   interact;
    private EquipmentComponent      equipment;
    private ProgressComponent       progress;

    private Rigidbody               rb;

    public PlayerVehiclePassengerState(Vehicle vehicle, VehicleEntrance entrance, Transform seat)
    {
        this.vehicle = vehicle;
        this.entrance = entrance;
        this.seat = seat;
    }

    public override ActorState HandleInput(GameObject parent)
    {
        if (input.Inventory)
            return new PlayerOpenInventoryState();

        if (input._Interact)
        {
            ExitVehicle(parent);
            Exit();
        }

        return null;
    }

    public override void Update(GameObject parent)
    {
        if (input.Aim.magnitude == 0f)
            parent.transform.rotation = Quaternion.Slerp(
                parent.transform.rotation,
                Quaternion.Euler(0f, 0f, seat.transform.eulerAngles.z),
                movement.aimingSpeed
            );
    }

    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<InputComponent>();
        movement = parent.GetComponent<MovementComponent>();
        interact = parent.GetComponent<InteractableComponent>();
        equipment = parent.GetComponent<EquipmentComponent>();
        rb = parent.GetComponent<Rigidbody>();
    }

    public override void OnEnter(GameObject parent)
    {
        movement.allowMovement = false;
        movement.allowAiming = true;

        interact.allowInteraction = false;

        equipment.allowItemUse = true;

        Collider collider = parent.GetComponent<Collider>();
        collider.enabled = false;

        parent.transform.parent = seat.transform;
        parent.transform.position = seat.position;
        parent.transform.rotation = seat.rotation;

        rb.isKinematic = true;
    }

    public override void OnExit(GameObject parent)
    {
    }

    private void ExitVehicle(GameObject parent)
    {
        parent.transform.position = (Vector2)entrance.transform.position;

        Collider collider = parent.GetComponent<Collider>();
        collider.enabled = true;

        rb.isKinematic = false;
    }
}
**/
