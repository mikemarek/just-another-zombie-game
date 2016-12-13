using UnityEngine;
using System.Collections;

public class WeaponFullAutoState : WeaponState
{
    private float cycleTime = 1f;

    public override WeaponState HandleInput(Weapon weapon, uint mode)
    {
        if (!weapon.attackModes[mode].pendingAttack && cycleTime >= 1f)
            return new WeaponActiveState();

        if (cycleTime >= 1f)
        {
            cycleTime = 0f;
            weapon.Attack(mode);
        }

        return null;
    }

    public override void Update(Weapon weapon, uint mode)
    {
        cycleTime += (1f / weapon.attackModes[mode].attackRate) * Time.deltaTime;
    }

    public override void Initialize(Weapon weapon, uint mode)
    {
    }

    public override void OnEnter(Weapon weapon, uint mode)
    {
        cycleTime = 0f;
        weapon.Attack(mode);
    }

    public override void OnExit(Weapon weapon, uint mode)
    {
        weapon.attackModes[mode].pendingAttack = false;
    }
}
