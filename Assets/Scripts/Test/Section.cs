/**
**/

using System;
using UnityEngine;
using System.Collections;

public class Section : MonoBehaviour
{
    [Header("Room Locations")]
    public  Transform[]     roomLocations;

    [Header("Item Spawn Locations")]
    public  Transform[]     itemSpawns;

    [Header("Room Prefabs")]
    public  GameObject[]    rooms;


    /**
    **/
    void Start()
    {
        /*System.Random rng = new System.Random();
        byte[] seeds = new byte[3];
        rng.NextBytes(seeds);
        InitializeSection(seeds);*/
    }


    /**
    **/
    void OnDestroy()
    {
        //
    }


    /**
    **/
    public void Initialize(byte[] seeds)
    {
        //Debug.Log( string.Format("{0}, {1}, {2}", seeds[0], seeds[1], seeds[2]) );

        for (int i = 0; i < roomLocations.Length; i++)
        {
            int index = Max(seeds);
            seeds[index] = 0;

            GameObject room = GameObject.Instantiate(rooms[index]) as GameObject;
            room.transform.SetParent(roomLocations[i], false);
            room.transform.localPosition = Vector3.zero;
        }
    }


    /**
    **/
    private int Max(byte[] bytes)
    {
        int index = -1;
        byte largest = 0;

        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] > largest)
            {
                index = i;
                largest = bytes[i];
            }
        }

        return index;
    }
}
