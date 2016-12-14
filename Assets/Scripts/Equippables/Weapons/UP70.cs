/**
* UP70.cs
* Created by Michael Marek (2016)
*
* Inventory item for an H&K UP70 handgun.
**/

using UnityEngine;
using System.Collections;

public class UP70 : Weapon
{
    /**
    **/
    public UP70()
    {
        itemType  = ItemType.UP70;
        stackSize = 0;
        maxStack  = 0;

        moveSpeedRatio = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponThreeBurstFireState(),                //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.05f,                                          //rate of fire
                Hitscan._9x19mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                3f,                                             //bullet spread
                20f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                24,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
