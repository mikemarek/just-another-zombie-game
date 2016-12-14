/**
* VehiclePassengerEntrance.cs
* Created by Michael Marek (2016)
*
* Have an actor sit in the passenger's seat of a vehicle.
**/

using UnityEngine;
using System.Collections;

public class VehiclePassengerEntrance : VehicleEntrance
{
    protected override void EnterVehicle(Vehicle vehicle, Transform seatPosition)
    {
        transitionState = new PlayerVehiclePassengerState(this, vehicle, seatPosition);
    }
}
