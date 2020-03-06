using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIsquadunit : MonoBehaviour
{
  public int currentAttackPlan;
  public float speed,walkspeed;
  public float rotForce = 6;

  public float accuracy,bufferForTargetPosition; //how close to a target counts as being there
  public float gunCooldown;
  public float gunCost;
  public float closedistance,fardistance,avoidCollisionClock;
  public float checkForwardDistance = 100.0f;
  public bool centerSquad,destroyed,canShoot;//centersquad for whether directions are given relative to the target or the squad leader

  public Transform lookTarget;
  public GameObject squadLeader,patroltarget,patrolparent;
  public Material conscriptedColor,avoidingCollisionColor,patrolColor;
  public List<Material> colors;

  private Vector3 openSpotToAvoidCollision,positionTarget;
  private Quaternion targetRotation;
  private Rigidbody rb;
  private Enemy myEnemy;
  // Use this for initialization
  void Awake () {

      rb = GetComponent<Rigidbody>();
      myEnemy = GetComponent<Enemy>();
      patrolparent = myEnemy.patrolparent;
      patroltarget = myEnemy.patroltarget;
      if(colors.Count > 0)
      {  transform.GetChild(0).GetComponent<Renderer>().material = colors[Random.Range(0,(int)colors.Count) ];}
  }

  public void Conscript(GameObject newsquadLeader)
  {
    squadLeader = newsquadLeader;
  }

  public void Fly(GameObject target)
  {


      if (squadLeader != null)
      {

              CheckForward(target);
                if(avoidCollisionClock <= 0){
                    FollowOrder();
                  }
                  else
                  {
                    AvoidCollision();
                  }



      }
      else
      {
        // Patrol();
        // if (gunCooldown <= 0)
        // {
        //     gunCooldown = gunCost * 10;
        //       myEnemy.CheckToNoticePlayer();
        //  }



      }

       gunCooldown -= Time.deltaTime;

  }
  //set where the squad member should go and what it should look at// all relative to the squad leader
    public void GetOrder(Vector3 newpositionTarget,Transform newlookTarget)
  {
    positionTarget = newpositionTarget;
    lookTarget = newlookTarget;
  }
  public void FollowOrder()
  {
    Transform centerTransform = squadLeader.transform;

    if(centerSquad == false && lookTarget != null)
    {
      centerTransform = lookTarget;

    }

    //if far from target position face it and move forward
    //otherwise face the look target
    if(Vector3.Distance(transform.position, positionTarget + centerTransform.position ) < bufferForTargetPosition)
    {
       targetRotation = Quaternion.LookRotation( (lookTarget.position)   - transform.position);

       if(Vector3.Distance(transform.position, positionTarget + centerTransform.position ) < bufferForTargetPosition * 0.5f)
       {
          targetRotation = Quaternion.LookRotation( (lookTarget.position)   - transform.position);
          transform.position = Vector3.MoveTowards(transform.position, positionTarget + centerTransform.position, walkspeed  * Time.deltaTime);



       }else{rb.AddForce(((positionTarget + centerTransform.position) - transform.position) * speed * Time.deltaTime, ForceMode.Impulse);}


    }
    else{

      targetRotation = Quaternion.LookRotation( (positionTarget + centerTransform.position)   - transform.position);
       rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);


  }
  public void AttackPlans(GameObject target)
  {
      switch(currentAttackPlan)
      {

        case 0: //get behind
         GetBehind(target);

        break;
        case 1: //get behind
         GetBehind(target);

        break;
        case 2: //strafe


        break;
        case 3:

        break;
        default:
          GetBehind(target);
        break;
      }

  }

  public void Attack(GameObject target)
  {
  CheckForward(target);
    if(avoidCollisionClock <= 0){
        if (target != null)
        {

              AttackPlans(target);

            if (gunCooldown <= -8.0f)
            {
                CalculateNextMove(target);
             }
        }
      }
      else
      {
        AvoidCollision();
      }

  }


  public void CalculateNextMove(GameObject targetship)
  {
      bool targetinfront = Vector3.Distance((transform.position + transform.forward),targetship.transform.position ) < Vector3.Distance((transform.position - transform.forward),targetship.transform.position ) ;
      bool targetfacingme = Vector3.Distance((targetship.transform.position + targetship.transform.forward),transform.position ) < Vector3.Distance((targetship.transform.position - targetship.transform.forward),transform.position );
      if(targetinfront == true )
      {
        //target In Front
          if(targetfacingme == true )
          {
            currentAttackPlan = 0; // chicken
          }

          else
          {
            currentAttackPlan = 1; // chase / get behind
          }


      }
      else
      { //target behind
              if(targetfacingme == true )
              {
                  currentAttackPlan = 2;
              }

              else
              {
                currentAttackPlan = 1;
              }
      }
      transform.GetChild(0).GetComponent<Renderer>().material = colors[currentAttackPlan + 1];
  }



  public void Patrol()
  {

    transform.GetChild(0).GetComponent<Renderer>().material = patrolColor;

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

        myEnemy.CheckToNoticePlayer();

       }



  }


  public void GetBehind(GameObject targetship)
  {
      transform.GetChild(0).GetComponent<Renderer>().material = colors[currentAttackPlan + 1];
        GameObject target = myEnemy.target;

          if (Vector3.Distance(transform.position,(target.transform.position  + target.transform.forward )  ) < Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) )
          {
            targetRotation = Quaternion.LookRotation(  (target.transform.position  - (target.transform.forward * 50) - (target.transform.up * 50) ) -transform.position  );

          }
          else
          {
              targetRotation = Quaternion.LookRotation( target.transform.position   - transform.position);

          }
          if(Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) > 30)
          {rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);}
          else   if(Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) > 10)
            {  rb.AddForce(((target.transform.position  - (target.transform.forward * closedistance)) - transform.position) * speed * Time.deltaTime);}
          else
          {
            transform.position = Vector3.MoveTowards(transform.position,(target.transform.position  - (target.transform.forward * closedistance)),speed * 0.1f * Time.deltaTime);

          }

          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
          float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

          if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
          if (gunCooldown <= 0 && canShoot == true)
          {

              myEnemy.FireGuns();
              gunCooldown = gunCost + Random.Range(0, 3.0f);

              // CalculateNextMove(target);
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
              if (hit.transform.gameObject != squadLeader)
              {
                if (avoidCollisionClock < 0) { avoidCollisionClock = 1.4f; }
                else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; } }
              }
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

  }
  public void AvoidCollision()
  {
    //TODO: scan around to find the open space rather than always rotating away 180
    RayCastToFindOpening();
    transform.GetChild(0).GetComponent<Renderer>().material = avoidingCollisionColor;
      targetRotation = Quaternion.LookRotation( transform.position -   (openSpotToAvoidCollision ));
      // targetRotation = Quaternion.LookRotation(  (transform.position  + (transform.up * 50) - (transform.forward * 50) ) - transform.position  );
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * 0.3f * Time.deltaTime);
      //either move forwad to avoid the obstacle of slow down to not collide
      if(avoidCollisionClock < 1){rb.AddForce(transform.forward * walkspeed * Time.deltaTime,ForceMode.Impulse);}
      else{ rb.AddForce(-transform.forward * walkspeed * Time.deltaTime,ForceMode.Impulse);}
      // rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * walkspeed,Time.deltaTime * speed );

  }
  }
