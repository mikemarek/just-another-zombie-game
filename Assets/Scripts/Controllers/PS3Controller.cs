/**
* PS3Controller.cs
* Created by Michael Marek (2016)
*
* A wrapper class providing PS3 controller functionality.
**/

using UnityEngine;
using System.Collections;

public class PS3Controller : Controller
{
    public PS3Controller()
    {
        LSDeadZone = 0.2f;
        RSDeadZone = 0.4f;
        DPDeadZone = 0.1f;
        LTDeadZone = 0.2f;
        RTDeadZone = 0.2f;

        map.LSx     = "axis 1";
        map.LSy     = "axis 2";
        map.RSx     = "axis 3";
        map.RSy     = "axis 5";
        map.LC      = "button 10";
        map.RC      = "button 11";
        map.Dx      = "axis 6";
        map.Dy      = "axis 7";
        map.LB      = "button 6";
        map.RB      = "button 7";
        map.LT      = "axis 4";
        map.RT      = "axis 5";
        map.Fl      = "button 3";
        map.Fd      = "button 2";
        map.Fr      = "button 1";
        map.Fu      = "button 0";
        map.Start   = "button 9";
        map.Select  = "button 8";
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
