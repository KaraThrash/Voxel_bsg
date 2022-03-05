using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ShipSystem
{
    public GameObject bullet;
    public GunType gunType;



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
            if (on)
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
        if (burstTracker > 0)
        {
            burstTracker -= Time.deltaTime;

            if (!OnCooldown())
            {
                FireBullet();
                cdTracker = STAT_CooldownTime();
            }


            if (burstTracker <= 0)
            {
                cdTracker = burstTime;

            }
        }

        if (on && burstTracker <= 0 && !OnCooldown()) { burstTracker = burstTime; }
    }


    public void Auto()
    {


        if (!OnCooldown())
        {
            FireBullet();
            cdTracker = STAT_CooldownTime();
        }
    }

    public void Laser()
    {
        burstTracker += Time.deltaTime;

        if (burstTracker >= burstTime)
        {
            FireBullet();
            cdTracker = STAT_CooldownTime();
            burstTracker = 0;
            on = false;
        }
    }

    public void SemiAuto()
    {

        if (!on)
        {
            FireBullet();

        }
    }


    public override void Activate()
    {


        if (GetGunType() == GunType.burst)
        {
            if (burstTracker <= 0)
            {
                burstTracker = burstTime;

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
            SemiAuto();


        }

        on = true;


    }

    public override void Deactivate()
    {
        on = false;
    }





    public void FireBullet()
    {

        if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
        {
            if (bullet != null)
            {
                GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
                clone.SetActive(true);
                clone.GetComponent<Bullet>().Launch(power);
            }
        }
        else 
        {
            Transform newBullet = BulletParent().GetChild(0);
            newBullet.position = transform.position;
            newBullet.rotation = transform.rotation;
            newBullet.gameObject.SetActive(true);
            newBullet.GetComponent<Bullet>().Launch(power);

        }

        
    }


    public Transform BulletParent()
    {
        if (bulletParent == null)
        {
            if (bulletParentName.Length < 1)
            {
                bulletParentName = "BulletParent_";
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


    public GunType GetGunType() { return gunType; }
    public void SetGunType(GunType _type) { gunType = _type; }
}
