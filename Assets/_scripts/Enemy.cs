using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public NpcManager npcManager;
    public GameObject patrolparent;
    public int speed;
    public int rotForce = 6;

    public Vector3 straferunspot; //for doing gun passes on large ships
    public GameObject shipTarget;
    public GameObject patroltarget;

    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCooldown;


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
    public int value = 1;
    public bool aitest,stationary;

    private Quaternion targetRotation;
    private Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
        ResetToNeutral();

    }
    public void ResetToNeutral()
    {
      if(rb == null){rb = GetComponent<Rigidbody>();}
      if(hp <= 0){hp = 1;}

      if(patrolparent == null){  GameObject.Find("PatrolPoints");}
      if(patrolparent != null){  patroltarget = patrolparent.transform.GetChild(Random.Range(0,patrolparent.transform.childCount)).gameObject;  }
    }
    // Update is called once per frame
    void Update()
    {

        GetComponent<AIattackpattern>().Fly();


    }

    public void CheckForward()
    {
        // possible issue with dradis detection
        RaycastHit hit;



        if (Physics.Raycast(transform.position, transform.forward, out hit, 310.0f))
        {

            if (hit.transform.gameObject == GetComponent<AIattackpattern>().target)
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
        //if (straferunspot != Vector3.zero)
        //{
        //    transform.Rotate(Vector3.right * -60 * Time.deltaTime);
        //   // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.3f * Time.deltaTime);
        //    transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed);

        //}
        //else
        //{


        //    // transform.position += (transform.forward * Time.deltaTime * speed);
        //    transform.Rotate(Vector3.right * -30 * Time.deltaTime);
        //}

        rb.drag = 2.0f;
        transform.Rotate(Vector3.right * -30 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, (transform.position + transform.forward), speed  * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
            hp -= 1;
            if (hp <= 0)
            {
                Die();
            }
        }


    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shippart")
        {
            other.gameObject.GetComponent<ShipPart>().TakeDamage(5);
            Instantiate(explosion, transform.position, transform.rotation);
            // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);

            Die();

        }
        if (other.gameObject.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
            hp -= 1;
            if (hp <= 0)
            {
                Die();
            }
        }
    }
    public void FireGuns()
    {
        Instantiate(bullet, gun1.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun2.transform.position, gun1.transform.rotation);

    }
    public void Die()
    {
        if (npcManager != null)
        {
            npcManager.NPCkilled(value);
        }
        //TODO: have npc manager disable them instead of spawning new ones all the time
        Destroy(this.gameObject);
    }
    public void Attack()
    {
        if (aitest == true)
        {

            GetComponent<AIattackpattern>().target = npcManager.GetClosestTarget(transform.position);

            if (GetComponent<AIattackpattern>().target != null)
            {
                GetComponent<AIattackpattern>().GetFarAndComeBack(GetComponent<AIattackpattern>().target);
                float angle = Vector3.Angle(GetComponent<AIattackpattern>().target.transform.position - transform.position, transform.forward);

                if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }
                if (gunCooldown <= 0 && canShoot == true)
                {

                    FireGuns();
                    gunCooldown = gunCost + Random.Range(0, 3.0f);

                }
            }
        }
        else
        {
            //gunCooldown -= Time.deltaTime;

            targetRotation = Quaternion.LookRotation(GetComponent<AIattackpattern>().target.transform.position - transform.position);

            float angle = Vector3.Angle(GetComponent<AIattackpattern>().target.transform.position - transform.position, transform.forward);
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
            if (Vector3.Distance(transform.position,GetComponent<AIattackpattern>().target.transform.position) > 50)
            {
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            }

            if (Vector3.Distance(transform.position, GetComponent<AIattackpattern>().target.transform.position) < 10)
            {
                overShootCoolDown = 2.0f;
            }

            if (Vector3.Distance(transform.position, GetComponent<AIattackpattern>().target.transform.position) > 1440)
            {
                transform.position = Vector3.MoveTowards(transform.position, GetComponent<AIattackpattern>().target.transform.position, speed * Time.deltaTime);

            }

        }
        //if (Vector3.Distance(transform.position, mydradis.GetComponent<Dradis>().target.transform.position) < 150 &&  gunCooldown > 0.1f)
        //{
        //    Debug.Log("avoiding collision");

        //    avoidCollisionClock = 28.0f;
        //    Vector3 tempVector3 = mydradis.GetComponent<Dradis>().target.transform.position;
        //    //straferunspot = new Vector3(transform.position.x + (transform.position.x - tempVector3.x * 1.2f), transform.position.y + (transform.position.y - tempVector3.y * 1.2f), transform.position.x + (transform.position.x - tempVector3.x * 1.2f));
        //    straferunspot = ( tempVector3 - transform.position) * 1.2f;
        //    targetRotation = Quaternion.LookRotation(transform.position - straferunspot);
        //    Instantiate(explosion, straferunspot, transform.rotation);
        //}
    }
    public void Patrol()
    {

        if (Vector3.Distance(patroltarget.transform.position, transform.position) < 10)
        { patroltarget = patrolparent.transform.GetChild(Random.Range(0, 5)).gameObject; }


        targetRotation = Quaternion.LookRotation(patroltarget.transform.position - transform.position);

        float angle = Vector3.Angle(patroltarget.transform.position - transform.position, transform.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

            // rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, patroltarget.transform.position, speed  * Time.deltaTime);




    }




}
