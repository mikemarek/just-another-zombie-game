/**
* Glock18c.cs
* Created by Michael Marek (2016)
*
* Inventory item for a pair of Glock 18c handguns.
**/

using UnityEngine;
using System.Collections;

public class Glock18c : Weapon
{
    /**
    **/
    public Glock18c()
    {
        itemType        = ItemType.Glock18c;
        stackSize       = 0;
        maxStack        = 0;
        multiUse        = true;
        moveSpeedRatio  = 0.6f;

        attackModes = new WeaponAttackMode[2] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.15f,                                          //rate of fire
                Hitscan._9x19mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                5f,                                             //bullet spread
                30f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                18,                                             //clip size
                0),                                             //current ammunition

            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.15f,                                          //rate of fire
                Hitscan._9x19mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                5f,                                             //bullet spread
                30f,                                            //weapon recoil
                1f,                                             //reload speed
                false,                                          //continuous reloading?
                18,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
