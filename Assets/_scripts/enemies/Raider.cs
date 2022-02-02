using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : Enemy
{
    public ShipSystem gun;

    public float engineThrottle, engineTorqueThrottle;
    public float lateralHort, lasteralVert;

    public float targetengineThrottle, targetengineTorqueThrottle;
    public float targetlateralHort, targetlasteralVert;

    public override void Act()
    {
        engineThrottle = Mathf.Lerp(engineThrottle, targetengineThrottle, Time.deltaTime * BrainTime());
        engineTorqueThrottle = Mathf.Lerp(engineTorqueThrottle, targetengineTorqueThrottle, Time.deltaTime * BrainTime());
        lateralHort = Mathf.Lerp(lateralHort, targetlateralHort, Time.deltaTime * BrainTime());
        lasteralVert = Mathf.Lerp(lasteralVert, targetlasteralVert, Time.deltaTime * BrainTime());



        if (AttackTarget() == null || ship == null) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }

        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(engineThrottle, engineTorqueThrottle);
        ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(lateralHort, lasteralVert);

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





    public override void Attacking()
    {

        ship.rotationTarget.LookAt(AttackTarget().position - (AttackTarget().forward * 5));
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget());

        float facing = Vector3.Angle(transform.forward, (AttackTarget().position - transform.position).normalized);
        float targetfacing = Vector3.Angle(AttackTarget().forward, (ship.transform.position - AttackTarget().position).normalized);


        if (Vector3.Distance(AttackTarget().position, ship.transform.position) > closeRange)
        {

            if (facing > 90)
            {

                targetengineThrottle = 0.2f;
                targetengineTorqueThrottle = 1;
               

            }
            else
            {
                if (Vector3.Distance(AttackTarget().position, ship.transform.position) < midRange)
                {
                    targetengineThrottle = 0;
                    targetengineTorqueThrottle = 1;
                }
                else 
                {
                    targetengineThrottle = 1;
                    targetengineTorqueThrottle = 1;
                }

                if (facing < angleTolerance)
                {
                    if (gun != null) { gun.Activate(); }
                }
                else { if (gun != null) { gun.Deactivate(); } }

            }
        }
        else 
        {

            targetengineThrottle = -1;
            engineThrottle = -1;
            targetengineTorqueThrottle = 1;

        }




        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = StateTime();

            DetermineRangeZone(AttackTarget().position);

            //make decision

            if (Vector3.Distance(AttackTarget().position, ship.transform.position) > farRange)
            {
                targetPos = AttackTarget().position + (AttackTarget().forward * 1);
                State(AiState.moving);
                targetlateralHort = 0 ;
                targetlasteralVert = 0;
            }
            else if(Vector3.Distance(AttackTarget().position, ship.transform.position) < closeRange * 0.5f)
            {
                State(AiState.adjusting);

                targetlateralHort = 0;
                targetlasteralVert = 0;
            }
            else
            {
                targetlateralHort = 0;
                targetlasteralVert = 0;
            }


        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving() 
    {
        ship.rotationTarget.LookAt(AttackTarget());
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget());

        float angle = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);


        float facing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);
        float targetfacing = Vector3.Angle(AttackTarget().forward, (ship.transform.position - AttackTarget().position).normalized);

        if (facing < 90)
        {
            targetengineThrottle = 1;
            targetengineTorqueThrottle = 1;
       
        }
        else
        {
            targetengineThrottle = 0.2f;
            targetengineTorqueThrottle = 1;
          
        }


        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            stateTimer = StateTime();


            if (Vector3.Distance(AttackTarget().position,ship.transform.position) < midRange)
            {
                targetPos = AttackTarget().position;
                State(AiState.attacking);
            }
            else
            {

            }

        }
    }


    
    public override void Adjusting()
    {
        Vector3 away = ship.transform.position + (ship.transform.position - AttackTarget().position).normalized;
        ship.rotationTarget.LookAt(away);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(away);




        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            stateTimer = StateTime();


            if (Vector3.Distance(AttackTarget().position, ship.transform.position) > midRange)
            {
                targetengineThrottle = 0.2f;
                targetengineTorqueThrottle = 1;
                State(AiState.attacking);
            }
            else if (Vector3.Distance(AttackTarget().position, ship.transform.position) > farRange)
            {
                State(AiState.moving);
                targetlateralHort = 0;
                targetlasteralVert = 0;
            }
            else
            {
                targetengineThrottle = 1;
                targetengineTorqueThrottle = 1;
            }


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

        //if (_newstate == AiState.attacking)
        //{
        //  //  stateTimer = 0;
        //    ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);


        //}
        //else if (_newstate == AiState.adjusting)
        //{
        //   // stateTimer = 0;
        //    ship.primaryEngine.GetComponent<EngineBasic>().Throttle(0, 1);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        //}
        //else if (_newstate == AiState.recovering)
        //{
        //   // stateTimer = StateTime();
        //    ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
        //    ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        //}




    }


}
