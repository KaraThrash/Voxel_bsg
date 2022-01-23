using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public NpcManager npcManager;
    public Ship ship;
    public bool canAct;
    public AiState state;
    public AiState pendingState;

    public Transform mapArea,target,patrolparent,patroltarget;

    public float hp;

    public float leashDistance, noticePlayerDistance = 50.0f;

    public bool alert;


    public float brainTime;
    private float timer;

    protected float stuckCounter, timeSinceLastAction; // check if stuck

   

    public AiState State() { return state; }
    public void State(AiState _state) 
    {
        OnStateChange(_state);
        state = _state;
    }

    public virtual void OnStateChange(AiState _newstate)
    {
        if (State() == _newstate) { return; } //didnt switch to a new state



        if (State() == AiState.dying) { return; }

        //reset the timer since the state changed: does it need stuck in loop options too?

        timeSinceLastAction = 0;
        stuckCounter = 0;



        if (State() == AiState.spawned)
        {


        }
        else if (State() == AiState.adjusting)
        {

            if (Vector3.Angle(transform.forward, (target.position - transform.position).normalized) < 10 || Vector3.Angle(transform.forward, (target.position - transform.position).normalized) > 80) { }
        }

        if (_newstate == AiState.attacking)
        {
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1,0);

      

        }
        else if (_newstate == AiState.adjusting)
        {
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(0,1);
        }
        else if (_newstate == AiState.recovering)
        {
            timer = brainTime;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
        }




    }















    void Start()
    {
        if (target == null || ship == null) { return; }
        State(AiState.adjusting);
       
    }
    void Awake()
    {


    }


    public void SetAlert(bool isAlert)
    {
      alert = isAlert;
    }


    public void Conscript( )
    {
      //conscriptable units can join a group and function with group logic

    }

    // Update is called once per frame
    void Update()
    {
        if (canAct) { Act(); }
       

    }


    public void Act()
    {
        if (target == null || ship == null) { return; }

        Debug.Log(Vector3.Angle(transform.forward, (target.position - transform.position).normalized));
        ship.rotationTarget.LookAt(target);
        if (State() == AiState.adjusting)
        { 
            ship.rotationTarget.LookAt(target);

            if (Vector3.Angle(transform.forward, (target.position - transform.position).normalized) < 10)
            {
                State(AiState.attacking);
            }


        }
        else if (State() == AiState.attacking)
        {


            if (Vector3.Distance(target.position, ship.transform.position) < 10)
            {
                ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
                ship.primaryEngine.GetComponent<EngineBasic>().Throttle(0, 1);
               
            }
            else 
            {
               // ship.rotationTarget.rotation = ship.transform.rotation;
            }

            if (Vector3.Angle(transform.forward, (target.position - transform.position).normalized) > 80)
            {
                timer += Time.deltaTime;
                if (timer >= brainTime)
                {
                    State(AiState.recovering);
                    pendingState = AiState.adjusting;
                }
               
            }


        }
        else if (State() == AiState.recovering)
        {
            timer -= Time.deltaTime;
            if (timer<= 0)
            {
                State(pendingState);
            }


        }

        if (Vector3.Angle(transform.position, target.position) < 10 || Vector3.Angle(transform.position, target.position) > 80)
        { 
        
        }


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
