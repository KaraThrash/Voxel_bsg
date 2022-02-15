using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public ShipSystem gun;

    public float engineThrottle, engineTorqueThrottle;
    public float lateralHort, lasteralVert;

    public float targetengineThrottle, targetengineTorqueThrottle;
    public float targetlateralHort, targetlateralVert;

    public override void Act()
    {
        engineThrottle = Mathf.Lerp(engineThrottle, targetengineThrottle, Time.deltaTime * DirectionChangeSpeed());
        engineTorqueThrottle = Mathf.Lerp(engineTorqueThrottle, targetengineTorqueThrottle, Time.deltaTime * DirectionChangeSpeed());
        lateralHort = Mathf.Lerp(lateralHort, targetlateralHort, Time.deltaTime * DirectionChangeSpeed());
        lasteralVert = Mathf.Lerp(lasteralVert, targetlateralVert, Time.deltaTime * DirectionChangeSpeed());




        if (AttackTarget() == null || ship == null) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }


        ship.secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(engineThrottle);
        ship.secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(engineTorqueThrottle);
        ship.primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(lateralHort);
        ship.primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(targetlateralVert);

        if (State() == AiState.adjusting)
        {
            Adjusting();
        }
        else if (State() == AiState.attacking)
        {
            Attacking();

        }
        else if (State() == AiState.recovering)
        {
            Recovering();

        }
        else if (State() == AiState.moving)
        {
            Moving();

        }
        if (Vector3.Angle(ship.transform.position, AttackTarget().position) < 10 || Vector3.Angle(ship.transform.position, AttackTarget().position) > 80)
        {

        }


    }


    public override void MakeDecision()
    {
        DetermineRangeZone(ship.transform.position);



        AiState newState = State();



        RaycastHit hit;

        RaycastAt(ShipTransform().position,AttackTarget(),Vector3.Distance(ShipTransform().position, AttackTarget().position));

        if (RaycastAt(ShipTransform().position, AttackTarget(), Vector3.Distance(ShipTransform().position, AttackTarget().position)))
        {
            Debug.Log("making Decision: CAN see attack target");
            newState = AiState.attacking;
   

        }
        else 
        {
            Debug.Log("making Decision: cant see attack target");
           
            if (Vector3.Distance(AttackTarget().position, ShipTransform().position) < closeRange)
            {
                //cant see the target and is very close
                newState = AiState.adjusting;

            }
            else
            {
                newState = AiState.moving;

                if (RaycastAt(ShipTransform().position, AttackTarget().position, closeRange))
                {
                    targetPos = ShipTransform().position + ((ShipTransform().right - ShipTransform().forward).normalized * closeRange);
                }
                else
                {
                    targetPos = ShipTransform().position +( (AttackTarget().position - ShipTransform().position).normalized * closeRange );
                }

            }
        }



        State(newState);

    }




    public override void Attacking()
    {
        targetPos = AttackTarget().position;

        ship.rotationTarget.LookAt(AttackTarget().position);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget().position);

        float facing = Vector3.Angle(ship.transform.forward, (targetPos - ship.transform.position).normalized);
        float secondaryFacing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);


        if (Vector3.Distance(ship.transform.position, targetPos) > closeRange)
        {
            targetengineThrottle = 1;
            targetengineTorqueThrottle = 1;
        }
        else
        {
            targetengineThrottle = 0;
            targetengineTorqueThrottle = 1;
        }
        if (secondaryFacing < angleTolerance)
        {
            if (gun != null) { gun.Activate(); }
        }
        else { if (gun != null) { gun.Deactivate(); } }





        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = StateTime();

               MakeDecision();

        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving()
    {


        RaycastHit hit;

        if (Physics.SphereCast(ShipTransform().position, 2.0f,ShipTransform().forward, out hit, closeRange))
        {
            ship.rotationTarget.LookAt(ShipTransform().position + ShipTransform().right);
            ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(ShipTransform().position + ShipTransform().right);
            targetengineThrottle = 0.2f;
        }
        else if (Physics.SphereCast(ShipTransform().position, 4.0f, ShipTransform().forward, out hit, closeRange))
        {
            ship.rotationTarget.LookAt(ShipTransform().position + ShipTransform().forward);
            ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(ShipTransform().position + ShipTransform().forward);
            targetengineThrottle = 1;
        }
        else 
        {
            ship.rotationTarget.LookAt(AttackTarget().position);
            ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget().position);
            targetengineThrottle = 1;
        }





        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 || Vector3.Distance(ShipTransform().position,targetPos) <= 1 )
        {
            stateTimer = StateTime();

            MakeDecision();

        }
    }



    public override void Adjusting()
    {
        Vector3 away = ship.transform.position + ((ship.transform.position - AttackTarget().position).normalized * midRange);
        ship.rotationTarget.LookAt(away);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(away);


        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = StateTime();

            MakeDecision();

        }
    }



    public override void Pathfinding() { }
    public override void Special() { }
    public override void Dying() { }
    public override void Idle() { }
    public override void Dodging() { }
    public override void Fleeing() { }
    public override void Spawned() { }
    public override void Inactive() { }
    public override void Ragdolling() { }


    public override void OnStateChange(AiState _newstate)
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

            if (Vector3.Angle(transform.forward, (AttackTarget().position - transform.position).normalized) < 10 || Vector3.Angle(transform.forward, (AttackTarget().position - transform.position).normalized) > 80) { }
        }

   
    }


}
