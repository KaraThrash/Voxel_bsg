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

    public  void Start()
    {
        GetSubID();
        if (EnemyManager())
        {
            EnemyManager().AddEnemyToList(this);

        }

        if (AttackTarget() == null || GetShip() == null)
        {
          //  AttackTarget(EnemyManager().AttackTarget());
            AttackTarget(FocusObject());

        }


        moveTo_Position = transform.position + transform.forward * stats.closeRange;
        lookAt_Position = transform.position + transform.forward * stats.closeRange;


        //FocusObject().position = Map().GetNextPatrolPoint(count_patrolPoint);
        FocusObject().position = moveTo_Position;// AttackTarget().position;


        State(AiState.attacking);


        if (Stats())
        {
            if (gun != null)
            {
                gun.bulletsPerBurst = Stats().bulletsPerBurst;
                gun.STAT_CooldownTime(Stats().firerate);
                gun.burstTime = Stats().timeBetweenBursts;
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

        Idle();

        //if (GetShip().PrimaryWeapon())
        //{
        //    float secondaryFacing = Vector3.Angle(ShipTransform().forward, (lookAt_Position - ShipTransform().position).normalized);
        //    if (secondaryFacing < Stats().angleTolerance)
        //    {
        //        if (gun != null) { gun.Activate(); }
        //    }
        //     else { if (gun != null) { gun.Deactivate(); } }
        //}

        if (Vector3.Angle(head_turret.forward, focusObject.position - head_turret.position) <= Stats().angleTolerance)
        {
            if (gun != null) { gun.Activate(); }

        }
        else { if (gun != null) { gun.Deactivate(); } }



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



        RaycastHit hit;
        if (Physics.Raycast(MainTransform().position, AttackTarget().position - MainTransform().position, out hit))
        {
            if (hit.transform != AttackTarget())
            {
                //Vector3 newpos = GameManager().MapManager().GetMap().GetClosestPatrolPoint(MainTransform().position + (MainTransform().forward * stats.closeRange));

                //moveTo_Position = newpos;
                //lookAt_Position = newpos;



                if (render && moveColor)
                {
                    render.material = moveColor;
                }

                return;
            }
            else
            {
                if (render && attackColor)
                {
                    render.material = attackColor;
                }
            }
        }


        FocusObject().position = AttackTarget().position + AttackTarget().forward * Stats().closeRange;

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


        //if (GetSubID() == SubID.A)
        //{
        //    FocusObject().position = AttackTarget().position;
        //    moveTo_Position = AttackTarget().position + ((MainTransform().position - AttackTarget().position).normalized * Stats().closeRange);

        //    lookAt_Position = AttackTarget().position;
        //}
        //else if (GetSubID() == SubID.B)
        //{
        //    FocusObject().position = AttackTarget().position;

        //    if (AttackTarget().GetComponent<Rigidbody>())
        //    {
        //        moveTo_Position = AttackTarget().position + AttackTarget().right + AttackTarget().GetComponent<Rigidbody>().velocity + (AttackTarget().right * Stats().closeRange);
        //        lookAt_Position = AttackTarget().position + AttackTarget().GetComponent<Rigidbody>().velocity * Stats().closeRange;
        //    }
        //    else
        //    {
        //        moveTo_Position = AttackTarget().position + AttackTarget().right * Stats().closeRange;
        //        lookAt_Position = AttackTarget().position + AttackTarget().forward * Stats().closeRange;
        //    }

        //}
        //else
        //{
        //    FocusObject().position = AttackTarget().position;
        //    moveTo_Position = AttackTarget().position + ((MainTransform().position - AttackTarget().position).normalized * Stats().closeRange);
        //    moveTo_Position += AttackTarget().up * Random.Range(-1.0f, 1.0f);
        //    moveTo_Position += AttackTarget().forward * Random.Range(-1.0f, 1.0f);
        //    lookAt_Position = AttackTarget().position;
        //}




       


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




}

