using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed,rotSpeed;
    public GameObject explosion,target;
    public bool large;
    
    public GameObject intialExplosion, childObject;
    
    public float lifetimeMax = 10.0f,lifeTime,impactForce,damage;

    public bool lance,missile,spray,twinLiked,boomerang; //toggles on instead of being a projectile
    public bool ice;

    private Vector3 direction,secondaryDirection;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (lance == false && missile == false)
        {
            transform.parent = GameObject.Find("BulletParent").transform;
           rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
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
            rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
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


    // Update is called once per frame
    void Update()
    {

        lifeTime -= Time.deltaTime;
        if(missile == true)
        {
          MissileLogic();
        }
        if (lifeTime <= 0 || (target != null && transform.position == target.transform.position)) { Die(); }
    }

    public void Spray()
    {
      Vector3 tempvec = transform.position + (transform.forward * 3);
      spray = false;
      GameObject clone = Instantiate(this.gameObject,transform.position + transform.forward,transform.rotation);
      // tempvec = (tempvec * -1);

      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec + transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec - transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec + transform.up + transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec - transform.up - transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      tempvec = transform.position + transform.right + (transform.forward * 3);
      clone = Instantiate(this.gameObject,tempvec ,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec + transform.right,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
    clone = Instantiate(this.gameObject,tempvec + transform.up,transform.rotation);
    clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
    clone = Instantiate(this.gameObject,tempvec - transform.up,transform.rotation);
    clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      tempvec = transform.position - transform.right + (transform.forward * 3);
      clone = Instantiate(this.gameObject,tempvec ,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec - transform.right ,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec + transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);
      clone = Instantiate(this.gameObject,tempvec - transform.up,transform.rotation);
      clone.GetComponent<Rigidbody>().AddForce((clone.transform.position - transform.position ) * (speed), ForceMode.Impulse);

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
