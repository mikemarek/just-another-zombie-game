/**
* VehicleEntrance.cs
* Created by Michael Marek (2016)
*
* Allows an actor to enter a vehicle. When an actor interacts with the vehicle entrance, we can
* specify an actor state for that actor to transition into. This way, if the actor enters the
* driver-side door, we can transition them to a state where they can control the vehicle. If the
* actor enters a passenger-side door we can put them in a state where they can shoot out the window
* of the vehicle, etc.
**/

using UnityEngine;
using System.Collections;

public class VehicleEntrance : Interactable
{
    [Space(10)]
    public  Vehicle    vehicle;
    public  Transform  seatPosition;
    public  ActorState transitionState;


    /**
    * Attempt to enter the vehicle.
    *
    * @param    GameObject  the actor entering the vehicle
    * @return   null
    **/
    public override void Interact(GameObject go)
    {
        EnterVehicle(vehicle, seatPosition);

        ActorControllerComponent controller = go.GetComponent<ActorControllerComponent>();
        controller.GotoState(transitionState);
    }


    /**
    * Enter the vehicle.
    *
    * @param    Vehicle     the vehicle that the actor is entering
    * @param    Transform   location of their seating position in the vehicle
    * @return   null
    **/
    protected virtual void EnterVehicle(Vehicle vehicle, Transform seatPosition)
    {
    }


    /**
    * Exit the vehicle.
    *
    * @param    Vehicle     the vehicle that the actor is exiting
    * @param    Transform   location of their seating position in the vehicle
    * @return   null
    **/
    protected virtual void ExitVehicle(Vehicle vehicle, Transform seatPosition)
    {
    }
}
