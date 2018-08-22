using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public GameObject resource;
    public int size;
    public int hp;
    private Vector3 floatdirection;
	// Use this for initialization
	void Start () {
        floatdirection = Vector3.Normalize(-transform.position - transform.position) * 5.0f;

    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = floatdirection * 10.0f;
        if (Vector3.Distance(transform.position, Vector3.zero) > 5000)
        { Destroy(this.gameObject); }

    }

    public void BreakApart()
    {
        if (size > 1)
        {
            
            GameObject clone = Instantiate(this.gameObject, transform.position + transform.localScale, transform.rotation) as GameObject;
            clone.GetComponent<Asteroid>().size = size - 1;
            clone.GetComponent<Asteroid>().hp = size - 1;
            clone.transform.localScale = new Vector3(transform.localScale.x - 5, transform.localScale.y - 5, transform.localScale.z - 5);
            int rnd = Random.Range(0,3);
            if (rnd == 1)
            { Instantiate(resource, transform.position - (transform.localScale * .2f), transform.rotation); }

            }
        size--;
        if (size <= 0) {
            Destroy(this.gameObject);

        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - 5, transform.localScale.y - 5, transform.localScale.z - 5);
            hp = size;
        }
    }


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet" || col.gameObject.tag == "BulletEnemy")
        {
            hp--;
            if (hp <= 0)
            { BreakApart(); }

        }
        else {
            Vector3 tempvector = Vector3.Normalize( transform.position - col.transform.position);

            floatdirection += ((tempvector * col.gameObject.GetComponent<Rigidbody>().mass) * 0.9f);

        }
    }

}
