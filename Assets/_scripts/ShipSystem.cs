using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{
    public SystemType systemType;

    public Ship ship;

    public Rigidbody rbTarget;

    public float power;

    public bool on;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Act();
    }

    public virtual void Act()
    {
        if (on && (ship && ship.CanAct()))
        {
            
        }
    }

    public virtual void Activate()
    { 
    
    }

    public virtual void Deactivate()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        ProcessCollisionEnter(collision);
    }

    public void OnTriggerEnter(Collider collision)
    {
        ProcessTriggerEnter(collision);
    }

    public virtual void ProcessCollisionEnter(Collision collision)
    {

    }

    public virtual void ProcessTriggerEnter(Collider collision)
    {

    }




    public void RbTarget(Rigidbody _rb) { rbTarget = _rb; }

    public Rigidbody RbTarget() 
    {
        if (rbTarget == null && GetComponent<Rigidbody>() != null)
        { rbTarget = GetComponent<Rigidbody>(); }

        return rbTarget;
    }

    public SystemType SystemType() { return systemType; }
    public void SystemType(SystemType _type) {  systemType = _type; }
}
