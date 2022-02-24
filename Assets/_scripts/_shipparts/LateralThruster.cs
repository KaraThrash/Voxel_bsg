using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralThruster : EngineBase
{
    public Transform target,rotationTarget;
    public string hortAxis;


    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards

    public Transform ActOn()
    {
        if (target == null) { return transform; }
        return target;
    
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

        if (ship && ship.CanAct() && power != 0)
        {
             targetPos = new Vector3(boundary.x * horizontal, boundary.y * vertical, 0);

            ActOn().localPosition = Vector3.Lerp(ActOn().localPosition, targetPos, Time.deltaTime * power);
            //if (Lateral() == Vector3.zero)
            //{
                

            //}
            //else 
            //{
            //    ActOn().localPosition = Vector3.Lerp(ActOn().localPosition, Vector3.zero, Time.deltaTime * power);
            //}
        }

        if (ship && ship.CanAct() && torquePower != 0)
        {

            // ActOn().transform.rotation = Quaternion.Lerp(ActOn().rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - rotationTarget.position, rotationTarget.up), Time.deltaTime * torquePower);
            if (focalDepth == 0)
            {
                ActOn().transform.rotation = Quaternion.Lerp(ActOn().rotation, rotationTarget.rotation, Time.deltaTime * torquePower);

            }
            else 
            {

                 ActOn().transform.rotation = Quaternion.Lerp(ActOn().rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - ActOn().position, rotationTarget.up), Time.deltaTime * torquePower);

            }

        }
    }

    public Vector3 Lateral()
    {
        return ((ActOn().parent.right * ActOn().localPosition.x) + (ActOn().parent.up * ActOn().localPosition.y)).normalized;
        //return new Vector3(ActOn().localPosition.x, ActOn().localPosition.y,0);

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