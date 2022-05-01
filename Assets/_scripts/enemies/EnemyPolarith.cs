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

       
        if (AttackTarget() == null ) { return; }

        if (brainTimer > 0) { brainTimer -= Time.deltaTime; }

        if (GetShip().Chasis() && GetShip().Chasis().ExternalForce() != Vector3.zero)
        {
            movementControls.Torque = 0;
            movementControls.Speed = 0;
            RB().velocity = (GetShip().Chasis().ExternalForce());
            return;
        }


        if (GetShip().PrimaryWeapon() )
        {
            float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);
            if (secondaryFacing < angleTolerance)
            {
                if (gun != null) { gun.Act(); }
            }
            else { if (gun != null) { gun.Deactivate(); } }
        }

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
        else if (State() == AiState.attackWindUp)
        {
            AttackWindUp();

        }
        else if (State() == AiState.postAttack)
        {
            PostAttack();

        }



    }


    public override void MakeDecision()
    {
      //  DetermineRangeZone(ship.transform.position);



        AiState newState = State();

        if (State() == AiState.attacking)
        {
            if (Vector3.Distance(ShipTransform().position, AttackTarget().position) > midRange)
            {
                newState = AiState.moving;


            }
            else if(Vector3.Distance(ShipTransform().position, FocusObject().position) < closeRange)
            {
                newState = AiState.attackWindUp;
                focusObject.position = AttackTarget().position + ((AttackTarget().position - transform.position).normalized * midRange) + (transform.up * 5);

            }

        }
        else if (State() == AiState.moving)
        {
            if (Vector3.Distance(ShipTransform().position, AttackTarget().position) < midRange )
            {
                newState = AiState.attackWindUp;
                focusObject.position = AttackTarget().position + ((AttackTarget().position - transform.position).normalized * midRange) + (transform.right * 5);

            }
        }

       


        State(newState);

    }



    public override void AttackWindUp()
    {
        movementControls.Speed = engineThrottle;
        movementControls.Torque = 0;
        if (Vector3.Distance(ShipTransform().position, AttackTarget().position) <= closeRange)
        {
            State(AiState.attacking);
        }

    }

    public override void Attacking()
    {

        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        movementControls.Speed = 0;
        movementControls.Torque = 0;

        ShipTransform().transform.rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.LookRotation(AttackTarget().position  - ShipTransform().position, AttackTarget().up), Time.deltaTime * torquePower);
        RB().velocity = Vector3.Lerp(RB().velocity, ( FocusObject().position - ShipTransform().position).normalized * engineThrottle, Time.deltaTime);


       // FocusObject().position = AttackTarget().position;

        if (secondaryFacing < angleTolerance)
        {
            if (gun != null) { gun.Activate(); }
        }
        else { if (gun != null) { gun.Deactivate(); } }



        //Debug.Log(Vector3.Angle(GetShip().transform.forward, (AttackTarget().position - transform.position).normalized));

         //   GetShip().EnemyAct();
       // GetShip().PrimaryEngine().Act();

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTimer = StateTime();

          MakeDecision();

        }
    }

    public override void PostAttack()
    {

        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        movementControls.Speed = 0;
        movementControls.Torque = 0;

        ShipTransform().transform.rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up), Time.deltaTime * torquePower);
        RB().velocity = Vector3.Lerp(RB().velocity, (FocusObject().position - ShipTransform().position).normalized * engineThrottle, Time.deltaTime );


        // FocusObject().position = AttackTarget().position;

        if (secondaryFacing < angleTolerance)
        {
            if (gun != null) { gun.Activate(); }
        }
        else { if (gun != null) { gun.Deactivate(); } }



        Debug.Log(Vector3.Angle(GetShip().transform.forward, (AttackTarget().position - transform.position).normalized));

        //   GetShip().EnemyAct();
        // GetShip().PrimaryEngine().Act();

        if (Vector3.Distance(ShipTransform().position, AttackTarget().position) <= closeRange)
        {
            State(AiState.moving);
        }
    }



    public override void Recovering() { }
    public override void TakingDamage() { }
    public override void Moving()
    {
        movementControls.Torque = Mathf.Lerp(movementControls.Torque, torquePower, Time.deltaTime * accelerate);
        movementControls.Speed = Mathf.Lerp(movementControls.Speed, engineThrottle, Time.deltaTime * accelerate);

        FocusObject().position = AttackTarget().position;

        GetShip().EnemyAct();

      // rotationTarget.rotation = Quaternion.Lerp(rotationTarget.rotation, ShipTransform().rotation, Time.deltaTime * torquePower);


        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 || Vector3.Distance(ShipTransform().position, AttackTarget().position) <= closeRange)
        {
            stateTimer = StateTime();

            MakeDecision();

        }
    }



    public override void Adjusting()
    {

        movementControls.Torque = Mathf.Lerp(movementControls.Torque, 0, Time.deltaTime * accelerate);
        movementControls.Speed = Mathf.Lerp(movementControls.Speed, 0, Time.deltaTime * accelerate);

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

          //  FocusObject().position = away;
        }
        else if (State() == AiState.moving)
        {
           // FocusObject().position = AttackTarget().position;
        }


    }


    public Transform FocusObject()
    {
        if (focusObject == null) { return AttackTarget(); }

        return focusObject;
    }

}
