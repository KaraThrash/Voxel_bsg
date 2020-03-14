using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDradis : MonoBehaviour
{
  public int currentAttackPlan;

  public float speed = 50,walkspeed = 10;
  public float rotForce = 6;
  public float accuracy = 2;//time to confirm it found something

  public float gunCost = 1;
  public float checkForwardDistance = 100.0f;

  public float gunCooldown,holdOnTargetTimer;//hold dradis contact to confirm seeing the player
  public bool destroyed,foundPlayer,maybeFoundPlayer;

  public GameObject dish;
  public Transform vertRotObj,hortRotObj;
  public Material avoidingCollisionColor,patrolColor;
  public Renderer myRenderer;
  public List<Material> colors;

  private Quaternion targetRotation;
  private Rigidbody rb;
  private Enemy myEnemy;
    // Start is called before the first frame update
    void Start()
    {


        rb = GetComponent<Rigidbody>();
        myEnemy = GetComponent<Enemy>();

        if(colors.Count > 0 && myRenderer != null)
        {  myRenderer.material = colors[0];}


    }

    //primary function of actions for an enemy
    public void Fly(GameObject target)
    {


          if (foundPlayer == true)
          {

            if (gunCooldown <= 0)
            {
                foundPlayer = false;
                gunCooldown = gunCost;
                if(colors.Count > 0 && myRenderer != null)
                {  myRenderer.material = colors[0];}
             }

          }
          else if (maybeFoundPlayer == true)
            {
              if(RayCastAt(myEnemy.npcManager.GetPlayerShip().transform.position,checkForwardDistance) == true)
              {
                holdOnTargetTimer += Time.deltaTime;
              }
              if (holdOnTargetTimer >= accuracy)
              {
                Scan();

               }
              if (gunCooldown <= 0)
              {
                  maybeFoundPlayer = false;

               }

            }
          else
          {
            hortRotObj.Rotate(0,rotForce * Time.deltaTime,0);
            if (gunCooldown <= 0)
            {
                gunCooldown = gunCost ;

                Scan();


             }



          }
          // if (gunCooldown > 0)
          // { gunCooldown -= Time.deltaTime; }
           gunCooldown -= Time.deltaTime;

    }
    public void Scan( )
    {
      //check if player is in the forward cone and in range
      if(checkForwardDistance > myEnemy.npcManager.GetDistanceToPlayer(transform.position) && myEnemy.npcManager.GetDistanceToPlayer(dish.transform.position) < myEnemy.npcManager.GetDistanceToPlayer(transform.position))
      {
        //dradis is blocked by other objects
              if(RayCastAt(myEnemy.npcManager.GetPlayerShip().transform.position,checkForwardDistance) == true)
              {    }
                        if (maybeFoundPlayer == true)
                        {
                        myEnemy.npcManager.AlertEnemiesInRange(this.gameObject,checkForwardDistance);
                            maybeFoundPlayer = false;
                            foundPlayer = true;
                            gunCooldown = gunCost * 100;
                            holdOnTargetTimer = 0;
                            if(colors.Count > 2 && myRenderer != null)
                            {
                              myRenderer.material = colors[2];
                            }

                        }else
                        {
                          if(colors.Count > 2 && myRenderer != null)
                          {
                            myRenderer.material = colors[1];
                          }
                          holdOnTargetTimer = 0;
                          gunCooldown = accuracy * 1.5f;
                          maybeFoundPlayer = true;
                            foundPlayer = false;
                        }



      }else
      {//scan to confirm finding the player, so there is that tense moment when you have almost been spotted
            if (maybeFoundPlayer == true)
            {
                maybeFoundPlayer = false;

                if(colors.Count > 0 && myRenderer != null)
                {
                  myRenderer.material = colors[0];
                }

            }
      }



    }


    public bool RayCastAt(GameObject target,float rng)
    {

      RaycastHit hit;



      if (Physics.Raycast(dish.transform.position, (target.transform.position - dish.transform.position), out hit, rng))
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



        }
        else {  }
    }




}
