/**
* Controller.cs
* Created by Michael Marek (2016)
*
* Provides an abstraction for inputs produced by the Controller object whcih is responsible for
* mapping Unity3D inputs to gamepad inputs. Instead of referencing our player inputs by their axes
* or button references, we can reference them by their actual action. By abstracting the player
* code from directly referencing the Controller object, it is also now possible to replace this
* component with one that artificially produces joystick inputs for the production of automated
* movement, A.I. controllers, etc.
**/

using UnityEngine;
using System.Collections;

public class PlayerInputComponent : MonoBehaviour
{
    public  Controller.ControllerSlot   controllerSlot;
    public  Controller.ControllerType   controllerType;

    private Controller                  controller;


    /**
    * Determine the type of Controller object needed based on the type of joystick plugged in.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        if (controllerSlot == Controller.ControllerSlot.Unplugged)
            return;

        switch (controllerType)
        {
            case Controller.ControllerType.PS3:
                controller = new PS3Controller();
            break;

            case Controller.ControllerType.PS4:
                controller = new PS4Controller();
            break;

            case Controller.ControllerType.XBox360:
                controller = new XBox360Controller();
            break;

            case Controller.ControllerType.XBoxOne:
                controller = new XBoxOneController();
            break;

            case Controller.ControllerType.None:
                controller = GetControllerByType();
            break;

            default:
            break;
        }

        if (controller == null)
            return;

        controller.controllerSlot = controllerSlot;
        controller.controllerType = controllerType;
        controller.Initialize();
    }


    /**
    * Update the controller inputs.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        if (controllerSlot == Controller.ControllerSlot.Unplugged)
            return;

        controller.Update();
    }


    /**
    * Get the type of joystick that is plugged into the current controller slot.
    *
    * @param    null
    * @return   string  internal name Unity assigns for this joystick type
    **/
    public string JoystickType()
    {
        int slot = (int)controllerSlot - 1;
        if (slot >= 0)
            return Input.GetJoystickNames()[(int)controllerSlot - 1];
        return "";
    }


    /**
    * Return a Controller object based on the type of joystick plugged into the controller slot.
    *
    * @param    null
    * @return   Controller  appropriate Controller object matching the type of joystick present
    **/
    public Controller GetControllerByType()
    {
        Debug.Log(JoystickType());
        switch (JoystickType())
        {
            case "PLAYSTATION(R)3 Controller": //PS3
                controllerType = Controller.ControllerType.PS3;
                return new PS3Controller();

            case "Wireless Controller": //PS4
                controllerType = Controller.ControllerType.PS4;
                return new PS4Controller();

            case "XBox 360": //XBox 360
                controllerType = Controller.ControllerType.XBox360;
                return new XBox360Controller();

            case "XBox One": //Xbox One
                controllerType = Controller.ControllerType.XBoxOne;
                return new XBoxOneController();

            default:
                return null;
        }
    }


    public Vector2  Move            { get { return controller.LeftStick;        } }
    public Vector2  Aim             { get { return controller.RightStick;       } }
    public Vector2  Scroll          { get { return controller.DPad;             } }
    public Vector2  Triggers        { get { return controller.Triggers;         } }

    public bool     _ScrollLeft     { get { return controller._DPadLeft;        } }
    public bool     ScrollLeft      { get { return controller.DPadLeft;         } }
    public bool     ScrollLeft_     { get { return controller.DPadLeft_;        } }

    public bool     _ScrollRight    { get { return controller._DPadRight;       } }
    public bool     ScrollRight     { get { return controller.DPadRight;        } }
    public bool     ScrollRight_    { get { return controller.DPadRight_;       } }

    public bool     _ScrollUp       { get { return controller._DPadUp;          } }
    public bool     ScrollUp        { get { return controller.DPadUp;           } }
    public bool     ScrollUp_       { get { return controller.DPadUp_;          } }

    public bool     _ScrollDown     { get { return controller._DPadDown;        } }
    public bool     ScrollDown      { get { return controller.DPadDown;         } }
    public bool     ScrollDown_     { get { return controller.DPadDown_;        } }

    public bool     _AltUse         { get { return controller._LeftTrigger;     } }
    public bool     AltUse          { get { return controller.LeftTrigger;      } }
    public bool     AltUse_         { get { return controller.LeftTrigger_;     } }

    public bool     _Use            { get { return controller._RightTrigger;    } }
    public bool     Use             { get { return controller.RightTrigger;     } }
    public bool     Use_            { get { return controller.RightTrigger_;    } }

    public bool     _Inventory      { get { return controller._LeftBumper;      } }
    public bool     Inventory       { get { return controller.LeftBumper;       } }
    public bool     Inventory_      { get { return controller.LeftBumper_;      } }

    public bool     _Melee          { get { return controller._RightBumper;     } }
    public bool     Melee           { get { return controller.RightBumper;      } }
    public bool     Melee_          { get { return controller.RightBumper_;     } }

    public bool     _Sprint         { get { return controller._LeftClick;       } }
    public bool     Sprint          { get { return controller.LeftClick;        } }
    public bool     Sprint_         { get { return controller.LeftClick_;       } }

    public bool     _RightClick     { get { return controller._RightClick;      } }
    public bool     RightClick      { get { return controller.RightClick;       } }
    public bool     RightClick_     { get { return controller.RightClick_;      } }

    public bool     _Reload         { get { return controller._FaceButtonLeft;  } }
    public bool     Reload          { get { return controller.FaceButtonLeft;   } }
    public bool     Reload_         { get { return controller.FaceButtonLeft_;  } }

    public bool     _Interact       { get { return controller._FaceButtonDown;  } }
    public bool     Interact        { get { return controller.FaceButtonDown;   } }
    public bool     Interact_       { get { return controller.FaceButtonDown_;  } }

    public bool     _Drop           { get { return controller._FaceButtonRight; } }
    public bool     Drop            { get { return controller.FaceButtonRight;  } }
    public bool     Drop_           { get { return controller.FaceButtonRight_; } }

    public bool     _Hotswap        { get { return controller._FaceButtonUp;    } }
    public bool     Hotswap         { get { return controller.FaceButtonUp;     } }
    public bool     Hotswap_        { get { return controller.FaceButtonUp_;    } }

    public bool     _Pause          { get { return controller._Start;           } }
    public bool     Pause           { get { return controller.Start ;           } }
    public bool     Pause_          { get { return controller.Start_;           } }
}
