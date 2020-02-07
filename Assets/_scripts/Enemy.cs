using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public NpcManager npcManager;



    public Vector3 straferunspot; //for doing gun passes on large ships

    public Transform guns;

    public GameObject target,patrolparent,patroltarget;

    public GameObject bullet;
    public GameObject leadDistanceTarget;
    public GameObject explosion;
    public GameObject dradisModel;
    public GameObject myWing;



    public int value = 1;
    public int hp;

    public bool destroyed,canShoot;
    public bool aitest,stationary,alert;

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

      //alert enemies are enemies that are in the general area of play.
      //enemies in area x dont need to active when player is in Y
      if(alert == true)
      {GetComponent<AIattackpattern>().Fly(target);}



    }

    public void FindTarget()
    {
      target = npcManager.GetClosestTarget(transform.position);
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
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

        foreach(Transform go in guns)
        {
          GameObject clone = Instantiate(bullet, go.transform.position, go.transform.rotation);
        }

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






}
