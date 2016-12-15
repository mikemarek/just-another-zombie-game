/**
* InputManager.cs
* Created by Michael Marek (2016)
*
* The Input Manager holds references to all of the different Controller objects in the game that
* are responsible for handling physical gamepad inputs. Controller objects are populated
* automatically based on the number of connected joysticks detected by Unity. The names of these
* joysticks are then mapped to each specific Controller object so that proper inputs can be
* retrieved.
**/

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private ControllerType[]    controllerTypes;    //different types of controllers plugged in
    private Controller[]        controllers;        //controller object references for gamepads
    private Controller          active;             //last used controller [currently active]


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    void Awake()
    {
        if (controllerTypes == null || controllers == null)
            InitializeControllers();
    }


    /**
    * Update inputs from each connected controller, and determine the currently active controller.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        for (int i = 0; i < controllers.Length; i++)
            if (controllers[i] != null)
                if (controllers[i].Update())
                    active = controllers[i];
    }


    /**
    * Populate a list of connected controllers with references to objects that can retrieve
    *  physical gamepad inputs.
    *
    * @param    null
    * @return   null
    **/
    public void InitializeControllers()
    {
        if (controllerTypes != null && controllers != null)
            return;

        int numControllers = ControllerSlot.GetNames(typeof(ControllerSlot)).Length;
        string[] joysticks = Input.GetJoystickNames();

        controllers = new Controller[numControllers];
        controllerTypes = new ControllerType[numControllers];


        for (int i = 0; i < joysticks.Length; i++)
        {
            if (i >= numControllers)
                break;

            controllerTypes[i] = GetControllerScheme(i);
            controllers[i] = GetControllerType(controllerTypes[i]);

            if (controllers[i] == null)
                continue;

            controllers[i].controllerType = controllerTypes[i];
            controllers[i].controllerSlot = (ControllerSlot)(i + 1);
            controllers[i].Initialize();
        }
    }


    /**
    * Determine the type of controller plugged in to a specific slot.
    *
    * @param    int             position of the connected joystick in Input.GetJoystickNames()
    * @return   ControllerType  the type of controller plugged in to the slot
    **/
    private ControllerType GetControllerScheme(int slot)
    {
        switch (Input.GetJoystickNames()[slot])
        {
            case "PLAYSTATION(R)3 Controller": //PS3
                return ControllerType.PS3;

            case "Wireless Controller": //PS4
                return ControllerType.PS4;

            case "Controller (XBOX 360 For Windows)": //XBox 360
                return ControllerType.XBox360;

            case "XBox One": //Xbox One
                return ControllerType.XBoxOne;

            default:
                return ControllerType.None;
        }
    }


    /**
    * Determine the type of Controller object needed to handle gamepad inputs based on the type
    * of controller plugged in.
    *
    * @param    ControllerType  the type of controller plugged in
    * @return   Controller      a Controller object that can handle that type of gamepad input
    **/
    private Controller GetControllerType(ControllerType type)
    {
        switch (type)
        {
            case ControllerType.PS3:
                return new PS3Controller();

            case ControllerType.PS4:
                return new PS4Controller();

            case ControllerType.XBox360:
                return new XBox360Controller();

            case ControllerType.XBoxOne:
                return new XBoxOneController();

            default:
                return null;
        }
    }


    /**
    * Print the list of currently connected controllers to the console.
    *
    * @param    null
    * @return   null
    **/
    private void PrintControllers()
    {
        string[] joysticks = Input.GetJoystickNames();
        string output = "[";

        for (int i = 0; i < joysticks.Length; i++)
            output += joysticks[i] + (i == joysticks.Length-1 ? "]" : ", ");

        Debug.Log(output);
    }


    public  Controller[]    connectedControllers    { get { return controllers; } }
    public  Controller      activeController        { get { return active;      } }
}
