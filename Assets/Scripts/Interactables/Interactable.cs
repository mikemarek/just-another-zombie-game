/**
* Interactable.cs
* Created by Michael Marek (2016)
*
* A base class for interactable objects in the game world. Any object with an 'interactable' script
* attached to it is able to be interacted with by an actor, and have certain functionality
* activated upon doing so. Classes can inherit from this one to create different types of
* interactle objects in the game world. This class provides several methods that can be overwritten
* to change the functionality of the interactable object, as well as prepare the object with a
* means to display a prompt message when an actor is close enough to it.
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public  float               interactionTime     = 0.5f;         //time taken to interact (sec)
    [Space(10)]
    public  TextMesh            textMesh            = null;         //text mesh for prompt message
    public  float               textFadeTime        = 0.25f;        //fade in/out time of text (sec)
    public  float               textAngle           = 15f;          //tilt angle of the text mesh
    [Space(10)]
    [TextArea]
    public  string              promptText          = "<empty>";    //displayed interaction message

    private bool                inUse               = false;        //is an actor currently interacting?

    private float               textAlpha           = 0f;
    private List<GameObject>    withinReach         = new List<GameObject>();
    private Vector3             rotation            = Vector3.zero;

    private MeshRenderer        render;


    /**
    * Initialize the properties of the displayed message text.
    *
    * @param    null
    * @return   null
    **/
    void Awake()
    {
        render = textMesh.gameObject.GetComponent<MeshRenderer>();

        textMesh.text = promptText;

        AlignText();

        Color color = render.material.color;
        color.a = textAlpha;
        render.material.color = color;
    }

    /**
    * Update the interactable object.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        if (textAlpha > 0f)
            AlignText();
    }


    /**
    * Keep track of actors nearby the interactable object. If any are within reach, fade in the
    * prompt message text so that they can see it.
    *
    * @param    Collider    the collider of the actor nearby
    * @return   null
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (!withinReach.Contains(collider.gameObject))
            {
                if (withinReach.Count == 0)
                {
                    textMesh.text = promptText;
                    StopAllCoroutines();
                    StartCoroutine(FadeInText());
                }

                withinReach.Add(collider.gameObject);
            }
        }
    }


    /**
    * Keep track of actors nearby the interactable object. If any are within reach, fade in the
    * prompt message text to provide some visual relief.
    *
    * @param    Collider    the collider of the actor leaving the vicinity of the interactable
    * @return   null
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (withinReach.Contains(collider.gameObject))
            {
                withinReach.Remove(collider.gameObject);
                Leave(collider.gameObject);

                if (withinReach.Count == 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(FadeOutText());
                    inUse = false;
                }
            }
        }
    }


    /**
    * Called when the object has been interacted with. Override this method in a subclass to change
    * the functionality of the interactable object.
    *
    * @param    GameObject  the actor that interacted with this object
    * @return   null
    **/
    public virtual void Interact(GameObject go)
    {
    }


    /**
    * Called when the object has stopped being interacted with. Override this method in a subclass
    * to change the functionality of the interactable object.
    *
    * @param    GameObject  the actor that stopped interacting with this object
    * @return   null
    **/
    public virtual void Leave(GameObject go)
    {
    }


    /**
    * Check if the object can be interacted with (it's not already being interacted with).
    *
    * @param    null
    * @return   bool    can we interact with this object?
    **/
    public bool Usable()
    {
        return !inUse;
    }


    /**
    * Start interacting with the object.
    *
    * @param    null
    * @return   null
    **/
    public void StartInteracting()
    {
        inUse = true;
    }


    /**
    * Stop interacting with the object.
    *
    * @param    null
    * @return   null
    **/
    public void StopInteracting()
    {
        inUse = false;
    }


    /**
    * Fade in the prompt message display text.
    *
    * @param    null
    * @return   null
    **/
    protected IEnumerator FadeInText()
    {
        while (textAlpha < 1f)
        {
            float delta = (1f / textFadeTime) * Time.deltaTime;
            textAlpha = (textAlpha + delta < 1f ? textAlpha + delta : 1f);

            Color color = render.material.color;
            color.a = textAlpha;
            render.material.color = color;

            yield return null;
        }
    }


    /**
    * Fade out the prompt message display text.
    *
    * @param    null
    * @return   null
    **/
    protected IEnumerator FadeOutText()
    {
        while (textAlpha > 0f)
        {
            float delta = (1f / textFadeTime) * Time.deltaTime;
            textAlpha = (textAlpha - delta > 0f ? textAlpha - delta : 0f);

            Color color = render.material.color;
            color.a = textAlpha;
            render.material.color = color;

            yield return null;
        }
    }


    /**
    * COMMENT
    *
    * @param    null
    * @return   null
    **/
    protected void AlignText()
    {
        rotation = new Vector3(
            0f,
            Mathf.Sin(textAngle * Mathf.Deg2Rad),
            Mathf.Cos(textAngle * Mathf.Deg2Rad));

        textMesh.transform.rotation = Quaternion.LookRotation(rotation);
    }
}
