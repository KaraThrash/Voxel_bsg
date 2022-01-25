using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralThruster : ShipSystem
{
    public Transform target,rotationTarget;
    public string hortAxis;
    public float torquePower;
    [Min(0.01f)]
    public float accelerationRate, decelRate;

    public float currentAcc, throttle,horizontal,vertical;

    public Vector3 boundary, targetPos; //lateral movement in localspace

    public int focalDepth; // how far in front the local ship should look towards


    public void Throttle(float _hort,float _vert)
    {
        horizontal = _hort;
        vertical = _vert;
    }

    public override void PlayerInput()
    {

        if (axis.Length == 0 || hortAxis.Length == 0)
        {
            return ;
        }

        if ((Input.GetAxis(axis) != 0 || Input.GetAxis(hortAxis) != 0) && ship.UseStamina(staminaCost))
        {
            vertical = Input.GetAxis(axis);

            horizontal = Input.GetAxis(hortAxis);

        }
        else 
        {
            vertical = 0;
            horizontal = 0;
        }
    }

    public override void Act()
    {
        if (GetSystemState() == SystemState.on)
        {

            //if (axis.Length > 0)
            //{

            //    throttle = Input.GetAxis(axis);
            //}
            //else if (positiveButton.Length > 0)
            //{
            //    if (Input.GetButton(positiveButton))
            //    {

            //        if (Input.GetButton(negativeButton))
            //        {
            //            throttle = 0;
            //        }
            //        else
            //        {
            //            throttle = 1;
            //        }

            //    }
            //    else if (Input.GetButton(negativeButton))
            //    {
            //        throttle = -1;
            //    }
            //    else { throttle = 0; }
            //}
            //else
            //{
            //    throttle = 1;
            //}



        }
        else if (GetSystemState() == SystemState.locked)
        {


        }
        else
        {
            

        }

        if (ship && ship.CanAct() && power != 0)
        {
             targetPos = new Vector3(boundary.x * horizontal, boundary.y * vertical,0);
            // targetPos = new Vector3(targetPos.x , targetPos.y,-(Mathf.Abs(targetPos.x) + Mathf.Abs(targetPos.y)));
             targetPos = new Vector3(targetPos.x , targetPos.y,0);
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