/**
**/

public class Bullet
{
    public  float   damage;
    public  int     penetration;
    public  float   falloff;
    public  float   maxDistance;

    public Bullet(float damage, int penetration, float falloff, float maxDistance)
    {
        this.damage      = damage;
        this.penetration = penetration;
        this.falloff     = falloff;
        this.maxDistance = maxDistance;
    }

    public static Bullet _9x19mm    { get { return new Bullet(15f,  0, 1f,   100f); } }
    public static Bullet _45ACP     { get { return new Bullet(25f,  0, 1f,   100f); } }
    public static Bullet _57x28mm   { get { return new Bullet(25f,  1, 0.5f, 100f); } }
    public static Bullet _44Magnum  { get { return new Bullet(100f, 2, 0.5f, 100f); } }
}
