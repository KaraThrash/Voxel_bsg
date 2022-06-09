using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : ShipSystem
{
    public GameObject bullet;
    public GunType gunType;

    public Transform gunParent; //if the gun has multiple muzzle's
    

    [Min(0.01f)]
    public float burstTime;
    public float burstTracker;

    private Transform bulletParent;
    private string bulletParentName = "";

    public override void PlayerInput()
    {
        if (on)
        {
            if (PositiveButtonUp())
            { Deactivate(); }
        }
        else 
        {
            if (PositiveButtonDown())
            { Activate(); }
        }
           
     
            
        
    }

    public override void Act()
    {




        if (GetGunType() == GunType.auto)
        {
            if (PositiveButton())
            {
                Auto();
            }

        }
        else if (GetGunType() == GunType.burst)
        {
            //the burst will finish even if the button is released

            Burst();

        }
        else if (GetGunType() == GunType.laser)
        {
            if (on)
            {
                Laser();
            }
        }


    }

    public void Burst()
    {
        if (!on)
        {
            return;
        }

        if (bulletCount < BurstBulletCount())
        {
            burstTracker -= Time.deltaTime;

            if (burstTracker <= 0 )
            {
                FireBullet();
                burstTracker = burstTime;
                bulletCount++;
            }

        }

        else
        {
            on = false;
            timer_Cooldown = STAT_CooldownTime();
            bulletCount = 0;
        }
    }


    public void Auto()
    {


        if (!OnCooldown())
        {
            FireBullet();
            timer_Cooldown = STAT_CooldownTime();
        }
    }

    public void Laser()
    {
        burstTracker += Time.deltaTime;

        if (burstTracker >= burstTime)
        {
            FireBullet();
            timer_Cooldown = STAT_CooldownTime();
            burstTracker = 0;
            on = false;
        }
    }

    public void SemiAuto()
    {
        FireBullet();

        if (!on)
        {

        }
    }


    public override void Activate()
    {


        if (GetGunType() == GunType.burst)
        {
            if (burstTracker <= 0)
            {
                burstTracker = burstTime;
                bulletCount = 0;
            }
        }
        else if (GetGunType() == GunType.auto)
        {


        }
        else if (GetGunType() == GunType.laser)
        {
            if (burstTracker >= 0)
            {
                burstTracker = 0;

            }


        }
        else if (GetGunType() == GunType.semiauto)
        {
            // SemiAuto();
            FireBullet();

        }

        on = true;


    }

    public override void Deactivate()
    {
        on = false;
    }





    public void FireBullet()
    {
        if (gunParent != null)
        {
            FireBullet(gunParent);
            return;
        }

        if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
        {
            if (bullet != null)
            {
                GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
                clone.SetActive(true);
                clone.GetComponent<Bullet>().Launch(STAT_Power());
            }
        }
        else 
        {
            Transform newBullet = BulletParent().GetChild(0);
            newBullet.position = transform.position;
            newBullet.rotation = transform.rotation;
            newBullet.gameObject.SetActive(true);
            newBullet.GetComponent<Bullet>().Launch(STAT_Power());

        }

        
    }


    public void FireBullet(Transform _guns)
    {
        if (_guns != null)
        {
            foreach (Transform el in _guns)
            {
                if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
                {
                    if (bullet != null)
                    {
                        GameObject clone = Instantiate(bullet, el.position, el.rotation);
                        clone.SetActive(true);
                        clone.GetComponent<Bullet>().Launch(STAT_Power());
                    }
                }
                else
                {
                    Transform newBullet = BulletParent().GetChild(0);
                    newBullet.position = el.position;
                    newBullet.rotation = el.rotation;
                    newBullet.gameObject.SetActive(true);
                    newBullet.GetComponent<Bullet>().Launch(STAT_Power());

                }
            }
            
        }

        


    }


    public Transform BulletParent()
    {
        if (bulletParent == null)
        {
            if (bulletParentName.Length < 1)
            {
                bulletParentName = "PARENT_Bullet";
            }

            GameObject findParent = GameObject.Find(bulletParentName);


            if (findParent == null)
            {
                bulletParent = new GameObject(bulletParentName).transform;
            }
            else { bulletParent = findParent.transform; }
        }

        return bulletParent;
    }

    private int bulletCount;
    public int bulletsPerBurst = 3;
    public int BurstBulletCount() { return bulletsPerBurst; }
    public void BurstBulletCount(int _type) { bulletsPerBurst = _type; }

    public GunType GetGunType() { return gunType; }
    public void SetGunType(GunType _type) { gunType = _type; }
}
