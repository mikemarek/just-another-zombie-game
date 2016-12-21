/**
* ButtonMap.cs
* Created by Michael Marek (2016)
*
* A simple data structure for mapping specific gamepad buttons and axis names to internal Unity
* Input Manager joystick names.
**/

public struct ButtonMap
{
    public  string  LSx;    //left joystick x-axis
    public  string  LSy;    //left joystick y-axis
    public  string  RSx;    //right joystick x-axis
    public  string  RSy;    //right joystick y-axis

    public  string  LC;     //left joystick click
    public  string  RC;     //right joystick click

    public  string  Dx;     //directional pad x-axis
    public  string  Dy;     //directional pad y-axis

    public  string  LB;     //left bumper [shoulder]
    public  string  RB;     //right bumper [shoulder]
    public  string  LT;     //left trigger
    public  string  RT;     //right trigger

    public  string  Fl;     //face button left
    public  string  Fd;     //face button down
    public  string  Fr;     //face button right
    public  string  Fu;     //face button up

    public  string  Start;  //start/options button
    public  string  Select; //select/back button
}
