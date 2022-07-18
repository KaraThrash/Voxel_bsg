using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LateralThruster : EngineBase
{
    public Transform rotationTarget;
    public Transform visual_jet;


    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards

    //to prevent the cost of players idly tapping the stick for figit reasons
    private float minValueToIgnoreStaminaCost = 0.5f; 




    //public override Vector3 Lateral()
    //{
    //    Vector3 up = ActOn().parent.up * vertical;
    //    Vector3 right = ActOn().parent.right * horizontal;

    //    if (PositiveButton())
    //    {
            
    //        return (up + right);
    //    }
    //    else { return Vector3.zero; }

    //}

    public override void Act()
    {

   

        if (ship && ship.CanAct() && STAT_Power() != 0)
        {

            targetPos = ship.transform.position;
            Quaternion newrot = rotationTarget.rotation;
            Vector3 fwdVector = (rotationTarget.position + (rotationTarget.forward * focalDepth)) - ship.transform.position;
            newrot = Quaternion.LookRotation((rotationTarget.position + fwdVector) - ActOn().position, rotationTarget.up);

            if (ship.Chasis() && ship.Chasis().ExternalForce().magnitude != 0)
            {

                if (Vector3.Angle(ActOn().right, ship.transform.right) > 40)
                {
                    newrot = Quaternion.LookRotation((rotationTarget.position + fwdVector) - ActOn().position, rotationTarget.right);
                }
                else
                {
                    newrot = Quaternion.LookRotation((rotationTarget.position + fwdVector) - ActOn().position, -rotationTarget.right);
                }
               


            }
            else
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
            }


                if (Vector3.Distance(ActOn().position, targetPos) < Time.deltaTime)
                {
                    ActOn().position = targetPos;

                }
                else
                {
                    float strafeSpeed = 1;
                    if (STAT_Power() != 0)
                    { strafeSpeed = STAT_PowerSecondary() / STAT_Power(); }



                    ActOn().position = Vector3.Lerp(ActOn().position, targetPos, Time.deltaTime * strafeSpeed);

                if (visual_jet)
                {
                    if (ActOn().position !=targetPos)
                    {
                   //     visual_jet.gameObject.SetActive(true);
                       // visual_jet.LookAt(transform.position - targetPos);
                    }
                    else
                    {
                       // visual_jet.LookAt(transform.position - transform.forward);
                    //    visual_jet.gameObject.SetActive(false);
                    }
                }
                    

                }



                if (focalDepth == 0)
                {
                    ActOn().transform.rotation = Quaternion.Slerp(ActOn().rotation, newrot, Time.deltaTime * torquePower * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

                }
                else
                {

                    if (ActOn().localPosition != Vector3.zero)
                    {
                    ActOn().transform.rotation = Quaternion.Slerp(ActOn().rotation, newrot, Time.deltaTime * STAT_Power() * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

                    //ActOn().transform.rotation = newrot;
                    }

                }





            


        }


    }


    public override Vector3 Lateral()
    {

        if (boundary.x != 0 && boundary.y != 0)
        {
            float xRatio = Mathf.Abs(ActOn().localPosition.x / boundary.x);
            float yRatio = Mathf.Abs(ActOn().localPosition.y / boundary.y);
            return ((Ship().Right() * horizontal * xRatio) + (Ship().Up() * vertical * yRatio));

        }
        return Vector3.zero;
        //return new Vector3(ActOn().localPosition.x, ActOn().localPosition.y,0);

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