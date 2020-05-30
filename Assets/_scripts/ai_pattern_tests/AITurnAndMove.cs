using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnAndMove : MonoBehaviour
{

    public int currentAttackPlan;

  // parameters to change/set how the ship controls
    public float speed = 50,walkspeed = 10;
    public float rotForce = 6;
    public float accuracy = 1;

    public float closedistance = 10,fardistance = 40,plusminus = 5;
    public float gunCost = 1;
    public float checkForwardDistance = 100.0f;
  //

    public float gunCooldown;

    public float avoidCollisionClock;

    public bool destroyed,canShoot;
    public bool flyaway,flypast;

    public GameObject patrolparent,patroltarget;
    public Material avoidingCollisionColor,patrolColor;
    public Renderer myRenderer;
    public List<Material> colors;

    private Vector3 straferunspot,tempTargetSpot,openSpotToAvoidCollision;
    private Quaternion targetRotation;
    private Rigidbody rb;
    private Enemy myEnemy;
      // Use this for initialization
      void Awake () {

          rb = GetComponent<Rigidbody>();
          myEnemy = GetComponent<Enemy>();
          patrolparent = myEnemy.patrolparent;
          patroltarget = myEnemy.patroltarget;
          if(colors.Count > 0 && myRenderer != null)
          {  transform.GetChild(0).GetComponent<Renderer>().material = colors[Random.Range(0,(int)colors.Count) ];}
      }


    public void Fly(GameObject target)
    {

      myEnemy.RechargeStamina();

          if (target != null)
          {

            Attack(target);
          }
          else
          {
            Patrol();
            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost * 10;

                if(myEnemy.inBattle == true)
                {  myEnemy.FindTarget();}
                  else{  myEnemy.CheckToNoticePlayer();}


             }



          }
          // if (gunCooldown > 0)
          // { gunCooldown -= Time.deltaTime; }
           gunCooldown -= Time.deltaTime;

    }



    public void Attack(GameObject target)
    {
      CheckForward(target);
        if(avoidCollisionClock <= 0){
            if (target != null)
            {

                  Chicken(target);


            }
          }
          else
          {
            AvoidCollision();
          }

    }

    public void Chicken(GameObject targetship)
    {
      if(colors.Count > 0 && myRenderer != null)
      {  myRenderer.material = colors[currentAttackPlan + 1];}
      float angle = Vector3.Angle((targetship.transform.position + targetship.GetComponent<Rigidbody>().velocity) - transform.position, transform.forward);

      if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
      if (gunCooldown <= 0 && canShoot == true)
      {

          myEnemy.FireGuns();
          gunCooldown = gunCost + Random.Range(0, 3.0f);

      }

        if(Vector3.Distance(transform.position,targetship.transform.position) >= closedistance && myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true){

            rb.AddForce(transform.forward * speed * 10 *  Time.deltaTime);
        }
        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position );
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

    }




      public void Patrol()
      {

        if(patrolColor != null && myRenderer != null)
        {  myRenderer.material = patrolColor;}

        if(myEnemy.patrolparent != null){
            if(patroltarget != null){

          if (Vector3.Distance(patroltarget.transform.position, transform.position) < 10)
          { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject; }


          targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);

          float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

              transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

              // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
              transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, walkspeed  * Time.deltaTime);
            }
            else
            {
              patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, myEnemy.patrolparent.transform.childCount)).gameObject;
            }
          }
          else{

            if(myEnemy.inBattle == true)
            {  myEnemy.FindTarget();}
              else{  myEnemy.CheckToNoticePlayer();}

           }



      }



      public void CheckForward(GameObject target)
      {
          // possible issue with dradis detection
          RaycastHit hit;



          if (Physics.Raycast(transform.position, transform.forward, out hit, checkForwardDistance))
          {

              if (hit.transform.gameObject == target)
              {
                  canShoot = true;
                  avoidCollisionClock = 0;

              }
              else
              {
                  if (Vector3.Distance(transform.position,hit.point) < plusminus)
                  {rb.AddForce(transform.forward * speed * -Time.deltaTime);}

                    if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                    else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

              }

          }
          else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
      }

      public void RayCastToFindOpening()
      {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, checkForwardDistance))
        {

          openSpotToAvoidCollision =  (myEnemy.target.transform.position -  hit.point).normalized ;
          return;
        }else{

          return;

        }

        //
        // if (Physics.Raycast(transform.position, transform.right + transform.forward, out hit, 15.0f))
        // {
        //
        //
        // }else {openSpotToAvoidCollision = transform.position + ((transform.right + transform.forward) * 12.5f); return;}
        //
        // if (Physics.Raycast(transform.position,  transform.forward - transform.right, out hit, 15.0f))
        // {
        //
        //
        // }else {openSpotToAvoidCollision = transform.position + ((transform.forward - transform.right) * 12.5f); return;}
        // if (Physics.Raycast(transform.position, transform.forward + transform.up, out hit, 15.0f))
        // {
        //
        //
        // }else{openSpotToAvoidCollision = transform.position + ((transform.up + transform.forward) * 12.5f); return;}
        // if (Physics.Raycast(transform.position, transform.forward - transform.up, out hit, 5.0f))
        // {
        //
        //
        // }else{openSpotToAvoidCollision = transform.position + ((transform.up + transform.forward) * 12.5f); return;}
        //
        // openSpotToAvoidCollision = transform.position - (( -transform.up + transform.forward * 5) );
      }
      public void AvoidCollision()
      {
        //TODO: scan around to find the open space rather than always rotating away 180
        RayCastToFindOpening();
        if(avoidingCollisionColor != null && myRenderer != null)
        {  myRenderer.material = avoidingCollisionColor;}


          targetRotation = Quaternion.LookRotation( transform.position -   (openSpotToAvoidCollision ));
          // targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
          //either move forwad to avoid the obstacle of slow down to not collide
          if(avoidCollisionClock < 1){rb.AddForce(transform.forward * walkspeed * Time.deltaTime,ForceMode.Impulse);}
          else{ rb.AddForce(-transform.forward * walkspeed * Time.deltaTime,ForceMode.Impulse);}
          // rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * walkspeed,Time.deltaTime * speed );

      }
  }
