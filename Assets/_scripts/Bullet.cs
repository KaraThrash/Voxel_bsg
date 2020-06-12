using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed,rotSpeed;
    public GameObject explosion,target;
    public bool large;
    public GameObject intialExplosion;
    private Rigidbody rb;
    public float lifetimeMax = 10.0f,lifeTime,impactForce,damage;

    public bool lance,missile,spray,twinLiked,boomerang; //toggles on instead of being a projectile
    public bool ice;
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

        if(boomerang == true)
        {
          rb.velocity = transform.forward * speed  *  Time.deltaTime;
          if(lifeTime < (lifetimeMax * 0.75f))
          {
            if(target != null)
            {
              // rb.AddForce(transform.forward * speed  *  Time.deltaTime);

              transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotSpeed * Time.deltaTime);
             }
          }
          transform.GetChild(0).Rotate(transform.forward * rotSpeed * 10 * Time.deltaTime);
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
        if (explosion != null)
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
