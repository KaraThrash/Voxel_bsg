using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//single shot weapon - > shoot on button press [allowing for a cooldown/firerate] 
//burst shot weapon - > button press starts the micro timer that checks the cooldown to fire a bullet






public class Gun : ShipSystem
{
    public GameObject bullet;
    public GunType gunType;

    [Min(0.01f)]
    public float burstTime,cooldownTime;
    public float burstTracker,cooldownTracker;

    public override void Act()
    {


        if (cooldownTracker > 0)
        {
            cooldownTracker -= Time.deltaTime;
        }


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

            if (cooldownTracker <= 0)
            {
                FireBullet();
                cooldownTracker = cooldownTime;
            }


            if (burstTracker <= 0)
            {
                cooldownTracker = burstTime;

            }
        }

        if (on && burstTracker <= 0 && cooldownTracker <= 0) { burstTracker = burstTime; }
    }


    public void Auto() 
    {
        cooldownTracker -= Time.deltaTime;

        if (cooldownTracker <= 0)
        {
            FireBullet();
            cooldownTracker = cooldownTime;
        }
    }

    public void Laser() 
    {
        burstTracker += Time.deltaTime;

        if (burstTracker >= burstTime)
        {
            FireBullet();
            cooldownTracker = cooldownTime;
            burstTracker = 0;
            on = false;
        }
    }


    public override void Activate()
    {
        on = true;

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


        
    }

    public override void Deactivate()
    {
        on = false;
    }


    public void FireBullet()
    {
        if (bullet != null)
        {
            GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
            clone.SetActive(true);
        }
    }


    public GunType GetGunType() { return gunType; }
    public void SetGunType(GunType _type) {  gunType = _type; }
}
