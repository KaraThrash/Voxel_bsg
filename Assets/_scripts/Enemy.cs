using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public NpcManager npcManager;

    public Transform mapArea,target,patrolparent,patroltarget;

    public float hp;

    public float leashDistance, noticePlayerDistance = 50.0f;

    public bool alert;

    void Start()
    {


    }
    void Awake()
    {


    }


    public void SetAlert(bool isAlert)
    {
      alert = isAlert;
      //TODO: return to rest location // leash
    }


    public void Conscript( )
    {
      //conscriptable units can join a group and function with group logic

    }

    // Update is called once per frame
    void Update()
    {

       

    }



    public void ReturnHome()
    {


       //  CheckForward();
  
    }


    public void FindTarget()
    {



    }


    public void CheckToNoticePlayer(float distanceToCheck=100.0f)
    {
      //compare distance from in front to behind the enemy to determine if the player is in a forward cone of vision
      //or if the player is extremely close

          RaycastHit hit;



          if (Physics.Raycast(transform.position, (transform.position - transform.position), out hit, distanceToCheck) )
          {
                  
          }



    }


    public void FindSquadMembers()
    {
 
    }

    public void OnCollisionEnter(Collision col)
    {
       

    }
    public void OnTriggerStay(Collider other)
    {
       
    }


    public void OnTriggerEnter(Collider other)
    {
       
    }
    public void FireGuns()
    {
       
    }
    public void CheckForward()
    {
        // possible issue with dradis detection
        RaycastHit hit;



        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f))
        {



              


        }
        
    }



    public void Die()
    {

        if (npcManager != null)
        {
           // npcManager.NPCkilled(GetComponent<Enemy>());
        }

    }

  

    public float DistanceToTarget(GameObject target)
    { return Vector3.Distance(transform.position, target.transform.position); }

    public float DistanceToTarget(Transform target)
    { return Vector3.Distance(transform.position, target.position); }

    public float DistanceToTarget(Vector3 target)
    { return Vector3.Distance(transform.position, target); }


    public bool RaycastAtTarget(Transform currenttarget, float distanceToCheck = 1500.0f)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, currenttarget.position - transform.position, out hit, distanceToCheck))
        {

            if (hit.transform == currenttarget)
            {
                return true;
            }
        }
        return false;

    }




}
