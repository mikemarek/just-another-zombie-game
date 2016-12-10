using UnityEngine;
using System.Collections;

public class VehicleDriverEntrance : VehicleEntrance
{
    protected override void EnterVehicle(Vehicle vehicle, Transform seatPosition)
    {
        transitionState = new PlayerVehicleDriverState(vehicle, this, seatPosition);
    }

    protected override void ExitVehicle(Vehicle vehicle, Transform seatPosition)
    {
    }
}
