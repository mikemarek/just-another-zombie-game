using UnityEngine;
using System.Collections;

public class Glock18c : Weapon
{
    void Awake()
    {
        itemType      = ItemManager.ItemType.Glock18c;
        stackSize     = 0;
        maxStackSize  = 0;
        allowMultiUse = true;

        reloadMoveSpeedRatio = 0.6f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[2] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.15f,                                          //rate of fire
                Bullet._9x19mm,                                 //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                5f,                                             //bullet spread
                30f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                18,                                             //clip size
                0                                               //current ammunition
            ),

            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.15f,                                          //rate of fire
                Bullet._9x19mm,                                 //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                5f,                                             //bullet spread
                30f,                                            //weapon recoil
                1f,                                             //reload speed
                false,                                          //continuous reloading?
                18,                                             //clip size
                0                                               //current ammunition
            )
        };
    }
}
