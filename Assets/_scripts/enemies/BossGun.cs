using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : Enemy
{
    public WeaponBase gun;

    public Animator anim;

    public MeshRenderer render;

    public Material deadColor;

    public Collider weakSpotCollider;

    private string AP_open = "open";

    public void ToggleOpenFace()
    {
        if (weakSpotCollider == null)
        { weakSpotCollider = GetComponent<Collider>(); }

        if (weakSpotCollider)
        {
            if (weakSpotCollider.enabled)
            {
                weakSpotCollider.enabled = false;
            }
            else
            {
                weakSpotCollider.enabled = true;
            }




            if (anim)
            { anim.SetTrigger(AP_open); }
        }
    
    }


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



     //   Attacking();

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