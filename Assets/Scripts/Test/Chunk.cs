/**
**/

using System;
using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour
{
    [Header("Section Spawn Locations")]
    public  Transform[]     sectionLocations;

    [Header("Section Prefabs")]
    public  GameObject[]    sections;


    /**
    **/
    public virtual void InitializeSeed(ref byte[] seed)
    {
        if (seed != null)
            return;

        System.Random rng = new System.Random();
        seed = new byte[51];
        rng.NextBytes(seed);
    }


    /**
    **/
    public virtual void Initialize(byte[] seeds)
    {
        for (int i = 0; i < sectionLocations.Length; i++)
        {
            GameObject house = GameObject.Instantiate(sections[0]) as GameObject;
            house.transform.SetParent(sectionLocations[i], false);
            house.transform.localPosition = Vector3.zero;

            byte[] seed = new byte[3]{
                seeds[2 + 3*i + 0],
                seeds[2 + 3*i + 1],
                seeds[2 + 3*i + 2]
            };

            Section section = house.GetComponent<Section>();
            section.Initialize(seed);
        }
    }


    /**
    **/
    void OnDestroy()
    {
        //
    }
}
