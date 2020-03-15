﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public NpcManager npcManager;



    public Vector3 straferunspot; //for doing gun passes on large ships

    public Transform guns;

    public GameObject mapArea,target,patrolparent,patroltarget;

    public GameObject bullet;
    public GameObject leadDistanceTarget;
    public GameObject explosion;
    public GameObject dradisModel;
    public GameObject squadLeader,myWing;



    public int value = 1,itemheldtype;
    public int hp;
    public float speed = 20;
    public float rotForce = 6;
    public float leashDistance;
    public bool friendly,conscriptable,destroyed,canShoot,returnHome,inBattle;
    public bool aitest,stationary,alert,inCombat;
    private float avoidCollisionClock;
    private Vector3 startPos,openSpotToAvoidCollision;
    private Quaternion targetRotation,startRot;
    private Rigidbody rb;

    // Use this for initialization
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
      // if(patrolparent == null){  patrolparent = GameObject.Find("PatrolPoints");}
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
      if(inBattle == false)
      {
         FlyOnMap();

       }
        else
        {
          FlyInBattle();
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
            else{  FindTarget();}



          }


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
        else{  FindTarget();}



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
      if(target != null){inCombat = true;}
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
          else{SendMessage("Fly",target);}
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
        }else{AvoidCollision();}
    }
    public void FindTarget()
    {

        target = npcManager.GetClosestTarget(transform.position);

        if(target != null && target.GetComponent<Fleetship>() != null)
        {
        target =  target.GetComponent<Fleetship>().GetClosestShipPart(transform.position).gameObject;
        }


    }
    public void CheckToNoticePlayer()
    {
      //compare distance from in front to behind the enemy to determine if the player is in a forward cone of vision
      //or if the player is extremely close
      if((npcManager.GetDistanceToPlayer(transform.position + (transform.forward * 15)) < npcManager.GetDistanceToPlayer(transform.position - (transform.forward * 2))
      ||
      npcManager.GetDistanceToPlayer(transform.position) < 5) && npcManager.GetDistanceToPlayer(transform.position) < leashDistance )

      {
        if(friendly == false){  target = npcManager.GetPlayerShip();}

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
        if (col.gameObject.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            if(hp > 0){
                  hp -= 1;
                  if (hp <= 0)
                  {
                      Die();
                  }
                }
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
                Instantiate(explosion, transform.position, transform.rotation);
                // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
                hp -= 1;
                if (hp <= 0)
                {
                    Die();
                }
            }
        }
    }
    public void FireGuns()
    {

        foreach(Transform go in guns)
        {
          GameObject clone = Instantiate(bullet, go.transform.position, go.transform.rotation);
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

    public void Die()
    {
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
    public void AvoidCollision()
    {
      //TODO: scan around to find the open space rather than always rotating away 180
      RayCastToFindOpening();

        targetRotation = Quaternion.LookRotation( transform.position -   (openSpotToAvoidCollision ));
        // targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
        //either move forwad to avoid the obstacle of slow down to not collide
        if(avoidCollisionClock < 1){rb.AddForce(transform.forward * speed * Time.deltaTime,ForceMode.Impulse);}
        else{ rb.AddForce(-transform.forward * speed * Time.deltaTime,ForceMode.Impulse);}
        // rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * walkspeed,Time.deltaTime * speed );
        if(returnHome == true && Vector3.Distance(transform.position,startPos) < 15.0f)
        {avoidCollisionClock = 0;}
    }




}
