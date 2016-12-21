/**
* Hitscan.cs
* Created by Michael Marek (2016)
*
* Instantaneous projectile properties for weapons performing hitscan attacks. A hitscan has several
* properties involved - base damage, surface penetration, damage falloff, and maximum distance.
* Damage and range are pretty self-explanatory. The surface penetration of a projectile will
* dictate how many objects it can travel through before fully stopping (this includes hard
* surfaces, environmental props, and other actors). Damage falloff is the percentage of the
* original damage the projectile will do after penetrating a single surface.
*
* Note that the damage falloff effect is accumulative. For example, a projectile with a base damage
* of 100, a penetration factor of 2, and a damage falloff of 0.5 (50%) will result in a damage
* value of only 25 after the projectile hits the final target:
*
* 1st: 100 * 100% = 100
* 2nd: 100 * 50% = 50
* 3rd: 50 * 50% = 25
**/

public class Hitscan
{
    public  float   damage;         //base projectile damage
    public  int     penetration;    //amount of targets the projectile can penetrate
    public  float   falloff;        //how much damage is lost after each target penetration [%]
    public  float   maxDistance;    //the maximum range the projectile will travel before stopping


    /**
    * Class constructor. Set hitscan projectile properties via constructor parameters.
    *
    * @param    ...
    * @return   null
    **/
    public Hitscan(float damage, int penetration, float falloff, float maxDistance)
    {
        this.damage      = damage;
        this.penetration = penetration;
        this.falloff     = falloff;
        this.maxDistance = maxDistance;
    }


    /**
    * Several pre-made hitscan projectile objects for various calibers of firearm ammunition.
    **/
    public static Hitscan _9x19mm   { get { return new Hitscan(15f,  0, 1f,   100f); } }
    public static Hitscan _45ACP    { get { return new Hitscan(25f,  0, 1f,   100f); } }
    public static Hitscan _57x28mm  { get { return new Hitscan(25f,  1, 0.5f, 100f); } }
    public static Hitscan _44Magnum { get { return new Hitscan(100f, 2, 0.5f, 100f); } }
}
