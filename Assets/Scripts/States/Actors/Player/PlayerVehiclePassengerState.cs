/**
* PlayerVehiclePassengerState.cs
* Created by Michael Marek (2016)
*
* Handles the control scheme for the player as they ride around in a vehicle. When entering a
* vehicle, we set the player's attached rigidbody as kinematic so that they won't collide with
* objects while inside of the car, and disable the VehicleEntrance script so that other players
cannot enter the same seat as the player.
**/

using UnityEngine;
using System.Collections;

public class PlayerVehiclePassengerState : ActorState
{
    private Vehicle                     vehicle;    //entrance we used to get into the vehicle
    private VehicleEntrance             entrance;   //vehicle that the actor has entered
    private Transform                   seat;       //location of our seat in the vehicle

    private bool                        staySeated; //do we remain in the vehicle on state change?

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private PlayerCameraComponent       camera;

    private Rigidbody                   rb;

    /**
    **/
    public PlayerVehiclePassengerState(VehicleEntrance entrance, Vehicle vehicle, Transform seat)
    {
        this.vehicle = vehicle;
        this.entrance = entrance;
        this.seat = seat;
    }

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        if (input.Inventory)
        {
            staySeated = true;
            return new PlayerManageInventoryState();
        }

        if (input._Interact)
        {
            staySeated = false;
            ExitVehicle(parent);
            Exit();
        }

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
        if (input.Aim.magnitude == 0f)
            parent.transform.rotation = Quaternion.Slerp(
                parent.transform.rotation,
                Quaternion.Euler(0f, 0f, seat.transform.eulerAngles.z),
                movement.aimingSpeed
            );
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

        rb = parent.GetComponent<Rigidbody>();

        staySeated = true;
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        movement.allowMovement = false;
        movement.allowAiming = true;

        interact.allowInteraction = false;

        equipment.allowItemUse = true;

        Collider collider = parent.GetComponent<Collider>();
        Collider car = vehicle.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, car, true);

        parent.transform.parent = seat.transform;
        parent.transform.position = seat.position;
        parent.transform.rotation = seat.rotation;

        entrance.gameObject.SetActive(false);

        camera.zoom = -0.25f;

        rb.isKinematic = true;
    }

    /**
    **/
    public override void OnExit(GameObject parent)
    {
    }

    /**
    **/
    private void ExitVehicle(GameObject parent)
    {
        if (staySeated)
            return;

        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        parent.transform.parent = scene.playerContainer;
        parent.transform.position = (Vector2)entrance.transform.position;

        Collider collider = parent.GetComponent<Collider>();
        Collider car = vehicle.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, car, false);

        entrance.gameObject.SetActive(true);

        camera.zoom = 0f;

        rb.isKinematic = false;
    }
}
