using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : ShipSystem
{
    [Min(0.01f)]
    public float accelerationRate, decelRate;
    public float currentAcc;

    public override void Act()
    {
        if (on )
        {
            if (currentAcc < 1 - accelerationRate)
            {
                currentAcc = Mathf.Lerp(currentAcc, 1, accelerationRate * Time.deltaTime);
            }
            else { currentAcc = 1; }
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
            RbTarget().AddForce(transform.forward * (power * currentAcc * Time.deltaTime),ForceMode.Impulse );
        }

        
    }





    public override void Activate()
    {
        on = true;
    }

    public override void Deactivate()
    {

        on = false;
    }


}
