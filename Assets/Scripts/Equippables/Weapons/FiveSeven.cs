using UnityEngine;
using System.Collections;

public class FiveSeven : Weapon
{
    void Awake()
    {
        itemType     = ItemManager.ItemType.FiveSeven;
        stackSize    = 0;
        maxStackSize = 0;

        reloadMoveSpeedRatio = 0.7f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[1] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_57x28mm),                               //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                0.2f,                                           //rate of fire
                Bullet._57x28mm,                                //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                30f,                                            //weapon recoil
                1f,                                             //reload speed
                false,                                          //continuous reloading?
                17,                                             //clip size
                0                                               //current ammunition
            )
        };
    }
}
