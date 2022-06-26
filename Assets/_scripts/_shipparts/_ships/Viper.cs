using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viper : Ship
{

    public override void Act()
    {

        if (timer_lockOutStaminaRecharge > 0)
        {
            timer_lockOutStaminaRecharge -= Time.deltaTime;
        }
        else { RechargeStamina(); }

        Movement();

        Weapons();

        if (PrimaryEngine())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, PrimaryEngine().GetTargetRotation(), PrimaryEngine().RotationAcceleration());

        }
    }




    public override void Weapons()
    {
        if (PrimaryWeapon())
        {
            PrimaryWeapon().Act();
        }

    }


    public override void Movement()
    {
        Vector3 newVelocity = Vector3.zero;

        if (PrimaryEngine())
        {
            //Get the engine's intended output
            newVelocity = PrimaryEngine().GetTargetVelocity() * primaryEngine.LinearAcceleration();
        }

        //NOTE: forces like recoild from a railgun or ground collisions are included here
        if (Chasis())
        {
            newVelocity = Chasis().ApplyExternalForces(newVelocity);

        }


        if (Map() && Map().OutOfBounds(transform.position))
        {
            //  TurnBack();
            // RB().velocity = newVelocity.magnitude * transform.forward;

            //  return;
        }

        RB().velocity = newVelocity;

    }


    public void RotateTankBody(float _acc)
    {



    }

}
