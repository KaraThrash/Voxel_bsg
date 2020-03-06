﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed,rotSpeed;
    public GameObject explosion,target;
    public bool large;
    public GameObject intialExplosion;
    private Rigidbody rb;
    public float lifeTime;
    public bool lance,missile; //toggles on instead of being a projectile
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (lance == false)
        {
            transform.parent = GameObject.Find("BulletParent").transform;
            rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
        }


       // if (large == true) { Instantiate(intialExplosion, transform.position, transform.rotation); }


    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (lance == false)
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
        if (lifeTime <= 0) { Die(); }
    }

    public void MissileLogic()
    {
          if(target != null)
          {
            rb.AddForce(transform.forward * speed  *  Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position ), rotSpeed * Time.deltaTime);
           }
    }

    public void OnCollisionEnter(Collision col)
    {
        GetComponent<Collider>().enabled = false;
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
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


    }

    public void Die()
    {



        Destroy(this.gameObject);

    }
}
