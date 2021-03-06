﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public NpcManager npcManager;



    public Vector3 straferunspot; //for doing gun passes on large ships

    public Transform guns;

    [Header("These should be set at run time")]
    public GameObject mapArea,target,patrolparent,patroltarget;

    public GameObject bullet;
    public GameObject leadDistanceTarget;
    public GameObject explosion;
    public GameObject dradisModel;
    public GameObject squadLeader,myWing;

    public Material tookdmgcolor,patrolColor,frozenColor;
    public Renderer myRenderer;

    public int value = 1,itemheldtype;
    public float hp;
    public float speed = 20;
    public float rotForce = 6;
    public float leashDistance, noticePlayerDistance = 50.0f;

    public int stamina,tempHp,tempStamina;
    public float  staminaRechargeRate = 1,currentstaminaRechargeBonus,staminaRechargeBonus,currentStamina,engineStaminaCost,gunStaminaCost;//stamina recharges faster when not being used

    public bool friendly,conscriptable,destroyed,canShoot,returnHome,inBattle;
    public bool aitest,stationary,alert,inCombat;


    private float avoidCollisionClock,controlLockout;
    private Vector3 startPos,openSpotToAvoidCollision,holdVelocity;
    private Quaternion targetRotation,startRot;
    private Rigidbody rb;


    void Start()
    {
      if(rb == null){rb = GetComponent<Rigidbody>();}
        // ResetToNeutral(npcManager);

    }
    void Awake()
    {
      if(rb == null){rb = GetComponent<Rigidbody>();}
        ResetToNeutral(npcManager);

    }
    public void ResetToNeutral(NpcManager npcmanager)
    {

      npcManager = npcmanager;
      startPos = transform.position;
      startRot = transform.rotation;
      if(rb == null){rb = GetComponent<Rigidbody>();}
      if(hp <= 0){hp = 1;}
      alert = false;
      inCombat = false;
      if(patrolparent != null){  patroltarget = patrolparent.transform.GetChild(Random.Range(0,patrolparent.transform.childCount)).gameObject;  }
    }
    public void SetAlert(bool isAlert)
    {
      alert = isAlert;
      //TODO: return to rest location // leash
      if(alert == false){returnHome = true;}else{returnHome = false;}
    }


    public void Conscript(GameObject newsquadLeader)
    {
      //conscriptable units can join a group and function with group logic
      if(conscriptable == true){

        squadLeader = newsquadLeader;
        if(GetComponent<AIsquadunit>() != null)
        {
          GetComponent<AIsquadunit>().Conscript(squadLeader);
          //activate conscripted units
          alert = true;
        }
      }

    }
    // Update is called once per frame
    void Update()
    {

        //getting frozen with ice or using special moves locks the controls
          if(controlLockout <= 0){

                  //space battles have different idle/passive logic than a stage/level has
                    if(inBattle == false)
                    {
                       FlyOnMap();

                     }
                      else
                      {
                        FlyInBattle();
                      }
              }else{

                  controlLockout -= Time.deltaTime;
                  rb.velocity = holdVelocity;
                  //if controls were locked because the ship was frozen with ice, reset the target velocity to zero
                  if(controlLockout <= 0){  holdVelocity = Vector3.zero;}

              }


    }


    public void FlyInBattle()
    {

          //during battle enemies will not leash to idle
            if(alert == true)
            {

              AlertActions();


            }
            else{



                AlertActions();

                if(target == null )
                {
                  if(friendly == true){

                    target = npcManager.GetClosestEnemy(this.gameObject);
                    if(target == this.gameObject){target = null;}
                  }
                  else{
                      FindTarget();
                  }



                }


            }



    }


  public void HitByBullet(GameObject hitby)
  {

        if (hitby.GetComponent<Bullet>() == null)
        { hitby = hitby.transform.parent.parent.gameObject; }
        controlLockout = 0.2f;
      rb.velocity = (transform.position - hitby.transform.position ).normalized * hitby.GetComponent<Bullet>().impactForce;
      rb.angularVelocity = (transform.position - transform.forward ).normalized * hitby.GetComponent<Bullet>().impactForce * 0.2f;

       
      if(tookdmgcolor != null)
      {
        SetRenderer(tookdmgcolor);
      }
      if(hitby.GetComponent<Bullet>().ice == true)
      {
        controlLockout = 2.2f;
        holdVelocity = (rb.velocity.normalized) * Mathf.Max(rb.velocity.magnitude,5.0f);

        if(frozenColor != null )
        {
            SetRenderer(frozenColor);
        }

      }

      hp -= hitby.GetComponent<Bullet>().damage;

            if (hp <= 0)
            {
                Die();
            }

  }
  public void SetRenderer(Material newColor)
  {
      if(myRenderer != null )
      {
        myRenderer.material = frozenColor;

      }
  }

  public void FlyOnMap()
  {

      //alert enemies are enemies that are in the general area of play.
      //enemies in area x dont need to active when player is in Y
          if(alert == true)
          {

            AlertActions();
            if(target == null )
            {
              if(friendly == true){

                target = npcManager.GetClosestEnemy(this.gameObject);
                if(target == this.gameObject){target = null;}
              }
              // else{  FindTarget();}



            }

          }
          else{


            //TODO: have enemies leash back to their start Position
            if(returnHome == true )
            {  if(inCombat == false)
              {ReturnHome();}
              else
            {
              AlertActions();
              //if targeting something dont return until leashing
              if(target != null && Vector3.Distance(target.transform.position,transform.position) > leashDistance)
              {
                target = null; inCombat = false; alert = false;
              }
              else
              {
                //the player is out of this enemies home area and the enemy is not targeting anything
                inCombat = false;
              }
            }

          }



        }
    }


    public void AlertActions()
    {
      if(target != null){
            //inCombat = true;
        }
      //if conscripted follow leader, otherwise act as normal
      if(squadLeader != null)
      {
          if(GetComponent<AIsquadunit>() != null)
          {
            GetComponent<AIsquadunit>().Fly(target);
          }
      }
        else
        {
          if(GetComponent<AIattackpattern>() != null)
          {
            GetComponent<AIattackpattern>().Fly(target);
          }else if(GetComponent<AIEvasion>() != null)
          {
            GetComponent<AIEvasion>().Fly(target);
          }
          else if(GetComponent<AIDinosaur>() != null)
          {
            GetComponent<AIDinosaur>().Fly(target);
          }
          else if(GetComponent<AISquadLeader>() != null)
          {
            GetComponent<AISquadLeader>().Fly(target);
          }
            else if (GetComponent<AIsandbox>() != null)
            {
                GetComponent<AIsandbox>().Fly(target);
            }
            else if (GetComponent<AI0>() != null)
            {
                GetComponent<AI0>().Fly(target);
            }
            else if (GetComponent<Ai2>() != null)
            {
                GetComponent<Ai2>().Fly(target);
            }
            else if (GetComponent<AITurnAndMove>() != null)
            {
                GetComponent<AITurnAndMove>().Fly(target);
            }
            else if(GetComponent<AIsquadunit>() != null)
          {
                GetComponent<AIsquadunit>().Fly(target);
            }else
            { SendMessage("Fly",target);}
        }

    }


    public void ReturnHome()
    {


         CheckForward();
        //TODO: have enemies leash back to their start Position
        if(avoidCollisionClock <= 0){
                  if(Vector3.Distance(transform.position,startPos) < 15.0f){
                      transform.rotation = Quaternion.Slerp(transform.rotation, startRot, rotForce  * Time.deltaTime);
                      transform.position = Vector3.MoveTowards(transform.position,startPos,speed * Time.deltaTime);

                    if(Vector3.Distance(transform.position,startPos) < 2.2f){
                      returnHome = false;
                      transform.position = startPos;

                      transform.rotation = startRot;
                      rb.isKinematic = true;
                      rb.isKinematic = false;
                      // returnHome = false;
                    }

                  }else
                  {
                    rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
                    targetRotation = Quaternion.LookRotation( startPos - transform.position);

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
                    if(Vector3.Distance(transform.position,startPos) <= 5.0f){
                      transform.rotation = startRot;
                      rb.velocity = Vector3.zero;
                    }
                  }
        }else{}
    }


    public void FindTarget()
    {

        //target = npcManager.GetClosestTarget(transform.position);

        if (target != null && target.GetComponent<Fleetship>() != null)
        {
            target = target.GetComponent<Fleetship>().GetClosestShipPart(transform.position).gameObject;
        }
        else { target = npcManager.GetClosestTarget(transform.position); }

    }


    public void CheckToNoticePlayer(float distanceToCheck=100.0f)
    {
      //compare distance from in front to behind the enemy to determine if the player is in a forward cone of vision
      //or if the player is extremely close

          RaycastHit hit;



          if (Physics.Raycast(transform.position, (npcManager.GetPlayerShip().transform.position - transform.position), out hit, distanceToCheck) )
          {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        //check that the player is in front - if the forward of the enemy is closer than the enemy it can be resonably assumed the player is in front
                        if ((npcManager.GetDistanceToPlayer(transform.position + (transform.forward * 4)) < npcManager.GetDistanceToPlayer(transform.position)
                        ||
                        npcManager.GetDistanceToPlayer(transform.position) < 10) && npcManager.GetDistanceToPlayer(transform.position) < leashDistance)

                        {
                            if (friendly == false) { target = npcManager.GetPlayerShip(); inCombat = true; }

                        }
                    }
          }



    }


    public void FindSquadMembers()
    {
      //check to find npcs to add to squad
      if(GetComponent<AISquadLeader>() != null)
      {
        GameObject newsquadmember = npcManager.GetClosestUnConscriptedEnemy(this.gameObject);
        if(newsquadmember != this.gameObject)
        {
          GetComponent<AISquadLeader>().AddSquadMember(newsquadmember);
        }
      }
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet" && hp > 0)
        {
          HitByBullet(col.gameObject);

        }


    }
    public void OnTriggerStay(Collider other)
    {
        if ((other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Enemy>() != null) || other.transform.tag == "Enviroment")
        {
            targetRotation = Quaternion.LookRotation( transform.position - (transform.position - transform.forward));

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,   Time.deltaTime);
            //rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, transform.position - (other.transform.position - transform.position),  5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.forward,  5 * Time.deltaTime);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shippart")
        {
            other.gameObject.GetComponent<ShipPart>().TakeDamage(5);
            Instantiate(explosion, transform.position, transform.rotation);
            // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);

            Die();

        }
        if (other.gameObject.tag == "Bullet")
        {
            if(hp > 0){
              if (other.gameObject.tag == "Bullet")
              {
                HitByBullet(other.gameObject);

              }
            }
        }
    }
    public void FireGuns()
    {
        if(UseStamina(gunStaminaCost) == true)
        {
            foreach(Transform go in guns)
            {
              GameObject clone = Instantiate(bullet, go.transform.position, go.transform.rotation);
            }
        }
    }
    public void CheckForward()
    {
        // possible issue with dradis detection
        RaycastHit hit;



        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f) && Vector3.Distance(hit.point, startPos) > 10)
        {



                if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; } }


        }
        else { avoidCollisionClock -= Time.deltaTime;  }
    }


    public void RechargeStamina()
    {
        if(currentStamina < stamina + tempStamina)
        {


          currentStamina += Time.deltaTime * (staminaRechargeRate + currentstaminaRechargeBonus);
          if(currentstaminaRechargeBonus < staminaRechargeBonus)
          {
            currentstaminaRechargeBonus += Time.deltaTime;
          }
        }

    }
    public bool UseStamina(float cost)
    {
        currentstaminaRechargeBonus = 0;
        if(currentStamina >= cost)
        {
          currentStamina -=  cost;

          return true;
        }
        return false;
    }

    public void Die()
    {
      Instantiate(explosion, transform.position, transform.rotation);
        if (npcManager != null)
        {
            npcManager.NPCkilled(GetComponent<Enemy>());
        }
        //TODO: have npc manager disable them instead of spawning new ones all the time
        // Destroy(this.gameObject);
    }

    public void RayCastToFindOpening()
    {
      RaycastHit hit;

      if (Physics.Raycast(transform.position, transform.forward, out hit, 25.0f) && Vector3.Distance(transform.position,startPos) > 25.0f)
      {

        openSpotToAvoidCollision =  (startPos -  hit.point).normalized ;
        return;
      }else{

        return;

      }


    }

    public float DistanceToTarget(GameObject target)
    { return Vector3.Distance(transform.position, target.transform.position); }

    public float DistanceToTarget(Transform target)
    { return Vector3.Distance(transform.position, target.position); }

    public float DistanceToTarget(Vector3 target)
    { return Vector3.Distance(transform.position, target); }

    public bool RaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, distanceToCheck))
        {

            if (hit.transform == currenttarget)
            {
                return true;
            }
        }
        return false;

    }




}
