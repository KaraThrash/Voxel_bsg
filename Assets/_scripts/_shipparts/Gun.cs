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
    public float burstTime;
    public float burstTracker;


    public override void PlayerInput()
    {
        
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
                timerCooldown = STAT_CooldownTime();
            }


            if (burstTracker <= 0)
            {
                timerCooldown = burstTime;

            }
        }

        if (on && burstTracker <= 0 && !OnCooldown()) { burstTracker = burstTime; }
    }


    public void Auto() 
    {
      

        if (!OnCooldown())
        {
            FireBullet();
            timerCooldown = STAT_CooldownTime();
        }
    }

    public void Laser() 
    {
        burstTracker += Time.deltaTime;

        if (burstTracker >= burstTime)
        {
            FireBullet();
            timerCooldown = STAT_CooldownTime();
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
        if (bullet != null)
        {
            GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
            clone.SetActive(true);
        }
    }


    public GunType GetGunType() { return gunType; }
    public void SetGunType(GunType _type) {  gunType = _type; }
}
