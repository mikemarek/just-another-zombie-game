/**
* MenuManager.cs
* Created by Michael Marek (2016)
*
* ...
**/

using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public  GameObject[]    menuItems;

    private int             selectedItem;
    private Animator[]      animators;

    private bool flip = true;


    /**
    **/
    void Start()
    {
        animators = new Animator[menuItems.Length];

        for (int i = 0; i < menuItems.Length; i++)
            animators[i] = menuItems[i].GetComponent<Animator>();

        selectedItem = 0;

        StartCoroutine(Test());
    }


    /**
    **/
    IEnumerator Test()
    {
        while (true)
        {
            if (flip)
                animators[0].Play("Fade In", -1);
            else
                animators[0].Play("Fade Out", -1);

            Debug.Log(flip);

            flip = !flip;

            yield return new WaitForSeconds(1f);
        }
    }


    /**
    **/
    private string CurrentAnimation(Animator animator)
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
