using UnityEngine;
using System.Collections;

public class VehicleEntrance : Interactable
{
    public  Vehicle    vehicle;
    public  Transform  seatPosition;
    public  ActorState transitionState;

    public override void Interact(GameObject go)
    {
        EnterVehicle(vehicle, seatPosition);

        ActorControllerComponent controller = go.GetComponent<ActorControllerComponent>();
        controller.GotoState(transitionState);
    }

    protected virtual void EnterVehicle(Vehicle vehicle, Transform seatPosition)
    {
    }

    protected virtual void ExitVehicle(Vehicle vehicle, Transform seatPosition)
    {
    }
}
