using UnityEngine;
using System.Collections;

public class PlayerVehicleDriverState : ActorState
{
    private Vehicle                     vehicle;
    private VehicleEntrance             entrance;
    private Transform                   seat;

    private PlayerInputComponent        input;
    private PlayerMovementComponent     movement;
    private PlayerInteractableComponent interact;
    private PlayerEquipmentComponent    equipment;
    private PlayerCameraComponent       camera;

    private Rigidbody                   rb;

    public PlayerVehicleDriverState(Vehicle vehicle, VehicleEntrance entrance, Transform seat)
    {
        this.vehicle = vehicle;
        this.entrance = entrance;
        this.seat = seat;
    }

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

    public override void Update(GameObject parent)
    {
        //parent.transform.position = Quaternion.Euler(0f, 0f, seat.transform.eulerAngles.z);
    }

    public override void Initialize(GameObject parent)
    {
        input = parent.GetComponent<PlayerInputComponent>();
        movement = parent.GetComponent<PlayerMovementComponent>();
        interact = parent.GetComponent<PlayerInteractableComponent>();
        equipment = parent.GetComponent<PlayerEquipmentComponent>();
        camera = parent.GetComponent<PlayerCameraComponent>();

        rb = parent.GetComponent<Rigidbody>();
    }

    public override void OnEnter(GameObject parent)
    {
        movement.allowMovement = false;
        movement.allowAiming = false;

        interact.allowInteraction = false;

        equipment.allowItemUse = false;

        Collider collider = parent.GetComponent<Collider>();
        //collider.enabled = false;
        Collider car = vehicle.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, car, true);

        parent.transform.parent = seat.transform;
        parent.transform.position = seat.position;
        parent.transform.rotation = seat.rotation;

        entrance.gameObject.SetActive(false);

        camera.zoom = -0.25f;

        rb.isKinematic = true;
    }

    public override void OnExit(GameObject parent)
    {
        SceneManager sm = GameObject.Find("Scene Manager").GetComponent<SceneManager>();

        entrance.gameObject.SetActive(true);

        parent.transform.parent = sm.playerContainer;
        parent.transform.position = (Vector2)entrance.transform.position;

        vehicle.ApplyGas(0f);
        vehicle.ApplyBrakes(0f);
        vehicle.ApplyEBrakes(0f);

        Collider collider = parent.GetComponent<Collider>();
        //collider.enabled = true;
        Collider car = vehicle.GetComponent<Collider>();
        Physics.IgnoreCollision(collider, car, false);

        camera.zoom = 0f;

        rb.isKinematic = false;
    }
}
