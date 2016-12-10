/**
**/

using UnityEngine;
using System.Collections;

public class PlayerInputComponent : MonoBehaviour
{
    public  Controller.ControllerSlot   controllerSlot;
    public  Controller.ControllerType   controllerType;

    private Controller                  controller;

    /**
    **/
    void Start()
    {
        if (controllerSlot == Controller.ControllerSlot.Unplugged)
            return;

        switch (controllerType)
        {
            case Controller.ControllerType.PS4:
                controller = new PS4Controller();
            break;

            case Controller.ControllerType.XBox360:
                controller = new XBox360Controller();
            break;

            default:
                controller = new PS4Controller();
            break;
        }

        controller.controllerSlot = controllerSlot;
        controller.Initialize();
    }

    /**
    **/
    void Update()
    {
        if (controllerSlot == Controller.ControllerSlot.Unplugged)
            return;

        controller.Update();
    }

    /**
    **/
    public Vector2  Move            { get { return controller.LeftStick;        } } //Left Stick
    public Vector2  Aim             { get { return controller.RightStick;       } } //Right Stick
    public Vector2  Scroll          { get { return controller.DPad;             } } //DPad
    public Vector2  RearTriggers    { get { return controller.RearTriggers;     } } //Rear Triggers

    public bool     _ScrollLeft     { get { return controller._DPadLeft;        } } //DPad Left
    public bool     ScrollLeft      { get { return controller.DPadLeft;         } } //DPad Left
    public bool     ScrollLeft_     { get { return controller.DPadLeft_;        } } //DPad Left

    public bool     _ScrollRight    { get { return controller._DPadRight;       } } //DPad Right
    public bool     ScrollRight     { get { return controller.DPadRight;        } } //DPad Right
    public bool     ScrollRight_    { get { return controller.DPadRight_;       } } //DPad Right

    public bool     _ScrollUp       { get { return controller._DPadUp;          } } //DPad Up
    public bool     ScrollUp        { get { return controller.DPadUp;           } } //DPad Up
    public bool     ScrollUp_       { get { return controller.DPadUp_;          } } //DPad Up

    public bool     _ScrollDown     { get { return controller._DPadDown;        } } //DPad Down
    public bool     ScrollDown      { get { return controller.DPadDown;         } } //DPad Down
    public bool     ScrollDown_     { get { return controller.DPadDown_;        } } //DPad Down

    public bool     _AltUse         { get { return controller._LeftTrigger;     } } //Left Trigger
    public bool     AltUse          { get { return controller.LeftTrigger;      } } //Left Trigger
    public bool     AltUse_         { get { return controller.LeftTrigger_;     } } //Left Trigger

    public bool     _Use            { get { return controller._RightTrigger;    } } //Right Trigger
    public bool     Use             { get { return controller.RightTrigger;     } } //Right Trigger
    public bool     Use_            { get { return controller.RightTrigger_;    } } //Right Trigger

    public bool     _Inventory      { get { return controller._LeftBumper;      } } //Left Bumper
    public bool     Inventory       { get { return controller.LeftBumper;       } } //Left Bumper
    public bool     Inventory_      { get { return controller.LeftBumper_;      } } //Left Bumper

    public bool     _Melee          { get { return controller._RightBumper;     } } //Right Bumper
    public bool     Melee           { get { return controller.RightBumper;      } } //Right Bumper
    public bool     Melee_          { get { return controller.RightBumper_;     } } //Right Bumper

    public bool     _Sprint         { get { return controller._LeftClick;       } } //Left Click
    public bool     Sprint          { get { return controller.LeftClick;        } } //Left Click
    public bool     Sprint_         { get { return controller.LeftClick_;       } } //Left Click

    public bool     _RightClick     { get { return controller._RightClick;      } } //Right Click
    public bool     RightClick      { get { return controller.RightClick;       } } //Right Click
    public bool     RightClick_     { get { return controller.RightClick_;      } } //Right Click

    public bool     _Reload         { get { return controller._FaceButtonLeft;  } } //Left Face Button
    public bool     Reload          { get { return controller.FaceButtonLeft;   } } //Left Face Button
    public bool     Reload_         { get { return controller.FaceButtonLeft_;  } } //Left Face Button

    public bool     _Interact       { get { return controller._FaceButtonDown;  } } //Bottom Face Button
    public bool     Interact        { get { return controller.FaceButtonDown;   } } //Bottom Face Button
    public bool     Interact_       { get { return controller.FaceButtonDown_;  } } //Bottom Face Button

    public bool     _Drop           { get { return controller._FaceButtonRight; } } //Right Face Button
    public bool     Drop            { get { return controller.FaceButtonRight;  } } //Right Face Button
    public bool     Drop_           { get { return controller.FaceButtonRight_; } } //Right Face Button

    public bool     _Hotswap        { get { return controller._FaceButtonUp;    } } //Top Face Button
    public bool     Hotswap         { get { return controller.FaceButtonUp;     } } //Top Face Button
    public bool     Hotswap_        { get { return controller.FaceButtonUp_;    } } //Top Face Button

    public bool     _Pause          { get { return controller._Start;           } } //Start
    public bool     Pause           { get { return controller.Start ;           } } //Start
    public bool     Pause_          { get { return controller.Start_;           } } //Start
}
