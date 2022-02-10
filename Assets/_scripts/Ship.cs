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

    public ShipSystem primaryEngine;// primary engine for moving forward
    public ShipSystem secondaryEngine;//secondary for turns and adjustments [can be the same engine]

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
            Act();
        }



    }



    
    public void Act()
    {

        RechargeStamina();

       // Rigidbody().velocity = Vector3.Lerp(Rigidbody().velocity,targetVelocity,Time.deltaTime * acceleration);

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

    }

    public virtual void ProcessTriggerEnter(Collider collision)
    {

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



    public ShipSystem PrimaryEngine()
    {

        return primaryEngine;
    }
    public ShipSystem SecondaryEngine()
    {

        return secondaryEngine;
    }


    public Quaternion RotationTarget()
    {

        return rotationTarget.rotation;
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







}
