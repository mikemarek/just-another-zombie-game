/**
* P226.cs
* Created by Michael Marek (2016)
*
* Inventory item for a Sig Sauer P226 handgun.
**/

using UnityEngine;
using System.Collections;

public class P226 : Weapon
{
    /**
    **/
    void Awake()
    {
        itemType  = ItemType.P226;
        stackSize = 0;
        maxStack  = 0;

        moveSpeedRatio = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Hitscan._9x19mm,                                //type of bullet fired
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
