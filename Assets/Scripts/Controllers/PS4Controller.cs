/**
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

        axesHandles = new string[8] {
            "axis 1", //move left               - left stick x
            "axis 2", //move up/down            - left stick y
            "axis 3", //aim left/right          - right stick x
            "axis 6", //aim up/down             - right stick y
            "axis 7", //scroll left/right       - dpad x
            "axis 8", //scroll up/down          - dpad y
            "axis 4", //primary item action     - L2
            "axis 5"  //secondary item action   - R2
        };
        buttonHandles = new string[9] {
            "button 4",  //open inventory       - L1
            "button 5",  //melee attack         - R1
            "button 10", //sprint               - L3
            "button 11", //                     - R3
            "button 0",  //reload weapon        - Square
            "button 1",  //interact             - X
            "button 2",  //drop item            - Circle
            "button 3",  //hotswap items        - Triangle
            "button 9"   //pause game           - Options
        };
    }

    public override void PreUpdate()
    {
        leftStick.y = -leftStick.y;
        rightStick.y = -rightStick.y;
        directionalPad.y = -directionalPad.y;
        rearTriggers.x = (rearTriggers.x + 1f) / 2f; // [-1...+1] => [0...1]
        rearTriggers.y = (rearTriggers.y + 1f) / 2f; // [-1...+1] => [0...1]
    }

    public override void PostUpdate()
    {
    }
}
