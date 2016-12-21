/**
* XBox360Controller.cs
* Created by Michael Marek (2016)
*
* A wrapper class providing XBox 360 controller functionality.
**/

using UnityEngine;
using System.Collections;

public class XBox360Controller : Controller
{
    public XBox360Controller()
    {
        LSDeadZone = 0.2f;
        RSDeadZone = 0.4f;
        DPDeadZone = 0.1f;
        LTDeadZone = 0.2f;
        RTDeadZone = 0.2f;

        map.LSx     = "axis 1";
        map.LSy     = "axis 2";
        map.RSx     = "axis 4";
        map.RSy     = "axis 5";
        map.LC      = "button 8";
        map.RC      = "button 9";
        map.Dx      = "axis 6";
        map.Dy      = "axis 7";
        map.LB      = "button 4";
        map.RB      = "button 5";
        map.LT      = "axis 9";
        map.RT      = "axis 10";
        map.Fl      = "button 2";
        map.Fd      = "button 0";
        map.Fr      = "button 1";
        map.Fu      = "button 3";
        map.Start   = "button 7";
    }

    public override void PreUpdate()
    {
        leftStick.y = -leftStick.y;
        rightStick.y = -rightStick.y;
        dpad.y = -dpad.y;
    }

    public override void PostUpdate()
    {
    }
}
