using UnityEngine;
using System.Collections;

public class WeaponM501FiringState : WeaponState
{
    public override WeaponState HandleInput(Weapon weapon, uint mode)
    {
        if (!weapon.attackModes[mode].pendingAttack)
            return new WeaponActiveState();

        //if ((weapon as M501).primed)
        //    weapon.Attack(mode);

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
