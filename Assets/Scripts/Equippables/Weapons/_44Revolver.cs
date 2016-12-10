using UnityEngine;
using System.Collections;

public class _44Revolver : Weapon
{
    void Awake()
    {
        itemType     = ItemManager.ItemType._44Revolver;
        stackSize    = 0;
        maxStackSize = 0;

        reloadMoveSpeedRatio = 0.7f;
        shootMoveSpeedRatio  = 0.6f;

        firingModes = new WeaponFiringMode[1] {
            new WeaponFiringMode(
                new WeaponActiveState(),                        //starting weapon state
                new WeaponSemiAutoState(),                      //attacking weapon state
                typeof(_44Magnum),                              //ammunition type used
                WeaponFiringMode.AttackType.Instant,            //attack type
                false,                                          //pending fire?
                1f,                                             //rate of fire
                Bullet._44Magnum,                               //type of bullet fired
                "Prefabs/Projectiles/Bullets/Bullet Tracer",    //type of projectile fired
                2f,                                             //bullet spread
                150f,                                           //weapon recoil
                0.75f,                                          //reload speed
                true,                                           //continuous reloading?
                6,                                              //clip size
                0                                               //current ammunition
            )
        };
    }

    public override int Reload(uint mode, int totalAmmo)
    {
        int needed = (int)firingModes[mode].clipSize - firingModes[mode].ammunition;

        if (needed <= 0)
            return 0;
        if (totalAmmo <= 0)
            return 0;

        firingModes[mode].ammunition++;
        return 1;
    }
}
