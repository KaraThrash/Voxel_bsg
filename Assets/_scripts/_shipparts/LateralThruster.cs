using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralThruster : EngineBase
{
    public Transform target,rotationTarget;
    public string hortAxis;


    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards


  

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
             target.localPosition = Vector3.Lerp(target.localPosition, targetPos, Time.deltaTime * power);
        }

        if (ship && ship.CanAct() && torquePower != 0)
        {

            // target.transform.rotation = Quaternion.Lerp(target.rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - rotationTarget.position, rotationTarget.up), Time.deltaTime * torquePower);
            if (focalDepth == 0)
            {
                target.transform.rotation = Quaternion.Lerp(target.rotation, rotationTarget.rotation, Time.deltaTime * torquePower);

            }
            else 
            {

                 target.transform.rotation = Quaternion.Lerp(target.rotation, Quaternion.LookRotation((rotationTarget.position + (rotationTarget.forward * focalDepth)) - target.position, rotationTarget.up), Time.deltaTime * torquePower);

            }

        }
    }

    public Vector3 Lateral()
    {
        if (Mathf.Abs(targetPos.x) - boundary.y < 0.1f * boundary.x)
        {
            if (Mathf.Abs(targetPos.y) - boundary.y < 0.1f * boundary.y)
            {
                return (target.right * targetPos.x) + (target.up * targetPos.y);
            }
            else
            {
                return (target.right * targetPos.x);
            }
        }
        else
        {
            if (Mathf.Abs(targetPos.y) - boundary.y < 0.1f * boundary.y)
            {
                return (target.up * targetPos.y);
            }
        }
        return Vector3.zero;
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