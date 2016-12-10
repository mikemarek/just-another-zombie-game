using UnityEngine;
using System.Collections;

public class P90 : Weapon
{
    void Awake()
    {
        itemType     = ItemManager.ItemType.P90;
        stackSize    = 0;
        maxStackSize = 0;

        reloadMoveSpeedRatio = 0.7f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[1] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_57x28mm),                               //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Bullet._57x28mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                30f,                                            //weapon recoil
                2f,                                             //reload speed
                false,                                          //continuous reloading?
                50,                                             //clip size
                0                                               //current ammunition
            )
        };
    }
}
