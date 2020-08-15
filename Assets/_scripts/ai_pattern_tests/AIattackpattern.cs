using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIattackpattern : MonoBehaviour {

  public int currentAttackPlan;

// parameters to change/set how the ship controls
  public float speed = 50,walkspeed = 10;
  public float rotForce = 6;
  public float accuracy = 1;

  public float closedistance = 10,fardistance = 40,plusminus = 5;
  public float gunCost = 1,targetInSight = 0;
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
            //how long the target is, or is not in sight
          TrackTimeTargetIsInSight(target);
          Attack(target);

        }
        else
        {
          Patrol();
          if (gunCooldown <= 0)
          {
              gunCooldown = gunCost * 3;

              if(myEnemy.inBattle == true)
              {  myEnemy.FindTarget();}
                else{  myEnemy.CheckToNoticePlayer();}


           }



        }
        // if (gunCooldown > 0)
        // { gunCooldown -= Time.deltaTime; }
         gunCooldown -= Time.deltaTime;

  }

  public void AttackPlans(GameObject target)
  {
    switch(currentAttackPlan)
    {
      case -1: //get away
        GetAway(target);

      break;
      case 0: //chicken
        Chicken(target);

      break;
      case 1: //get behind
       GetBehind(target);

      break;
      case 2: //strafe run
      StrafeRun(target);

      break;
      case 3:// be evasive
                ZigZag(target);
                break;
    case 4: //verticalDive
        VerticalDive(target);

        break;
            case 5: //recover
                Recover(target);

                break;
            default:
        GetBehind(target);
      break;
    }

  }


  public void Attack(GameObject target)
  {
    //CheckForward(target);
      if(avoidCollisionClock <= 0){
          if (target != null)
          {

                AttackPlans(target);

              if (gunCooldown <= -8.0f)
              {
                    gunCooldown = 0;
                  CalculateNextMove(target);
               }
          }
        }
        else
        {
          AvoidCollision();
        }
        if (Input.GetKeyDown(KeyCode.M)) { CalculateNextMove(target); }
  }

    public float DistanceToTarget(GameObject target)
    { return Vector3.Distance(transform.position, target.transform.position); }

    public float DistanceToTarget(Transform target)
    { return Vector3.Distance(transform.position, target.position); }

    public float DistanceToTarget(Vector3 target)
    { return Vector3.Distance(transform.position, target); }


    //how long the target is, or is not in sight
    public void TrackTimeTargetIsInSight(GameObject target)
    {
        if (RaycastAtTarget(target.transform) == true)
        {
            if (targetInSight < 0) { targetInSight = 0; }
            targetInSight += Time.deltaTime;
        }
        else
        {
            if (targetInSight > 0) { targetInSight = 0; }
            targetInSight -= Time.deltaTime;
        }
    }

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

    public void Recover(GameObject targetship)
    {
        if (colors.Count > 5 && myRenderer != null)
        { myRenderer.material = colors[5]; }
        if (myEnemy.stamina > 5 || DistanceToTarget(targetship) < closedistance) { CalculateNextMove(targetship); }
    }

      public void GetAway(GameObject targetship)
      {
                transform.GetChild(0).GetComponent<Renderer>().material = colors[0];
                if(colors.Count > 0 && myRenderer != null)
                {  myRenderer.material = colors[0];}
                targetRotation = Quaternion.LookRotation(tempTargetSpot - transform.position );
                //not impulse, momentuem based

                if(myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
                {
                      rb.AddForce(transform.forward * speed  *  Time.deltaTime,ForceMode.Impulse);

                }
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
                if (DistanceToTarget(targetship) > fardistance || DistanceToTarget(tempTargetSpot) < closedistance)
                {
                     CalculateNextMove(targetship);
                }

    }

    public void StrafeRun(GameObject targetship)
    {

        if (colors.Count > 2 && myRenderer != null)
        { myRenderer.material = colors[2]; }

        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        {
            rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (DistanceToTarget(tempTargetSpot) < 1)
        {
            //target has low stamina and is vunerable to follow up
            if (targetship.GetComponent<PlayerControls>().playerStats.GetStamina() < 3)
            {
                tempTargetSpot = targetship.transform.position + (transform.up * closedistance);
                currentAttackPlan = 4; 
            }
            else { CalculateNextMove(targetship); }
        }

    }

    public void VerticalDive(GameObject targetship)
    {

        if (colors.Count > 4 && myRenderer != null)
        { myRenderer.material = colors[4]; }

        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        {
            rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (DistanceToTarget(tempTargetSpot) < 1)
        {
            //target has low stamina and is vunerable to follow up
            if (targetship.GetComponent<PlayerControls>().playerStats.GetStamina() < 3)
            {
                tempTargetSpot = targetship.transform.position - (transform.up * closedistance);
                currentAttackPlan = 4;
            }
            else { CalculateNextMove(targetship); }
        }

    }

    public void ZigZag(GameObject targetship) 
    {
        //dodge up down left right when the player has stamina to shoot and is facing you
        if (DistanceToTarget(tempTargetSpot) < 2)
        {
            float rnd = Random.Range(0, 10.0f);
            if (rnd > 8) { tempTargetSpot = transform.position + (transform.up * 5) + (transform.forward * 5); }
            else if (rnd > 6) { tempTargetSpot = transform.position + (transform.right * 5) + (transform.forward * 5); }
            else if (rnd > 4) { tempTargetSpot = transform.position - (transform.right * 5) + (transform.forward * 5); }
            else  { tempTargetSpot = transform.position - (transform.up * 5) + (transform.forward * 5); }
        }

        if (colors.Count > 3 && myRenderer != null)
        { myRenderer.material = colors[3]; }

        targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
        //not impulse, momentuem based

        if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        {
            rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (DistanceToTarget(targetship) < closedistance || targetship.GetComponent<PlayerControls>().playerStats.GetStamina() < 2)
        {
            CalculateNextMove(targetship);
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

        // CalculateNextMove(target);
    }

      if(myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true){
          targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position );
          //not impulse, momentuem based

          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
      }
      rb.AddForce(transform.forward * speed * 10 *  Time.deltaTime);
      if(Vector3.Distance(targetship.transform.position,transform.position) < 7)
      {
        tempTargetSpot = transform.position + (transform.forward * closedistance);
        currentAttackPlan = -1;
      }
  }
  public void CalculateNextMove(GameObject targetship)
  {
        bool targetinfront = Vector3.Distance((transform.position + transform.forward),targetship.transform.position ) < Vector3.Distance((transform.position - transform.forward),targetship.transform.position ) ;
        bool targetfacingme = Vector3.Distance((targetship.transform.position + targetship.transform.forward),transform.position ) < Vector3.Distance((targetship.transform.position - targetship.transform.forward),transform.position );
        float targetStamina = targetship.GetComponent<PlayerControls>().playerStats.GetStamina();


        if (targetfacingme == true)
        {
            if (targetStamina > 2)
            {
                tempTargetSpot = transform.position + (transform.right * 5);
                currentAttackPlan = 3; // zigzag
            }
            else if (myEnemy.stamina > gunCost * 3)
            {
                //pick a spot to the side of the target
                tempTargetSpot = targetship.transform.position + (transform.right * closedistance);
                currentAttackPlan = 2; // strafe run
            }
            else
            {
                currentAttackPlan = 0; // chicken
            }
        }
        else 
        {
            if (targetinfront == true)
            {
                //target In Front

                currentAttackPlan = 1; // getbehind
            }
            else { currentAttackPlan = 5;//recover 
            }


            


            
        }

        //if (targetinfront == true )
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
        //if(colors.Count > 0 && myRenderer != null)
        //{  myRenderer.material = colors[currentAttackPlan + 1];}

  }

    public void GetFarAndComeBack(GameObject targetship)
    {
      if(colors.Count > 0 && myRenderer != null)
      {  myRenderer.material = colors[currentAttackPlan + 1];}

           float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

           if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
           if (gunCooldown <= 0 && canShoot == true)
           {

               myEnemy.FireGuns();
               gunCooldown = gunCost + Random.Range(0, 3.0f);

               // CalculateNextMove(target);
           }

            if (flyaway == true)
            {

                if (flypast == true)
                {
                    if (Vector3.Distance(transform.position, targetship.transform.position) > closedistance)
                    {

                        flypast = false;

                    }
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(transform.position - targetship.transform.position);
                    if(Vector3.Distance(targetship.transform.position,transform.position) < closedistance)
                    {
                      tempTargetSpot = targetship.transform.position + (targetship.transform.forward * fardistance);
                      currentAttackPlan = -1;
                    }
                }



                if (Vector3.Distance(transform.position, targetship.transform.position) > fardistance)
                {
                    flyaway = false;
                    flypast = false;

                }

            }
            else
            {
               targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
                if (Vector3.Distance(transform.position, targetship.transform.position) < closedistance)
                {
                    flyaway = true;
                    flypast = true;

                }


            }

              if(myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
              {
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
          }
    }

    public void Patrol()
    {

      if(patrolColor != null && myRenderer != null)
      {  myRenderer.material = patrolColor;}

      if(myEnemy.patrolparent != null){
          if(patroltarget != null){

        if (Vector3.Distance(patroltarget.transform.position, transform.position) < 3)
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


    public void GetBehind(GameObject targetship)
    {

      if(colors.Count > currentAttackPlan && myRenderer != null)
      {  myRenderer.material = colors[currentAttackPlan];}

          GameObject target = myEnemy.target;

            if (Vector3.Distance(transform.position,(target.transform.position  + target.transform.forward )  ) < Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) )
            {
              targetRotation = Quaternion.LookRotation(  (target.transform.position  - (target.transform.forward * 50) - (target.transform.up * 50) ) -transform.position  );

            }
            else
            {
                targetRotation = Quaternion.LookRotation( target.transform.position   - transform.position);

            }

                if(myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
                {

                        if(Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) > 30)
                        {rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);}
                        else   if(Vector3.Distance(transform.position,(target.transform.position  - (target.transform.forward * closedistance) )) > 10)
                          {  rb.AddForce(((target.transform.position  - (target.transform.forward * closedistance)) - transform.position) * speed * Time.deltaTime);}
                        else
                        {
                          transform.position = Vector3.MoveTowards(transform.position,(target.transform.position  - (target.transform.forward * closedistance)),speed * 0.1f * Time.deltaTime);

                        }
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
                if (Vector3.Distance(transform.position,hit.point) < plusminus)
                {rb.AddForce(transform.forward * speed * -Time.deltaTime);}

                  if (avoidCollisionClock <= 0) { avoidCollisionClock = 1.4f; }
                  else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }

            }

        }
        else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
    }

    public void RayCastForward()
    {

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
