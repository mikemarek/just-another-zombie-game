/**
* _44Revolver.cs
* Created by Michael Marek (2016)
*
* Inventory item for a generic .44 revolver. Single-shot reload.
**/

using UnityEngine;
using System.Collections;

public class _44Revolver : Weapon
{
    /**
    **/
    void Awake()
    {
        itemType        = ItemType._44Revolver;
        stackSize       = 0;
        maxStack        = 0;
        moveSpeedRatio  = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_44Magnum),                              //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                1f,                                             //rate of fire
                Hitscan._44Magnum,                              //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                150f,                                           //weapon recoil
                0.75f,                                          //reload speed
                true,                                           //continuous reloading?
                6,                                              //clip size
                0)                                              //current ammunition
        };
    }

    /**
    **/
    public override int Reload(uint mode, int availableAmmo)
    {
        int neededAmmo = NeededAmmunition(mode);

        if (neededAmmo <= 0)    return 0;
        if (availableAmmo <= 0) return neededAmmo;

        attackModes[mode].ammunition++;

        return 1;
    }
}
