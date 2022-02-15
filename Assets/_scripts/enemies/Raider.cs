using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : Enemy
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

        ship.primaryEngine.GetComponent<EngineBasic>().Thrust_Throttle(engineThrottle);
        ship.primaryEngine.GetComponent<EngineBasic>().Roll_Throttle(engineTorqueThrottle);
        ship.secondaryEngine.GetComponent<LateralThruster>().Horizontal_Throttle(lateralHort);
        ship.secondaryEngine.GetComponent<LateralThruster>().Vertical_Throttle( lasteralVert);

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

        //when moving head on make lateral moves to avoid possible attacks
        float facing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);
        float targetfacing = Vector3.Angle(AttackTarget().forward, (ship.transform.position - AttackTarget().position).normalized);

        if (targetfacing < 90 && facing < 90)
        {

                targetlateralVert = -1 * Mathf.Sign(targetlateralVert);
                targetlateralHort = -1 * Mathf.Sign(targetlateralVert);

        }
        else
        {
                targetlateralVert = 0;
                targetlateralHort = 0;
        }

        
        AiState newState = State();

        targetPos = AttackTarget().position - (AttackTarget().forward * closeRange);

        if (Vector3.Distance(targetPos, ship.transform.position) > farRange)
        {
            newState = AiState.moving;

        }
        else if (Vector3.Distance(AttackTarget().position , ship.transform.position) < closeRange )
        {
            newState = AiState.adjusting;

        }
        else 
        {
            if (State() == AiState.adjusting && (Vector3.Distance(targetPos, ship.transform.position) < midRange ) )
            {
                newState = AiState.adjusting;
            }
            else 
            {
                newState = AiState.attacking;
            }
            
        }

        //to make the lateral movements not always the same direction
        if (newState == AiState.adjusting)
        {
            targetengineThrottle = 1;
            targetengineTorqueThrottle = 1;
            targetlateralVert = 0;
                targetlateralHort = 0;
            

        }
        else if (newState == AiState.attacking)
        {
            if (targetfacing < facing)
            {
                targetlateralHort = -targetlateralHort;
            }


        }
        else if (newState  == AiState.recovering)
        {
            if (targetfacing < facing)
            {
                targetlateralVert = -targetlateralVert;
                targetlateralHort = -targetlateralHort;
            }
        
        }
        else if (newState == AiState.moving)
        {

        }

        State(newState);

    }




    public override void Attacking()
    {
        targetPos = AttackTarget().position - (AttackTarget().forward * closeRange);

        ship.rotationTarget.LookAt(targetPos);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget().position);

        float facing = Vector3.Angle(ship.transform.forward, (targetPos - ship.transform.position).normalized);
        float secondaryFacing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);


        if (Vector3.Distance(ship.transform.position, targetPos) > 5)
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
           
         //   MakeDecision();
            
        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving() 
    {
        targetPos = AttackTarget().position;

        targetPos += AttackTarget().up * ( 0.5f *  Vector3.Distance(AttackTarget().position , ship.transform.position));

        ship.rotationTarget.LookAt(targetPos);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(targetPos);

        float facing = Vector3.Angle(ship.transform.forward, (targetPos - ship.transform.position).normalized);
        float secondaryFacing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);

        float angle = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);




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
