using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLarge : Enemy
{
    public ShipPhysics movementControls;
    public ShipSystem gun;

    public GameObject effect_Explosion;

    private float timer_inView; //track how long the target has been in view

    public Transform patrolTarget;
    private int count_PatrolPoint;

    private Vector3 targetVelocity;

    public override void Act()
    {


        if (AttackTarget() == null) { return; }

        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }



        if (ship.Hitpoints() <= 0)
        {
            Dying();
            return;

        }




        if (CheckInView(AttackTarget()))
        {

            if (timer_inView <= 0)
            { timer_inView = 0.5f; }
            else
            {
                timer_inView += (Time.deltaTime * 2);

            }

        }
        else
        {
            if (timer_inView > 0)
            {
                timer_inView -= Time.deltaTime;

            }
        }



        timer_State -= Time.deltaTime;

        if (timer_State <= 0)
        {
            timer_State = StateTime();

            MakeDecision();
            return;
        }




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




    



    public override void OnStateChange(AiState _newstate)
    {
       


    }




}