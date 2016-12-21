/**
* Uzi.cs
* Created by Michael Marek (2016)
*
* Inventory item for an Uzi (9x19mm carbine variant) submachine gun.
**/

using UnityEngine;
using System.Collections;

public class Uzi : Weapon
{
    /**
    **/
    public Uzi()
    {
        itemType  = ItemType.Uzi;
        stackSize = 0;
        maxStack  = 0;

        moveSpeedRatio = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Hitscan._9x19mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                7f,                                             //bullet spread
                40f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                30,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
