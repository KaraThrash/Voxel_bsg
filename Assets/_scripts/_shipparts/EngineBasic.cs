﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBasic : ShipSystem
{
    // Start is called before the first frame update
    public float torquePower;
    [Min(0.01f)]
    public float accelerationRate, decelRate;

    public float currentAcc, throttle;


  


    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {

            if (axis.Length > 0)
            {

                throttle = Input.GetAxis(axis);
            }
            else if (positiveButton.Length > 0)
            {
                if (Input.GetButton(positiveButton))
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
                throttle = 1;
            }


            currentAcc = Mathf.Lerp(currentAcc, Mathf.Abs(throttle), accelerationRate * Time.deltaTime);

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

        if (ship && ship.CanAct() && power != 0)
        {
            RbTarget().velocity = Vector3.Lerp(RbTarget().velocity,transform.forward * power * currentAcc, Time.deltaTime * accelerationRate);
        }

        if (ship && ship.CanAct() && torquePower != 0)
        {
            ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, ship.RotationTarget(), Time.deltaTime * torquePower );
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