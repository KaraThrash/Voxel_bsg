using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralThruster : EngineBase
{
    public Transform rotationTarget;
    public string hortAxis;


    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards


    public override Vector3 Lateral()
    {
        Vector3 up = ActOn().parent.up * vertical;
        Vector3 right = ActOn().parent.right * horizontal;

        //if (Mathf.Abs(ActOn().localPosition.x) >= boundary.x * 0.7f) { right = ActOn().parent.right * ActOn().localPosition.x; }
        //if (Mathf.Abs(ActOn().localPosition.y) >= boundary.y * 0.7f) { up = ActOn().parent.up * ActOn().localPosition.y; }

        //if (Mathf.Abs(horizontal) == 1) { right = ActOn().parent.right * horizontal; }
        //if (Mathf.Abs(vertical) == 1) { up = ActOn().parent.up * vertical; }

        if (PositiveButton())
        {
            return (up + right).normalized;
        }
        else { return Vector3.zero; }

        
        
        //return new Vector3(ActOn().localPosition.x, ActOn().localPosition.y,0);

    }

    public override void Act()
    {

        if (GetSystemState() == SystemState.maneuver)
        { return; }

        if (GetSystemState() == SystemState.on)
        {



        }
        else if (GetSystemState() == SystemState.locked)
        {


        }
        else
        {
            

        }

        if (ship && ship.CanAct() && STAT_Power() != 0)
        {
             targetPos = new Vector3(boundary.x * horizontal, boundary.y * vertical, 0);
            if (Vector3.Distance(targetPos, ActOn().localEulerAngles) < 0.1f)
            {
                ActOn().localPosition = targetPos;
            }
            else 
            {

                if (PositiveButton())
                { }
                    //Vector3 pos = RaycastAtNextPosition(ActOn().localPosition, targetPos);
                    if (RaycastAtNextPosition(ActOn(), targetPos + ActOn().localPosition) )
                    {
                        ActOn().localPosition = Vector3.Lerp(ActOn().localPosition, targetPos, Time.deltaTime * STAT_Power() * Ship().ShipInput().GetParameter(ShipInputParameters.lateral));

                    }
                    else
                    {
                        ActOn().localPosition = Vector3.Lerp(ActOn().localPosition, Vector3.zero, Time.deltaTime * STAT_Power() * Ship().ShipInput().GetParameter(ShipInputParameters.lateral));

                    }
                
                

            }
            //if (Lateral() == Vector3.zero)
            //{


            //}
            //else 
            //{
            //    ActOn().localPosition = Vector3.Lerp(ActOn().localPosition, Vector3.zero, Time.deltaTime * STAT_Power());
            //}
        }

        if (ship && ship.CanAct() && torquePower != 0)
        {

            // ActOn().transform.rotation = Quaternion.Lerp(ActOn().rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - rotationTarget.position, rotationTarget.up), Time.deltaTime * torquePower);
            if (focalDepth == 0)
            {
                ActOn().transform.rotation = Quaternion.Slerp(ActOn().rotation, rotationTarget.rotation, Time.deltaTime * torquePower * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

            }
            else 
            {

                ActOn().transform.rotation = Quaternion.Slerp(ActOn().rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - ActOn().position, rotationTarget.up), Time.deltaTime * torquePower * Ship().ShipInput().GetParameter(ShipInputParameters.turn));

            }

           


        }
    }
 


    //public Vector3 oldLateral()
    //{

    //    if (boundary.x - Mathf.Abs(ActOn().localPosition.x) < 0.1f * boundary.x)
    //    {
    //        if (boundary.y - Mathf.Abs(ActOn().localPosition.y) < 0.1f * boundary.y)
    //        {
    //            return (ActOn().right * ActOn().localPosition.x) + (ActOn().up * ActOn().localPosition.y);
    //        }
    //        else
    //        {
    //            return (ActOn().right * ActOn().localPosition.x);
    //        }
    //    }
    //    else
    //    {
    //        if (boundary.y - Mathf.Abs(ActOn().localPosition.y) < 0.1f * boundary.y)
    //        {
    //            return (ActOn().up * ActOn().localPosition.y);
    //        }
    //    }
    //    return Vector3.zero;
    //}



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