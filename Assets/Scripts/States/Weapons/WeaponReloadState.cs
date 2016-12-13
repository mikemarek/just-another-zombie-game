using UnityEngine;
using System.Collections;

public class WeaponReloadState : WeaponState
{
    public override WeaponState HandleInput(Weapon weapon, uint mode)
    {
        return null;
    }

    public override void Update(Weapon weapon, uint mode)
    {
    }

    public override void Initialize(Weapon weapon, uint mode)
    {
    }

    public override void OnEnter(Weapon weapon, uint mode)
    {
        weapon.attackModes[mode].pendingAttack = false;
    }

    public override void OnExit(Weapon weapon, uint mode)
    {
    }
}
