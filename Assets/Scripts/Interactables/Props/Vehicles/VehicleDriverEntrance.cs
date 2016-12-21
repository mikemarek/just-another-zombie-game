/**
* VehicleDriverEntrance.cs
* Created by Michael Marek (2016)
*
* Have an actor take the driver's seat of a vehicle.
**/

using UnityEngine;
using System.Collections;

public class VehicleDriverEntrance : VehicleEntrance
{
    protected override void EnterVehicle(Vehicle vehicle, Transform seatPosition)
    {
        transitionState = new PlayerVehicleDriverState(this, vehicle, seatPosition);
    }
}
