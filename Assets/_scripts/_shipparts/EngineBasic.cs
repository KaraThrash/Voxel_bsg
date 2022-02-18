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
            lateralEngine.SetSystemState(SystemState.maneuver);
            //maneuverRotation = transform.right;
            maneuverRotation = transform.position - (transform.forward * 100);
        }
    }



    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {
           
            Accelerate(throttle_A);
            if (maneverCooldown > 0) { maneverCooldown -= Time.deltaTime; }

        }
        else if (GetSystemState() == SystemState.locked)
        {


        }
        else if (GetSystemState() == SystemState.damaged)
        {
            LockoutTimer(LockoutTimer() - Time.deltaTime);
            if (LockoutTimer() <= 0)
            {
              
                SetSystemState(SystemState.on);

            }
        }
        else
        {
            if (currentAcc > 0)
            {
                Accelerate(0);
            }

        }

        if (GetSystemState() == SystemState.maneuver)
        {
            maneverCooldown = 1;
            Debug.Log(maneuverRotation);
            float rot = Time.deltaTime * (180 );
            maneverRotCount += rot;

            
            //Ship().transform.Rotate(maneuverRotation * rot);
            Ship().transform.rotation = Quaternion.Lerp(Ship().transform.rotation, Quaternion.LookRotation(maneuverRotation - Ship().transform.position, Ship().transform.up), Time.deltaTime * torquePower);

            RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, Ship().Forward() * power * 0.15f, Time.deltaTime );

            if (maneverRotCount >= 180 )
            {
                
                SetSystemState(SystemState.on);
                lateralEngine.SetSystemState(SystemState.on);
            }


        }
        else 
        {
            if (Ship() && Ship().CanAct() && power != 0)
            {

                if (throttle_A != 0)
                {
                    targetVelocity = Vector3.Lerp(targetVelocity, Ship().Forward() * power * currentAcc, Time.deltaTime * accelerationRate);

                }
                else
                {
                    targetVelocity = Vector3.Lerp(targetVelocity, Vector3.zero, Time.deltaTime * decelRate);
                }


                if (lateralEngine != null && PositiveButton())
                {
                    RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, targetVelocity + (lateralEngine.Lateral() * lateralPower), Time.deltaTime * accelerationRate);
                }
                else
                {
                    RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, targetVelocity, Time.deltaTime * accelerationRate);

                }

            }

            if (ship && Ship().CanAct() && torquePower != 0)
            {
                Ship().transform.rotation = Quaternion.Lerp(Ship().transform.rotation, Ship().RotationTarget(), Time.deltaTime * torquePower * throttle_B);
            }
        }
        
    }




    public void Accelerate(float _throttle)
    {
        if (_throttle < 0)
        {
            currentAcc = Mathf.Lerp(currentAcc, 0, brakePower * Time.deltaTime);

        }
        else if (throttle_A > 0)
        {
            currentAcc = Mathf.Lerp(currentAcc, Mathf.Abs(_throttle), accelerationRate * Time.deltaTime);

        }
        else
        {
            currentAcc = 0;
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