using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIattackpattern : MonoBehaviour {

    public int currentAttackPlan;
    public float speed,walkspeed;
    public float rotForce = 6;

    public float accuracy;
    public float gunCooldown;
    public float gunCost;
    public float closedistance,fardistance,avoidCollisionClock;

    public bool destroyed,canShoot;
    public bool flyaway,flypast;

    public GameObject patrolparent,patroltarget;
    public List<Material> colors;
    private Vector3 straferunspot,tempTargetSpot;
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
                myEnemy.FindTarget();
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

          if (target != null)
          {

                AttackPlans(target);

              if (gunCooldown <= -8.0f)
              {
                  CalculateNextMove(target);
               }
          }


  }
  public void GetAway(GameObject targetship)
  {
        transform.GetChild(0).GetComponent<Renderer>().material = colors[0];
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
    float angle = Vector3.Angle(targetship.transform.position - transform.position, transform.forward);

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
        transform.GetChild(0).GetComponent<Renderer>().material = colors[currentAttackPlan + 1];
  }

    public void GetFarAndComeBack(GameObject targetship)
    {
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

      patroltarget = myEnemy.patroltarget;

      if(patroltarget != null){
        if (Vector3.Distance(patroltarget.transform.position, transform.position) < 10)
        { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, 5)).gameObject; }


        targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);

        float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

            // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, walkspeed  * Time.deltaTime);
          }else{myEnemy.FindTarget(); }



    }


    public void GetBehind(GameObject targetship)
    {
          GameObject target = myEnemy.target;
            //gunCooldown -= Time.deltaTime;

           // targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);



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

    public void CheckForward()
    {
      GameObject target = myEnemy.target;
        // possible issue with dradis detection
        RaycastHit hit;



        if (Physics.Raycast(transform.position, transform.forward, out hit, 310.0f))
        {

            if (hit.transform.gameObject == myEnemy.target)
            {
                canShoot = true;


            }
            else
            {
                rb.drag = 0.5f;
                //canShoot = false;
                if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                else { if (avoidCollisionClock < 3) { straferunspot = Vector3.zero; avoidCollisionClock += Time.deltaTime; } }
            }

        }
        else { avoidCollisionClock -= Time.deltaTime; canShoot = true; }
    }
    public void AvoidCollision()
    {

        rb.drag = 2.0f;
        transform.Rotate(Vector3.right * -30 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, (transform.position + transform.forward), speed  * Time.deltaTime);
    }
}
