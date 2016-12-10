/**
**/

using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    [HideInInspector]   public  WeaponFiringMode[]  firingModes;
    [HideInInspector]   public  float               reloadMoveSpeedRatio;
    [HideInInspector]   public  float               shootMoveSpeedRatio;

    private Transform               container;
    private PlayerCameraComponent   cam;

    //-------------------------------------------------------------------------

    /**
    **/
    public override void StartUse(uint mode)
    {
        if (mode >= firingModes.Length)
            return;

        firingModes[mode].pendingFire = true;
    }

    /**
    **/
    public override void StopUse(uint mode)
    {
        if (mode >= firingModes.Length)
            return;

        firingModes[mode].pendingFire = false;
    }

    /**
    **/
    public override void MultiUse(uint[] modes)
    {
        foreach (uint mode in modes)
            StartUse(mode);
    }

    //-------------------------------------------------------------------------

    /**
    **/
    public override void OnPickup()
    {
        cam = gameObject.GetComponent<PlayerCameraComponent>();
    }

    //-------------------------------------------------------------------------

    /**
    **/
    void Start()
    {
        container = GameObject.Find("Game Manager").GetComponent<GameManager>().projectileContainer;
    }

    /**
    **/
    void Update()
    {
        for (uint i = 0; i < firingModes.Length; i++)
        {
            //handle weapon state activity
            WeaponState state = firingModes[i].currentState.HandleInput(this, i);
            if (state != null)
                GotoState(i, state);
            firingModes[i].currentState.Update(this, i);
        }

        UpdateWeapon();
    }

    /**
    **/
    public void GotoState(uint mode, WeaponState state)
    {
        firingModes[mode].currentState.OnExit(this, mode);
        firingModes[mode].currentState = state;
        firingModes[mode].currentState.Initialize(this, mode);
        firingModes[mode].currentState.OnEnter(this, mode);
    }

    /**
    **/
    public virtual void UpdateWeapon()
    {
    }

    //-------------------------------------------------------------------------

    /**
    **/
    public void Attack(uint mode)
    {
        if (firingModes[mode].ammunition == 0)
            return;
        if (firingModes[mode].currentState is WeaponReloadState)
            return;

        switch (firingModes[mode].attackType)
        {
            case WeaponFiringMode.AttackType.Instant:
                InstantAttack(mode);
            break;

            case WeaponFiringMode.AttackType.Projectile:
                ProjectileAttack(mode);
            break;

            case WeaponFiringMode.AttackType.Custom:
                CustomAttack(mode);
            break;

            case WeaponFiringMode.AttackType.None:
            default:
            return;
        }

        ConsumeAmmunition(mode);
    }

    /**
    **/
    protected void InstantAttack(uint mode)
    {
        if (firingModes[mode].bullet == null)
            return;

        int mask = Physics.AllLayers;
        mask = mask & ~(LayerMask.NameToLayer("Environment"));
        mask = mask & ~(LayerMask.NameToLayer("Player"));
        mask = mask & ~(LayerMask.NameToLayer("Zombie"));
        mask = mask & ~(LayerMask.NameToLayer("Prop"));

        float distanceTravelled = 0f;
        float shotOffset = SpreadOffset(mode);

        Vector3 muzzlePoint = transform.position - 0.5f * Vector3.forward;

        Vector3 direction = new Vector3(
            Mathf.Cos((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            Mathf.Sin((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            0f);

        RaycastHit info = new RaycastHit();

        for (int i = 0; i <= firingModes[mode].bullet.penetration; i++)
        {
            bool hit;
            if (i == 0)
                hit = Physics.Raycast(
                    muzzlePoint,
                    direction,
                    out info,
                    firingModes[mode].bullet.maxDistance,
                    mask);
            else
                hit = Physics.Raycast(
                    info.point + direction * 0.0001f, //place raycast origin inside collider to ignore
                    direction,
                    out info,
                    firingModes[mode].bullet.maxDistance - distanceTravelled,
                    mask);

            float damage = firingModes[mode].bullet.damage * Mathf.Pow(firingModes[mode].bullet.falloff, i);

            if (!hit)
            {
                if (i == 0)
                    info.point = muzzlePoint + direction * firingModes[mode].bullet.maxDistance;
                else
                    info.point += direction * (firingModes[mode].bullet.maxDistance - distanceTravelled);
            }

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

            distanceTravelled += info.distance;

            if (!hit)
                break;

            HealthComponent health = info.transform.gameObject.GetComponent<HealthComponent>();
            if (health != null)
                health.Damage(damage, info.point);
        }

        if (firingModes[mode].projectile != "")
        {
            GameObject bullet = Instantiate(
                Resources.Load(firingModes[mode].projectile),
                muzzlePoint,
                Quaternion.Euler(0f, 0f, transform.eulerAngles.z + shotOffset)
            ) as GameObject;
            bullet.transform.SetParent(container);

            BulletTracer tracer = bullet.GetComponent<BulletTracer>();
            tracer.SetTarget(info.point);
        }

        InduceRecoil(firingModes[mode].recoil * -direction);
    }

    /**
    **/
    protected void ProjectileAttack(uint mode)
    {
        if (firingModes[mode].projectile == null)
            return;

        float shotOffset = SpreadOffset(mode);

        Vector3 muzzlePoint = transform.position - 0.5f * Vector3.forward;

        Vector3 direction = new Vector3(
            Mathf.Cos((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            Mathf.Sin((gameObject.transform.eulerAngles.z + shotOffset) * Mathf.Deg2Rad),
            0f);

        if (firingModes[mode].projectile != "")
        {
            GameObject go = Instantiate(
                Resources.Load(firingModes[mode].projectile),
                muzzlePoint,
                Quaternion.Euler(0f, 0f, transform.eulerAngles.z + shotOffset)
            ) as GameObject;

            Projectile projectile = go.GetComponent<Projectile>();

            projectile.SetOwner(gameObject);
        }
    }

    /**
    **/
    protected virtual void CustomAttack(uint mode)
    {
    }

    /**
    **/
    protected virtual void ConsumeAmmunition(uint mode)
    {
        if (firingModes[mode].clipSize == 0)
            return;

        if (firingModes[mode].ammunition > 0)
            firingModes[mode].ammunition--;
    }

    /**
    **/
    private void InduceRecoil(Vector3 recoil)
    {
        cam.Recoil(recoil);
    }

    /**
    **/
    private float SpreadOffset(uint mode)
    {
        return -0.5f * firingModes[mode].spread + Random.Range(0f, firingModes[mode].spread);
    }

    //-------------------------------------------------------------------------

    /**
    **/
    public virtual string CurrentAmmunition()
    {
        string ammo = firingModes[0].ammunition.ToString();
        for (int i = 1; i < firingModes.Length; i++)
            if (firingModes[i].clipSize > 0 && firingModes[i].ammunition > -1)
                ammo += "/" + firingModes[i].ammunition;
            else
                continue;
        return ammo;
    }

    /**
    **/
    public virtual int Reload(uint mode, int totalAmmo)
    {
        int needed = (int)firingModes[mode].clipSize - firingModes[mode].ammunition;

        if (needed <= 0)
            return 0;
        if (totalAmmo <= 0)
            return 0;

        if (needed <= totalAmmo)
        {
            if (firingModes[mode].ammunition > 0) //one in the chamber
                firingModes[mode].ammunition++;
            firingModes[mode].ammunition += needed;
            return needed;
        }
        else
        {
            if (firingModes[mode].ammunition > 0) //one in the chamber
                firingModes[mode].ammunition++;
            firingModes[mode].ammunition += totalAmmo;
            return totalAmmo;
        }
    }
}
