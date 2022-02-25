using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public Rigidbody rb;

    public Transform rotationTarget;

    public bool canAct;

    public List<ShipSystem> systems;

    public float stamina, maxStamina, staminaRechargeRate;

    public float acceleration;

    public EngineBase primaryEngine;// primary engine for moving forward
    public EngineBase secondaryEngine;//secondary for turns and adjustments [can be the same engine]

    public ChasisBase chasis;

    public Vector3 velocityTarget;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BulletParent_" + this.GetType().ToString());
        AddSystems();
    }

    public void AddSystems()
    {
        if (systems == null)
        { systems = new List<ShipSystem>(); }

        foreach (Transform el in transform)
        {
            if (el.GetComponent<ShipSystem>())
            {
                systems.Add(el.GetComponent<ShipSystem>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canAct)
        {
            Act();
        }



    }



    
    public void Act()
    {

        RechargeStamina();

        Movement();
      
        // Rigidbody().velocity = Vector3.Lerp(Rigidbody().velocity,targetVelocity,Time.deltaTime * acceleration);

    }


    public void Movement()
    {
        //get engines target velocity
        //check the enviroment and the hull's modifiers to the engine's velocity

        float accel = 0;
        Vector3 newVelocity = Vector3.zero;

        if (PrimaryEngine())
        {
            newVelocity = PrimaryEngine().GetTargetVelocity();
            accel = primaryEngine.LinearAcceleration();

        }

        Vector3 externalForceVelocity = Vector3.zero; 
        if (Chasis())
        {
            newVelocity = Chasis().ExternalForce();

        }

        

     //   newVelocity = Vector3.Lerp(RB().velocity, newVelocity , accel);

        //hull/enviroment modifications
        if ( Chasis().ExternalForce() == Vector3.zero)
        {
             RB().velocity = Vector3.Lerp(RB().velocity, newVelocity , accel);
            //RB().velocity = newVelocity + externalForceVelocity;
        }
        else if (Chasis().BelowMinimumMagnitude())
        {
            RB().velocity = Vector3.Lerp(externalForceVelocity, newVelocity, accel);
            //RB().velocity = newVelocity + externalForceVelocity;
        }
        else 
        {
            RB().velocity = Chasis().ExternalForce();
        }
        

        transform.rotation = Quaternion.Slerp(transform.rotation, primaryEngine.GetTargetRotation(), primaryEngine.RotationAcceleration());
    }















    public virtual void Control(KeyCode _input, bool _on)
    {
        foreach (ShipSystem el in systems)
        {
           
            if (_on)
            {
                el.Control(_input);

            }
        }
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

        if (Chasis() != null && collision.transform.CompareTag("Enviroment"))
        {
            CollideWithEnviroment(collision);
        }

    }

    public virtual void ProcessTriggerEnter(Collider collision)
    {

    }




    public void CollideWithEnviroment(Collision collision)
    {
       // Chasis().ExternalForce(collision.impulse.magnitude * (transform.position - collision.contacts[0].point));
        Chasis().ExternalForce(collision.impulse.magnitude * Vector3.Reflect( collision.contacts[0].point - transform.position, collision.impulse).normalized);
      //  PrimaryEngine().CollideWithEnviroment(collision);
        //TODO: engine after impact


    }











    /// <summary>
    /// Stamina
    /// </summary>



    public void RechargeStamina()
    {
        if (stamina < maxStamina)
        {
            stamina += (Time.deltaTime * staminaRechargeRate);

            if (stamina > maxStamina) { stamina = maxStamina; }
        }

        SetStaminaText(stamina.ToString());
    }

    public bool UseStamina(float _cost)
    {
        // round stamina check so that a cost of 0.2relativeRot.will use 0.1 stamina leaving -0.1
        if (_cost <= Mathf.Ceil(stamina) ) 
        { 
            stamina -= _cost;
            return true;
        }

        return false;
    }

    public bool CheckStamina(float _cost)
    {
        // round stamina check so that a cost of 0.2relativeRot.will use 0.1 stamina leaving -0.1
        if (_cost <= Mathf.Ceil(stamina))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Stamina
    /// </summary>



    public void CanAct(bool _on)
    {canAct = _on;}

    public bool CanAct()
    { return canAct; }



    public EngineBase PrimaryEngine()
    {

        return primaryEngine;
    }
    public EngineBase SecondaryEngine()
    {

        return secondaryEngine;
    }

    public ChasisBase Chasis()
    {

        return chasis;
    }

    public Quaternion RotationTarget()
    {

        return rotationTarget.rotation;
    }



    public void SetVelocity(Vector3 _velocity)
    {
        if (RB() == null) { return; }

        RB().velocity = _velocity;
    }

    public Vector3 GetVelocity()
    {
        if (RB() == null) { return Vector3.zero; }

       return RB().velocity;
    }


    public Rigidbody RB()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        return rb;
    }





    /// UI
    /// 

    public Text staminaBar;

    public void SetStaminaText(string _stamina)
    {
        if (staminaBar != null) { staminaBar.text = stamina.ToString(); }
    }

    ///


    //TODO: use these to set relative directions based on preference: e.g. ship forward or camera forward etc
    public Vector3 Forward() { return transform.forward; }
    public Vector3 Right() { return transform.right; }
    public Vector3 Up() { return transform.up; }



}
