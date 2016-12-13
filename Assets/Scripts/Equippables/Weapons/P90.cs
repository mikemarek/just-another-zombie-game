/**
* P90.cs
* Created by Michael Marek (2016)
*
* Inventory item for an FN P90 submachine gun.
**/

using UnityEngine;
using System.Collections;

public class P90 : Weapon
{
    /**
    **/
    void Awake()
    {
        itemType  = ItemType.P90;
        stackSize = 0;
        maxStack  = 0;

        moveSpeedRatio = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_57x28mm),                               //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Hitscan._57x28mm,                               //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                30f,                                            //weapon recoil
                2f,                                             //reload speed
                false,                                          //continuous reloading?
                50,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
