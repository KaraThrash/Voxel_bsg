using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EngineBase : ShipSystem
{
    public string axis_throttle_A, axis_throttle_B;
    public string axis_horizontal, axis_vertical;

    public float lateralPower;
    public float torquePower;
    [Min(0.01f)]
    public float rate_Acceleration; 
    public float  brakePower; 
    public float  rate_Deceleration; //brake power for manual slow as determined by negative throttle, decelRate is the engines ambient force to return to rest

    public float throttle_A, throttle_B;
    public float horizontal, vertical;
    public float current_Acceleration;

    public Transform target;
    public Vector3 forwardVelocity;
    public Vector3 lateralVelocity;
    public Vector3 verticalVelocity;

    public Quaternion targetRotation;

    public void Thrust_Throttle(float _value)
    {
        throttle_A = _value;
    }

    public void Roll_Throttle(float _value)
    {
        throttle_B = _value;
    }

    public void Horizontal_Throttle(float _value)
    {
        horizontal = _value;
    }

    public void Vertical_Throttle(float _value)
    {
        vertical = _value;
    }

    public void ThrustRollHortVert(float _thrust,float _roll,float _hort,float _vert)
    {
        throttle_A = _thrust;
        throttle_B = _roll;
        horizontal = _hort;
        vertical = _vert;
    }

    public override void PlayerInput()
    {
        if (axis_throttle_A.Length > 0)
        {
            if ((Input.GetAxis(axis_throttle_A) != 0) && ship.UseStamina(staminaCost))
            {
                throttle_A = Input.GetAxis(axis_throttle_A);

            }
            else { throttle_A = 0; }

        }
        else if (positiveButton != Buttons.none && ship.CheckStamina(staminaCost))
        {
            if (PositiveButton())
            {

                if (NegativeButton())
                {
                    throttle_A = 0;
                }
                else
                {
                    throttle_A = 1;
                }

            }
            else if (NegativeButton())
            {
                throttle_A = -1;
            }
            else { throttle_A = 0; }
        }
        else
        {
            throttle_A = 0;
        }

        //if lacking stamina, cant throttle
        if (throttle_A != 0 && ship.UseStamina(staminaCost) == false)
        {
            throttle_A = 0;
        }


        if(axis_throttle_B.Length > 0)
        {
            if ((Input.GetAxis(axis_throttle_B) != 0) && ship.UseStamina(0))
            {
                throttle_B = Input.GetAxis(axis_throttle_B);

            }
            else { throttle_B = 0; }

        }

        if (axis_horizontal.Length > 0)
        {
            if ((Input.GetAxis(axis_horizontal) != 0) && ship.UseStamina(0))
            {
                horizontal = Input.GetAxis(axis_horizontal);

            }
            else { horizontal = 0; }

        }
        if (axis_vertical.Length > 0)
        {
            if ((Input.GetAxis(axis_vertical) != 0) && ship.UseStamina(0))
            {
                vertical = Input.GetAxis(axis_vertical);

            }
            else { vertical = 0; }

        }
    }



    public virtual Vector3 Check_Min_Velocity(Vector3 _velocity)
    {
        //TODO: check enviroment type

        //if (_velocity.magnitude < 1.1f)
        //{
        //    return ship.transform.forward * 11.1f;
        //}

        return _velocity;
    }

    public virtual Vector3 GetTargetVelocity()
    {

        return Check_Min_Velocity(forwardVelocity + lateralVelocity + verticalVelocity);
    }

    public float LinearAcceleration()
    {
        return  rate_Acceleration;
    }

    public float RotationAcceleration()
    {
        return Time.deltaTime * torquePower ;
    }

    public virtual Quaternion GetTargetRotation()
    {

        return transform.rotation;
    }

    public virtual void StartManeuver(Maneuver _maneuver)
    {
    
    }


    public Transform ActOn()
    {
        if (target == null) { return transform; }
        return target;

    }
    public virtual Vector3 Lateral()
    {
        return ((ActOn().parent.right * ActOn().localPosition.x) + (ActOn().parent.up * ActOn().localPosition.y)).normalized * lateralPower;
        //return new Vector3(ActOn().localPosition.x, ActOn().localPosition.y,0);

    }

    public bool RaycastAtNextPosition(Transform _pos,Vector3 _newPos)
    {


        RaycastHit hit;
        if (Physics.Raycast(_pos.position, _newPos ,out hit, ~LayerMask.GetMask("Enviroment")))
        {
            return false;// (hit.point - _pos) * 0.95f;
        }
        return true;
    }

}
