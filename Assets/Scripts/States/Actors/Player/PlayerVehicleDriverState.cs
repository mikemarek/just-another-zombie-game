/**
* PlayerVehicleDriverState.cs
* Created by Michael Marek (2016)
*
* Handles the control scheme for the player as they drive a vehicle. When entering a vehicle, we
* set the player's attached rigidbody as kinematic so that they won't collide with objects while
* inside of the car, and disable the VehicleEntrance script so that other players cannot enter the
* same seat as the player.
**/

using UnityEngine;
using System.Collections;

public class PlayerVehicleDriverState : ActorState
{
    private VehicleEntrance             entrance;   //entrance we used to get into the vehicle
    private Vehicle                     vehicle;    //vehicle that the actor has entered
    private Transform                   seat;       //location of our seat in the vehicle

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private PlayerCameraComponent       camera;

    private Rigidbody                   rb;

    /**
    **/
    public PlayerVehicleDriverState(VehicleEntrance entrance, Vehicle vehicle, Transform seat)
    {
        this.entrance = entrance;
        this.vehicle = vehicle;
        this.seat = seat;
    }

    /**
    **/
    public override ActorState HandleInput(GameObject parent)
    {
        vehicle.ApplyGas(input.Triggers.y);
        vehicle.ApplyBrakes(input.Triggers.x);
        vehicle.ApplyEBrakes(input.Drop ? 1f : 0f);
        vehicle.Steer(input.Move.x);

        if (input._Interact)
            Exit();

        return null;
    }

    /**
    **/
    public override void Update(GameObject parent)
    {
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
    }

    /**
    **/
    public override void OnEnter(GameObject parent)
    {
        movement.allowMovement = false;
        movement.allowAiming = false;

        interact.allowInteraction = false;

        equipment.allowItemUse = false;

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
        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        parent.transform.parent = scene.playerContainer;
        parent.transform.position = (Vector2)entrance.transform.position;

        vehicle.ApplyGas(0f);
        vehicle.ApplyBrakes(0f);
        vehicle.ApplyEBrakes(0f);

        Collider collider = parent.GetComponent<Collider>();
        Collider car = vehicle.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, car, false);

        entrance.gameObject.SetActive(true);

        camera.zoom = 0f;

        rb.isKinematic = false;
    }
}
