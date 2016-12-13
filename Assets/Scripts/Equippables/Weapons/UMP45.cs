/**
* UMP45.cs
* Created by Michael Marek (2016)
*
* Inventory item for an H&K UMP45 submachine gun.
**/

using UnityEngine;
using System.Collections;

public class UMP45 : Weapon
{
    /**
    **/
    void Awake()
    {
        itemType  = ItemType.UMP45;
        stackSize = 0;
        maxStack  = 0;

        moveSpeedRatio = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_45ACP),                                 //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Hitscan._45ACP,                                 //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                3f,                                             //bullet spread
                30f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                30,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
