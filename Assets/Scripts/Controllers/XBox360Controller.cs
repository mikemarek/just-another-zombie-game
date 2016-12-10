/**
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

        axesHandles = new string[8] {
            "axis 1", //move left               - left stick x
            "axis 2", //move up/down            - left stick y
            "axis 4", //aim left/right          - right stick x
            "axis 5", //aim up/down             - right stick y
            "axis 6", //scroll left/right       - dpad x
            "axis 7", //scroll up/down          - dpad y
            "axis 9", //primary item action     - Left Trigger
            "axis 10" //secondary item action   - Right Trigger
        };
        buttonHandles = new string[9] {
            "button 4",  //open inventory       - Left Bumber
            "button 5",  //melee attack         - Right Bumper
            "button 8",  //sprint               - Left Click
            "button 9",  //                     - Right Click
            "button 2",  //reload weapon        - X
            "button 0",  //interact             - A
            "button 1",  //drop item            - B
            "button 3",  //hotswap items        - Y
            "button 7"   //pause game           - Start
        };
    }

    public override void PreUpdate()
    {
        leftStick.y = -leftStick.y;
        rightStick.y = -rightStick.y;
        directionalPad.y = -directionalPad.y;
    }

    public override void PostUpdate()
    {
    }
}
