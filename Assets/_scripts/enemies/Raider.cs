using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Raider : Enemy
{
    public ShipPhysics movementControls;
    public ShipSystem gun;

    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform patrolTarget;
    private int count_PatrolPoint;

    private Vector3 targetVelocity;

    private Vector3 moveDir;
    private Vector3 lookAt;


    public override void Act()
    {


        if (AttackTarget() == null || ship == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }

        


        if (ship.Hitpoints() <= 0)
        {
            //is dead
            Dying();
            return;

        }

        if (GetShip().Chasis() && GetShip().Chasis().ExternalForce() != Vector3.zero)
        {
            RB().velocity = (GetShip().Chasis().ExternalForce()) * 1;
            RB().angularVelocity = (GetShip().Chasis().ExternalForce()) * 10;
            return;
        }





        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();
            return;
        }

        if (State() == AiState.idle)
        {
            float pwr = Time.deltaTime * Stats().torquePower;
            ShipTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.LookRotation(FocusObject().position - ShipTransform().position, -ShipTransform().forward), pwr);
            RB().velocity = Vector3.Lerp(RB().velocity, GetShip().transform.forward.normalized * Stats().engineThrottle, Stats().accelerate * Time.deltaTime);

            if (Vector3.Distance(FocusObject().position, ShipTransform().position) < Stats().rangeVarience)
            {
                count_patrolPoint++;
                FocusObject().position = Map().GetNextPatrolPoint(count_patrolPoint);
            }
            // count_patrolPoint

            if (CheckInView(AttackTarget()))
            {
                timer_inView += Time.deltaTime;

                if (timer_inView >= BrainTime())
                { SetStance(Stance.neutral) ; }

            }
        }
        else
        {

            float pwr = Time.deltaTime * Stats().torquePower;
            ShipTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, ShipTransform().up), pwr);

            if (Vector3.Distance(FocusObject().position, ShipTransform().position) > Stats().rangeVarience)
            {
                RB().velocity = Vector3.Lerp(RB().velocity, (FocusObject().position - GetShip().transform.position).normalized * Stats().engineThrottle, Stats().accelerate * Time.deltaTime);
            }


        }


        





        //if (State() == AiState.adjusting)
        //{
        //    Adjusting();
        //}
        //else if (State() == AiState.attacking)
        //{
        //    Attacking();

        //}
        //else if (State() == AiState.recovering)
        //{
        //    Recovering();

        //}
        //else if (State() == AiState.moving)
        //{
        //    Moving();

        //}
        //else if (State() == AiState.attackWindUp)
        //{
        //    AttackWindUp();

        //}
        //else if (State() == AiState.postAttack)
        //{
        //    PostAttack();

        //}
        //else if (State() == AiState.idle)
        //{
        //    Idle();

        //}
        //else { Idle(); }


    }


    public override bool CheckInView(Transform _lookingFor)
    {
        RaycastHit hit;

        if (Physics.SphereCast(ShipTransform().position, 1.0f, (_lookingFor.position - ShipTransform().position), out hit) && hit.transform == _lookingFor)
        {

            return true;
        }

        return false;
    }

    

    public override void MakeDecision()
    {

        if (ship.Hitpoints() <= 0)
        {
            Explode();

            EnemyManager().GameManager().GetEnemyDeathEvent().Invoke(this);
            ship.Die();
            return;

        }

        AiState newState = State();

        RelativeFacing facing = AiBrain.Facing(ShipTransform(), AttackTarget());
        float distToPlayer = Vector3.Distance(AttackTarget().position, GetShip().transform.position);
        RangeBand currentRange = RangeTo(FocusObject(), ShipTransform());
        Vector3 distPast = Vector3.zero;

        if (distToPlayer <= Stats().leashDistance && timer_inView > 0)
        {


            if (currentRange == RangeBand.melee)
            {
                SetStance(Stance.aggressive);
                State(AiState.attacking);
                //distPast = (AttackTarget().position - ShipTransform().position).normalized * Stats().farRange;
                distPast = (AttackTarget().transform.right + AttackTarget().transform.up + AttackTarget().transform.forward) * Stats().farRange;
                FocusObject().position = AttackTarget().position + distPast;

            }
            else if ((facing == RelativeFacing.behind || facing == RelativeFacing.backToBack))
            {
                SetStance(Stance.neutral);
                State(AiState.attacking);
                distPast = (AttackTarget().position - ShipTransform().position).normalized * Stats().farRange;
                FocusObject().position = AttackTarget().position + distPast;

            }
            else
            {
                distPast = AttackTarget().forward * Stats().farRange;
                FocusObject().position = AttackTarget().position - distPast;
                State(AiState.moving);
            }

            if (GetShip().PrimaryWeapon())
            {
                float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);
                if (secondaryFacing < Stats().angleTolerance)
                {
                    if (gun != null) { gun.Activate(); }
                }
                else { if (gun != null) { gun.Deactivate(); } }
            }


        }
        else
        {
            if (State() == AiState.attacking)
            {

                State(AiState.postAttack);
                if (gun != null) { gun.Deactivate(); }

            }
            else
            {

                if (State() == AiState.postAttack)
                {
                    if (patrolTarget != null && patrolTarget.childCount > 0)
                    {

                        FocusObject().position = patrolTarget.GetChild(0).position;

                    }

                    State(AiState.idle);
                    if (gun != null) { gun.Deactivate(); }

                }
            }
        }


        if (GetShip().PrimaryWeapon())
        {
            float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);
            if (secondaryFacing < Stats().angleTolerance)
            {
                if (gun != null) { gun.Activate(); }
            }
            // else { if (gun != null) { gun.Deactivate(); } }
        }




    }

    public void Explode()
    {
        if (effect_Explosion != null)
        {
            effect_Explosion.transform.parent = null;
            effect_Explosion.transform.position = ship.transform.position;
            effect_Explosion.SetActive(true);
        }

        
    }

    public override void AttackWindUp()
    {


    }

    public override void Attacking()
    {
        if (gun != null) { gun.Act_Fixed(); }

        movementControls.Speed = 0;
        movementControls.Torque = 0;


        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        //  FocusObject().position = AttackTarget().position;// - (AttackTarget().forward * 3);



        Quaternion rot = Quaternion.identity;
        Vector3 newVel = (AttackTarget().position - ShipTransform().position).normalized;

        if (GetStance() == Stance.aggressive)
        {
            rot = Quaternion.LookRotation(FocusObject().position - ShipTransform().position, AttackTarget().up);
            newVel = ShipTransform().forward;
        }
        //else if (GetStance() == Stance.neutral)
        //{
        //    rot = Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up);
        //    newVel = Vector3.zero;
        //}
        else
        {
            rot = Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up);

            if (GetSubID() == SubID.A)
            {
                newVel = (newVel + AttackTarget().right).normalized;
            }
            else if (GetSubID() == SubID.B)
            {
                newVel = (newVel - AttackTarget().right).normalized;
            }
            else if (GetSubID() == SubID.C)
            {
                newVel = (newVel + AttackTarget().up).normalized;
            }
            else
            {
                newVel = (newVel - AttackTarget().up).normalized;
            }

        }


        RB().velocity = Vector3.Lerp(RB().velocity, newVel * Stats().engineThrottle, Stats().accelerate * Time.deltaTime);

        float pwr = Time.deltaTime * Stats().torquePower;
        ShipTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);




    }

    public override void PostAttack()
    {
        RB().velocity = Vector3.Lerp(RB().velocity, Vector3.zero, Stats().accelerate * Time.deltaTime);
        float pwr = Time.deltaTime * Stats().torquePower;
        ShipTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, Quaternion.identity, pwr);
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

        // ShipTransform().transform.localRotation = Quaternion.Slerp(ShipTransform().transform.localRotation, Quaternion.identity, Time.deltaTime * Stats().torquePower);


        // FocusObject().position = AttackTarget().position;

        GetShip().EnemyAct();



        timer_State -= Time.deltaTime;
        if (timer_State <= 0)
        {
            // timer_State = StateTime();

            // MakeDecision();

        }
    }



    public override void Adjusting()
    {


    }



    public override void Pathfinding() { }
    public override void Special() { }
    public override void Dying()
    {

        FocusObject().position = ShipTransform().position + ShipTransform().right - ShipTransform().up + ShipTransform().forward;
        timer_State -= Time.deltaTime;

       // RB().angularVelocity = transform.forward * 12;
       // RB().velocity = Vector3.down * 5;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }
    public override void Idle()
    {
        if (gun != null) { gun.Deactivate(); }
        //  movementControls.Torque = Stats().torquePower;
        // movementControls.Speed = Stats().engineThrottle;

        Quaternion toFace = Quaternion.LookRotation(FocusObject().position - GetShip().transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, toFace, Mathf.Min(Stats().torquePower * Time.deltaTime, 1));

       // GetShip().transform.position += GetShip().transform.forward * Time.deltaTime * Stats().engineThrottle;

        float dist = Vector3.Distance(focusObject.position, GetShip().transform.position);

        if (dist <= Stats().rangeVarience)
        {
            count_PatrolPoint++;


            if (patrolTarget != null && patrolTarget.childCount > 0)
            {
                if (patrolTarget.childCount <= count_PatrolPoint)
                { count_PatrolPoint = 0; }

                FocusObject().position = patrolTarget.GetChild(count_PatrolPoint).position;

            }
            else
            {
                FocusObject().position = (GetShip().transform.position - GetShip().transform.forward + GetShip().transform.up);
            }


        }
        else
        {

        }

        RB().velocity = Vector3.Lerp(RB().velocity, GetShip().transform.forward * Stats().engineThrottle, Stats().accelerate * Time.deltaTime);


    }
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


    public RangeBand RangeTo(Transform _from, Transform _target)
    {
        //for conditions based around another object, like a treasure or fleetship [i.e.:do x when y is close to z]
        if (Vector3.Distance(_from.position, _target.position) >= Stats().farRange * 2)
        {
            return RangeBand.extreme;
        }
        else if (Vector3.Distance(_from.position, _target.position) < Stats().closeRange * 0.5f)
        {
            return RangeBand.melee;
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