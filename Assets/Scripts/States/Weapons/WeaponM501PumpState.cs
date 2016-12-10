using UnityEngine;
using System.Collections;

public class WeaponM501PumpState : WeaponState
{
    private float cycleTime;

    public override WeaponState HandleInput(Weapon weapon, uint mode)
    {
        if (!weapon.firingModes[mode].pendingFire && cycleTime >= 1f)
            return new WeaponActiveState();
        return null;
    }

    public override void Update(Weapon weapon, uint mode)
    {
        cycleTime += (1f / weapon.firingModes[mode].rateOfFire) * Time.deltaTime;
    }

    public override void Initialize(Weapon weapon, uint mode)
    {
    }

    public override void OnEnter(Weapon weapon, uint mode)
    {
        cycleTime = 0f;
    }

    public override void OnExit(Weapon weapon, uint mode)
    {
        //if (!(weapon as M501).primed)
        //    weapon.Attack(mode);
    }
}
