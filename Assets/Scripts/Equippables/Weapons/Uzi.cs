using UnityEngine;
using System.Collections;

public class Uzi : Weapon
{
    void Awake()
    {
        itemType     = ItemManager.ItemType.Uzi;
        stackSize    = 0;
        maxStackSize = 0;

        reloadMoveSpeedRatio = 0.7f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[1] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponFullAutoState(),                      //attacking weapon state
                typeof(_9x19mm),                                //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.1f,                                           //rate of fire
                Bullet._9x19mm,                                 //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                7f,                                             //bullet spread
                40f,                                            //weapon recoil
                1.5f,                                           //reload speed
                false,                                          //continuous reloading?
                30,                                             //clip size
                0                                               //current ammunition
            )
        };
    }
}
