using UnityEngine;


public class TestEnemy : Enemy
{
    public WeaponBase gun;

    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform patrolTarget;
    private int count_PatrolPoint;

    private Vector3 targetVelocity;
    private Vector3 moveTo_Position;
    private Vector3 lookAt_Position;



    public override void Init()
    {
        GetSubID();
        if (EnemyManager())
        {
            EnemyManager().AddEnemyToList(this);

        }

        if (AttackTarget() == null || GetShip() == null)
        {
            AttackTarget(EnemyManager().AttackTarget());

        }


        moveTo_Position = transform.position + transform.forward * stats.closeRange;
        lookAt_Position = transform.position + transform.forward * stats.closeRange;


        //FocusObject().position = Map().GetNextPatrolPoint(count_patrolPoint);
        FocusObject().position = AttackTarget().position;


        State(AiState.attacking);


        if (Stats())
        {
            if (gun != null)
            {
                gun.bulletsPerBurst = Stats().bulletsPerBurst;
                gun.STAT_CooldownTime( Stats().firerate);
                gun.burstTime =  Stats().timeBetweenBursts;
            }

        }

        stateTime = 5;
        brainTime = 12;
        timer_Brain = brainTime;
        directionChangeSpeed = 12;
    }


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
        SetEngineParticles();







        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();
            return;
        }

        Attacking();

        //if (GetShip().PrimaryWeapon())
        //{
        //    float secondaryFacing = Vector3.Angle(ShipTransform().forward, (lookAt_Position - ShipTransform().position).normalized);
        //    if (secondaryFacing < Stats().angleTolerance)
        //    {
        //        if (gun != null) { gun.Activate(); }
        //    }
        //     else { if (gun != null) { gun.Deactivate(); } }
        //}




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

            if (effect_Explosion != null)
            {
                effect_Explosion.transform.parent = null;
                effect_Explosion.transform.position = ship.transform.position;
                effect_Explosion.SetActive(true);
            }

            ship.Die();

            return;

        }



        if (GetSubID() == SubID.A)
        {
            FocusObject().position = AttackTarget().position;
            moveTo_Position = AttackTarget().position + (( MainTransform().position - AttackTarget().position ).normalized * Stats().closeRange);

            lookAt_Position = AttackTarget().position;
        }
        else if (GetSubID() == SubID.B)
        {
            FocusObject().position = AttackTarget().position;

            if (AttackTarget().GetComponent<Rigidbody>())
            {
                moveTo_Position = AttackTarget().position + AttackTarget().right + AttackTarget().GetComponent<Rigidbody>().velocity + (AttackTarget().right * Stats().closeRange);
                lookAt_Position = AttackTarget().position  + AttackTarget().GetComponent<Rigidbody>().velocity * Stats().closeRange;
            }
            else
            {
                moveTo_Position = AttackTarget().position + AttackTarget().right * Stats().closeRange;
                lookAt_Position = AttackTarget().position + AttackTarget().forward * Stats().closeRange;
            }

        }
        else 
        {
            FocusObject().position = AttackTarget().position;
            moveTo_Position = AttackTarget().position + ((MainTransform().position - AttackTarget().position).normalized * Stats().closeRange);
            moveTo_Position += AttackTarget().up * Random.Range(-1.0f, 1.0f);
            moveTo_Position += AttackTarget().forward * Random.Range(-1.0f, 1.0f);
            lookAt_Position = AttackTarget().position;
        }




        if (GetShip().PrimaryWeapon() && Vector3.Distance(moveTo_Position, ShipTransform().position) < Stats().midRange)
        {
            float secondaryFacing = Vector3.Angle(ShipTransform().forward, (lookAt_Position - ShipTransform().position).normalized);
            if (secondaryFacing < Stats().angleTolerance)
            {
                if (gun != null) { gun.Activate(); }
            }
             else { if (gun != null) { gun.Deactivate(); } }
        }


    }



    public override void AttackWindUp()
    {


    }

    public override void Attacking()
    {
        if (gun != null) { gun.Act_Fixed(); }




        Quaternion rot = Quaternion.LookRotation(lookAt_Position - ShipTransform().position, AttackTarget().up);
        Vector3 newVel = (moveTo_Position - ShipTransform().position).normalized;


        if (Vector3.Distance(moveTo_Position, ShipTransform().position) > Stats().closeRange)
        { 
            
            float facingAngle = Vector3.Angle(ShipTransform().forward, (lookAt_Position - ShipTransform().position).normalized);
            if (facingAngle < Stats().angleTolerance)
            {
                RB().velocity = Vector3.Lerp(RB().velocity, newVel * Stats().engineThrottle, Stats().accelerate * Time.deltaTime);
            }
        }

        float pwr = Time.deltaTime * Stats().torquePower;
        ShipTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);




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




}

