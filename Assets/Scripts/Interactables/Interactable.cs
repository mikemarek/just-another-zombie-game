/**
**/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    public  float               interactionTime     = 0.5f;
    [Space(10)]
    public  TextMesh            textMesh            = null;
    public  float               textFadeTime        = 0.25f;
    public  float               textAngle           = 15f;
    [Space(10)]
    [TextArea]
    public  string              promptText          = "<empty>";

    private bool                inUse               = false;

    private float               textAlpha           = 0f;
    private List<GameObject>    withinReach         = new List<GameObject>();
    private Vector3             rotation            = Vector3.zero;

    private MeshRenderer        render;

    /**
    **/
    void Awake()
    {
        render = textMesh.gameObject.GetComponent<MeshRenderer>();

        textMesh.text = promptText;

        rotation = new Vector3(0f, Mathf.Sin(textAngle * Mathf.Deg2Rad), Mathf.Cos(textAngle * Mathf.Deg2Rad));
        textMesh.transform.rotation = Quaternion.LookRotation(rotation);

        Color color = render.material.color;
        color.a = textAlpha;
        render.material.color = color;
    }

    /**
    **/
    void Update()
    {
        rotation = new Vector3(0f, Mathf.Sin(textAngle * Mathf.Deg2Rad), Mathf.Cos(textAngle * Mathf.Deg2Rad));
        textMesh.transform.rotation = Quaternion.LookRotation(rotation);
    }

    /**
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
    **/
    public virtual void Interact(GameObject go)
    {
    }

    /**
    **/
    public virtual void Leave(GameObject go)
    {
    }

    /**
    **/
    public bool Usable()
    {
        return !inUse;
    }

    /**
    **/
    public void StartInteracting()
    {
        inUse = true;
    }

    /**
    **/
    public void StopInteracting()
    {
        inUse = false;
    }

    /**
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
}
