using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBasic : ShipSystem
{
    public LateralThruster lateralEngine;

    public float lateralPower;
    public float torquePower;
    [Min(0.01f)]
    public float accelerationRate, brakePower,decelRate; //brake power for manual slow as determined by negative throttle, decelRate is the engines ambient force to return to rest

    public float currentAcc, throttle,torqueThrottle = 1;

    public Vector3 targetVelocity;

    public void Throttle(float _value, float _torqueValue)
    {
        throttle = _value;
        torqueThrottle = _torqueValue;
    }

    public override void PlayerInput()
    {
        if (axis.Length > 0)
        {
            if ((Input.GetAxis(axis) != 0) && ship.UseStamina(staminaCost))
            {
                throttle = Input.GetAxis(axis);

            }
            else { throttle = 0; }
           
        }
        else if (positiveButton.Length > 0 && ship.CheckStamina(staminaCost))
        {
            if (Input.GetButton(positiveButton) )
            {

                if (Input.GetButton(negativeButton))
                {
                    throttle = 0;
                }
                else
                {
                    throttle = 1;
                }

            }
            else if (Input.GetButton(negativeButton))
            {
                throttle = -1;
            }
            else { throttle = 0; }
        }
        else
        {
            throttle = 0;
        }

        //if lacking stamina, cant throttle
        if (throttle != 0 && ship.UseStamina(staminaCost) == false)
        {
            throttle = 0; 
        }

    }


    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {
            Accelerate(throttle);


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

        if (ship && ship.CanAct() && power != 0)
        {

            if (throttle != 0)
            {
                targetVelocity = Vector3.Lerp(targetVelocity, ship.transform.forward * power * currentAcc, Time.deltaTime * accelerationRate);
                
            }
            else 
            {
                targetVelocity = Vector3.Lerp(targetVelocity, Vector3.zero, Time.deltaTime * decelRate);
            }


            if (lateralEngine != null)
            {
                RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, targetVelocity + (lateralEngine.Lateral() * lateralPower), Time.deltaTime * accelerationRate);
            }
            else 
            {
                RbTarget().velocity = Vector3.Lerp(RbTarget().velocity, targetVelocity, Time.deltaTime * accelerationRate);

            }

        }

        if (ship && ship.CanAct() && torquePower != 0)
        {
            ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, ship.RotationTarget(), Time.deltaTime * torquePower * torqueThrottle);
        }
    }




    public void Accelerate(float _throttle)
    {
        if (_throttle < 0)
        {
            currentAcc = Mathf.Lerp(currentAcc, 0, brakePower * Time.deltaTime);

        }
        else if (throttle > 0)
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