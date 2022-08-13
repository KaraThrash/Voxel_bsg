using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : Enemy
{
    public WeaponBase gun;

    public MeshRenderer render;
    public Material deadColor;




    public override void Init()
    {
      
    }


    public override void Act()
    {




        if (timer_Brain > 0) { timer_Brain -= Time.deltaTime; }


        //if (ship.Hitpoints() <= 0)
        //{
        //    Dying();
        //    return;

        //}



        Attacking();

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

            if (render && deadColor)
            { render.material = deadColor; }

            return;

        }


        

      



    }



    public override void AttackWindUp()
    {


    }

    public override void Attacking()
    {



    

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

  

        timer_State -= Time.deltaTime;
        if (timer_State <= 0)
        {
            // timer_State = StateTime();

            // MakeDecision();

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


    

    }


 

}