using UnityEngine;

public class BasicEnemy : Enemy
{
    public ShipPhysics movementControls;
    public ShipSystem gun;

    public Transform focusObject; //the object the polarith AI is trying to move towards. Use Polarith for general navigating, and set polarith to zero for specific actions
    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform patrolTarget;
    private int count_PatrolPoint;

    private Vector3 targetVelocity;

    public override void Act()
    {


        if (AttackTarget() == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }

        if (GetShip().Chasis() && GetShip().Chasis().ExternalForce() != Vector3.zero)
        {
            //movementControls.Torque = 0;
            //movementControls.Speed = 0;

            RB().velocity = (GetShip().Chasis().ExternalForce());
            return;
        }

        if (ship.Hitpoints() <= 0)
        {
            Dying();
            return;

        }


        if (State() == AiState.idle)
        {
            Idle();

        }
        else 
        {
            RaycastHit hit;

            if (Physics.SphereCast(ShipTransform().position, 1.0f, (AttackTarget().position - ShipTransform().position), out hit) && hit.transform == AttackTarget())
            {
                timer_inView += (Time.deltaTime * 2);

                // return;
            }
            else
            {
                if (timer_inView > 0)
                {
                    timer_inView -= Time.deltaTime;

                }
            }

            if (timer_inView > 0)
            {
                
                if (State() == AiState.moving)
                {
                    Moving();

                }
                else { Attacking(); }
            }
            else
            {
                Moving();
            }
        }


        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }


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
        else if (State() == AiState.idle)
        {
            Idle();

        }
        else { Idle(); }


    }


    public override void MakeDecision()
    {

        if (ship.Hitpoints() <= 0)
        {

            if (effect_Explosion != null)
            {
                effect_Explosion.transform.parent = null;
                effect_Explosion.transform.position = ship.transform.position;
                effect_Explosion.SetActive(true);
            }
            
            ship.Die();

            return;

        }

        AiState newState = State();

        RelativeFacing facing = AiBrain.Facing(ShipTransform(), AttackTarget());
        float distToPlayer = Vector3.Distance(AttackTarget().position, GetShip().transform.position);
        RangeBand currentRange = RangeTo(FocusObject(), ShipTransform());


        if (distToPlayer <= Stats().leashDistance && timer_inView > 0)
        {
            

            
            if ((facing == RelativeFacing.behind || facing == RelativeFacing.chicken || facing == RelativeFacing.backToBack) && currentRange  != RangeBand.melee)
            {
                SetStance(Stance.neutral);
                State(AiState.attacking);

                if (facing == RelativeFacing.chicken)
                { SetStance(Stance.defensive); }
                else  if (facing == RelativeFacing.backToBack || currentRange == RangeBand.far || currentRange == RangeBand.extreme)
                { SetStance(Stance.aggressive); }

            }
            else
            {
                FocusObject().position = AttackTarget().position - AttackTarget().forward * Stats().closeRange;
                State(AiState.moving); 
            }

            if (State() == AiState.idle)
            {
            }


        }
        else
        {
            if (State() == AiState.attacking)
            {

                State(AiState.postAttack);


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


                }
            }
        }


        if (State() == AiState.idle)
        {
            if (gun != null) { gun.Deactivate(); }


        }
        else if (State() == AiState.moving)
        {
            if (gun != null) { gun.Deactivate(); }


        }
        else
        {

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

        }

     

    }



    public override void AttackWindUp()
    {
       

    }

    public override void Attacking()
    {
        movementControls.Speed = 0;
        movementControls.Torque = 0;

        
        float secondaryFacing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);

        //  FocusObject().position = AttackTarget().position;// - (AttackTarget().forward * 3);


        if (RangeTo(FocusObject(), ShipTransform()) != RangeBand.melee)
        {
            Quaternion rot = Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up);
            float pwr = Time.deltaTime * Stats().torquePower;
            ShipTransform().transform.rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);
        }

        Vector3 newVel = (AttackTarget().position - ShipTransform().position).normalized;

        if (GetStance() == Stance.aggressive)
        {
            newVel =  ShipTransform().forward ;
        }
        else if (GetStance() == Stance.neutral)
        {
            newVel = Vector3.zero;
        }
        else 
        {
            

            if (GetSubID() == SubID.A )
            {
                newVel = (newVel + AttackTarget().right).normalized;
            }
            else if(GetSubID() == SubID.B)
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


        RB().velocity = Vector3.Lerp(RB().velocity, newVel * Stats().engineThrottle, Time.deltaTime);






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

       // ShipTransform().transform.localRotation = Quaternion.Slerp(ShipTransform().transform.localRotation, Quaternion.identity, Time.deltaTime * Stats().torquePower);


        // FocusObject().position = AttackTarget().position;

        GetShip().EnemyAct();



        timer_State -= Time.deltaTime;
        if (timer_State <= 0 )
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
    public override void Idle() 
    {
        if (gun != null) { gun.Deactivate(); }
        //  movementControls.Torque = Stats().torquePower;
        // movementControls.Speed = Stats().engineThrottle;

        Quaternion toFace = Quaternion.LookRotation(FocusObject().position - GetShip().transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, toFace, Mathf.Min(Stats().torquePower * Time.deltaTime, 1));

        GetShip().transform.position += GetShip().transform.forward * Time.deltaTime * Stats().engineThrottle;

        float dist = Vector3.Distance(focusObject.position, GetShip().transform.position);

        if (dist <= Stats().rangeVarience )
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


    public RangeBand RangeTo(Transform _from,Transform _target)
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

