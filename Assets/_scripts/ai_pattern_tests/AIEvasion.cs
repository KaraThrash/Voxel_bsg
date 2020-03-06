using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEvasion : MonoBehaviour
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
  public List<Material> colors;
  private Vector3 strafeDirection,tempTargetSpot,dir;
  private Quaternion targetRotation;
  private Rigidbody rb;
  private Enemy myEnemy;
  // Use this for initialization
  void Awake () {

      rb = GetComponent<Rigidbody>();
      myEnemy = GetComponent<Enemy>();

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
              myEnemy.CheckToNoticePlayer();
         }



      }
      // if (gunCooldown > 0)
      // { gunCooldown -= Time.deltaTime; }
       gunCooldown -= Time.deltaTime;

}


public void Attack(GameObject target)
{
  targetRotation = Quaternion.LookRotation(target.transform.position - transform.position );
        if (Vector3.Distance(target.transform.position,transform.position) > fardistance)
        {
            rb.AddForce(transform.forward  * speed  *  Time.deltaTime,ForceMode.Impulse);
              // dir = Vector3.zero;
        }
        else
        {
          if (Vector3.Distance(target.transform.position,transform.position) > closedistance)
          {
              rb.AddForce(dir * speed  *   Time.deltaTime,ForceMode.Impulse);
          }
          else
          {
              rb.AddForce(dir * speed  *  Time.deltaTime,ForceMode.Impulse);
          }


        }
        if(Input.GetMouseButtonDown(0))
        {
          if(dir == new Vector3(1,0,0) )
          {
            dir = new Vector3(-1,0,0);
          }
          else
          {
            if(dir == new Vector3(0,1,0) )
            {
              dir = new Vector3(0,-1,0);
            }
            else
            {
              dir = new Vector3(1,0,0);
            }
          }
        }

          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

          float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);

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
              else {  }
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
