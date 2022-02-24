using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public ShipSystem gun;

    public float engineThrottle, engineTorqueThrottle;
    public float lateralHort, lateralVert;

    public float targetengineThrottle, targetengineTorqueThrottle;
    public float targetlateralHort, targetlateralVert;

    public override void Act()
    {
        engineThrottle = Mathf.Lerp(engineThrottle, targetengineThrottle, Time.deltaTime * DirectionChangeSpeed());
        engineTorqueThrottle = Mathf.Lerp(engineTorqueThrottle, targetengineTorqueThrottle, Time.deltaTime * DirectionChangeSpeed());
        lateralHort = Mathf.Lerp(lateralHort, targetlateralHort, Time.deltaTime * DirectionChangeSpeed());
        lateralVert = Mathf.Lerp(lateralVert, targetlateralVert, Time.deltaTime * DirectionChangeSpeed());




        if (AttackTarget() == null || ship == null) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }


       // ship.secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(lateralHort);
        //ship.secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle(lateralVert);
        ship.primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(engineThrottle);
        ship.primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(engineTorqueThrottle);

        ship.Movement();

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


    public void AttemptToMoveForwardOld(bool _on)
    {
        if (_on)
        {
            RaycastHit hit;
            Vector3 _targetpos = Vector3.zero;

            if (Physics.SphereCast(ShipTransform().position, 2.0f, ShipTransform().forward, out hit, 2 + ship.RB().velocity.magnitude))
            {
                targetengineThrottle -= Time.deltaTime;
                targetengineTorqueThrottle += Time.deltaTime;



                if (Vector3.Distance(ShipTransform().position + ShipTransform().right, AttackTarget().position) < Vector3.Distance(ShipTransform().position - ShipTransform().right, AttackTarget().position))
                {
                    _targetpos = ShipTransform().position + ShipTransform().right;
                }
                else
                {
                    _targetpos = (ShipTransform().position - ShipTransform().right);
                }

            }
            else
            {

                targetengineThrottle = 1;
                targetengineTorqueThrottle = 1;

                if (Vector3.Distance(ShipTransform().position + ShipTransform().right, AttackTarget().position) < Vector3.Distance(ShipTransform().position + ShipTransform().forward, AttackTarget().position))
                {
                    _targetpos = (ShipTransform().position + ShipTransform().right);
                }
                else if (Vector3.Distance(ShipTransform().position - ShipTransform().right, AttackTarget().position) < Vector3.Distance(ShipTransform().position + ShipTransform().forward, AttackTarget().position))
                {
                    _targetpos = (ShipTransform().position - ShipTransform().right);
                }
                else if (Vector3.Distance(ShipTransform().position - ShipTransform().up, AttackTarget().position) < Vector3.Distance(ShipTransform().position + ShipTransform().forward, AttackTarget().position))
                {
                    _targetpos = (ShipTransform().position - ShipTransform().up);
                }
                else if (Vector3.Distance(ShipTransform().position + ShipTransform().up, AttackTarget().position) < Vector3.Distance(ShipTransform().position + ShipTransform().forward, AttackTarget().position))
                {
                    _targetpos = (ShipTransform().position + ShipTransform().up);
                }
                else { _targetpos = (AttackTarget().position); }




            }

            Vector3 relativePos = _targetpos - ShipTransform().position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            ship.rotationTarget.rotation = rotation;
            ship.rotationTarget.rotation = Quaternion.Slerp(ship.rotationTarget.rotation, rotation, Time.deltaTime * BrainTime());
        }

    }
    public void AttemptToMoveForward(bool _on)
    {
        if (_on)
        {
            RaycastHit hit;
            Vector3 _targetpos = Vector3.zero;

            if (Physics.SphereCast(ship.rotationTarget.position, 2.0f, ShipTransform().forward, out hit, 2 + ship.RB().velocity.magnitude))
            {

                targetengineTorqueThrottle = 0.5f;

                ship.rotationTarget.rotation = Quaternion.Slerp(ship.rotationTarget.rotation, Quaternion.LookRotation(ShipTransform().position - AttackTarget().position  , Vector3.up), Time.deltaTime * 1);


            }
            else 
            {

            
                targetengineTorqueThrottle = 1;

                ship.rotationTarget.rotation = Quaternion.Slerp(ship.rotationTarget.rotation, Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, Vector3.up), Time.deltaTime * BrainTime());





            }

            if (Vector3.Angle(ship.rotationTarget.forward, ship.transform.forward) < 10)
            {
                targetengineThrottle = 1;
                targetengineTorqueThrottle = 0;
            }
            else 
            {
                targetengineTorqueThrottle = 0;
                targetengineThrottle = 0;
            }
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

        AttemptToMoveForward(true);

        RaycastHit hit;

        //if (Physics.SphereCast(ShipTransform().position, 2.0f,ShipTransform().forward, out hit, ship.RB().velocity.magnitude))
        //{
        //    ship.rotationTarget.LookAt(ShipTransform().position + ShipTransform().right);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(ShipTransform().position + ShipTransform().right);
        //    targetengineThrottle = 0.2f;


        //}
        //else if (Physics.SphereCast(ShipTransform().position, 4.0f, ShipTransform().forward, out hit, ship.RB().velocity.magnitude))
        //{
        //    ship.rotationTarget.LookAt(ShipTransform().position + ShipTransform().forward);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(ShipTransform().position + ShipTransform().forward);
        //    targetengineThrottle = 1;
        //}
        //else 
        //{
        //    ship.rotationTarget.LookAt(AttackTarget().position);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget().position);
        //    targetengineThrottle = 1;
        //}





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
