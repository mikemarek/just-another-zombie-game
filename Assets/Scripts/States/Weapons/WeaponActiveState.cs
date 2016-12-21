using UnityEngine;
using System.Collections;

public class WeaponActiveState : WeaponState
{
    public override WeaponState HandleInput(Weapon weapon, uint mode)
    {
        if (weapon.attackModes[mode].pendingAttack)
            return weapon.attackModes[mode].attackingState;
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
    }

    public override void OnExit(Weapon weapon, uint mode)
    {
    }
}
