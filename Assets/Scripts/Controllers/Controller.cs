/**
* Controller.cs
* Created by Michael Marek (2016)
*
* A wrapper class for providing standardized gamepad input values to the game. Rather than creating
* different character controllers for each gamepad type, we can use this class to utilize a set of
* standardized Unity input values and only using the ones corrisponding with certain gamepads.
*
*
* Before use, you must set up your standardized inputs in Unity, under Project Settings > Input:
*
* for each Joy Num [Joystick 1-4]:
*   for each Axis [X-axis, Y-axis, Axis 3...20]:
*     create axis input ('Joystick Axis')
*     create button input ('Key or Mouse Button')
*
* These should be named in a standard manner (eg. "joystick 1 axis 5" or "joystick 5 button 3").
* Finally, depending on your naming convention for each joystick, edit or override in a subclass
* the function JoystickName() to return a valid joystick name based on the controller slot.
*
*
* Once your inputs are set up, you can inherit from this class and map your gamepad inputs to those
* in Unity. In your inherited class' constructor, you can set both the variables 'axesHandles[]'
* and 'buttonHandles[]' to hold the string names of the Unity axes and button values for each
* gamepad axes and button. You can also use the methods PreUpdate() and PostUpdate() to modify the
* values of the joystick axes before they are read. For example, different joysticks can represent
* rear trigger presses as float values from [-1, +1], [0, 1], or [0, 2], while the game accepts
* input in the range of [0, 1]. These methods are ideal for correcting issues like this.
*
* For a typical "next-gen" controller (think PS3 and XBox360), you are going to have a
* total of 8 axes and 9 buttons. This input wrapper assumes you are using a gamepad following a
* similar design, and provides metrics for determining input as such (see getter functions at the
* bottom of this class). Below is a quick reference chart for what indices in 'axesHandles[]' and
* 'buttonHandles[]' map to which gamepad inputs:
*
* axesHandles[8]    = {LSx, LSy, RSx, RSy, Dx, Dy, LTx, LTy}
* buttonHandles[9]  = {LB, RB, LC, RC, F<, Fv, F>, F^, Start}
**/

using UnityEngine;
using System.Collections;

public class Controller
{
    public enum ControllerSlot {
        Unplugged,
        Controller1,
        Controller2,
        Controller3,
        Controller4
    };

    public enum ControllerType {
        None,
        PS3,
        PS4,
        XBox360,
        XBoxOne
    };

    public      ControllerSlot  controllerSlot;
    public      ControllerType  controllerType;

    public      float           LSDeadZone;     //left joystick deadzone
    public      float           RSDeadZone;     //right joystick deadzone
    public      float           DPDeadZone;     //directional pad deadzone
    public      float           LTDeadZone;     //left trigger deadzone
    public      float           RTDeadZone;     //right trigger deadzone

    protected   ButtonMap       map;            //gamepad input => Unity input map

    protected   Vector2         leftStick;      //left joystick x/y-axis values
    protected   Vector2         rightStick;     //right joystick x/y-axis values
    protected   Vector2         dpad;           //directional pad x/y-axis values
    protected   Vector2         triggers;       //left [x] and right [y] trigger values

    private     string          joyname;        //Unity Input Manager joystick reference name
    private     bool[]          axesStates;     //current state of axes [in-use or not]
    private     bool[]          axesPress;      //if an axis started use during this frame
    private     bool[]          axesRelease;    //if an axis stopped use during this frame


    /**
    * Initialize axes state arrays and get joystick name.
    *
    * @param    null
    * @return   null
    **/
    public void Initialize()
    {
        axesStates = new bool[6];
        axesPress = new bool[6];
        axesRelease = new bool[6];

        joyname = JoystickName();
    }


    /**
    * Allows a controller to modify its raw input values before the base controller class uses
    * them to calculate actual game input. Useful for normalizing axes inputs or for swapping axes
    * that are potentially inverted (ie. PS4 controller has inverted left stick y-axis from Unity -
    * use this method to set leftStick.y = -leftStick.y so that we get valid input).
    *
    * @param    null
    * @return   null
    **/
    public virtual void PreUpdate()
    {
        //
    }


    /**
    * Allows a controller to modify its raw input values after the base controller class uses
    * them to calculate actual game input. Useful for normalizing axes inputs or for swapping axes
    * that are potentially inverted (ie. PS4 controller has inverted left stick y-axis from Unity -
    * use this method to set leftStick.y = -leftStick.y so that we get valid input).
    *
    * @param    null
    * @return   null
    **/
    public virtual void PostUpdate()
    {
    }


    /**
    * Captures raw input values for each of the joystick axes and buttons and tracks events around
    * the activation and deactivation of axes ('press' and 'release' events for each axis).
    *
    * @param    null
    * @return   null
    **/
    public void Update()
    {
        //get raw axes input values
        leftStick.x     = Input.GetAxis(joyname + map.LSx);
        leftStick.y     = Input.GetAxis(joyname + map.LSy);
        rightStick.x    = Input.GetAxis(joyname + map.RSx);
        rightStick.y    = Input.GetAxis(joyname + map.RSy);
        dpad.x          = Input.GetAxis(joyname + map.Dx);
        dpad.y          = Input.GetAxis(joyname + map.Dy);
        triggers.x      = Input.GetAxis(joyname + map.LT);
        triggers.y      = Input.GetAxis(joyname + map.RT);

        PreUpdate();

        //subtract deadzone to left and right triggers
        triggers.x -= LTDeadZone;
        triggers.y -= RTDeadZone;

        //reset axes activation/deactivation booleans
        for (int i = 0; i < 6; i++)
        {
            axesPress[i] = false;
            axesRelease[i] = false;
        }

        //calculate axes activation/deactivation values and semaphores
        axesPress[0] = dpad.x < -DPDeadZone && !axesStates[0];
        axesPress[1] = dpad.x > +DPDeadZone && !axesStates[1];
        axesPress[2] = dpad.y < -DPDeadZone && !axesStates[2];
        axesPress[3] = dpad.y > +DPDeadZone && !axesStates[3];
        axesPress[4] = triggers.x > LTDeadZone && !axesStates[4];
        axesPress[5] = triggers.y > RTDeadZone && !axesStates[5];

        axesRelease[0] = dpad.x < -DPDeadZone && axesStates[0];
        axesRelease[1] = dpad.x < +DPDeadZone && axesStates[1];
        axesRelease[2] = dpad.y < -DPDeadZone && axesStates[2];
        axesRelease[3] = dpad.y < +DPDeadZone && axesStates[3];
        axesRelease[4] = triggers.x < LTDeadZone && axesStates[4];
        axesRelease[5] = triggers.y < RTDeadZone && axesStates[5];

        axesStates[0] = dpad.x < -DPDeadZone;
        axesStates[1] = dpad.x > +DPDeadZone;
        axesStates[2] = dpad.y < -DPDeadZone;
        axesStates[3] = dpad.y > +DPDeadZone;
        axesStates[4] = triggers.x > LTDeadZone;
        axesStates[5] = triggers.y > RTDeadZone;

        //apply deadzone to left and right joysticks
        if (leftStick.magnitude < LSDeadZone)
            leftStick = Vector2.zero;
        if (rightStick.magnitude < RSDeadZone)
            rightStick = Vector2.zero;

        //add deadzone back to triggers - need actual trigger values now for getter Triggers()
        triggers.x += LTDeadZone;
        triggers.y += RTDeadZone;

        PostUpdate();
    }


    /**
    * Returns the joystick name that is used to reference Unity Input Manager values. This method
    * can be overwritten in case of different naming conventions for various gamepad types.
    *
    * @param    null
    * @return   string  joystick reference name
    **/
    public virtual string JoystickName()
    {
        return System.String.Format("joystick {0} ", (int)controllerSlot);
    }


    /**
    * Determine if the controller plugged into this controller slot is unplugged.
    *
    * @param    null
    * @return   bool    is the controller unplugged?
    **/
    public bool Unplugged()
    {
        /*if (Input.GetAxisRaw(joyname + map.LSx) == 0f &&
            Input.GetAxisRaw(joyname + map.LSy) == 0f &&
            Input.GetAxisRaw(joyname + map.RSx) == 0f &&
            Input.GetAxisRaw(joyname + map.RSy) == 0f &&
            Input.GetAxisRaw(joyname + map.Dx)  == 0f &&
            Input.GetAxisRaw(joyname + map.Dy)  == 0f &&
            Input.GetAxisRaw(joyname + map.LT)  == 0f &&
            Input.GetAxisRaw(joyname + map.RT)  == 0f)
            return true;*/
        return false;
    }


    /**
    * Various getter methods for calculated axes and button inputs. An underscore (_) preceding
    * the gamepad input value returns true only on the frame that axis or button was activated. An
    * underscore (_) after the gamepad input value returns true only on the frame that axis or
    * button was deactivated. Without an underscore (_) will return either the axis value during
    * that frame (taking into account deadzone), or a boolean if the button is being held down.
    **/
    public Vector2  LeftStick           { get { return leftStick;                                   } }
    public Vector2  RightStick          { get { return rightStick;                                  } }
    public Vector2  DPad                { get { return dpad;                                        } }
    public Vector2  Triggers            { get { return triggers;                                    } }

    public bool     _DPadLeft           { get { return axesPress[0];                                } }
    public bool     DPadLeft            { get { return dpad.x < -DPDeadZone;                        } }
    public bool     DPadLeft_           { get { return axesRelease[0];                              } }

    public bool     _DPadRight          { get { return axesPress[1];                                } }
    public bool     DPadRight           { get { return dpad.x > +DPDeadZone;                        } }
    public bool     DPadRight_          { get { return axesRelease[1];                              } }

    public bool     _DPadUp             { get { return axesPress[2];                                } }
    public bool     DPadUp              { get { return dpad.y < -DPDeadZone;                        } }
    public bool     DPadUp_             { get { return axesRelease[2];                              } }

    public bool     _DPadDown           { get { return axesPress[3];                                } }
    public bool     DPadDown            { get { return dpad.y > +DPDeadZone;                        } }
    public bool     DPadDown_           { get { return axesRelease[3];                              } }

    public bool     _LeftTrigger        { get { return axesPress[4];                                } }
    public bool     LeftTrigger         { get { return triggers.x > LSDeadZone;                     } }
    public bool     LeftTrigger_        { get { return axesRelease[4];                              } }

    public bool     _RightTrigger       { get { return axesPress[5];                                } }
    public bool     RightTrigger        { get { return triggers.y > RSDeadZone;                     } }
    public bool     RightTrigger_       { get { return axesRelease[5];                              } }

    public bool     _LeftBumper         { get { return Input.GetButtonDown  (joyname + map.LB);     } }
    public bool     LeftBumper          { get { return Input.GetButton      (joyname + map.LB);     } }
    public bool     LeftBumper_         { get { return Input.GetButtonUp    (joyname + map.LB);     } }

    public bool     _RightBumper        { get { return Input.GetButtonDown  (joyname + map.RB);     } }
    public bool     RightBumper         { get { return Input.GetButton      (joyname + map.RB);     } }
    public bool     RightBumper_        { get { return Input.GetButtonUp    (joyname + map.RB);     } }

    public bool     _LeftClick          { get { return Input.GetButtonDown  (joyname + map.LC);     } }
    public bool     LeftClick           { get { return Input.GetButton      (joyname + map.LC);     } }
    public bool     LeftClick_          { get { return Input.GetButtonUp    (joyname + map.LC);     } }

    public bool     _RightClick         { get { return Input.GetButtonDown  (joyname + map.RC);     } }
    public bool     RightClick          { get { return Input.GetButton      (joyname + map.RC);     } }
    public bool     RightClick_         { get { return Input.GetButtonUp    (joyname + map.RC);     } }

    public bool     _FaceButtonLeft     { get { return Input.GetButtonDown  (joyname + map.Fl);     } }
    public bool     FaceButtonLeft      { get { return Input.GetButton      (joyname + map.Fl);     } }
    public bool     FaceButtonLeft_     { get { return Input.GetButtonUp    (joyname + map.Fl);     } }

    public bool     _FaceButtonDown     { get { return Input.GetButtonDown  (joyname + map.Fd);     } }
    public bool     FaceButtonDown      { get { return Input.GetButton      (joyname + map.Fd);     } }
    public bool     FaceButtonDown_     { get { return Input.GetButtonUp    (joyname + map.Fd);     } }

    public bool     _FaceButtonRight    { get { return Input.GetButtonDown  (joyname + map.Fr);     } }
    public bool     FaceButtonRight     { get { return Input.GetButton      (joyname + map.Fr);     } }
    public bool     FaceButtonRight_    { get { return Input.GetButtonUp    (joyname + map.Fr);     } }

    public bool     _FaceButtonUp       { get { return Input.GetButtonDown  (joyname + map.Fu);     } }
    public bool     FaceButtonUp        { get { return Input.GetButton      (joyname + map.Fu);     } }
    public bool     FaceButtonUp_       { get { return Input.GetButtonUp    (joyname + map.Fu);     } }

    public bool     _Start              { get { return Input.GetButtonDown  (joyname + map.Start);  } }
    public bool     Start               { get { return Input.GetButton      (joyname + map.Start);  } }
    public bool     Start_              { get { return Input.GetButtonUp    (joyname + map.Start);  } }

    public bool     _Select             { get { return Input.GetButtonDown  (joyname + map.Select); } }
    public bool     Select              { get { return Input.GetButton      (joyname + map.Select); } }
    public bool     Select_             { get { return Input.GetButtonUp    (joyname + map.Select); } }
}
