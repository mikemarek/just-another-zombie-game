/**
* Weapon.cs
* Created by Michael Marek (2016)
*
* A weapon is an inventory item that can be used by the player, normally used to fire some sort of
* projectile. However, the Weapon class does not have to pertain simply to firearms. There are
* numerous methods available to override in order to provide an almost endless amount of
* functionality for a weapon.
*
* Weapons have two main types of projectiles - hitscan and dynamic. Hitscan projectiles are
* raytraced through the scene to determine their collision point and are therefore instantaneous.
* Dynamic projectiles are your classic 'physics-based' projectile that travel through the game
* world at a finite speed until collision.
*
* A weapon supports an almost unlimited amount of "attack modes" (well, 4294967295 different modes
* to be precise) which can be enabled or disabled by setting the 'mode[s]' parameter for the
* usage methods StartUse(), StopUse(), and MultiUse(). These different modes can easily be detected
* by using a simple switch statement when overrided by a subclass.
*
* A weapon's functionality is modeled after a non-deterministic finite state automaton. In layman's
* terms, what this means is that each weapon attack mode starts off in an initial 'active' state.
* When the weapon is fired, the state machine transitions from this 'active' state into an 'attack'
* state. This 'attack' state is what actually fires the weapon. Reloading works the same way except
* we transition to a 'reload' state instead of an attacking one. The nice thing about this design
* is that the 'active' state is reusable for every single weapon, and all we need to do is swap out
* an 'attack' state for one that provides the functionality we desire for a weapon.
**/

using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    [HideInInspector]   public  WeaponAttackMode[]  attackModes;        //various attack modes for weapon
    [HideInInspector]   public  float               moveSpeedRatio;     //actor move speed % when shooting+reloading

    private Transform               container;                          //spawned projectile container
    private PlayerCameraComponent   cam;


    /**
    * Get a reference to the global projectiles container on weapon initialize that we will spawn
    * projectiles in when the actor attacks.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        if (container != null)
            return;

        SceneManager scene = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        container = scene.projectileContainer;
    }


    /**
    * Update each attack mode's current state and transition to a new weapon state if needed.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        for (uint i = 0; i < attackModes.Length; i++)
        {
            WeaponState state = attackModes[i].currentState.HandleInput(this, i);

            if (state != null)
                GotoState(i, state);

            attackModes[i].currentState.Update(this, i);
        }

        UpdateWeapon();
    }


    /**
    * Transition an attack mode to a new weapon state.
    *
    * @param    uint            the attack mode that is transitioning
    * @param    WeaponState     the new state to transition into
    * @return   null
    **/
    public void GotoState(uint mode, WeaponState state)
    {
        attackModes[mode].currentState.OnExit(this, mode);
        attackModes[mode].currentState = state;
        attackModes[mode].currentState.Initialize(this, mode);
        attackModes[mode].currentState.OnEnter(this, mode);
    }


    /**
    * We can perform any weapon-specific updates here.
    *
    * @param    null
    * @return   null
    **/
    public virtual void UpdateWeapon()
    {
    }


    /**
    * Start using the weapon. Flag the specified attack mode as "ready to fire".
    *
    * @param    uint    firing mode to activate
    * @return   null
    **/
    public override void StartUse(uint mode)
    {
        if (mode >= attackModes.Length)
            return;

        attackModes[mode].pendingAttack = true;
    }


    /**
    * Start using the weapon. Flag the specified attack mode as "stop firing".
    *
    * @param    uint    firing we to deactivate
    * @return   null
    **/
    public override void StopUse(uint mode)
    {
        if (mode >= attackModes.Length)
            return;

        attackModes[mode].pendingAttack = false;
    }


    /**
    * Use multiple attack modes on the weapon at the same time.
    *
    * @param    uint[]  array of firing modes to activate
    * @return   null
    **/
    public override void MultiUse(uint[] modes)
    {
        foreach (uint mode in modes)
            StartUse(mode);
    }


    /**
    * Get a reference to the actor's camera component upon weapon pickup (so we may apply camera
    * effects when we attack with the weapon, if necessary).
    *
    * @param    null
    * @return   null
    **/
    public override void OnPickup()
    {
        cam = gameObject.GetComponent<PlayerCameraComponent>();
    }


    /**
    * A generic attack method that determines the type of attack for the attack mode.
    *
    * @param    uint    the attack mode that is being utilized
    * @return   null
    **/
    public void Attack(uint mode)
    {
        if (attackModes[mode].ammunition == 0)
            return;

        if (attackModes[mode].currentState is WeaponReloadState)
            return;

        switch (attackModes[mode].attackType)
        {
            case WeaponAttackMode.AttackType.Instant:
                InstantAttack(mode);
            break;

            case WeaponAttackMode.AttackType.Projectile:
                ProjectileAttack(mode);
            break;

            case WeaponAttackMode.AttackType.Custom:
                CustomAttack(mode);
            break;

            case WeaponAttackMode.AttackType.None:
            default:
            return;
        }

        ConsumeAmmunition(mode);
    }


    /**
    * Perform a hitscan (instant) to determine the recipient of an attack.
    *
    * @param    mode    attack mode that is launching a projectile
    * @return   null
    **/
    protected void InstantAttack(uint mode)
    {
        //no properties to set for our hitscan; exit
        if (attackModes[mode].bullet == null)
            return;

        //reference to the attack mode
        WeaponAttackMode attackMode = attackModes[mode];

        //total distance travelled by projectile
        float distanceTravelled = 0f;

        //inaccuracy of the projectile
        float shotOffset = SpreadOffset(mode);

        //only collide with objects on specific collision layers
        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Player"));
        mask = mask & ~(LayerMask.NameToLayer("Zombie"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        //location of the muzzle of the weapon
        Vector3 muzzlePoint = transform.position - 0.5f * Vector3.forward;

        //direction we are attacking in
        Vector3 direction = new Vector3(
            Mathf.Cos((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            Mathf.Sin((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            0f);

        //hitscan collision information
        RaycastHit info = new RaycastHit();

        for (int i = 0; i <= attackMode.bullet.penetration; i++)
        {
            bool hit;

            //first hitscan - project from muzzle into game world
            if (i == 0)
                hit = Physics.Raycast(
                    muzzlePoint,
                    direction,
                    out info,
                    attackMode.bullet.maxDistance,
                    mask);
            //subsequent hitscans - project from last collision point into game world
            else
                hit = Physics.Raycast(
                    info.point + direction * 0.0001f, //place origin inside collider to ignore
                    direction,
                    out info,
                    attackMode.bullet.maxDistance - distanceTravelled,
                    mask);

            //we haven't hit anything - calculate final position of the projectile
            if (!hit)
                if (i == 0)
                    info.point = muzzlePoint + direction * attackMode.bullet.maxDistance;
                else
                    info.point += direction * (attackMode.bullet.maxDistance - distanceTravelled);

            //add an aesthetic marker for where the hitscan projectile impacted
            if (info.collider != null)
            {
                if (info.collider.tag == "Environment" || info.collider.tag == "Prop")
                {
                    GameObject hitpoint = Instantiate(
                        Resources.Load("Prefabs/Projectiles/Impacts/Hit Registration Point"),
                        info.point,
                        Quaternion.identity
                    ) as GameObject;
                    hitpoint.transform.SetParent(container);
                }
            }

            //yup, haven't hit anything - no point in checking damage calculations
            if (!hit)
                break;

            distanceTravelled += info.distance;

            //resultant damage based on number of surface penetrations and damage falloff
            float damage = attackMode.bullet.damage * Mathf.Pow(attackMode.bullet.falloff, i);

            //if the target has a health component, deal damage
            HealthComponent health = info.transform.gameObject.GetComponent<HealthComponent>();
            if (health != null)
                health.Damage(damage, info.point);
        }

        //add an aesthetic projectile to represent the hitscan
        if (attackMode.projectile != "")
        {
            GameObject bullet = Instantiate(
                Resources.Load(attackMode.projectile),
                muzzlePoint,
                Quaternion.Euler(0f, 0f, transform.eulerAngles.z + shotOffset)
            ) as GameObject;
            bullet.transform.SetParent(container);

            BulletTracer tracer = bullet.GetComponent<BulletTracer>();
            tracer.SetTarget(info.point);
        }

        //induce a recoil effect in the player camera
        InduceRecoil(attackMode.recoil * -direction);
    }


    /**
    * Spawn a dynamic projectile.
    *
    * @param    uint    attack mode that is launching a projectile
    * @return   null
    **/
    protected void ProjectileAttack(uint mode)
    {
        if (attackModes[mode].projectile == null)
            return;

        //inaccuracy of the projectile
        float shotOffset = SpreadOffset(mode);

        //location of the muzzle of the weapon
        Vector3 muzzlePoint = transform.position - 0.5f * Vector3.forward;

        if (attackModes[mode].projectile != "")
        {
            GameObject go = Instantiate(
                Resources.Load(attackModes[mode].projectile),
                muzzlePoint,
                Quaternion.Euler(0f, 0f, transform.eulerAngles.z + shotOffset)
            ) as GameObject;

            Projectile projectile = go.GetComponent<Projectile>();
            projectile.SetOwner(gameObject);
        }
    }


    /**
    * If a weapon requires a type of attack that is not classied as a hitscan or dynamic
    * projectile, or the weapon requires special setup before attacking, this method can be
    * overwritten in order to provide a manner in which to do it.
    *
    * @param    null
    * @return   null
    **/
    protected virtual void CustomAttack(uint mode)
    {
    }


    /**
    * After attacking, an attacking mode will potentially need to consume ammunition.
    *
    * @param    uint    the attack mode that has attacked and needs to use up ammunition
    * @return   null
    **/
    protected virtual void ConsumeAmmunition(uint mode)
    {
        if (attackModes[mode].clipSize == 0)
            return;

        if (attackModes[mode].ammunition > 0)
            attackModes[mode].ammunition--;
    }


    /**
    * Create a printable string representing the current ammunition for each attack mode.
    *
    * @param    null
    * @return   string  formatted string of each attack mode's ammunition count
    **/
    public virtual string CurrentAmmunition()
    {
        string ammo = AmmoCount(0);

        for (uint i = 1; i < attackModes.Length; i++)
                ammo += "/" + AmmoCount(i);

        return ammo;
    }


    /**
    * Return a string of the ammunition count of an attack mode. The attack mode is assumed to have
    * infinit ammunition if the attack mode has no clip size or the count is below a valid amount.
    *
    * @param    uint    the attack mode to check ammunition count for
    * @return   string  string of the attack mode ammunition count
    **/
    protected string AmmoCount(uint mode)
    {
        if (attackModes[mode].clipSize <= 0 || attackModes[mode].ammunition < 0)
            return "∞";
        return attackModes[mode].ammunition.ToString();
    }


    /**
    * Calculate how much ammunition is needed to fully reload a weapon attack mode.
    *
    * @param    uint    the attack mode we wish to check for required ammunition
    * @return   int     how much ammunition is needed to fully reload
    **/
    protected int NeededAmmunition(uint mode)
    {
        return (int)attackModes[mode].clipSize - attackModes[mode].ammunition;
    }


    /**
    * Reload a weapon attack mode based on an available amount of ammunition.
    *
    * @param    uint    the attack mode we wish to reload
    * @param    int     how much ammunition is available to reload with
    * @return   int     how much ammunition was consumed attempting to reload
    **/
    public virtual int Reload(uint mode, int availableAmmo)
    {
        int neededAmmo = NeededAmmunition(mode);

        if (neededAmmo <= 0)    return 0;
        if (availableAmmo <= 0) return neededAmmo;

        //we have more ammo available than we need
        if (neededAmmo <= availableAmmo)
        {
            attackModes[mode].ammunition += neededAmmo;
            return neededAmmo;
        }
        //we need more ammo than what is available
        else
        {
            attackModes[mode].ammunition += availableAmmo;
            return availableAmmo;
        }
    }


    /**
    * Induce a recoil effect in the actor's camera (if present).
    *
    * @param    null
    * @return   null
    **/
    private void InduceRecoil(Vector3 recoil)
    {
        if (cam != null)
            cam.Recoil(recoil);
    }


    /**
    * Calculate the inherent amount of spread the weapon projectile will have.
    *
    * @param    null
    * @return   null
    **/
    private float SpreadOffset(uint mode)
    {
        return -0.5f * attackModes[mode].spread + Random.Range(0f, attackModes[mode].spread);
    }
}
