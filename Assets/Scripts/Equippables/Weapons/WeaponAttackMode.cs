/**
* WeaponAttackMode.cs
* Created by Michael Marek (2016)
*
* A weapon holds one or several attacking modes. These attack modes determine the characteristics
* of the attack, such as what type of attack is performed, what sort of projectiles are created
* during the attack, how many attacks are available to the actor, etc.
**/

using System;
using UnityEngine;
using System.Collections;

public class WeaponAttackMode
{
    public enum AttackType {
        None,
        Instant,
        Projectile,
        Custom
    };

    public  WeaponState currentState;       //the current weapon state of this attack mode
    public  WeaponState attackingState;     //which state to transition into when attacking

    public  Type        ammunitionType;     //the type of ammunition consumed when reloading
    public  AttackType  attackType;         //the type of attack that's performed when attacking

    public  bool        pendingAttack;      //are we ready to attack?
    public  float       attackRate;         //how quickly we can attack [sec]

    public  Hitscan     bullet;             //if hitscan projectile attack, hitscan properties used
    public  string      projectile;         /*if dynamic projectile attack, the projectile spawned
                                              if hitscan projectile attack, aethetic projectile*/

    public  float       spread;             //inherent spread of the hitscan or dynamic projectiles
    public  float       recoil;             //magnitude of recoil induced to camera when attacking

    public  float       reloadSpeed;        //how fast we are able to reload the attack mode
    public  bool        continuousReload;   //if reloading singles, do we never stop reloading?

    public  uint        clipSize;           //the maximum amount of ammunition allowed available
    public  int         ammunition;         //the current amount of ammunition available to use


    /**
    * Class constructor. Set attack mode properties via constructor parameters.
    *
    * @param    ...
    * @return   null
    **/
    public WeaponAttackMode(
        WeaponState currentState,
        WeaponState attackingState,
        System.Type ammunitionType,
        AttackType  attackType,
        bool        pendingAttack,
        float       attackRate,
        Hitscan      bullet,
        string      projectile,
        float       spread,
        float       recoil,
        float       reloadSpeed,
        bool        continuousReload,
        uint        clipSize,
        int         ammunition)
    {
        this.currentState       = currentState;
        this.attackingState     = attackingState;
        this.ammunitionType     = ammunitionType;
        this.attackType         = attackType;
        this.pendingAttack      = pendingAttack;
        this.attackRate         = attackRate;
        this.bullet             = bullet;
        this.projectile         = projectile;
        this.spread             = spread;
        this.recoil             = recoil;
        this.reloadSpeed        = reloadSpeed;
        this.continuousReload   = continuousReload;
        this.clipSize           = clipSize;
        this.ammunition         = ammunition;
    }
}
