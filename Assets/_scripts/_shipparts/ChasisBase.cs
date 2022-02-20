using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasisBase : ShipSystem
{
    [Range(0.01f, 10.0f)]
    public float externalForceDecay; // how quickly the ship removes external forces acting on it: e.g. the counter force from collisions or taking damage 
    [Range(0,1.0f)]
    public float externalForceResist; // how much the ship resists external forces when they are applied

    public float externalForceMagnitudeCap;
    public float minMagnitude;
    public float externalForceTimeCap;

    public float timer;

    public Vector3 externalForce;

    public Vector3 ExternalForce() { return externalForce; }
    public void ExternalForce(Vector3 _force) 
    {
        timer = ExternalForceTimeCap();
        externalForce += (externalForceResist * _force);
        if (externalForce.magnitude > externalForceMagnitudeCap)
        { externalForce = externalForce.normalized * externalForceMagnitudeCap; }
    }

    public void ReduceExternalForce()
    {
        timer -= Time.deltaTime;
        if ( timer <= 0)
        {
            externalForce = Vector3.zero;
        }
        externalForce = Vector3.Lerp(ExternalForce(), Vector3.zero, Time.deltaTime * externalForceDecay);
    }


    public override void Act()
    {
        if (ExternalForce() != Vector3.zero)
        {
            ReduceExternalForce();
        }
        
    }



    public bool BelowMinimumMagnitude()
    { return ExternalForce().magnitude < minMagnitude; }

    public float ExternalForceTimeCap()
    { return externalForceTimeCap; }

    public virtual Vector3 VelocityChange(Vector3 _velocity)
    { return _velocity; }

    public virtual Vector3 AngularVelocityChange(Vector3 _velocity)
    { return _velocity; }



}
