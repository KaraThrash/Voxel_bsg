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

    public override void Act_Fixed()
    {

        if (STAT_cooldownTime < 0.1f) { STAT_cooldownTime = 0.1f; }


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

        if (Ship() == null || MyEnemy() != null)
        {
            EnemyFireBullet();
            return;
        }


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
                if (Ship() != null && Ship().GetEquipment() != null && Ship().GetEquipment().GetBullet() != null)
                {
                    clone.GetComponent<Bullet>().bulletType = (BulletType)Ship().GetEquipment().GetBullet().subtype;
                }
                else
                {
                    clone.GetComponent<Bullet>().bulletType = BulletType.basic;
                }
                clone.SetActive(true);
                clone.GetComponent<Bullet>().Init();
                clone.GetComponent<Bullet>().Launch(Ship());
            }
        }
        else 
        {
            Transform newBullet = BulletParent().GetChild(0);

            if (Ship() != null && Ship().GetEquipment() != null && Ship().GetEquipment().GetBullet() != null)
            {
                newBullet.GetComponent<Bullet>().bulletType = (BulletType)Ship().GetEquipment().GetBullet().subtype;
            }
            else
            {
                newBullet.GetComponent<Bullet>().bulletType = BulletType.basic;
            }

            newBullet.gameObject.SetActive(true);
            newBullet.GetComponent<Bullet>().Init();
            newBullet.position = transform.position;
            newBullet.rotation = transform.rotation;
            newBullet.GetComponent<Bullet>().Launch(Ship());

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
                    if (Ship().GetEquipment().GetBullet() != null && bullet != null)
                    {
                        GameObject clone = Instantiate(bullet, el.position, el.rotation);
                        clone.GetComponent<Bullet>().bulletType = (BulletType)Ship().GetEquipment().GetBullet().subtype;

                        clone.GetComponent<Bullet>().Init();
                        clone.SetActive(true);
                        clone.GetComponent<Bullet>().Launch(Ship());
                    }
                    else
                    {
                        if (bullet != null)
                        {
                            GameObject clone = Instantiate(bullet, el.position, el.rotation);

                            clone.GetComponent<Bullet>().Init();
                            clone.SetActive(true);
                            clone.GetComponent<Bullet>().Launch(Ship());
                        }
                    }
                }
                else
                {
                    Transform newBullet = BulletParent().GetChild(0);

                    if (Ship().GetEquipment().GetBullet() != null)
                    { 
                        newBullet.GetComponent<Bullet>().bulletType = (BulletType)Ship().GetEquipment().GetBullet().subtype;

                    }

                    newBullet.gameObject.SetActive(true);
                    newBullet.GetComponent<Bullet>().Init();
                    newBullet.position = el.position;
                    newBullet.rotation = el.rotation;
                    newBullet.GetComponent<Bullet>().Launch(Ship());

                }
            }
            
        }

        


    }


    public void EnemyFireBullet()
    {
        if (MyEnemy() == null || MyEnemy().Stats() == null)
        {
            return;
        }


        if (gunParent == null)
        {
            
            return;
        }


        foreach (Transform el in  gunParent)
        {
            GameObject newBullet = null;

            if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
            {
                if (bullet != null)
                {
                    newBullet = Instantiate(bullet, el.position, el.rotation);
                }

            }
            else
            {
                newBullet = BulletParent().GetChild(0).gameObject;
            }

            newBullet.GetComponent<Bullet>().bulletType = MyEnemy().Stats().bulletType;
            newBullet.transform.position = el.position;
            newBullet.transform.rotation = el.rotation;

            newBullet.SetActive(true);
            newBullet.GetComponent<Bullet>().Init();
            newBullet.GetComponent<Bullet>().Launch(MyEnemy().Stats());
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
