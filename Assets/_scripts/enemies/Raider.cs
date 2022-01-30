using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : Enemy
{
    


    public override void Act()
    {
        Debug.Log(Vector3.Angle(transform.position + transform.forward, ship.rotationTarget.position + ship.rotationTarget.forward));

        if (AttackTarget() == null || ship == null) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }

        

        if (State() == AiState.adjusting)
        {
            Moving();
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
        ship.rotationTarget.LookAt(targetPos);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(targetPos);

        float angle = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);
         angle = ((90 - angle) / 90);

        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1 ,1);
        ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0,0);

        if (Vector3.Distance(targetPos, ship.transform.position) > 1)
        {
           // ship.primaryEngine.GetComponent<EngineBasic>().Throttle(((90 - Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized)) / 90), 0);
        }
        else 
        {
            //decreasing throttle at close range
            //ship.primaryEngine.GetComponent<EngineBasic>().Throttle(Vector3.Distance(targetPos, ship.transform.position), 0);
        }



        //if (Vector3.Distance(AttackTarget().position, ship.transform.position) < 10)
        //{
        //    if (CheckBrain())
        //    {
        //        ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(1, 1);
        //        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
        //    }


        //    stateTimer += Time.deltaTime;

        //    if (stateTimer >= stateTime)
        //    {
        //        State(AiState.recovering);
        //        pendingState = AiState.adjusting;
        //    }

        //}
        //else if (Vector3.Distance(AttackTarget().position, ship.transform.position) < 30)
        //{

        //    if (CheckBrain())
        //    {
        //        ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(1, 1);
        //        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1.0f, 0.1f);
        //    }
        //}
        //else
        //{
        //    if (CheckBrain())
        //    {
        //        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1 - ((Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized)) / 90), 1.0f);
        //    }
        //    // ship.rotationTarget.rotation = ship.transform.rotation;
        //}

        //if (Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized) > 80)
        //{
        //    stateTimer += Time.deltaTime;
        //    if (stateTimer >= brainTime)
        //    {
        //        State(AiState.recovering);
        //        pendingState = AiState.adjusting;
        //    }

        //}


        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 && Vector3.Distance(ship.transform.position,targetPos) < rangeVarience)
        {

            
            stateTimer = StateTime();

            DetermineRangeZone(AttackTarget().position);

            //make decision
            float facing = Vector3.Angle(transform.forward, (AttackTarget().position - transform.position).normalized);
            float targetfacing = Vector3.Angle(AttackTarget().forward, (ship.transform.position - AttackTarget().position).normalized);

            if (facing < targetfacing)
            {
                targetPos = AttackTarget().position - (AttackTarget().forward * 10);
            }
            else 
            {
                targetPos = AttackTarget().position + (AttackTarget().up * 10);
            }


            //if (RangeZone() == RangeBand.close)
            //{
            //    targetPos = AttackTarget().position + (AttackTarget().forward * 10);
            //    State(AiState.attacking);
            //}
            //else
            //{
            //    State(AiState.moving);
            //    targetPos = AttackTarget().position - (AttackTarget().forward * 25);
            //}


        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving() 
    {
        ship.rotationTarget.LookAt(targetPos);
        ship.secondaryEngine.GetComponent<LateralThruster>().rotationTarget.LookAt(AttackTarget());

        float angle = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);
        angle = ((90 - angle) / 90);

        ship.primaryEngine.GetComponent<EngineBasic>().Throttle(.2f, 1);
        ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);


        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = StateTime();

            DetermineRangeZone(AttackTarget().position);

            //make decision
            float facing = Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized);
            float targetfacing = Vector3.Angle(AttackTarget().forward, (ship.transform.position - AttackTarget().position).normalized);

            if (facing < 90)
            {
                if (facing < targetfacing)
                {
                    
                }
                
            }


            if (RangeZone() == RangeBand.close)
            {
                targetPos = AttackTarget().position + (AttackTarget().forward * 10);
                State(AiState.attacking);
            }
            else 
            {

                targetPos = AttackTarget().position - (AttackTarget().forward * 25); 
            }


        }
    }

    public override void Pathfinding() { }
    
    public override void Adjusting() { }
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

        if (_newstate == AiState.attacking)
        {
          //  stateTimer = 0;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);


        }
        else if (_newstate == AiState.adjusting)
        {
           // stateTimer = 0;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(0, 1);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        }
        else if (_newstate == AiState.recovering)
        {
           // stateTimer = StateTime();
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        }




    }


}
