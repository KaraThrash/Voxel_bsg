using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponBase : ShipSystem
{
    public GameObject bullet;
    public GunType gunType;
    public List<Bullet_Type> bulletTypes;
    public Transform gunParent; //if the gun has multiple muzzle's
    

    [Min(0.01f)] 
    public float burstTime;
    public float burstTracker;
    public float bulletSize=0.2f;


    private SubID track_bulletSequence; //when alternating with twinlinked assign A/B

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
            if (PositiveButton() || on)
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
                Shoot();
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
            Shoot();
            timer_Cooldown = STAT_CooldownTime();
        }
        else 
        {
            timer_Cooldown -= Time.deltaTime;
        }
    }

    public void Laser()
    {
        burstTracker += Time.deltaTime;

        if (burstTracker >= burstTime)
        {
            Shoot();
            timer_Cooldown = STAT_CooldownTime();
            burstTracker = 0;
            on = false;
        }
    }

    public void SemiAuto()
    {
        Shoot();

        if (!on)
        {

        }
    }


    public override void Activate()
    {
        if ((ship == null || ship.canAct == false) && myEnemy == null)
        {
            return;
        }

        if (on)
        {
            return;
        }

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
            Shoot();

        }

        on = true;


    }

    public override void Deactivate()
    {
        on = false;
    }





    public void Shoot()
    {

        if (Ship() == null || MyEnemy() != null)
        {
            EnemyFireBullet();
            return;
        }





        if (gunParent != null)
        {
            foreach (Transform el in gunParent)
            {
                FireBullet(el);
            }


            return;
        }
        else
        {
            FireBullet(transform);
        }

        //if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
        //{
        //    if (bullet != null)
        //    {
        //        GameObject clone = Instantiate(bullet, transform.position, transform.rotation);
        //        if (Ship() != null && Ship().GetEquipment() != null && Ship().GetEquipment().GetBullet() != null)
        //        {
        //            clone.GetComponent<Bullet>().bulletType = (Bullet_Type)Ship().GetEquipment().GetBullet().subtype;
        //        }
        //        else
        //        {
        //            clone.GetComponent<Bullet>().bulletType = Bullet_Type.basic;
        //        }
        //        clone.SetActive(true);
        //        clone.GetComponent<Bullet>().Init();
        //        clone.GetComponent<Bullet>().Launch(Ship());
        //    }
        //}
        //else 
        //{
        //    Transform newBullet = BulletParent().GetChild(0);

        //    if (Ship() != null && Ship().GetEquipment() != null && Ship().GetEquipment().GetBullet() != null)
        //    {
        //        newBullet.GetComponent<Bullet>().bulletType = (Bullet_Type)Ship().GetEquipment().GetBullet().subtype;
        //    }
        //    else
        //    {
        //        newBullet.GetComponent<Bullet>().bulletType = Bullet_Type.basic;
        //    }

        //    newBullet.gameObject.SetActive(true);
        //    newBullet.GetComponent<Bullet>().Init();
        //    newBullet.position = transform.position;
        //    newBullet.rotation = transform.rotation;
        //    newBullet.GetComponent<Bullet>().Launch(Ship());

        //}

        
    }


    public void FireBullet(Transform _gun)
    {
        if (_gun != null)
        {
            GameObject newBullet = null;

            if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
            {
                if (bullet != null)
                {
                    newBullet = Instantiate(bullet, transform.position, transform.rotation);
                }

            }
            else
            {
                newBullet = BulletParent().GetChild(0).gameObject;
            }

            if (newBullet == null || newBullet.GetComponent<Bullet>() == null) { return; }

            newBullet.transform.position = _gun.position;
            newBullet.transform.rotation = _gun.rotation;

            newBullet.SetActive(true);
            newBullet.GetComponent<Bullet>().subid = track_bulletSequence;
            newBullet.GetComponent<Bullet>().Init();
            newBullet.GetComponent<Bullet>().Launch(Ship());


            AlternateSubId();
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

            GameObject newBullet = null;

            if (BulletParent().childCount == 0 || BulletParent().GetChild(0).gameObject.activeSelf)
            {
                if (bullet != null)
                {
                    newBullet = Instantiate(bullet, transform.position, transform.rotation);
                }

            }
            else
            {
                newBullet = BulletParent().GetChild(0).gameObject;
            }

            newBullet.GetComponent<Bullet>().bulletType = MyEnemy().Stats().bulletType;
            newBullet.transform.position = transform.position;
            newBullet.transform.rotation = transform.rotation;

            newBullet.SetActive(true);
            newBullet.GetComponent<Bullet>().Init();
            newBullet.GetComponent<Bullet>().Launch(MyEnemy().Stats());
            newBullet.transform.localScale = Vector3.one * MyEnemy().Stats().bulletSize;
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
            newBullet.transform.localScale = Vector3.one * MyEnemy().Stats().bulletSize;
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

    public List<Bullet_Type> BulletTypes()
    {
        if (bulletTypes == null)
        {
            bulletTypes = new List<Bullet_Type>();
        }
        return bulletTypes;
    }


    public void AlternateSubId()
    {
        if (track_bulletSequence == SubID.A)
        {
            track_bulletSequence = SubID.B;
        }
        else 
        {
            track_bulletSequence = SubID.A;
        }
    }

}
