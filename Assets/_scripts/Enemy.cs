using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public NpcManager npcManager;



    public Vector3 straferunspot; //for doing gun passes on large ships

    public Transform guns;

    public GameObject mapArea,target,patrolparent,patroltarget;

    public GameObject bullet;
    public GameObject leadDistanceTarget;
    public GameObject explosion;
    public GameObject dradisModel;
    public GameObject myWing;



    public int value = 1;
    public int hp;
    public float speed = 20;
    public float rotForce = 6;
    public float leashDistance;
    public bool destroyed,canShoot,returnHome;
    public bool aitest,stationary,alert,inCombat;

    private Vector3 startPos;
    private Quaternion targetRotation,startRot;
    private Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
      if(rb == null){rb = GetComponent<Rigidbody>();}
        ResetToNeutral(npcManager);

    }
    public void ResetToNeutral(NpcManager npcManage)
    {
      npcManager = npcManage;
      startPos = transform.position;
      startRot = transform.rotation;
      if(rb == null){rb = GetComponent<Rigidbody>();}
      if(hp <= 0){hp = 1;}
      alert = false;
      if(patrolparent == null){  GameObject.Find("PatrolPoints");}
      if(patrolparent != null){  patroltarget = patrolparent.transform.GetChild(Random.Range(0,patrolparent.transform.childCount)).gameObject;  }
    }
    public void SetAlert(bool isAlert)
    {
      alert = isAlert;
      //TODO: return to rest location // leash
      if(alert == false){returnHome = true;}else{returnHome = false;}
    }
    // Update is called once per frame
    void Update()
    {

      //alert enemies are enemies that are in the general area of play.
      //enemies in area x dont need to active when player is in Y
      if(alert == true)
      {

        AlertActions();



      }
      else{


        //TODO: have enemies leash back to their start Position
        if(returnHome == true && inCombat == false)
        {ReturnHome();}else
        {
          AlertActions();
          if(Vector3.Distance(target.transform.position,transform.position) > leashDistance)
          {
            target = null; inCombat = false; alert = false;
          }
        }

      }



    }
    public void AlertActions()
    {
      if(target != null){inCombat = true;}
      if(GetComponent<AIattackpattern>() != null)
      {
        GetComponent<AIattackpattern>().Fly(target);
      }else
      {
        if(GetComponent<AIEvasion>() != null)
        {
          GetComponent<AIEvasion>().Fly(target);
        }
      }
    }
    public void ReturnHome()
    {
        //TODO: have enemies leash back to their start Position

          if(Vector3.Distance(transform.position,startPos) < 2.0f){
              transform.rotation = Quaternion.Slerp(transform.rotation, startRot, rotForce  * Time.deltaTime);
              transform.position = Vector3.MoveTowards(transform.position,startPos,speed * Time.deltaTime);

            if(Vector3.Distance(transform.position,startPos) < 0.2f){
              transform.position = startPos;

              transform.rotation = startRot;
              rb.isKinematic = true;
              rb.isKinematic = false;
              returnHome = false;
            }

          }else
          {
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
            targetRotation = Quaternion.LookRotation( startPos - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
            if(Vector3.Distance(transform.position,startPos) <= 2.0f){
              transform.rotation = startRot;
              rb.velocity = Vector3.zero;
            }
          }
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
            if(hp > 0){
                  hp -= 1;
                  if (hp <= 0)
                  {
                      Die();
                  }
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
            if(hp > 0){
                Instantiate(explosion, transform.position, transform.rotation);
                // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
                hp -= 1;
                if (hp <= 0)
                {
                    Die();
                }
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
            npcManager.NPCkilled(GetComponent<Enemy>());
        }
        //TODO: have npc manager disable them instead of spawning new ones all the time
        // Destroy(this.gameObject);
    }






}
