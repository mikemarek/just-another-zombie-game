/**
* MenuManager.cs
* Created by Michael Marek (2016)
*
* DESCRIPTION
**/

using System;
using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Selectable Menu Items")]
    public  GameObject[]    menuItems;

    [Header("Animation Properties")]
    [Range(0f, 1f)]
    public  float           animationSpeed  = 0.1f;
    [Space(8)]
    public  Color           selectedColor   = Color.red;
    public  Color           inactiveColor   = Color.white;
    [Space(8)]
    public  Vector3         selectedScale   = 1.5f * Vector3.one;
    public  Vector3         inactiveScale   = 1.0f * Vector3.one;

    public  Action[]        buttonEvents;

    private int             selectedItem;
    private MeshRenderer[]  render;
    private IEnumerator[]   fades;

    private InputManager    input;


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        GameObject go = GameObject.Find("Input Manager");
        input = go.GetComponent<InputManager>();

        render = new MeshRenderer[menuItems.Length];
        fades = new IEnumerator[menuItems.Length];

        buttonEvents = new Action[6]
        {
            Play,
            Training,
            Options,
            Help,
            Credits,
            Exit
        };

        for (int i = 0; i < menuItems.Length; i++)
        {
            render[i] = menuItems[i].GetComponent<MeshRenderer>();
            fades[i] = FadeOut(i);
            StartCoroutine(fades[i]);
        }

        selectedItem = 0;

        fades[selectedItem] = FadeIn(selectedItem);
        StartCoroutine(fades[selectedItem]);
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    void LateUpdate()
    {
        if (input.activeController._DPadUp)
            ScrollUp();
        else if (input.activeController._DPadDown)
            ScrollDown();

        if (input.activeController._FaceButtonDown)
            buttonEvents[selectedItem].Invoke();
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    private void ScrollUp()
    {
        if (fades[selectedItem] != null)
            StopCoroutine(fades[selectedItem]);
        fades[selectedItem] = FadeOut(selectedItem);
        StartCoroutine(fades[selectedItem]);

        if (--selectedItem < 0)
            selectedItem = menuItems.Length-1;

        if (fades[selectedItem] != null)
            StopCoroutine(fades[selectedItem]);
        fades[selectedItem] = FadeIn(selectedItem);
        StartCoroutine(fades[selectedItem]);
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    private void ScrollDown()
    {
        if (fades[selectedItem] != null)
            StopCoroutine(fades[selectedItem]);
        fades[selectedItem] = FadeOut(selectedItem);
        StartCoroutine(fades[selectedItem]);

        if (++selectedItem > menuItems.Length-1)
            selectedItem = 0;

        if (fades[selectedItem] != null)
            StopCoroutine(fades[selectedItem]);
        fades[selectedItem] = FadeIn(selectedItem);
        StartCoroutine(fades[selectedItem]);
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    private IEnumerator FadeIn(int item)
    {
        Color color = render[item].material.color;
        Vector3 scale = render[item].transform.localScale;

        while (color != selectedColor)
        {
            color = Color.Lerp(color, selectedColor, animationSpeed);
            scale = Vector3.Lerp(scale, selectedScale, animationSpeed);

            render[item].material.color = color;
            render[item].transform.localScale = scale;

            yield return null;
        }
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    private IEnumerator FadeOut(int item)
    {
        Color color = render[item].material.color;
        Vector3 scale = render[item].transform.localScale;

        while (color != inactiveColor)
        {
            color = Color.Lerp(color, inactiveColor, animationSpeed);
            scale = Vector3.Lerp(scale, inactiveScale, animationSpeed);

            render[item].material.color = color;
            render[item].transform.localScale = scale;

            yield return null;
        }
    }


    /**
    **/
    private void Play()     { Debug.Log("Play Game");   }
    private void Training() { Debug.Log("Training");    }
    private void Options()  { Debug.Log("Options");     }
    private void Help()     { Debug.Log("Help");        }
    private void Credits()  { Debug.Log("Credits");     }
    private void Exit()     { Debug.Log("Exit");        }
}
