using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public Rigidbody rb;

    public Transform cam;

    public bool canAct;

    public List<ShipSystem> systems;

    public float stamina, maxStamina, staminaRechargeRate;

    public float acceleration;

    public Quaternion rotationTarget;
    public Vector3 velocityTarget;


    // Start is called before the first frame update
    void Start()
    {
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
           // Act();

           // RechargeStamina();
            
        }



    }



    public bool useAngularVelocity;
    
    public void Act()
    {
        if (!canAct)
        {
            return;

        }


        RechargeStamina();

       // Rigidbody().velocity = Vector3.Lerp(Rigidbody().velocity,targetVelocity,Time.deltaTime * acceleration);

        if (useAngularVelocity)
        {
           // Rigidbody().angularVelocity = Vector3.Lerp(Rigidbody().angularVelocity, GetTargetAngularVelocity(), Time.deltaTime * acceleration);

        }
        else 
        {
           // transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, Time.deltaTime * GetRotationMagnitude());
        }

        //cam.transform.rotation


        // Rigidbody().angularVelocity = new Vector3(pitch, yaw, roll);
        //if (InputControls.ActionButtonDown())
        //{
        //    Debug.Log("InputControls.RightTrigger() <>" + InputControls.RightTrigger());
        //    Control(SystemType.engine, 1);
        //}

        //else if (InputControls.ActionButtonUp())
        //{
        //    Control(SystemType.engine, false);
        //}

    }














    public virtual void Control(SystemType _system, bool _on)
    {
        foreach (ShipSystem el in systems)
        {
            if (_on)
            {
                el.Activate();
            }
            else 
            {
                el.Deactivate();
            }
            

            if (el.GetSystemType() == _system)
            {

            }
        }
    }
    public virtual void Control(SystemType _system, float _value)
    {

        //TODO: question: should there be a list of buttons that are set and check if the input is on the list?
        foreach (ShipSystem el in systems)
        {
            if (UseStamina(el.StaminaCost()))
            { 
                el.Activate();
            }

            if (el.GetSystemType() == _system)
            {

            }
        }
    }

    public virtual void Control(KeyCode _input, bool _on)
    {
        foreach (ShipSystem el in systems)
        {
           
            if (_on)
            {
                el.Control(_input);
                //if (el.activateKey == _input)
                //{ el.Activate(); }
                //else if (el.deactivateKey == _input)
                //{ el.Deactivate(); }


            }
        }
    }


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



    public void CanAct(bool _on)
    {canAct = _on;}

    public bool CanAct()
    { return canAct; }


    public Vector3 targetVelocity, targetAngularVelocity;
    public Vector3 GetTargetVelocity( )
    {
        Vector3 newvel = Vector3.zero;
        foreach (ShipSystem el in systems)
        {

            if (el.GetComponent<Thruster>())
            {
                newvel += el.GetComponent<Thruster>().GetThrust();

            }
        }
        return newvel;
    }

    public Vector3 GetTargetAngularVelocity( )
    {

        Vector3 newvel = Vector3.zero;

        foreach (ShipSystem el in systems)
        {

            if (el.GetComponent<Thruster>())
            {
                newvel += el.GetComponent<Thruster>().GetAngularThrust();

            }
        }

        return newvel;
    }

    public float GetRotationMagnitude()
    {

        float magnitude = 0;

        foreach (ShipSystem el in systems)
        {

            if (el.GetComponent<Thruster>())
            {
                magnitude += el.GetComponent<Thruster>().GetAngularThrust().magnitude;

            }
        }

        return magnitude;
    }


    /// UI
    /// 

    public Text staminaBar;

    public void SetStaminaText(string _stamina)
    {
        if (staminaBar != null) { staminaBar.text = stamina.ToString(); }
    }

    ///


    public Rigidbody Rigidbody()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        return rb;
    }


}
