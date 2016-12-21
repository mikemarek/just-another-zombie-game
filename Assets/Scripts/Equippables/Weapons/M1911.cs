/**
* M1911.cs
* Created by Michael Marek (2016)
*
* Inventory item for a Colt M1911 handgun.
**/

using UnityEngine;
using System.Collections;

public class M1911 : Weapon
{
    /**
    **/
    public M1911()
    {
        itemType        = ItemType.M1911;
        stackSize       = 0;
        maxStack        = 0;
        moveSpeedRatio  = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_45ACP),                                 //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.15f,                                          //rate of fire
                Hitscan._45ACP,                                 //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                3f,                                             //bullet spread
                30f,                                            //weapon recoil
                1f,                                             //reload speed
                false,                                          //continuous reloading?
                17,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
