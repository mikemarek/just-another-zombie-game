﻿using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    public enum ItemType {
        P226,
        M1911,
        FiveSeven,
        UP70,
        Glock18c,
        _44Revolver,
        Uzi,
        UMP45,
        Bizon,
        P90,
        DoubleBarrel,
        M500,
        M590,
        SPAS12,
        USAS12,
        AC552,
        AK74U,
        AK47,
        SCARL,
        M4A1GL,
        MG36,
        L42A1,
        SSG3000,
        PSG1,
        M82A1,
        Flamethrower,
        RPG7,
        ChinaLake,
        M60,
        _____,
        Molotov,
        HEGrenade,
        PressureMine,
        C4Explosive,
        _1,
        _2,
        _3,
        _9x19mm,
        _45ACP,
        _57x28mm,
        _44Magnum,
        _12gShell,
        _556x45mmNATO,
        _762x39mm,
        _762x51mmNATO,
        _50BMG,
        _762x51mmBELT,
        RPG,
        _40mmGrenade,
        _4,
        _5,
        _6,
        HealthKit,
        SmallBackpack,
        LargeBackpack,
        Gasoline,
        Defibrilator,
        ZombieHead,
        RadioPart,
        _7,
        _8,
        None,
    };

    [Header("Item Display Names")]
    public  string[]        names;

    [Header("Item Display Icons")]
    public  Sprite[]        icons;

    [Header("Item Pickup Prefabs")]
    public  GameObject[]    prefabs;

    /**
    **/
	void Start()
    {
	}

    /**
    **/
	void Update()
    {
	}
}