/**
* PS4Controller.cs
* Created by Michael Marek (2016)
*
* A wrapper class providing PS4 controller functionality.
**/

using UnityEngine;
using System.Collections;

public class PS4Controller : Controller
{
    public PS4Controller()
    {
        LSDeadZone = 0.2f;
        RSDeadZone = 0.4f;
        DPDeadZone = 0.1f;
        LTDeadZone = 0.2f;
        RTDeadZone = 0.2f;

        map.LSx     = "axis 1";
        map.LSy     = "axis 2";
        map.RSx     = "axis 3";
        map.RSy     = "axis 6";
        map.LC      = "button 10";
        map.RC      = "button 11";
        map.Dx      = "axis 7";
        map.Dy      = "axis 8";
        map.LB      = "button 4";
        map.RB      = "button 5";
        map.LT      = "axis 4";
        map.RT      = "axis 5";
        map.Fl      = "button 0";
        map.Fd      = "button 1";
        map.Fr      = "button 2";
        map.Fu      = "button 3";
        map.Start   = "button 9";
    }

    public override void PreUpdate()
    {
        leftStick.y = -leftStick.y;
        rightStick.y = -rightStick.y;
        dpad.y = -dpad.y;
        triggers.x = (triggers.x + 1f) / 2f; // [-1...+1] => [0...1]
        triggers.y = (triggers.y + 1f) / 2f; // [-1...+1] => [0...1]
    }

    public override void PostUpdate()
    {
    }
}
