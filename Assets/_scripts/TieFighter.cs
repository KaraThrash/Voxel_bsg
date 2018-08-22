using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieFighter : MonoBehaviour {

    public GameObject fwdObject;
    public int speed;
    public int rotForce = 6;


    public GameObject shipTarget;
    private Quaternion targetRotation;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCooldown;
    private Rigidbody rb;

    public string patrolPointType;
    public bool canPatrol;
    public GameObject explosion;
    public GameObject dradisModel;
    public float avoidCollisionClock;
    public bool canShoot;
    public GameObject myWing;
    public bool destroyed;
    public int hp;
    public int accuracy; //difference of angle that the raider can start firing 0 being perfect
    public float gunCost; //what to set the cooldown to after firing
    public GameObject leadDistanceTarget;
    public float overShootCoolDown;
    public bool tieFighter;
    // Use this for initialization
    void Start()
    {
        hp = 1;
        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        if (overShootCoolDown > 0)
        {
            overShootCoolDown -= Time.deltaTime;
        }
        // CheckForward();
        //leadDistanceTarget.transform.localPosition = new Vector3(0, 0, (rb.velocity.magnitude * 0.2f));
        if (shipTarget != null) { Attack(); }
        if (avoidCollisionClock <= 0f)
        {
        }
        else { AvoidCollision(); }

    }

    public void CheckForward()
    {
        // possible issue with dradis detection
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 110.0f))
        {
            if (hit.transform.gameObject == shipTarget)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
                if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; } }
            }

        }
        else { avoidCollisionClock -= 0.1f; canShoot = true; }
    }
    public void AvoidCollision()
    {
        transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed * Time.deltaTime);
        // transform.position += (transform.forward * Time.deltaTime * speed);

        
    }
    public void OnCollisionEnter(Collision col)
    {
        // if (col.gameObject.tag == "Bullet")
        // {
        //Instantiate(explosion, transform.position, transform.rotation);
        //TakeDamage(1);

        //  }
    }
    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.transform.name);
        //TODO: fix border to just be an actual border instead of full sphere
        // if (other.tag == "Viper") { shipTarget = other.transform.parent.gameObject;  }
        // if (other.tag == "Fleetship") { if (shipTarget == null) { shipTarget = other.transform.parent.gameObject; } }
    }
    public void FireGuns()
    {
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun2.transform.position, gun1.transform.rotation);
        //GameObject clone = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
        //clone.GetComponent<Rigidbody>().velocity += rb.velocity;
        //GameObject clone1 = Instantiate(bullet, gun1.transform.position, gun2.transform.rotation) as GameObject;
        //clone1.GetComponent<Rigidbody>().velocity += rb.velocity;
    }

    public void Attack()
    {


        gunCooldown -= Time.deltaTime;
      
        targetRotation = Quaternion.LookRotation(shipTarget.transform.position - transform.position);
        
        float angle = Vector3.Angle(shipTarget.transform.position - transform.position, transform.forward);
        if (overShootCoolDown <= 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        }
        if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
        if (gunCooldown <= 0 && canShoot == true)
        {
         
            FireGuns();
            gunCooldown = gunCost + Random.Range(0, 3.0f);

        }

        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse); 


        if (Vector3.Distance(transform.position, shipTarget.transform.position) < 10)
        {
            overShootCoolDown = 2.0f;
        }

        if (Vector3.Distance(transform.position, shipTarget.transform.position) > 1440)
        {
            transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, speed);

        }


    }
    public void Patrol()
    {


            transform.position = Vector3.MoveTowards(transform.position, transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation,transform.rotation, speed * Time.deltaTime);
        


    }

    // 
    public void Die(int byWho)
    {
        //myWing.GetComponent<FighterWing>().roundManager.GetComponent<RoundManager>().CylonKilled(1, byWho);
        //GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);

        // myWing.GetComponent<PhotonView>().RPC("ShipDestroyed", PhotonTargets.AllViaServer, myNumber);
        // myWing.GetComponent<FighterWing>().ShipDestroyed(myNumber,byWho);

    }

    //


}
