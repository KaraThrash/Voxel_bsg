using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISquadLeader : MonoBehaviour
{
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
  public List<AIsquadunit> squad;
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
          if(Input.GetKeyDown(KeyCode.O))
          {
            CommandSquad( target);
          }
          if(Input.GetKeyDown(KeyCode.I))
          {
            CommandSquadWithPlan( target,1);
          }
        if (Input.GetKeyDown(KeyCode.J))
        {
            CommandSquadWithPlan(target, 5);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            CommandSquadWithPlan(target, 3);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CommandSquadWithPlan(target, 4);
        }

        if (target != null)
        {
            myEnemy.inCombat = true;
            Attack(target);
        }
        else
        {
            Patrol();
            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost * 10;
                myEnemy.CheckToNoticePlayer();
            }



        }
        // if (gunCooldown > 0)
        // { gunCooldown -= Time.deltaTime; }
        gunCooldown -= Time.deltaTime;

    }
    public void CommandSquad(GameObject target)
    {
      Vector3 temppos = (transform.forward * -15) + transform.up * 5;
      int count = 4;
      foreach(AIsquadunit ai in squad)
      {
        ai.GetOrder(temppos,target.transform,0);
        if(count % 2 == 0)
        {
          if(count % 3 == 0)
          {temppos = ((transform.right * -count * 5)) - (transform.forward * 5 * count);}
          else{temppos = ( (transform.right * count * 5)) - (transform.forward * 5 * count);}


        }
        else
        {
          if(count % 3 == 1)
          {temppos = ( (transform.up * -count * 4)) - (transform.forward * 5 * count);}
          else{temppos = ( (transform.up * count * 4)) - (transform.forward * 5 * count);}


        }

        count++;
      }
    }

    public void CommandSquadWithPlan(GameObject target,int newplan)
    {
        Vector3 temppos = (transform.forward * -15) + transform.up * 5;
        int count = 4;
        foreach (AIsquadunit ai in squad)
        {
            if (ai != null)
            {
                ai.GetOrder(temppos, target.transform, newplan);
                if (count % 2 == 0)
                {
                    if (count % 3 == 0)
                    { temppos = ((transform.right * -count * 5)) - (transform.forward * 5 * count); }
                    else { temppos = ((transform.right * count * 5)) - (transform.forward * 5 * count); }


                }
                else
                {
                    if (count % 3 == 1)
                    { temppos = ((transform.up * -count * 4)) - (transform.forward * 5 * count); }
                    else { temppos = ((transform.up * count * 4)) - (transform.forward * 5 * count); }


                }
            }
            count++;
        }
    }

    public void AddSquadMember(GameObject newsquadmember)
    {
      if(newsquadmember.GetComponent<AIsquadunit>() != null)
      {squad.Add(newsquadmember.GetComponent<AIsquadunit>());}
      newsquadmember.GetComponent<Enemy>().Conscript(this.gameObject);
      newsquadmember.GetComponent<AIsquadunit>().GetOrder((transform.forward * 15) * -(squad.Count + 1) ,this.transform);
    }

    public void CommandSquad2(GameObject target)
    {
      Vector3 temppos = (transform.forward * -15);
      int count = 0;
      if(count < squad.Count)
      {
        squad[count].GetOrder(temppos,target.transform);
         temppos = (transform.up * 15);
        count++;
      }
      if(count < squad.Count)
      {
        squad[count].GetOrder(temppos,target.transform);
          temppos = (transform.right * -15);
        count++;
      }
      if(count < squad.Count)
      {
        squad[count].GetOrder(temppos,target.transform);
      temppos = (transform.right * -15);
        count++;
      }


    }
    public void CommandOneUnit(GameObject target,Vector3 newpos,  int whichunit,int newplan) 
    {
        if (squad.Count > whichunit)
        {
            squad[whichunit].GetOrder( newpos,target.transform, newplan);
        }

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
                ZigZag(target);
                break;
        default:
          GetBehind(target);
        break;
      }
          //TODO: relative commands based on current attack pattern
        // CommandSquad(target);
    }

    public void Attack(GameObject target)
    {

            if (target != null)
            {

                  AttackPlans(target);

                if (gunCooldown <= -3.0f)
                {
                    CalculateNextMove(target);
                    //check to find conscriptable enemies
                    myEnemy.FindSquadMembers();
                    gunCooldown = 0;
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

        if (myEnemy.UseStamina(myEnemy.engineStaminaCost * Time.deltaTime) == true)
        {
            rb.AddForce((tempTargetSpot - transform.position).normalized * speed * Time.deltaTime, ForceMode.Impulse);

        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        if (myEnemy.DistanceToTarget(targetship) < closedistance || targetship.GetComponent<PlayerControls>().playerStats.GetStamina() < 2)
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
        bool targetinfront = Vector3.Distance((transform.position + transform.forward), targetship.transform.position) < Vector3.Distance((transform.position - transform.forward), targetship.transform.position);
        bool targetfacingme = Vector3.Distance((targetship.transform.position + targetship.transform.forward), transform.position) < Vector3.Distance((targetship.transform.position - targetship.transform.forward), transform.position);
        float targetStamina = targetship.GetComponent<PlayerControls>().playerStats.GetStamina();


        if (targetfacingme == true)
        {
            if (targetStamina > 2)
            {
                tempTargetSpot = transform.position + (transform.right * 5);
                currentAttackPlan = 3; // zigzag
                CommandSquad(targetship);
                CommandOneUnit(targetship, (transform.forward * 10),(int)Random.Range(0,squad.Count),2);
            }
            else if (myEnemy.stamina > gunCost * 3)
            {
                //pick a spot to the side of the target
                tempTargetSpot = targetship.transform.position + (transform.right * closedistance);
                currentAttackPlan = 2; // strafe run
                CommandSquadWithPlan(targetship,4);
                CommandOneUnit(targetship, (transform.forward * 10), (int)Random.Range(0, squad.Count), 5);
            }
            else
            {
                currentAttackPlan = 0; // chicken
                CommandSquadWithPlan(targetship,3);
                CommandOneUnit(targetship, (transform.forward * 10), (int)Random.Range(0, squad.Count), 5);
            }
        }
        else
        {
            if (targetinfront == true)
            {
                //target In Front

                currentAttackPlan = 1; // getbehind
                CommandSquadWithPlan(targetship, 4);
                CommandOneUnit(targetship, (transform.forward * 10), (int)Random.Range(0, squad.Count), 3);
            }
            else
            {
                currentAttackPlan = 5;//recover 
                CommandSquad(targetship);
                CommandOneUnit(targetship, (transform.forward * 10), (int)Random.Range(0, squad.Count), 3);
            }






        }
        //if (targetinfront == true )
        //  {
        //    //target In Front
        //      if(targetfacingme == true )
        //      {
        //        currentAttackPlan = 0; // chicken
        //        CommandSquad(targetship);
        //    }

        //      else
        //      {
        //        currentAttackPlan = 1; // chase / get behind
        //        CommandSquad(targetship);
        //    }


        //  }
        //  else
        //  { //target behind
        //          if(targetfacingme == true )
        //          {
        //              currentAttackPlan = 2;
        //        CommandSquad(targetship);
        //    }

        //          else
        //          {
        //            currentAttackPlan = 1;
        //        CommandSquad(targetship);
        //    }
        //  }
            

        //    transform.GetChild(0).GetComponent<Renderer>().material = colors[currentAttackPlan + 1];
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
            }else{myEnemy.CheckToNoticePlayer(); }



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
