using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy")]
public class Stats_Enemy : ScriptableObject
{



    public int pointValue = 1;
    public int closeRange= 10;
    public int midRange = 20;
    public int farRange = 30;

    public float rangeVarience = 5; //check against varience not against the opposite range e.g.: when close check if further than close+varience to prevent riding the line  
    public float angleTolerance = 5; //check against varience not against the opposite range e.g.: when close check if further than close+varience to prevent riding the line  
                                 //angle tolerance for the cone around a perfect facing
    public float leashDistance = 100.0f;
    public float  noticePlayerDistance = 50.0f;
    public float  makeDecisionTime = 5.0f;

    public float gunDamage = 1;
    public float torquePower = 1;
    public float engineThrottle = 1;
    public float accelerate = 1;
    public float decelerate = 1;

    public GunType guntype;
    public BulletType bulletType;
  

    public int bulletsPerBurst;
    public float bulletLifeTime;
    public float bulletSpeed;
    public float timeBetweenBursts;
    public float firerate;


}
