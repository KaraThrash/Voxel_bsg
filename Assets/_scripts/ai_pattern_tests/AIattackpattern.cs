using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIattackpattern : MonoBehaviour {

  public int currentAttackPlan;

// parameters to change/set how the ship controls
  public float speed = 50,walkspeed = 10;
  public float rotForce = 6;
  public float accuracy = 1;

  public float closedistance = 10,fardistance = 40;
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
      case 2: //strafe
      GetFarAndComeBack(target);

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
  public void GetAway(GameObject targetship)
  {
        transform.GetChild(0).GetComponent<Renderer>().material = colors[0];
        if(colors.Count > 0 && myRenderer != null)
        {  myRenderer.material = colors[0];}
    targetRotation = Quaternion.LookRotation(tempTargetSpot - transform.position );
    //not impulse, momentuem based
    rb.AddForce(transform.forward * speed  *  Time.deltaTime,ForceMode.Impulse);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
    if (gunCooldown <= -3.0f)
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

      targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position );
      //not impulse, momentuem based
      rb.AddForce(transform.forward * speed * 10 *  Time.deltaTime);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
      if(Vector3.Distance(targetship.transform.position,transform.position) < closedistance)
      {
        tempTargetSpot = transform.position + (transform.forward * closedistance);
        currentAttackPlan = -1;
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
        if(colors.Count > 0 && myRenderer != null)
        {  myRenderer.material = colors[currentAttackPlan + 1];}

  }

    public void GetFarAndComeBack(GameObject targetship)
    {
      if(colors.Count > 0 && myRenderer != null)
      {  myRenderer.material = colors[currentAttackPlan + 1];}
          // GameObject target = myEnemy.target;
            //gunCooldown -= Time.deltaTime;

           // targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);
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

            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
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


    public void GetBehind(GameObject targetship)
    {

      if(colors.Count > 0 && myRenderer != null)
      {  myRenderer.material = colors[currentAttackPlan + 1];}

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

                if (avoidCollisionClock < 0) { avoidCollisionClock = 1.4f; }
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
