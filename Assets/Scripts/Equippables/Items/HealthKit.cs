﻿/**
**/

using UnityEngine;
using System.Collections;

public class HealthKit : Item
{
    [Header("Health Pack Healing Amount")]
    public  float units = 100f;

    private ActorControllerComponent controller;

    public override void StartUse(uint mode)
    {
        if (mode == 0)
            controller.GotoState(new PlayerHealingState());
    }

    public override void StopUse(uint mode)
    {
    }

    //-------------------------------------------------------------------------

    public override void OnEquip()
    {
    }

    public override void OnUnequip()
    {
    }

    public override void OnPickup()
    {
        controller = gameObject.GetComponent<ActorControllerComponent>();
    }

    public override void OnDrop()
    {
        controller = null;
    }
}