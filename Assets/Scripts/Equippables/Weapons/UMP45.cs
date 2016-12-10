using UnityEngine;
using System.Collections;

public class UMP45 : Weapon
{
    void Awake()
    {
        itemType     = ItemManager.ItemType.UMP45;
        stackSize    = 0;
        maxStackSize = 0;

        reloadMoveSpeedRatio = 0.7f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[1] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_45ACP),                                 //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Bullet._45ACP,                                  //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                3f,                                             //bullet spread
                30f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                30,                                             //clip size
                0                                               //current ammunition
            )
        };
    }
}
