/**
* FiveSeven.cs
* Created by Michael Marek (2016)
*
* Inventory item for an FN Five-Seven handgun.
**/

using UnityEngine;
using System.Collections;

public class FiveSeven : Weapon
{
    /**
    **/
    void Awake()
    {
        itemType        = ItemType.FiveSeven;
        stackSize       = 0;
        maxStack        = 0;
        moveSpeedRatio  = 0.7f;

        attackModes = new WeaponAttackMode[1] {
            new WeaponAttackMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_57x28mm),                               //ammunition type used
                WeaponAttackMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.2f,                                           //rate of fire
                Hitscan._57x28mm,                               //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                30f,                                            //weapon recoil
                1f,                                             //reload speed
                false,                                          //continuous reloading?
                17,                                             //clip size
                0)                                              //current ammunition
        };
    }
}
