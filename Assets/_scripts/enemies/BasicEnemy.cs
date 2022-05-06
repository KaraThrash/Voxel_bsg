using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polarith.AI.Package;

public class BasicEnemy : Enemy
{
    public ShipPhysics movementControls;
    public ShipSystem gun;

    public Transform focusObject; //the object the polarith AI is trying to move towards. Use Polarith for general navigating, and set polarith to zero for specific actions



    
    public override void Act()
    {


        if (AttackTarget() == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }

        if (GetShip().Chasis() && GetShip().Chasis().ExternalForce() != Vector3.zero)
        {
            movementControls.Torque = 0;
            movementControls.Speed = 0;
            RB().velocity = (GetShip().Chasis().ExternalForce());
            return;
        }

        if (ship.Hitpoints() <= 0)
        {
            Dying();
            return;

        }

        RaycastHit hit;

        if (Physics.SphereCast(ShipTransform().position, 1.0f,(AttackTarget().position - ShipTransform().position),out hit))
        {
            if (hit.transform == AttackTarget())
            {
                Attacking();
                return;
            }
        }
        Moving();
        return;

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

        if (ship.Hitpoints() <= 0)
        {
            ship.Die();
            return;

        }

        if (GetShip().PrimaryWeapon())
        {
            float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);
            if (secondaryFacing < Stats().angleTolerance)
            {
                if (gun != null) { gun.Act(); }
            }
            else { if (gun != null) { gun.Deactivate(); } }
        }


        FocusObject().position = AttackTarget().position;
        return;
        AiState newState = State();

        if (State() == AiState.attacking)
        {
            newState = AiState.moving;
            movementControls.Speed = Stats().engineThrottle;
            movementControls.Torque = Stats().torquePower;

        }
        else if (State() == AiState.moving)
        {

            RangeBand currentRange = RangeTo();

            if (currentRange == RangeBand.extreme)
            {
                newState = AiState.moving;
                movementControls.Speed = Stats().engineThrottle;
                movementControls.Torque = Stats().torquePower;
            }
            else if (currentRange == RangeBand.close)
            {
                newState = AiState.attacking;
                movementControls.Speed = Stats().engineThrottle;
                movementControls.Torque = Stats().torquePower;
                FocusObject().position = AttackTarget().position - (AttackTarget().forward * 3);
            }
            else 
            {
                newState = AiState.attacking;
                movementControls.Speed = Stats().engineThrottle;
                movementControls.Torque = Stats().torquePower;
                FocusObject().position = AttackTarget().position - (AttackTarget().forward * 3);
            }
            

        }
        else if (State() == AiState.recovering)
        {
            movementControls.Speed = Stats().engineThrottle;
            movementControls.Torque = Stats().torquePower;

            FocusObject().position = AttackTarget().position ;
            newState = AiState.attacking;

        }



        timer_State = 5;
        State(newState);

    }



    public override void AttackWindUp()
    {
       

    }

    public override void Attacking()
    {
        movementControls.Speed = 0;
        movementControls.Torque = 0;


        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        FocusObject().position = AttackTarget().position;// - (AttackTarget().forward * 3);

        ShipTransform().transform.rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up), Time.deltaTime * Stats().torquePower);


        if (RangeTo(FocusObject(),ShipTransform()) == RangeBand.close || RangeTo(FocusObject(), ShipTransform()) == RangeBand.melee)
        {

            RB().velocity = Vector3.Lerp(RB().velocity, ShipTransform().forward * -Stats().engineThrottle, Time.deltaTime);
        }
        else if (RangeTo(FocusObject(), ShipTransform()) == RangeBand.mid)
        {

           // RB().velocity = Vector3.Lerp(RB().velocity, AttackTarget().GetComponent<Ship>().RB().velocity * 1.1f, Time.deltaTime * Stats().accelerate);
        }
        else
        {
            RB().velocity = Vector3.Lerp(RB().velocity, ShipTransform().forward * Stats().engineThrottle, Time.deltaTime);
        }



        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }

    public override void PostAttack()
    {

       
    }



    public override void Recovering()
    {
        timer_State -= Time.deltaTime;
        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }
    public override void TakingDamage() { }
    public override void Moving()
    {
        movementControls.Torque = Mathf.Lerp(movementControls.Torque, Stats().torquePower, Time.deltaTime * Stats().accelerate);
        movementControls.Speed = Mathf.Lerp(movementControls.Speed, Stats().engineThrottle, Time.deltaTime * Stats().accelerate);

        ShipTransform().transform.localRotation = Quaternion.Slerp(ShipTransform().transform.localRotation, Quaternion.identity, Time.deltaTime * Stats().torquePower);


        // FocusObject().position = AttackTarget().position;

        GetShip().EnemyAct();



        timer_State -= Time.deltaTime;
        if (timer_State <= 0 )
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }



    public override void Adjusting()
    {


    }



    public override void Pathfinding() { }
    public override void Special() { }
    public override void Dying() 
    {

        movementControls.Torque = 2;
        movementControls.Speed = 1;
        FocusObject().position = ShipTransform().position + ShipTransform().right - ShipTransform().up + ShipTransform().forward;
        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }
    public override void Idle() { }
    public override void Dodging() { }
    public override void Fleeing() { }
    public override void Spawned() { }
    public override void Inactive() { }
    public override void Ragdolling() { }








    public RangeBand RangeTo()
    {
        if (Vector3.Distance(ShipTransform().position, AttackTarget().position) >= Stats().farRange * 2)
        {
            return RangeBand.extreme;
        }
        else if (Vector3.Distance(ShipTransform().position, AttackTarget().position) < Stats().closeRange * 0.5f)
        {
            return RangeBand.melee;
        }
        else if (Vector3.Distance(ShipTransform().position, AttackTarget().position) <= Stats().midRange)
        {
            return RangeBand.close;
        }
        else if (Vector3.Distance(ShipTransform().position, AttackTarget().position) > Stats().closeRange)
        {
            return RangeBand.mid;
        }

        return RangeBand.unknown;
    }



    public RangeBand RangeTo(Transform _target)
    {
        if (Vector3.Distance(ShipTransform().position, _target.position) >= Stats().farRange * 2)
        {
            return RangeBand.extreme;
        }
        else if (Vector3.Distance(ShipTransform().position, _target.position) < Stats().closeRange * 0.5f)
        {
            return RangeBand.melee;
        }
        else if (Vector3.Distance(ShipTransform().position, _target.position) <= Stats().midRange)
        {
            return RangeBand.close;
        }
        else if (Vector3.Distance(ShipTransform().position, _target.position) > Stats().closeRange)
        {
            return RangeBand.mid;
        }

        return RangeBand.unknown;
    }


    public RangeBand RangeTo(Transform _from,Transform _target)
    {
        //for conditions based around another object, like a treasure or fleetship [i.e.:do x when y is close to z]
        if (Vector3.Distance(_from.position, _target.position) >= Stats().farRange * 2)
        {
            return RangeBand.extreme;
        }
        else if (Vector3.Distance(_from.position, _target.position) <= Stats().midRange)
        {
            return RangeBand.close;
        }
        else if (Vector3.Distance(_from.position, _target.position) > Stats().closeRange)
        {
            return RangeBand.mid;
        }

        return RangeBand.unknown;
    }



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
            Vector3 away = ShipTransform().position + (ShipTransform().forward * Stats().farRange);

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

