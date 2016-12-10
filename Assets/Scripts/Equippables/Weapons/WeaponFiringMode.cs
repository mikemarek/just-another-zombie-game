/**
**/

using UnityEngine;
using System.Collections;

public class WeaponFiringMode
{
    public enum AttackType {
        None,
        Instant,
        Projectile,
        Custom
    };

    public  WeaponState     currentState;
    public  WeaponState     attackingState;

    public  System.Type     ammunitionType;
    public  AttackType      attackType;

    public  bool            pendingFire;
    public  float           rateOfFire;

    public  Bullet          bullet;
    public  string          projectile;

    public  float           spread;
    public  float           recoil;

    public  float           reloadSpeed;
    public  bool            continuousReload;

    public  uint            clipSize;
    public  int             ammunition;

    /**
    **/
    public WeaponFiringMode(
        WeaponState currentState,
        WeaponState attackingState,
        System.Type ammunitionType,
        AttackType  attackType,
        bool        pendingFire,
        float       rateOfFire,
        Bullet      bullet,
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
        this.pendingFire        = pendingFire;
        this.rateOfFire         = rateOfFire;
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
