using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDinosaur : MonoBehaviour
{
  // move to spot above/below Target
  // face target spot until a raycast at the target suceeds, then face target

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
    // Start is called before the first frame update
    void Start()
    {


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

        if(currentAttackPlan == 0)
        {
          if(Vector3.Distance(target.transform.position,transform.position) < fardistance)
          {
            currentAttackPlan = 1;
          }
        }
        else if(currentAttackPlan == 1)
        {
          if(Vector3.Distance(target.transform.position,transform.position) > (fardistance * 2))
          {
            currentAttackPlan = 0;
          }
        }
        if (target != null)
        {

          if(currentAttackPlan == 0)
            {
              if(avoidCollisionClock <= 0){
                      Approach(target);
                }
                else
                {
                  AvoidCollision();
                }


            }
              else if(currentAttackPlan == 1)
                {TryToAttack(target);}
                else  {TryToAttack(target);}




        }


    }
    //move towards the target's area, to approach from above
    public void Approach(GameObject target)
    {
      targetRotation = Quaternion.LookRotation((target.transform.position + (target.transform.up * fardistance)) - transform.position );
      //not impulse, momentuem based
      rb.AddForce(transform.forward * speed  *  Time.deltaTime,ForceMode.Impulse);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
    }

    //when in range try and get above the target, if from that vantage it cant see, it tries to get below
    public void TryToAttack(GameObject target)
    {
            targetRotation = Quaternion.LookRotation(target.transform.position  - transform.position );
            if(RayCastAt(target,fardistance + 5) == true)
            {
              if (gunCooldown <= 0 )
              {

                  myEnemy.FireGuns();
                  gunCooldown = gunCost + Random.Range(0, 3.0f);

                  // CalculateNextMove(target);
              }
            }
            else{
                targetRotation = Quaternion.LookRotation((target.transform.position - (target.transform.up * fardistance)) - transform.position );
                  if(RayCastAt(((transform.forward + transform.right)), 15) == true){  rb.AddForce((transform.up + transform.right) * -speed  *  Time.deltaTime,ForceMode.Impulse);}
                  else{    rb.AddForce((transform.forward + transform.right) * speed  *  Time.deltaTime,ForceMode.Impulse);}


            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
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
