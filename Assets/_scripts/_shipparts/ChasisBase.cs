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

    public MeshRenderer externalForceIndicator;


    public Vector3 ExternalForce() 
    {
        if (externalForce.magnitude < Time.deltaTime || timer <= Time.deltaTime)
        { return Vector3.zero; }

        return externalForce;
    }
    public void ExternalForce(Vector3 _force) 
    {
        //the strength to resist external forces is applied when determining the ship's movement
        timer = ExternalForceTimeCap();
        externalForce += _force;

        if (externalForce.magnitude > externalForceMagnitudeCap)
        { externalForce = externalForce.normalized * externalForceMagnitudeCap; }
        SetVisualIndicator(true);
        //TODO: envirment modifiers: gravity/friction

    }

    public void ExternalForce(Vector3 _dir,float _impulse)
    {
        if (_impulse >= minMagnitude)
        {
            SetVisualIndicator(true);

            timer = ExternalForceTimeCap();
            externalForce += (_dir * _impulse);

            if (externalForce.magnitude > externalForceMagnitudeCap)
            { externalForce = externalForce.normalized * externalForceMagnitudeCap; }
        }
        
    }


    public void ReduceExternalForce()
    {
        if (Ship() && Ship().Hitpoints() <= 0) { return; }
        timer -= Time.deltaTime;
        if ( timer <= 0)
        {
            externalForce = Vector3.zero;
            SetVisualIndicator(false);

        }
        externalForce = Vector3.Lerp(ExternalForce(), Vector3.zero, Time.deltaTime * externalForceDecay);
    }



    public Vector3 ApplyExternalForces(Vector3 _force)
    {
        //The [ship this chasis is attached to] passes its intended velocity [_force]
        //This chasis applies any external forces: e.g. recoil from a weapon firing, collisions

        //using 'Lerp' with no delta time component allows us to apply how much the chasis resists these forces
        // a value of '1' means there is no resistance

        Vector3 modifiedForce = _force;
        //_force +
        modifiedForce = Vector3.Lerp(modifiedForce,  ExternalForce(), 1 - STAT_PowerSecondary());

        return Vector3.Lerp(modifiedForce, _force + ExternalForce(), 1);
    }



    public override void CollideWithShip(Collision collision)
    {

        ExternalForce(Vector3.Reflect(collision.contacts[0].point - (transform.position), collision.contacts[0].normal).normalized, Ship().GetVelocity().magnitude * 1.2f);
    }


    public override void CollideWithEnviroment(Collision collision)
    {
        //ExternalForce(Vector3.Reflect(collision.contacts[0].point - (transform.position), collision.contacts[0].normal).normalized, Ship().GetVelocity().magnitude * 1.2f);
        ExternalForce( ((transform.position - collision.contacts[0].point).normalized + Ship().GetVelocity().normalized) * ( Ship().GetVelocity().magnitude * 1.2f));
    }

    public override int DamageFromCollision(Collision collision)
    {

        return 1;
    }


    public override void Act()
    {
        if (ExternalForce() != Vector3.zero)
        {
            ReduceExternalForce();
        }
        else { SetVisualIndicator(false); }
        
    }


    public void SetVisualIndicator(bool _on)
    {
        if (externalForceIndicator != null) { externalForceIndicator.enabled = _on; }
    }


    public bool BelowMinimumMagnitude(Vector3 _force)
    { return _force.magnitude < minMagnitude; }
    public bool BelowMinimumMagnitude()
    { return ExternalForce().magnitude < minMagnitude; }

    public float ExternalForceTimeCap()
    { return externalForceTimeCap; }

    public virtual Vector3 VelocityChange(Vector3 _velocity)
    { return _velocity; }

    public virtual Vector3 AngularVelocityChange(Vector3 _velocity)
    { return _velocity; }



}
