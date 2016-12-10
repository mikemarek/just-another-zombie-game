/**
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

    /**
    **/
    public Color colour     { get { return color;                                           } }
    public float progress   { get { return (value < 0f) ? 0f : (value > 1f) ? 1f : value;   } }
}
