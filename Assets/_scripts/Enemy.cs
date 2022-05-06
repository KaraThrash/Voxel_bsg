using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Stats_Enemy stats;
    public NpcManager npcManager;
    public Rigidbody rb;
    public Ship ship;

    public bool canAct;

    public AiState state;
    private AiState pendingState;
    public Stance stance;

    public Transform attackTarget;

    public float hp;
    [SerializeField]
    private Transform shipTransform;


    //AI stuff
    /// 
    /// 
    /// 

    public bool alert;


    private float brainTime, stateTime; //brain time for checking if the enemy responds to an external condition [e.g. player being in their line of fire], state time for state specific holds
    protected float directionChangeSpeed; // how quickly this enemy changes the direction they want to go



    protected Vector3 targetPos;
    protected Vector3 targetDirection;
    protected Quaternion targetRotation;

    protected float timer_State, timer_Brain;

    protected float stuckCounter, timeSinceLastAction; // check if stuck


    private float angle;

    public float Angle
    {
        get { return angle; }
        set { angle = value; }
    }






    /// 
    /// 
    /// 
    //AI stuff


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


    }



    //public enum AiState
    //{
    //recovering, takingDamage, 
    //pathfinding, moving, 
    //attackWindUp,attacking, postAttack,
    //adjusting, special, dying, 
    //dodging,fleeing,
    //idle, spawned,
    //inactive,
    //ragdolling
    //}


    public virtual void Recovering() { }
    public virtual void TakingDamage() { }
    public virtual void Moving() { }
    public virtual void Pathfinding() { }
    public virtual void AttackWindUp() { }
    public virtual void Attacking() { }
    public virtual void PostAttack() { }
    public virtual void Adjusting() { }
    public virtual void Special() { }
    public virtual void Dying() { }
    public virtual void Idle() { }
    public virtual void Dodging() { }
    public virtual void Fleeing() { }
    public virtual void Spawned() { }
    public virtual void Inactive() { }
    public virtual void Ragdolling() { }

    public void RotationTargetLookAt(Vector3 _pos)
    {
        if (GetShip() != null && GetShip().rotationTarget != null)
        {
            GetShip().rotationTarget.LookAt(_pos);
        }
    }

    public void RotationTargetLookAt(Transform _pos)
    {
        if (GetShip() != null && GetShip().rotationTarget != null)
        {
            GetShip().rotationTarget.LookAt(_pos);
        }
    }









    //check if the current state of play is in the enemies advantage, and whether they should defend or attack
    public virtual bool CheckAdvantage() { return false; }

    //check the enviromenty, and current state to determine what to do next
    public virtual void MakeDecision() { }







    void Start()
    {
        if (AttackTarget() == null || GetShip() == null) { return; }
        State(AiState.attacking);
        stateTime = 5;
        brainTime = 12;
        directionChangeSpeed = 12;
    }
    void Awake()
    {


    }


    public void SetAlert(bool isAlert)
    {
        alert = isAlert;
    }

    public bool CheckBrain()
    {
        if (timer_Brain <= 0)
        {
            timer_Brain = BrainTime();
            return true;
        }
        return false;
    }


    public void Conscript()
    {
        //conscriptable units can join a group and function with group logic

    }

    // Update is called once per frame
    void Update()
    {
        if (canAct) { Act(); }


    }


    public virtual void Act()
    {
        Debug.Log(Vector3.Angle(transform.position + transform.forward, GetShip().rotationTarget.position + GetShip().rotationTarget.forward));

        if (AttackTarget() == null || GetShip() == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }

        RotationTargetLookAt(AttackTarget());

        if (ship.Hitpoints() <= 0 || State() == AiState.dying)
        {
            Dying();

            return;
        }


        if (State() == AiState.adjusting)
        {

            if (CheckBrain())
            {
                GetShip().secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(1);
                GetShip().secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(1);
                GetShip().primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(0.1f);
                GetShip().primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(1);
            }

            if (Vector3.Angle(transform.forward, GetShip().rotationTarget.forward) < 10)
            {
                State(AiState.attacking);
            }


        }
        else if (State() == AiState.attacking)
        {


            if (Vector3.Distance(AttackTarget().position, GetShip().transform.position) < 10)
            {
                if (CheckBrain())
                {
                    GetShip().secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(1);
                    GetShip().secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(1);
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(1);
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(0);
                }


                timer_State += Time.deltaTime;
                if (timer_State >= stateTime)
                {
                    State(AiState.recovering);
                    pendingState = AiState.adjusting;
                }

            }
            else if (Vector3.Distance(AttackTarget().position, ship.transform.position) < 30)
            {

                if (CheckBrain())
                {
                    GetShip().secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(1);
                    GetShip().secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(1);
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(1);
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(0.1f);
                }
            }
            else
            {
                if (CheckBrain())
                {
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(1 - ((Vector3.Angle(GetShip().transform.forward, (AttackTarget().position - GetShip().transform.position).normalized)) / 90));
                    GetShip().primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(1);
                }
                // ship.rotationTarget.rotation = ship.transform.rotation;
            }

            if (Vector3.Angle(GetShip().transform.forward, (AttackTarget().position - transform.position).normalized) > 80)
            {
                timer_State += Time.deltaTime;
                if (timer_State >= brainTime)
                {
                    State(AiState.recovering);
                    pendingState = AiState.adjusting;
                }

            }


        }
        else if (State() == AiState.recovering)
        {
            GetShip().secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(1);
            GetShip().secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(1);
            GetShip().primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(1);
            GetShip().primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(1);

            timer_State -= Time.deltaTime;
            if (timer_State <= 0)
            {
                State(pendingState);
            }


        }

        if (Vector3.Angle(GetShip().transform.position, AttackTarget().position) < 10 || Vector3.Angle(GetShip().transform.position, AttackTarget().position) > 80)
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


    public void CheckToNoticePlayer(float distanceToCheck = 100.0f)
    {
        //compare distance from in front to behind the enemy to determine if the player is in a forward cone of vision
        //or if the player is extremely close

        RaycastHit hit;



        if (Physics.Raycast(transform.position, (transform.position - transform.position), out hit, distanceToCheck))
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





    public Transform AttackTarget()
    {
        return attackTarget;
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


    public Stats_Enemy Stats()
    {
        if (stats == null)
        { stats = new Stats_Enemy(); }

        return stats;
    }


    public Transform ShipTransform()
    {
        if (shipTransform == null)
        {
            if (ship != null) { shipTransform = ship.transform; }
            else { return transform; }
        }
        return shipTransform;
    }


    public Rigidbody RB()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        return rb;
    }

    public Ship GetShip()
    {
        return ship; 
  
    }



    public float StateTime() { return stateTime; }
    public float BrainTime() { return brainTime; }
    public float DirectionChangeSpeed() { return directionChangeSpeed; }
    public AiState State() { return state; }
    public Stance Stance() { return stance; }
    public void Stance(Stance _stance) { stance = _stance; }












    public float DistanceToTarget(GameObject target)
    { return Vector3.Distance(transform.position, target.transform.position); }

    public float DistanceToTarget(Transform target)
    { return Vector3.Distance(transform.position, target.position); }

    public float DistanceToTarget(Vector3 target)
    { return Vector3.Distance(transform.position, target); }




    public bool RaycastAt(Vector3 _from, Vector3 _to, float _range = 100.0f)
    {
        RaycastHit hit;

        if (Physics.Raycast(_from, _to - _from, out hit, _range)) { return true; }

        return false;

    }

    public bool RaycastAt(Vector3 _from, Transform _to, float _range = 100.0f)
    {
        RaycastHit hit;

        if (Physics.Raycast(_from, _to.position - _from, out hit, _range) && hit.transform == _to)
        {
            return true;
        }

        return false;

    }


}
