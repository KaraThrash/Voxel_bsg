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

        ship.rotationTarget.LookAt(AttackTarget());

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

        if (Vector3.Angle(ship.transform.position, AttackTarget().position) < 10 || Vector3.Angle(ship.transform.position, AttackTarget().position) > 80)
        {

        }


    }

    public override void Attacking()
    {

        


        if (Vector3.Distance(AttackTarget().position, ship.transform.position) < 10)
        {
            if (CheckBrain())
            {
                ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(1, 1);
                ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
            }


            stateTimer += Time.deltaTime;

            if (stateTimer >= stateTime)
            {
                State(AiState.recovering);
                pendingState = AiState.adjusting;
            }

        }
        else if (Vector3.Distance(AttackTarget().position, ship.transform.position) < 30)
        {

            if (CheckBrain())
            {
                ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(1, 1);
                ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1.0f, 0.1f);
            }
        }
        else
        {
            if (CheckBrain())
            {
                ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1 - ((Vector3.Angle(ship.transform.forward, (AttackTarget().position - ship.transform.position).normalized)) / 90), 1.0f);
            }
            // ship.rotationTarget.rotation = ship.transform.rotation;
        }

        if (Vector3.Angle(ship.transform.forward, (AttackTarget().position - transform.position).normalized) > 80)
        {
            stateTimer += Time.deltaTime;
            if (stateTimer >= brainTime)
            {
                State(AiState.recovering);
                pendingState = AiState.adjusting;
            }

        }


        stateTime -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = stateTime;

            DetermineRangeZone(AttackTarget().position);

            if (RangeZone() == RangeBand.close)
            { 

            }


        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving() { }
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
            stateTimer = 0;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);


        }
        else if (_newstate == AiState.adjusting)
        {
            stateTimer = 0;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(0, 1);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        }
        else if (_newstate == AiState.recovering)
        {
            stateTimer = stateTime;
            ship.primaryEngine.GetComponent<EngineBasic>().Throttle(1, 0);
            ship.secondaryEngine.GetComponent<LateralThruster>().Throttle(0, 0);
        }




    }


}
