using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public GameObject explosion;
    public bool large;
    public GameObject intialExplosion;
    private Rigidbody rb;
    public float lifeTime;
    // Use this for initialization
    void Start()
    {
        transform.parent = GameObject.Find("BulletParent").transform;
        rb = GetComponent<Rigidbody>();
       // if (large == true) { Instantiate(intialExplosion, transform.position, transform.rotation); }
        rb.AddForce(transform.forward * (speed), ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) { Die(); }
    }
    public void OnCollisionEnter(Collision col)
    {
        GetComponent<Collider>().enabled = false;
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        lifeTime = 0.5f;
      

    }
    public void Die()
    {



        Destroy(this.gameObject);

    }
}
