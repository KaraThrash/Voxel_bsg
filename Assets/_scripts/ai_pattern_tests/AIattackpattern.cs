using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIattackpattern : MonoBehaviour {
    public GameObject target;
    public GameObject patroltarget;
    public GameObject bullet;

    public Transform guns;

    public float accuracy;
    public float gunCooldown;
    public float gunCost;
    public float closedistance;
    public float fardistance;

    public int speed;
    public int rotForce = 6;
    public bool canShoot,stationary;
    public bool flyaway;
    public bool flypast;

    private Quaternion targetRotation;
    private Rigidbody rb;
    private Enemy myEnemy;
    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
        myEnemy = GetComponent<Enemy>();
    }

	// Update is called once per frame
	void Update () {

	}

  public void Fly()
  {
    if (stationary == false)
    {

        if (target != null)
        {

          Attack();
        }
        else
        {
          Patrol();
          if (gunCooldown <= 0)
          {
              gunCooldown = gunCost * 10;
                FindTarget();
           }



        }
        if (gunCooldown > 0)
        { gunCooldown -= Time.deltaTime; }

    }
  }
  public void FireGuns()
  {
    foreach(Transform go in guns)
    {
      GameObject clone = Instantiate(bullet, go.transform.position, go.transform.rotation);
    }

  }
  public void Attack()
  {


          if (target != null)
          {
              GetFarAndComeBack(target);
              float angle = Vector3.Angle(target.transform.position - transform.position, transform.forward);

              if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
              if (gunCooldown <= 0 && canShoot == true)
              {

                  FireGuns();
                  gunCooldown = gunCost + Random.Range(0, 3.0f);

              }
          }


  }



  public void FindTarget()
  {
    target = GetComponent<Enemy>().npcManager.GetClosestTarget(transform.position);
  }


    public void GetFarAndComeBack(GameObject targetship)
    {

        //gunCooldown -= Time.deltaTime;

       // targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);

        if (flyaway == true)
        {

            if (flypast == true)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > closedistance)
                {

                    flypast = false;

                }
            }
            else
            {
                targetRotation = Quaternion.LookRotation(transform.position - target.transform.position);

            }



            if (Vector3.Distance(transform.position, target.transform.position) > fardistance)
            {
                flyaway = false;
                flypast = false;

            }

        }
        else
        {
           targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) < closedistance)
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

      if(patroltarget != null){
        if (Vector3.Distance(patroltarget.transform.position, transform.position) < 10)
        { patroltarget = myEnemy.patrolparent.transform.GetChild(Random.Range(0, 5)).gameObject; }


        targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);

        float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

            // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, speed  * Time.deltaTime);
          }else{FindTarget(); }



    }

}
