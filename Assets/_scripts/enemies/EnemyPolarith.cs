using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polarith.AI.Package;
public class EnemyPolarith : Enemy
{
   public ShipPhysics movementControls;
    public ShipSystem gun;

    public Transform focusObject, rotationTarget;

    public float torquePower,engineThrottle;
    public float accelerate,decelerate;


    public float targetengineThrottle, targetengineTorqueThrottle;


    public override void Act()
    {

        if (Vector3.Angle(transform.forward, (focusObject.position - transform.position).normalized) > 5)
        {

            movementControls.Torque = Mathf.Lerp(movementControls.Torque, torquePower, Time.deltaTime * accelerate);
            
        }
        else 
        {
            movementControls.Torque = Mathf.Lerp(movementControls.Torque, 0, Time.deltaTime * decelerate);
            RB().angularVelocity = Vector3.Lerp(RB().angularVelocity,Vector3.zero,Time.deltaTime * decelerate);
        }


        if (Vector3.Distance(transform.position, focusObject.position) > 5)
        {

            movementControls.Speed = Mathf.Lerp(movementControls.Speed, engineThrottle, Time.deltaTime * accelerate);
        }
        else
        {
            movementControls.Speed = Mathf.Lerp(movementControls.Speed, 0, Time.deltaTime * decelerate);
            RB().velocity = Vector3.Lerp(RB().velocity, Vector3.zero, Time.deltaTime * decelerate);
        }
        return;
        if (AttackTarget() == null ) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }


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
   


    }


    public override void MakeDecision()
    {
      //  DetermineRangeZone(ship.transform.position);



        AiState newState = State();

        if (State() == AiState.attacking)
        {
            if (Vector3.Distance(ShipTransform().position, AttackTarget().position) < closeRange)
            {
                newState = AiState.moving;


            }
        }else if (State() == AiState.moving)
        {
            if (Vector3.Distance(ShipTransform().position, AttackTarget().position) > midRange)
            {
                newState = AiState.attacking;


            }
        }

       


        State(newState);

    }




    public override void Attacking()
    {

        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        FocusObject().position = AttackTarget().position;

        rotationTarget.rotation = Quaternion.Lerp(rotationTarget.rotation, Quaternion.LookRotation((AttackTarget().position - rotationTarget.position), rotationTarget.up), Time.deltaTime * torquePower);

        if (Vector3.Distance(ShipTransform().position, AttackTarget().position) < closeRange)
        {
            if (secondaryFacing < angleTolerance)
            {
                if (gun != null) { gun.Activate(); }
            }
            else { if (gun != null) { gun.Deactivate(); } }
        }
        


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



        rotationTarget.rotation = Quaternion.Lerp(rotationTarget.rotation, ShipTransform().rotation, Time.deltaTime * torquePower);


        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 || Vector3.Distance(ShipTransform().position, targetPos) <= 1)
        {
            stateTimer = StateTime();

            MakeDecision();

        }
    }



    public override void Adjusting()
    { 

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
        FocusObject().parent = null;
        if (State() == _newstate) { return; } //didnt switch to a new state



        if (State() == AiState.dying) { return; }

        //reset the timer since the state changed: does it need stuck in loop options too?

        timeSinceLastAction = 0;
        stuckCounter = 0;



         if (State() == AiState.attacking)
        {
            Vector3 away = ShipTransform().position + (ShipTransform().forward * farRange);

            FocusObject().position = away;
        }
        else if (State() == AiState.moving)
        {
            FocusObject().position = AttackTarget().position;
        }


    }


    public Transform FocusObject()
    {
        if (focusObject == null) { return AttackTarget(); }

        return focusObject;
    }

}
