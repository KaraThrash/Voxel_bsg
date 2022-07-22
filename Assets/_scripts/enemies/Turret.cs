using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    public WeaponBase gun;

    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform patrolTarget;
    public Transform head_turret;
    private int count_PatrolPoint;

    private Vector3 targetVelocity;
    private Vector3 moveTo_Position;
    private Vector3 lookAt_Position;


    public MeshRenderer render;
    public Material moveColor;
    public Material attackColor;
    public Material outOfRangeColor;



    public override void  Init()
    {
        GetSubID();

        if (EnemyManager())
        {
            EnemyManager().AddEnemyToList(this);

        }

        if (AttackTarget() == null || GetShip() == null)
        {
          //  AttackTarget(EnemyManager().AttackTarget());
          //  AttackTarget(FocusObject());

        }


        moveTo_Position = transform.position + transform.forward * stats.closeRange;
        lookAt_Position = transform.position + transform.forward * stats.closeRange;


        //a boss turret is the dradis dish that controls the group
        if (GetSubID() == SubID.none || GetSubID() == SubID.Boss)
        {
            FocusObject().position = moveTo_Position;
        }

        State(AiState.attacking);


        if (Stats())
        {
            if (gun != null)
            {
                gun.bulletsPerBurst = Stats().bulletsPerBurst;
                gun.STAT_CooldownTime(Stats().firerate);
                gun.burstTime = Stats().timeBetweenBursts;
            }
            stateTime = Stats().makeDecisionTime;
            brainTime = Stats().makeDecisionTime;
            timer_Brain = Stats().makeDecisionTime;
            directionChangeSpeed = 12;
        }
        else
        {
            stateTime = 5;
            brainTime = 12;
            timer_Brain = brainTime;
            directionChangeSpeed = 12;
        }

    
    }


    public override void Act()
    {


        if (AttackTarget() == null) { return; }
        if (head_turret == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }

     
        //if (ship.Hitpoints() <= 0)
        //{
        //    Dying();
        //    return;

        //}

        SetEngineParticles();







        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();
            return;
        }

        if (GetSubID() == SubID.Boss  )
        {
            if (State() == AiState.attacking)
            {
                DradisDish();
            }
            else
            {
                MainTransform().Rotate(0,  Stats().torquePower * 10 * Time.deltaTime, 0);
            }
        }
        else
        {
            Idle();


            if (State() == AiState.attacking)
            {
                if (Vector3.Angle(head_turret.forward, focusObject.position - head_turret.position) <= Stats().angleTolerance)
                {
                    if (gun != null) { gun.Activate(); }

                }
                else { if (gun != null) { gun.Deactivate(); } }
            }
            else
            {
               // focusObject.position = MainTransform().position + MainTransform().right;
            }
            
        }
        

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

        //if (ship.Hitpoints() <= 0)
        //{

        //    if (effect_Explosion != null)
        //    {
        //        effect_Explosion.transform.parent = null;
        //        effect_Explosion.transform.position = ship.transform.position;
        //        effect_Explosion.SetActive(true);
        //    }

        //    ship.Die();

        //    return;

        //}


        if (AttackTarget() && DistanceToTarget(AttackTarget()) > Stats().noticePlayerDistance)
        {

            if (render && outOfRangeColor)
            { render.material = outOfRangeColor; }

            State(AiState.idle);

            return;
        }
        else { State(AiState.attacking); }



        RaycastHit hit;

        if (Physics.SphereCast(MainTransform().position, 0.5f,AttackTarget().position - MainTransform().position, out hit))
        {
            if (hit.transform != AttackTarget())
            {
                if (AttackTarget().GetComponent<Ship>() && Physics.Raycast(MainTransform().position, AttackTarget().GetComponent<Ship>().MainTransform().position - MainTransform().position, out hit))
                {
                    if (hit.transform == AttackTarget())
                    {
                        if (GetSubID() == SubID.Boss || GetSubID() == SubID.none)
                        {
                            FocusObject().position = AttackTarget().position ;
                        }

                        if (render && moveColor)
                        {
                            render.material = attackColor;
                        }

                        return;
                    }
                }

                if (render && moveColor)
                {
                    render.material = moveColor;
                }

                return;
            }
            else
            {
                if ( GetSubID() == SubID.none || GetSubID() == SubID.Boss)
                {
                    FocusObject().position = AttackTarget().position ;
                }

                if (render && attackColor)
                {
                    render.material = attackColor;
                }
            }
        }

        //a boss turret is the dradis dish that controls the group
        if (GetSubID() == SubID.none )
        {
            FocusObject().position = AttackTarget().position + AttackTarget().forward * Stats().closeRange;
        }
        

        if (AttackTarget().GetComponent<Rigidbody>())
        {
            moveTo_Position = AttackTarget().position + AttackTarget().right + AttackTarget().GetComponent<Rigidbody>().velocity + (AttackTarget().right * Stats().closeRange);
            lookAt_Position = AttackTarget().position + AttackTarget().GetComponent<Rigidbody>().velocity * Stats().closeRange;
        }
        else
        {
            moveTo_Position = AttackTarget().position + AttackTarget().right * Stats().closeRange;
            lookAt_Position = AttackTarget().position + AttackTarget().forward * Stats().closeRange;
        }




       


    }



    public override void AttackWindUp()
    {


    }

    public override void Attacking()
    {

        if (gun != null) { gun.Act_Fixed(); }


        if (Vector3.Angle(head_turret.forward, focusObject.position - head_turret.position) <= Stats().angleTolerance)
        {
        

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
       

        float xrot = Vector3.SignedAngle(head_turret.forward, focusObject.position - head_turret.position, head_turret.right);
        float yrot = Vector3.SignedAngle(MainTransform().forward, focusObject.position - MainTransform().position, MainTransform().up);


        if (Vector3.Angle(MainTransform().up, focusObject.position - MainTransform().position) <= Stats().angleTolerance )
        {


            yrot = 0;

        }

        if (Vector3.Angle(MainTransform().forward, focusObject.position - head_turret.position) + (xrot * Stats().torquePower * Time.deltaTime) > Stats().rangeVarience )
        {
            xrot = 0;

        }

        if (Vector3.Angle(head_turret.forward, focusObject.position - head_turret.position) <= Stats().angleTolerance)
        {
            xrot = 0;

        }

        head_turret.Rotate(xrot * Stats().torquePower * Time.deltaTime,0,0);
        MainTransform().Rotate(0, yrot * Stats().torquePower * Time.deltaTime,0);

    }


    public void DradisDish()
    {
        float xrot = Vector3.SignedAngle(head_turret.forward, AttackTarget().position - head_turret.position, head_turret.right);
        float yrot = Vector3.SignedAngle(MainTransform().forward, AttackTarget().position - MainTransform().position, MainTransform().up);


        if (Vector3.Angle(MainTransform().up, AttackTarget().position - MainTransform().position) <= Stats().angleTolerance)
        {


            yrot = 0;

        }

        if (Vector3.Angle(MainTransform().forward, AttackTarget().position - head_turret.position) + (xrot * Stats().torquePower * Time.deltaTime) > Stats().rangeVarience)
        {
            xrot = 0;

        }

        if (Vector3.Angle(head_turret.forward, AttackTarget().position - head_turret.position) <= Stats().angleTolerance)
        {
            xrot = 0;

        }

        head_turret.Rotate(xrot * Stats().torquePower * Time.deltaTime, 0, 0);
        MainTransform().Rotate(0, yrot * Stats().torquePower * Time.deltaTime, 0);
    }

}

