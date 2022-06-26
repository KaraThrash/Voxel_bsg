using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Ship
{
public Transform tankBody;
public Transform tankTurret,turretRestSpot;
public override void Act()
{

    RechargeStamina();

    Movement();

    Weapons();


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

    tankTurret.position = turretRestSpot.position;

    //get engines target velocity
    //

    float accel = 0;
    Vector3 newVel = Vector3.zero;

        

    if (PrimaryEngine())
    {
        //Get the engine's intended output


        newVel = PrimaryEngine().GetTargetVelocity() * primaryEngine.LinearAcceleration();
        Vector3 bodyFwd = transform.forward;

        //the tank only moves forward on its treads
        //the body+turret should be independent of each other so we get the forward based on the body not the whole ship
        //  bodyFwd = new Vector3(bodyFwd.x * newVel.x, bodyFwd.y * newVel.y, bodyFwd.z * newVel.z).normalized;
        newVel = bodyFwd * newVel.magnitude;

        accel = primaryEngine.LinearAcceleration();

    }


    //NOTE: forces like recoild from a railgun or ground collisions are included here
    if (Chasis())
    {
        //newVel = Chasis().ApplyExternalForces(newVel);

    }


    if (Map() && Map().OutOfBounds(transform.position))
    {
        //  TurnBack();
        // RB().velocity = newVelocity.magnitude * transform.forward;

        //  return;
    }

    if ( tankTurret != null && tankBody != null)
    {
        Quaternion newrot = Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * 10)) - tankTurret.position);
        tankTurret.rotation = Quaternion.Slerp(tankTurret.rotation, newrot, primaryEngine.RotationAcceleration());

        if (newVel.magnitude != 0 )
        {

            RotateTankBody(primaryEngine.RotationAcceleration());

        }

    }


        RB().AddForce( newVel * Time.deltaTime,ForceMode.Impulse);


    if (PrimaryEngine())
    {
        //note: part of a test for a less rigid camera
        //  Quaternion newrot = Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * 10)) - transform.position);

        //transform.rotation = Quaternion.Slerp(transform.rotation, primaryEngine.GetTargetRotation(), primaryEngine.RotationAcceleration());

    }

}


public void RotateTankBody(float _acc)
{

    Vector3 tankRight = tankBody.position + tankBody.right;
    Vector3 turretForward = tankTurret.position + tankTurret.forward;


    float agl = Vector3.Angle((tankBody.position + tankBody.up).normalized, (tankTurret.position + tankTurret.forward).normalized);

    if (Mathf.Abs(tankTurret.eulerAngles.y - tankBody.eulerAngles.y) > 1)
    {
            if (tankTurret.eulerAngles.y - tankBody.eulerAngles.y < 0)
            {
                tankBody.Rotate(-tankBody.up * _acc);
            }
            else
            {
                tankBody.Rotate(tankBody.up * _acc);
            }
    }


}

}
