/**
* ProgressComponent.cs
* Created by Michael Marek (2016)
*
* Different actions that the player engage in take a finite amount of time to complete. This class
* provides a way for those actions to easily display their progress. Change the progress value
* (0...1) and associated action display colour and a seperate UI element can use them to provide
* visual feedback to the player.
**/

using UnityEngine;
using System.Collections;

public class ProgressComponent : MonoBehaviour
{
    private Color   color   = Color.clear;
    private float   value   = 0f;

    /**
    **/
    public void SetProgress(float progress)
    {
        value = progress;
    }

    /**
    **/
    public void SetColour(Color color)
    {
        this.color = color;
    }

    /**
    **/
    public void Reset()
    {
        color = Color.clear;
        value = 0f;
    }

    public Color colour     { get { return color;                                           } }
    public float progress   { get { return (value < 0f) ? 0f : (value > 1f) ? 1f : value;   } }
}
