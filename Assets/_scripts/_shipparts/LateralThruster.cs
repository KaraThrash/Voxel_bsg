using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LateralThruster : EngineBase
{
    public Transform rotationTarget;


    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards

    //to prevent the cost of players idly tapping the stick for figit reasons
    private float minValueToIgnoreStaminaCost = 0.5f; 


    public override Vector3 Lateral()
    {
        Vector3 up = ActOn().parent.up * vertical;
        Vector3 right = ActOn().parent.right * horizontal;

        if (PositiveButton())
        {
            
            return (up + right);
        }
        else { return Vector3.zero; }

    }

    public override void Act()
    {

   

        if (ship && ship.CanAct() && STAT_Power() != 0)
        {


            targetPos = new Vector3(boundary.x * horizontal, boundary.y * vertical, 0);
            targetPos = boundary.x * horizontal * rotationTarget.right;
            targetPos += boundary.y * vertical * rotationTarget.up;
            targetPos += ship.transform.position;
            
            //if (horizontal != 0 || vertical != 0)
            //{
            //    targetPos = new Vector3(boundary.x * horizontal, boundary.y * vertical, 0);
            //}


            if (PositiveButton())
            {
             //   targetPos = Vector3.zero;
            }


            if ((Mathf.Abs(horizontal) + Mathf.Abs(vertical)) > minValueToIgnoreStaminaCost)
            {
                if (ship.CheckStamina(StaminaCost()) == false)
                {
                    targetPos = ship.transform.position;// Vector3.zero;
                }
                else 
                {
                    ship.UseStamina(StaminaCost());
                }
            }

            if (Vector3.Distance(ActOn().position, targetPos) < Time.deltaTime )
            {
                ActOn().position = targetPos;

            }
            else 
            {
                ActOn().position = Vector3.Lerp(ActOn().position, targetPos, Time.deltaTime * STAT_PowerSecondary());
            }



            if (focalDepth == 0)
            {
                ActOn().transform.rotation = Quaternion.Slerp(ActOn().rotation, rotationTarget.rotation, Time.deltaTime * torquePower * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

            }
            else
            {

                Vector3 fwdVector = (rotationTarget.position + (rotationTarget.forward * focalDepth)) - ship.transform.position;
                ActOn().transform.rotation = Quaternion.LookRotation((rotationTarget.position + fwdVector) - ActOn().position, rotationTarget.up);
            }





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