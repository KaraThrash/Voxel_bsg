using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLarge : Enemy
{
    public ShipPhysics movementControls;
    public ShipSystem gun;

    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform rotateTransform;
    public Transform parent_patrolPoints;



    private Vector3 targetVelocity;

    private Vector3 moveTo_Position;
    private Vector3 lookAt_Position;

    public GameObject prefab_enemy;

    public List<BossGun> bossGuns;


  


    public override void Init()
    {
        GetSubID();

        if (EnemyManager())
        {
            EnemyManager().AddEnemyToList(this);

        }

        if (AttackTarget() == null || GetShip() == null)
        {
            if (EnemyManager() && EnemyManager().AttackTarget())
            {
                AttackTarget(EnemyManager().AttackTarget());

            }

        }

        if (parent_patrolPoints && parent_patrolPoints.childCount > 0)
        {
            moveTo_Position = parent_patrolPoints.GetChild(0).position ;
            lookAt_Position = parent_patrolPoints.GetChild(0).position;
        }

     //   moveTo_Position = transform.position + transform.forward * stats.closeRange;
       // lookAt_Position = transform.position + transform.forward * stats.closeRange;


        //FocusObject().position = Map().GetNextPatrolPoint(count_patrolPoint);
        FocusObject().position = moveTo_Position;// AttackTarget().position;


        State(AiState.attacking);


       

        if (Stats())
        {

            foreach (BossGun el in Guns())
            {

                if (el.GetSubID() == SubID.A)
                {
                    el.ToggleOpenFace();

                }

                el.canAct = true;
                el.gun.on = true;
                el.gun.bulletsPerBurst = Stats().bulletsPerBurst;
                el.gun.STAT_CooldownTime(Stats().firerate);
                el.gun.burstTime = Stats().timeBetweenBursts;
            }


    


            stateTime = Stats().makeDecisionTime;
            brainTime = Stats().makeDecisionTime;
            timer_Brain = Stats().makeDecisionTime;
            directionChangeSpeed = 12;
            SetHitpoints(Stats().hitPoints);

        }
        else
        {
            stateTime = 5;
            brainTime = 12;
            timer_Brain = brainTime;
            directionChangeSpeed = 12;
            SetHitpoints(10);
        }


    }


    public override void Act()
    {


        if (AttackTarget() == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }



        if (ship && ship.Hitpoints() <= 0)
        {
            Dying();
            return;

        }


        if (State() == AiState.attacking)
        { Attacking(); }
        else if (State() == AiState.moving) 
        { 
            //Moving(); 
        }
        else { Idle(); }


        if (parent_patrolPoints && parent_patrolPoints.childCount > count_patrolPoint)
        {
            if (Vector3.Distance(moveTo_Position, transform.position) < 1)
            {
                count_patrolPoint++;
                if (count_patrolPoint >= parent_patrolPoints.childCount)
                { count_patrolPoint = 0; }

                moveTo_Position = parent_patrolPoints.GetChild(count_patrolPoint).position;
                lookAt_Position = parent_patrolPoints.GetChild(count_patrolPoint).position;
            }


            Moving();
        }




        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();
            return;
        }




    }


    public override void Attacking()
    {

        Quaternion rot = Quaternion.LookRotation(AttackTarget().position - ShipTransform().position, AttackTarget().up);
    

        float pwr = Time.deltaTime * Stats().torquePower;
      //  MainTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);
      //  MainTransform().Rotate(MainTransform().right * Stats().torquePower * 2 * Time.deltaTime);

     // rotateTransform.Rotate(0,0,  Stats().torquePower * 5 * Time.deltaTime);
    }

    public override void Moving()
    {

        Quaternion rot = Quaternion.LookRotation(moveTo_Position - ShipTransform().position, ShipTransform().up);


        float pwr = Time.deltaTime * Stats().torquePower;
        MainTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);

        RB().velocity = MainTransform().forward * Stats().engineThrottle ;

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

        if (ship && ship.Hitpoints() <= 0)
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

        


        //float facing = Vector3.Angle(ShipTransform().forward, (AttackTarget().position - ShipTransform().position).normalized);
        //if (facing < Stats().angleTolerance)
        //{

        //    foreach (BossGun el in Guns())
        //    {

        //        if (el.GetSubID() == SubID.A)
        //        {
        //            el.canAct = true;
        //            el.gun.on = true;

        //        }
        //        else
        //        {
                    
        //            el.canAct = false;
        //            el.gun.on = false;
        //        }
                

        //    }


    
        //}
        //else if (facing > 180 - Stats().angleTolerance)
        //{
        //    foreach (BossGun el in Guns())
        //    {

        //        if (el.GetSubID() == SubID.B)
        //        {
        //            if (el.Hitpoints() > 0)
        //            {
        //                EnemyManager().SpawnEnemy(prefab_enemy, el.gun.transform);
        //            }

        //        }
        //        else
        //        {
        //            el.canAct = false;
        //            el.gun.on = false;
        //        }


        //    }
        //}



        if (State() == AiState.attacking)
        {
            State(AiState.moving);
            timer_State = StateTime() * 1;

            foreach (BossGun el in Guns())
            {

                if (el.GetSubID() == SubID.B)
                {
                    if (el.Hitpoints() > 0)
                    {
                        EnemyManager().SpawnEnemy(prefab_enemy, el.gun.transform);
                    }

                }
                else
                {
                    el.canAct = false;
                    el.gun.on = false;
                }


            }

        }
        else if (State() == AiState.idle) { State(AiState.moving); }
        else 
        {
            foreach (BossGun el in Guns())
            {

                if (el.GetSubID() == SubID.B)
                {
                    if (el.Hitpoints() > 0)
                    {
                        EnemyManager().SpawnEnemy(prefab_enemy, el.gun.transform);
                    }

                }
                else
                {
                    el.canAct = true;
                    el.gun.on = true;
                }


            }
            State(AiState.attacking);

        }
    }


    public override void Dying()
    {

 
        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();

        }
    }


    public override void Idle()
    {

        //Quaternion rot = Quaternion.LookRotation( ShipTransform().position - AttackTarget().position , AttackTarget().up);


        //float pwr = Time.deltaTime * Stats().accelerate;
        //MainTransform().rotation = Quaternion.Slerp(ShipTransform().transform.rotation, rot, pwr);


        //RB().velocity += MainTransform().forward * Stats().decelerate * Time.deltaTime;



        //timer_State -= Time.deltaTime;

        //if (timer_State <= 0)
        //{
        //    timer_State = StateTime();

        //    MakeDecision();

        //}
    }





    public override void OnStateChange(AiState _newstate)
    {
       


    }


    public List<BossGun> Guns()
    {
        if (bossGuns == null || bossGuns.Count == 0)
        {
            List<Transform> rndList = new List<Transform>();
            bossGuns = new List<BossGun>();

            UniversalFunctions.GetDeepChildren(transform, rndList);

            foreach (Transform el in rndList)
            {
                if (el.GetComponent<BossGun>())
                {
                    bossGuns.Add(el.GetComponent<BossGun>());
                }

            }

        }

        return bossGuns;
    }



}