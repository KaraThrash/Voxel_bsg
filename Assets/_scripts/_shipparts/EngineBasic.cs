using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EngineBasic : EngineBase
{
    public LateralThruster lateralEngine;

    
    public Vector3 maneuverRotation;
    public float maneverRotCount;

    public float maneverCooldown;



    public override void StartManeuver(Maneuver _maneuver)
    {
        if (maneverCooldown > 0) { return; }

        if (_maneuver == Maneuver.spinAround)
        {
            maneverRotCount = 0;
            SetSystemState(SystemState.maneuver);
            maneuverRotation = transform.position - (transform.forward * 100);
        }
    }


    public override Quaternion GetTargetRotation()
    {

        return targetRotation;
    }


    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {
           
            Accelerate(throttle_A);

            if (maneverCooldown > 0) { maneverCooldown -= Time.deltaTime; }

        }
        else
        {
            if (current_Acceleration > 0)
            {
                Accelerate(0);
            }

        }

        if (GetSystemState() == SystemState.maneuver)
        {
            //maneverCooldown = 1;
            //Debug.Log(maneuverRotation);
            //float rot = Time.deltaTime * (180 );
            //maneverRotCount += rot;

            
            ////Ship().transform.Rotate(maneuverRotation * rot);
            //Ship().transform.rotation = Quaternion.Lerp(Ship().transform.rotation, Quaternion.LookRotation(maneuverRotation - Ship().transform.position, Ship().transform.up), Time.deltaTime * torquePower);

            //RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, Ship().Forward() * power * 0.15f, Time.deltaTime );

            //if (maneverRotCount >= 180 )
            //{
                
            //    SetSystemState(SystemState.on);
            //    lateralEngine.SetSystemState(SystemState.on);
            //}


        }
        else 
        {
            if (Ship() && Ship().CanAct() && STAT_Power() != 0)
            {

                if (throttle_A != 0)
                {
                    forwardVelocity = Vector3.Lerp(forwardVelocity, (Ship().Forward() * (STAT_Power() * current_Acceleration)) , Time.deltaTime );
                    forwardVelocity *= Ship().ShipInput().GetParameter(ShipInputParameters.thrust);
                }
                else
                {
                    //Vector3.zero +
                    forwardVelocity = Vector3.Lerp(forwardVelocity, forwardVelocity * current_Acceleration, Time.deltaTime);
                    forwardVelocity *= Ship().ShipInput().GetParameter(ShipInputParameters.thrust);
                }

            }


            targetRotation = Ship().RotationTarget();// Quaternion.Slerp(targetRotation, Ship().RotationTarget(), Time.deltaTime  *  STAT_Power() * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

            if (ship && Ship().CanAct() && torquePower != 0)
            {
               // Ship().transform.rotation
                   
            }
        }
        
    }






    public override Vector3 Lateral()
    {
        return (Ship().Right() * horizontal) + (Ship().Up() * vertical);
        //return new Vector3(ActOn().localPosition.x, ActOn().localPosition.y,0);

    }




    public void Accelerate(float _throttle)
    {
        float rate = rate_Acceleration * Time.deltaTime;

        if (_throttle == 0)
        {
            rate = -rate_Deceleration * Time.deltaTime;
        }

        if (PositiveButton())
        {
            //glide
        }
        else if (NegativeButton())
        {
            //brake
            current_Acceleration = Mathf.Clamp(current_Acceleration - (brakePower * Time.deltaTime), 0, 1);
        }
        else
        {
            current_Acceleration = Mathf.Clamp(current_Acceleration + rate, 0, 1);
        }

    }



    public override void Control(KeyCode _input)
    {


        if (GetSystemState() == SystemState.on)
        {
            if (_input == deactivateKey)
            {
                Deactivate();
            }
        }
        else if (GetSystemState() == SystemState.off)
        {
            if (_input == activateKey)
            {
                Activate();
            }
        }

    }



    public override void ProcessCollisionEnter(Collision collision)
    {

    }

    public override void CollideWithEnviroment(Collision collision)
    {
        lockoutTimer = GameConstants.SYSTEM_STUN;
        SetSystemState(SystemState.locked);
    }

    public override void Activate()
    {
        on = true;
        SetSystemState(SystemState.on);
    }

    public override void Deactivate()
    {
        SetSystemState(SystemState.off);
        on = false;
    }
}