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
    public Renderer myRenderer;
    public List<Material> colors;
    private Vector3 openSpotToAvoidCollision,positionTarget, tempTargetSpot;
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentAttackPlan = 0;
        }
        if (squadLeader != null)
      {
            Attack(target);
            //CheckForward(target);
            //    if(avoidCollisionClock <= 0){
                   
            //      }
            //      else
            //      {
            //        AvoidCollision();
            //      }



      }
      else
      {
            Attack(target);
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
    public void GetOrder(Vector3 newpositionTarget,Transform newlookTarget,int newplan=-1)
  {
        transform.localScale = new Vector3(1, 1, 1);
        positionTarget = newpositionTarget;
        lookTarget = newlookTarget;
        if (newplan != -1 && currentAttackPlan < 3) {
            if (newplan == 1)
            { tempTargetSpot = transform.position + (newlookTarget.up * closedistance); }
            else if (newplan == 3)
            { tempTargetSpot = transform.position + (transform.right * closedistance); }
            else if (newplan == 4)
            { tempTargetSpot = newlookTarget.position + (newlookTarget.right * closedistance); }
            else if (newplan == 5)
            { tempTargetSpot = newlookTarget.position + (newlookTarget.up * closedistance); }
            else { tempTargetSpot = newlookTarget.position + (newlookTarget.up * closedistance); }
            currentAttackPlan = newplan; 
        
        }
        positionTarget = tempTargetSpot;
    }

  public void GetInFormation()
  {
        Transform centerTransform = squadLeader.transform;

        if(centerSquad == false && lookTarget != null)
        {
          //centerTransform = lookTarget;

        }

        targetRotation = Quaternion.LookRotation((lookTarget.position) - transform.position);
        rb.velocity = Vector3.Lerp(rb.velocity, ((positionTarget + centerTransform.position) - transform.position).normalized * speed,Time.deltaTime);
        rb.AddForce(((positionTarget + centerTransform.position) - transform.position).normalized * speed * Time.deltaTime);
    

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);


  }


    public void GetInFrontOfLeader()
    {
        Transform centerTransform = squadLeader.transform;

        if (centerSquad == false && lookTarget != null)
        {
            //centerTransform = lookTarget;

        }
        transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        targetRotation = Quaternion.LookRotation((lookTarget.position) - transform.position);
        rb.velocity = Vector3.Lerp(rb.velocity, (((centerTransform.forward * 20) + centerTransform.position) - transform.position).normalized * speed, Time.deltaTime);
        //rb.AddForce(((positionTarget + centerTransform.position) - transform.position).normalized * speed * Time.deltaTime);


        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);


    }
    public void AttackPlans(GameObject target)
  {
      switch(currentAttackPlan)
      {

        case 0: //get behind
         GetInFormation();

        break;
        case 1: //get behind
         GetBehindTarget(target);

        break;
        case 2: //
        GetInFrontOfLeader();

                break;
        case 3:
                ZigZag(target);
        break;
            case 4:
                StrafeRun(target);
                break;
            case 5:
                VerticalDive(target);
                break;
            case 6:
                VerticalDive(target);
                break;
            default:
                GetInFormation();
        break;
      }

  }



    public void FollowOrder(GameObject target)
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentAttackPlan = 0;
        }
        AttackPlans(target);
    }


     public void Attack(GameObject target)
     {
        AttackPlans(target);

        //CheckForward(target);
        //if (avoidCollisionClock <= 0)
        //{
        //    if (target != null)
        //    {

        //        AttackPlans(target);

        //        if (gunCooldown <= -8.0f)
        //        {
        //            CalculateNextMove(target);
        //        }
        //    }
        //}
        //else
        //{
        //    AvoidCollision();
        //}

    }


  public void CalculateNextMove(GameObject targetship)
  {
      //bool targetinfront = Vector3.Distance((transform.position + transform.forward),targetship.transform.position ) < Vector3.Distance((transform.position - transform.forward),targetship.transform.position ) ;
      //bool targetfacingme = Vector3.Distance((targetship.transform.position + targetship.transform.forward),transform.position ) < Vector3.Distance((targetship.transform.position - targetship.transform.forward),transform.position );
      //if(targetinfront == true )
      //{
      //  //target In Front
      //    if(targetfacingme == true )
      //    {
      //      currentAttackPlan = 0; // chicken
      //    }

      //    else
      //    {
      //      currentAttackPlan = 1; // chase / get behind
      //    }


      //}
      //else
      //{ //target behind
      //        if(targetfacingme == true )
      //        {
      //            currentAttackPlan = 2;
      //        }

      //        else
      //        {
      //          currentAttackPlan = 1;
      //        }
      //}
      //transform.GetChild(0).GetComponent<Renderer>().material = colors[currentAttackPlan + 1];
  }


    public void ZigZag(GameObject targetship)
    {
        //dodge up down left right when the player has stamina to shoot and is facing you
        if (myEnemy.DistanceToTarget(tempTargetSpot) < 2)
        {
            float rnd = Random.Range(0, 10.0f);
            if (rnd > 8) { tempTargetSpot = transform.position + (transform.up * 5) + (transform.forward * 5); }
            else if (rnd > 6) { tempTargetSpot = transform.position + (transform.right * 5) + (transform.forward * 5); }
            else if (rnd > 4) { tempTargetSpot = transform.position - (transform.right * 5) + (transform.forward * 5); }
            else { tempTargetSpot = transform.position - (transform.up * 5) + (transform.forward * 5); }
        }



        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        //if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        //{
        //    rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        //}
        rb.velocity = Vector3.Lerp(rb.velocity, (tempTargetSpot - transform.position).normalized * speed, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

    }

    public void StrafeRun(GameObject targetship)
    {

        if (colors.Count > 2 && myRenderer != null)
        { myRenderer.material = colors[2]; }

        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        //if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        //{
        //    rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        //}
        rb.velocity = Vector3.Lerp(rb.velocity, (tempTargetSpot - transform.position).normalized * speed, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (myEnemy.DistanceToTarget(tempTargetSpot) < 4)
        {

            if (myEnemy.DistanceToTarget(targetship) < closedistance)
            {

                tempTargetSpot = transform.position + (transform.right * 5) - (transform.forward * 5);
            }
            else { tempTargetSpot = transform.position + (transform.right * 5) + (transform.forward * 1); }
        }

    }

    public void VerticalDive(GameObject targetship)
    {

        if (colors.Count > 4 && myRenderer != null)
        { myRenderer.material = colors[4]; }

        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        //if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        //{
        //    rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        //}
        rb.velocity = Vector3.Lerp(rb.velocity, (tempTargetSpot - transform.position).normalized * speed, Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (myEnemy.DistanceToTarget(tempTargetSpot) < 4)
        {
            if (currentAttackPlan == 6) { currentAttackPlan = 0; }
            else { currentAttackPlan = 6; tempTargetSpot = targetship.transform.position - (transform.up * 25); }
            
        }

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


  public void GetBehindTarget(GameObject targetship)
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
