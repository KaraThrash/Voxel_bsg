using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnAndMove : MonoBehaviour
{

      public int currentAttackPlan;

      public float speed = 50,walkspeed = 10;
      public float rotForce = 6;
      public float accuracy = 1;

      public float closedistance = 10,fardistance = 40;
      public float gunCost = 1;
      public float checkForwardDistance = 100.0f;

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
      private float lastCollisionHitDistance;
        // Start is called before the first frame update
        void Start()
        {

          lastCollisionHitDistance = -1;
            rb = GetComponent<Rigidbody>();
            myEnemy = GetComponent<Enemy>();
            patrolparent = myEnemy.patrolparent;
            patroltarget = myEnemy.patroltarget;
            if(colors.Count > 0 && myRenderer != null)
            {  transform.GetChild(0).GetComponent<Renderer>().material = colors[Random.Range(0,(int)colors.Count) ];}


        }

        public void Fly(GameObject target)
        {


              if (target != null)
              {

                Attack(target);
              }
              else
              {

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




            if (target != null)
            {


                        if(avoidCollisionClock <= 0)
                        {
                                TryToAttack(target);

                        }
                        else
                        {
                                AvoidCollision();
                        }


              }








        }

        //when in range try and get above the target, if from that vantage it cant see, it tries to get below
        public void TryToAttack(GameObject target)
        {

          targetRotation = Quaternion.LookRotation(target.transform.position - transform.position );

          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
          if(lastCollisionHitDistance > closedistance || lastCollisionHitDistance == -1)
          {
            rb.AddForce(transform.forward * speed * Time.deltaTime,ForceMode.Impulse);
          }else{rb.velocity = Vector3.Lerp(rb.velocity,Vector3.zero,Time.deltaTime);}
          // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        }

        public bool RayCastAt(GameObject target,float rng)
        {

          RaycastHit hit;



          if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, rng))
          {


                if (hit.transform.gameObject == target)
                {
                    return true;

                }

          }
          return false;

        }

        public bool RayCastAt(Vector3 dir,float rng)
        {

          RaycastHit hit;



          if (Physics.Raycast(transform.position, dir, out hit, rng))
          {

                    return true;

          }
          return false;

        }
        public void CheckForward(GameObject target)
        {
            // possible issue with dradis detection
            RaycastHit hit;

            float rng = checkForwardDistance;
                if(Vector3.Distance(target.transform.position,transform.position) < checkForwardDistance ){rng = Vector3.Distance(target.transform.position,transform.position) ;}

            if (Physics.Raycast(transform.position, transform.forward, out hit, rng))
            {

                if (hit.transform.gameObject == target)
                {
                    lastCollisionHitDistance = 0;
                    canShoot = true;
                    avoidCollisionClock = 0;

                }
                else
                {
                    lastCollisionHitDistance = Vector3.Distance(hit.point,transform.position);


                    if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                    else { if (avoidCollisionClock < 2) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }
                }

            }
            else
            {
              lastCollisionHitDistance = 9999;
              avoidCollisionClock -= Time.deltaTime; canShoot = true;

            }
        }

        public void RayCastToFindOpening()
        {
          RaycastHit hit;

          if (Physics.SphereCast(transform.position, 4,transform.forward, out hit, closedistance))
          {

            openSpotToAvoidCollision =  (myEnemy.target.transform.position -  hit.point).normalized ;
            return;
          }else{

            return;

          }

        }
        public void AvoidCollision()
        {
          //TODO: scan around to find the open space rather than always rotating away 180
          RayCastToFindOpening();
          if(avoidingCollisionColor != null && myRenderer != null)
          {  myRenderer.material = avoidingCollisionColor;}




            if(lastCollisionHitDistance > closedistance)
            {
              rb.AddForce(transform.forward * walkspeed * Time.deltaTime,ForceMode.Impulse);
            }else
            {
              // rb.velocity = Vector3.zero;
              targetRotation = Quaternion.LookRotation( transform.position -   (openSpotToAvoidCollision ));
              // targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
              transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
              //either move forwad to avoid the obstacle of slow down to not collide
            }

        }


    }
