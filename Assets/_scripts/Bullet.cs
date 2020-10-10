using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed,rotSpeed;
    public GameObject explosion,target;
    public bool large;
    
    public GameObject intialExplosion, childObject;
    
    public float lifetimeMax = 10.0f,lifeTime,impactForce,damage;

    public float timeIncrement = 1.0f,incrementTimer = 1.0f, spiralRange = 40.0f;
    private int toggleValue = 1;
    public bool lance,missile,spray,twinLiked,boomerang,spiral; //toggles on instead of being a projectile
    public bool ice;

    private Vector3 direction,secondaryDirection, relativeVelocity;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (lance == false && missile == false)
        {
            transform.parent = GameObject.Find("BulletParent").transform;
           //rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
        }
         if (spray == true)
        {
          spray = false;
          Spray();
        }
        if (twinLiked == true)
       {
         TwinLink();
       }

        // if (large == true) { Instantiate(intialExplosion, transform.position, transform.rotation); }

        if (childObject == null && transform.childCount > 0) { childObject = transform.GetChild(0).gameObject; }

    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (lance == false && missile == false)
        {
            transform.parent = GameObject.Find("BulletParent").transform;
            rb.velocity = (transform.forward * speed);
        }


       // if (large == true) { Instantiate(intialExplosion, transform.position, transform.rotation); }


    }


    public void Launched(GameObject newtarget=null)
    {
        if (newtarget) { target = newtarget; }

        if (boomerang == true)
        {
            direction = (target.transform.forward + target.transform.right).normalized;
            secondaryDirection = -target.transform.right;
        }
        

    }
    public void SetRelativeVelocity(Vector3 newvel)
    { relativeVelocity = newvel; }

    // Update is called once per frame
    void Update()
    {

        lifeTime -= Time.deltaTime;
        if (missile == true)
        {
            MissileLogic();
        }
        else if (spiral == true)
        {
            Spiral();
            rb.velocity = (transform.forward * speed) + relativeVelocity;
        }
        else 
        {
            rb.velocity = (transform.forward * speed) + relativeVelocity;
        }
        if (lifeTime <= 0 || (target != null && transform.position == target.transform.position)) { Die(); }
    }

    public void Spray()
    {
      Vector3 tempvec = transform.position + (transform.forward * 5);
      spray = false;

        //SpawnBullet(tempvec, this.gameObject);

        SpawnBullet(tempvec + transform.up, this.gameObject);
        SpawnBullet(tempvec - transform.up, this.gameObject);
        SpawnBullet(tempvec + transform.right, this.gameObject);
        SpawnBullet(tempvec - transform.right, this.gameObject);

        tempvec = transform.position + (transform.forward * 3);

        SpawnBullet(tempvec + transform.up + transform.right, this.gameObject);
        SpawnBullet(tempvec - transform.up + transform.right, this.gameObject);

        SpawnBullet(tempvec + transform.up - transform.right, this.gameObject);
        SpawnBullet(tempvec - transform.up - transform.right, this.gameObject);

        SpawnBullet(tempvec + transform.up + transform.up, this.gameObject);
        SpawnBullet(tempvec - transform.up - transform.up, this.gameObject);
        SpawnBullet(tempvec + transform.right + transform.right, this.gameObject);
        SpawnBullet(tempvec - transform.right - transform.right, this.gameObject);


    }


    public GameObject SpawnBullet(Vector3 spawnLoc, GameObject toSpawn)
    {

        GameObject clone = Instantiate(toSpawn, spawnLoc, transform.rotation);
        Vector3 direction = clone.transform.position - transform.position;
        clone.transform.rotation = Quaternion.LookRotation(direction);
        return clone;

    }

    public void Spiral()
    {
        float inc =  Time.deltaTime;
        incrementTimer -= inc;
        transform.Rotate(transform.right * inc * toggleValue);
        transform.Rotate(transform.up * inc * toggleValue * spiralRange);
        if (incrementTimer <= 0)
        {
            incrementTimer = timeIncrement;

            toggleValue *= -1;
        }
    }


    public void TwinLink()
    {

    }


    public void MissileLogic()
    {


        //boomerang has it target set to the player that fired it.
        if(boomerang == true)
        {
            //rb.velocity = transform.forward * speed  *  Time.deltaTime;
            //first phase of it's life move away from from the firing object, second phase to what was the alternate side from the firing position
            //last phase return to the firing object
            if (lifeTime > (lifetimeMax * 0.75f))
            {
                rb.velocity = direction * speed ;
                if (target != null)
                {
                    // rb.AddForce(transform.forward * speed  *  Time.deltaTime);

                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotSpeed * Time.deltaTime);
                }
            }
            else if (lifeTime > (lifetimeMax * 0.45f))
            {
                rb.velocity = Vector3.Slerp(rb.velocity, secondaryDirection * speed, speed * Time.deltaTime);
                if (target != null)
                {
                    // rb.AddForce(transform.forward * speed  *  Time.deltaTime);

                    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, rotSpeed * Time.deltaTime);
                }
            }
            else 
            {
                rb.velocity = Vector3.Slerp(rb.velocity, (target.transform.position - transform.position).normalized * speed, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.transform.position) < 1) { lifeTime = 0; }
            }
              childObject.transform.Rotate(0,rotSpeed * 10 * Time.deltaTime,0);
        }
        else
        {
          rb.velocity = transform.forward * speed  *  Time.deltaTime;
          if(target != null)
          {
            // rb.AddForce(transform.forward * speed  *  Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotSpeed * Time.deltaTime);
           }
        }


    }

    public void OnCollisionEnter(Collision col)
    {
        GetComponent<Collider>().enabled = false;
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if(col.gameObject.GetComponent<Enemy>() != null)
        {HandleEnemyImpact(col.gameObject);}
          lifeTime = 0.1f;

    }
    public void OnTriggerEnter(Collider col)
    {
        //GetComponent<Collider>().enabled = false;
        if (explosion != null && boomerang == false)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        //lifeTime = 0.5f;

        if(col.gameObject.GetComponent<Enemy>() != null)
        {HandleEnemyImpact(col.gameObject);}
    }

    public void HandleEnemyImpact(GameObject col)
    {
      // col.GetComponent<Enemy>().HitByBullet(this.gameObject);
    }

    public void Die()
    {



        Destroy(this.gameObject);

    }
}
