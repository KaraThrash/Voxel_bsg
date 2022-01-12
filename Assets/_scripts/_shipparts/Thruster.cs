using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : ShipSystem
{
    [Min(0.01f)]
    public float accelerationRate, decelRate;

    public float currentAcc,throttle;

    public override void Act()
    {
        if (GetSystemState() == SystemState.on )
        {
            if (currentAcc < 1 - accelerationRate)
            {
                currentAcc = Mathf.Lerp(currentAcc, 1, accelerationRate * Time.deltaTime);
            }
            else { currentAcc = 1; }
        }
        else if (GetSystemState() == SystemState.locked)
        {
           

        }
        else 
        {
            if (currentAcc > 0)
            {
                if (currentAcc < decelRate)
                {
                    currentAcc = 0;
                }
                currentAcc = Mathf.Lerp(currentAcc, 0, decelRate * Time.deltaTime);

            }

        }

        if (ship && ship.CanAct() && currentAcc != 0)
        {
            float pwr = power * currentAcc * Time.deltaTime;
            throttle = 0;

            //todo: glide/throttle lock/cruise control
            if (axis.Length > 0 && Input.GetAxis(axis) != 0)
            {
                throttle = Input.GetAxis(axis);
            }

            RbTarget().AddForce(transform.forward * (pwr * throttle), ForceMode.Impulse );

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
        SetSystemState( SystemState.on);
    }

    public override void Deactivate()
    {
        SetSystemState(SystemState.off);
        on = false;
    }


}
