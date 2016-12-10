/**
**/

using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class Roof : MonoBehaviour
{
    public  GameObject[]        roofs;

    [Space(10)]
    public  float               fadeTime;
    public  float               minimumAlpha;

    private bool                inHouse     = false;
    private float               currentFade = 1f;
    private List<GameObject>    occupants   = new List<GameObject>();

    /**
    **/
    void Update()
    {
        if (!inHouse && occupants.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
        else if (inHouse && occupants.Count == 0)
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        inHouse = occupants.Count > 0;
    }

    /**
    **/
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            if (!occupants.Contains(collider.gameObject))
                occupants.Add(collider.gameObject);
    }

    /**
    **/
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
            if (occupants.Contains(collider.gameObject))
                occupants.Remove(collider.gameObject);
    }

    /**
    **/
    IEnumerator FadeIn()
    {
        MeshRenderer[] meshes = new MeshRenderer[roofs.Length];
        for (int i = 0; i < roofs.Length; i++)
        {
            meshes[i] = roofs[i].GetComponent<MeshRenderer>();
            meshes[i].shadowCastingMode = ShadowCastingMode.On;
        }

        while (currentFade < 1f)
        {
            float delta = (1f / fadeTime) * Time.deltaTime;
            currentFade = (currentFade + delta < 1f ? currentFade + delta : 1f);

            for (int i = 0; i < meshes.Length; i++)
            {
                Color color = meshes[i].material.color;
                color.a = currentFade;
                meshes[i].material.color = color;
            }

            yield return new WaitForSeconds(delta);
        }
    }

    /**
    **/
    IEnumerator FadeOut()
    {
        MeshRenderer[] meshes = new MeshRenderer[roofs.Length];
        for (int i = 0; i < roofs.Length; i++)
        {
            meshes[i] = roofs[i].GetComponent<MeshRenderer>();
        }

        while (currentFade > minimumAlpha)
        {
            float delta = (1f / fadeTime) * Time.deltaTime;
            currentFade = (currentFade - delta > minimumAlpha ? currentFade - delta : minimumAlpha);

            for (int i = 0; i < meshes.Length; i++)
            {
                Color color = meshes[i].material.color;
                color.a = currentFade;
                meshes[i].material.color = color;
            }

            yield return new WaitForSeconds(delta);
        }

        for (int i = 0; i < meshes.Length; i++)
            meshes[i].shadowCastingMode = ShadowCastingMode.Off;
    }
}
