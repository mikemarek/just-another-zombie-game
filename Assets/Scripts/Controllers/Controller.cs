/**
 * axes     : LSx, LSy, RSx, RSy, Dx, Dy, LTx, LTy
 * buttons  : LB, RB, LC, RC, F<, Fv, F>, F^, Start
**/

using UnityEngine;
using System.Collections;

public class Controller
{
    public  enum            ControllerSlot  {Unplugged, Controller1, Controller2, Controller3, Controller4};
    public  enum            ControllerType  {None, PS4, PS3, XBox360};

    public  float           LSDeadZone;
    public  float           RSDeadZone;
    public  float           DPDeadZone;
    public  float           LTDeadZone;
    public  float           RTDeadZone;

    public  ControllerSlot  controllerSlot;
    public  ControllerType  controllerType;
    public  string[]        axesHandles;
    public  string[]        buttonHandles;

    public  Vector2         leftStick;
    public  Vector2         rightStick;
    public  Vector2         directionalPad;
    public  Vector2         rearTriggers;

    private string          name;
    private bool[]          axesState;
    private bool[]          axesPress;
    private bool[]          axesRelease;

    public Controller()
    {
    }

    public void Initialize()
    {
        axesState = new bool[6];
        axesPress = new bool[6];
        axesRelease = new bool[6];

        switch (controllerSlot)
        {
            case ControllerSlot.Controller1:
                name = "joystick 1";
                break;
            case ControllerSlot.Controller2:
                name = "joystick 2";
                break;
            case ControllerSlot.Controller3:
                name = "joystick 2";
                break;
            case ControllerSlot.Controller4:
                name = "joystick 4";
                break;
            default:
                name = "joystick 1";
                break;
        }
    }

    public virtual void PreUpdate()
    {
        //
    }

    public void Update()
    {
        //get raw axis input values
        leftStick.x      = Input.GetAxis(name + " " + axesHandles[0]);
        leftStick.y      = Input.GetAxis(name + " " + axesHandles[1]);
        rightStick.x     = Input.GetAxis(name + " " + axesHandles[2]);
        rightStick.y     = Input.GetAxis(name + " " + axesHandles[3]);
        directionalPad.x = Input.GetAxis(name + " " + axesHandles[4]);
        directionalPad.y = Input.GetAxis(name + " " + axesHandles[5]);
        rearTriggers.x   = Input.GetAxis(name + " " + axesHandles[6]);
        rearTriggers.y   = Input.GetAxis(name + " " + axesHandles[7]);

        PreUpdate();

        rearTriggers.x -= LTDeadZone;
        rearTriggers.y -= RTDeadZone;

        for (int i = 0; i < 6; i++)
        {
            axesPress[i] = false;
            axesRelease[i] = false;
        }

        axesPress[0] = directionalPad.x < -DPDeadZone && !axesState[0];
        axesPress[1] = directionalPad.x > +DPDeadZone && !axesState[1];
        axesPress[2] = directionalPad.y < -DPDeadZone && !axesState[2];
        axesPress[3] = directionalPad.y > +DPDeadZone && !axesState[3];
        axesPress[4] = rearTriggers.x   > LTDeadZone && !axesState[4];
        axesPress[5] = rearTriggers.y   > RTDeadZone && !axesState[5];

        axesRelease[0] = directionalPad.x < -DPDeadZone && axesState[0];
        axesRelease[1] = directionalPad.x < +DPDeadZone && axesState[1];
        axesRelease[2] = directionalPad.y < -DPDeadZone && axesState[2];
        axesRelease[3] = directionalPad.y < +DPDeadZone && axesState[3];
        axesRelease[4] = rearTriggers.x   < LTDeadZone && axesState[4];
        axesRelease[5] = rearTriggers.y   < RTDeadZone && axesState[5];

        axesState[0] = directionalPad.x < -DPDeadZone;
        axesState[1] = directionalPad.x > +DPDeadZone;
        axesState[2] = directionalPad.y < -DPDeadZone;
        axesState[3] = directionalPad.y > +DPDeadZone;
        axesState[4] = rearTriggers.x   > LTDeadZone;
        axesState[5] = rearTriggers.y   > RTDeadZone;

        if (leftStick.magnitude < LSDeadZone)
            leftStick = Vector2.zero;
        if (rightStick.magnitude < RSDeadZone)
            rightStick = Vector2.zero;

        PostUpdate();
    }

    public virtual void PostUpdate()
    {
        //
    }

    public Vector2  LeftStick           { get { return leftStick;                                               } }
    public Vector2  RightStick          { get { return rightStick;                                              } }
    public Vector2  DPad                { get { return directionalPad;                                          } }
    public Vector2  RearTriggers        { get { return rearTriggers + new Vector2(LTDeadZone, RTDeadZone);      } }

    public bool     _DPadLeft           { get { return axesPress[0];                                            } }
    public bool     DPadLeft            { get { return directionalPad.x < -DPDeadZone;                          } }
    public bool     DPadLeft_           { get { return axesRelease[0];                                          } }

    public bool     _DPadRight          { get { return axesPress[1];                                            } }
    public bool     DPadRight           { get { return directionalPad.x > +DPDeadZone;                          } }
    public bool     DPadRight_          { get { return axesRelease[1];                                          } }

    public bool     _DPadUp             { get { return axesPress[2];                                            } }
    public bool     DPadUp              { get { return directionalPad.y < -DPDeadZone;                          } }
    public bool     DPadUp_             { get { return axesRelease[2];                                          } }

    public bool     _DPadDown           { get { return axesPress[3];                                            } }
    public bool     DPadDown            { get { return directionalPad.y > +DPDeadZone;                          } }
    public bool     DPadDown_           { get { return axesRelease[3];                                          } }

    public bool     _LeftTrigger        { get { return axesPress[4];                                            } }
    public bool     LeftTrigger         { get { return rearTriggers.x > LSDeadZone;                             } }
    public bool     LeftTrigger_        { get { return axesRelease[4];                                          } }

    public bool     _RightTrigger       { get { return axesPress[5];                                            } }
    public bool     RightTrigger        { get { return rearTriggers.y > RSDeadZone;                             } }
    public bool     RightTrigger_       { get { return axesRelease[5];                                          } }

    public bool     _LeftBumper         { get { return Input.GetButtonDown  (name + " " + buttonHandles[0]);    } }
    public bool     LeftBumper          { get { return Input.GetButton      (name + " " + buttonHandles[0]);    } }
    public bool     LeftBumper_         { get { return Input.GetButtonUp    (name + " " + buttonHandles[0]);    } }

    public bool     _RightBumper        { get { return Input.GetButtonDown  (name + " " + buttonHandles[1]);    } }
    public bool     RightBumper         { get { return Input.GetButton      (name + " " + buttonHandles[1]);    } }
    public bool     RightBumper_        { get { return Input.GetButtonUp    (name + " " + buttonHandles[1]);    } }

    public bool     _LeftClick          { get { return Input.GetButtonDown  (name + " " + buttonHandles[2]);    } }
    public bool     LeftClick           { get { return Input.GetButton      (name + " " + buttonHandles[2]);    } }
    public bool     LeftClick_          { get { return Input.GetButtonUp    (name + " " + buttonHandles[2]);    } }

    public bool     _RightClick         { get { return Input.GetButtonDown  (name + " " + buttonHandles[3]);    } }
    public bool     RightClick          { get { return Input.GetButton      (name + " " + buttonHandles[3]);    } }
    public bool     RightClick_         { get { return Input.GetButtonUp    (name + " " + buttonHandles[3]);    } }

    public bool     _FaceButtonLeft     { get { return Input.GetButtonDown  (name + " " + buttonHandles[4]);    } }
    public bool     FaceButtonLeft      { get { return Input.GetButton      (name + " " + buttonHandles[4]);    } }
    public bool     FaceButtonLeft_     { get { return Input.GetButtonUp    (name + " " + buttonHandles[4]);    } }

    public bool     _FaceButtonDown     { get { return Input.GetButtonDown  (name + " " + buttonHandles[5]);    } }
    public bool     FaceButtonDown      { get { return Input.GetButton      (name + " " + buttonHandles[5]);    } }
    public bool     FaceButtonDown_     { get { return Input.GetButtonUp    (name + " " + buttonHandles[5]);    } }

    public bool     _FaceButtonRight    { get { return Input.GetButtonDown  (name + " " + buttonHandles[6]);    } }
    public bool     FaceButtonRight     { get { return Input.GetButton      (name + " " + buttonHandles[6]);    } }
    public bool     FaceButtonRight_    { get { return Input.GetButtonUp    (name + " " + buttonHandles[6]);    } }

    public bool     _FaceButtonUp       { get { return Input.GetButtonDown  (name + " " + buttonHandles[7]);    } }
    public bool     FaceButtonUp        { get { return Input.GetButton      (name + " " + buttonHandles[7]);    } }
    public bool     FaceButtonUp_       { get { return Input.GetButtonUp    (name + " " + buttonHandles[7]);    } }

    public bool     _Start              { get { return Input.GetButtonDown  (name + " " + buttonHandles[8]);    } }
    public bool     Start               { get { return Input.GetButton      (name + " " + buttonHandles[8]);    } }
    public bool     Start_              { get { return Input.GetButtonUp    (name + " " + buttonHandles[8]);    } }
}
