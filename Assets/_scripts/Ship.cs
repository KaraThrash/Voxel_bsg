﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShipInputParameters { thrust,lateral,vertical,roll,turn,weapons,all}



public class ShipInput 
{
    //modifier for a specific element of the controls for locking out during special actions
    // or for damaged/destryoed systems

    float thrust = 1;
    float lateral = 1;
    float vertical = 1;

    float roll = 1;
    float turn = 1;

    float weapons = 1;

    public void SetParameter(ShipInputParameters _parameter, float _value)
    {
        if (_parameter == ShipInputParameters.thrust)
        { thrust = _value; }
        else if (_parameter == ShipInputParameters.lateral)
        { lateral = _value; }
        else if (_parameter == ShipInputParameters.vertical)
        { vertical = _value; }

        else if (_parameter == ShipInputParameters.roll)
        { roll = _value; }
        else if (_parameter == ShipInputParameters.turn)
        { turn = _value; }

        else if (_parameter == ShipInputParameters.weapons)
        { weapons = _value; }
        else
        {
             thrust = 1;
             lateral = 1;
             vertical = 1;

             roll = 1;
             turn = 1;

             weapons = 1;
        }
    }


    public float GetParameter(ShipInputParameters _parameter)
    {
        if (_parameter == ShipInputParameters.thrust)
        { return thrust; }
        else if (_parameter == ShipInputParameters.lateral)
        { return lateral; }
        else if (_parameter == ShipInputParameters.vertical)
        { return vertical; }

        else if (_parameter == ShipInputParameters.roll)
        { return roll; }
        else if (_parameter == ShipInputParameters.turn)
        { return turn; }

        else if (_parameter == ShipInputParameters.weapons)
        { return weapons; }
        else { return 1; }

        
    }

}



public class Ship : MonoBehaviour
{
    public Rigidbody rb;

    //the engines push the ship 'forward' and combined with the lateral control this transform should be used
    // to apply the force of the engine to the correct forwar: If moving laterally while flying the 'forward' should
    // be slightly off center from camera logically
    public Transform mainTransform;


    public Transform rotationTarget;

    public bool canAct;

    public List<ShipSystem> systems;
    public int currentHealth;
    public float stamina, maxStamina, staminaRechargeRate;

    public float acceleration;

    public EngineBase primaryEngine;// primary engine for moving forward
    public EngineBase secondaryEngine;//secondary for turns and adjustments [can be the same engine]

    public WeaponBase primaryWeapon;

    public ChasisBase chasis;

    public Vector3 velocityTarget;

    public UiShipDisplay uiDisplay;

    private ShipInput shipInput;

    public ShipInput ShipInput() 
    {
        if (shipInput == null)
        { shipInput = new ShipInput(); }

        return shipInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddSystems();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (canAct)
        {
            Act();
        }



    }



    //Engine -> chasis -> weapons -> other

    public void Act()
    {

        RechargeStamina();

        Movement();

        Weapons();


    }

    public void EnemyAct()
    {
        //the ideal case is enemies are identical to the player in function(a ship is a ship)
        //In the interest of not over engineering dont figure out to represent the enemies needs as if they had a controller

 

        if (Chasis() && Chasis().ExternalForce() != Vector3.zero)
        {
            // SetVelocity(Chasis().ExternalForce());
            

        }
        else
        {
            if (PrimaryWeapon())
            {
                
            }
        }




    }


    public void Weapons()
    {
        if (PrimaryWeapon())
        {
            PrimaryWeapon().Act();
        }

    }


    public void Movement()
    {
        //get engines target velocity
        //

        float accel = 0;
        Vector3 newVelocity = Vector3.zero;

        if (PrimaryEngine())
        {
            //Get the engine's intended output
            newVelocity = PrimaryEngine().GetTargetVelocity();
            newVelocity += SecondaryEngine().Lateral() * PrimaryEngine().lateralPower;
            accel = primaryEngine.LinearAcceleration();

        }


        //NOTE: forces like recoild from a railgun or ground collisions are included here
        if (Chasis())
        {
            newVelocity = Chasis().ApplyExternalForces(newVelocity);

        }
        

        RB().velocity = newVelocity;

        if (PrimaryEngine())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, primaryEngine.GetTargetRotation(), primaryEngine.RotationAcceleration());

        }
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
        else if (Chasis() != null && collision.transform.GetComponent<Ship>())
        {
            CollideWithShip(collision);
        }

    }

    public virtual void ProcessTriggerEnter(Collider collision)
    {

    }




    public void CollideWithEnviroment(Collision collision)
    {
        // Chasis().ExternalForce(collision.impulse.magnitude * (transform.position - collision.contacts[0].point));
        // Chasis().ExternalForce(collision.impulse.magnitude * Vector3.Reflect(collision.contacts[0].point - transform.position, collision.impulse).normalized);

        if (Chasis() && Chasis().BelowMinimumMagnitude(collision.impulse))
        {
            Chasis().CollideWithEnviroment(collision);
            PrimaryEngine().CollideWithEnviroment(collision);
        }
        
        //TODO: engine after impact


    }

    public void CollideWithShip(Collision collision)
    {
        if (Chasis() && Chasis().BelowMinimumMagnitude(collision.impulse))
        {
            Chasis().ExternalForce(Vector3.Reflect(collision.contacts[0].point - transform.position, collision.impulse).normalized, collision.impulse.magnitude);
        }
       



    }


    public void TakeDamage(int _dmg)
    {
        //apply dmg reduc, external forces, other talents etc

        Hitpoints(-_dmg);
    }

    public void Hitpoints(int _change)
    {
        if (currentHealth != -1)
        {
            currentHealth += _change;

            if (currentHealth <= 0)
            { 
                //NOTE: die when it needs to 'make decision' to create the dying animation
                
                //Die();
            }
        }
        
    }

    public int Hitpoints()
    { return currentHealth; }


    public void Die()
    {

        Destroy(gameObject);
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
        if (_cost <= Mathf.Ceil(stamina))
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
    { canAct = _on; }

    public bool CanAct()
    { return canAct; }

    public WeaponBase PrimaryWeapon()
    {

        return primaryWeapon;
    }

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
    public Vector3 Forward() { return MainTransform().forward; }
    public Vector3 Right() { return MainTransform().right; }
    public Vector3 Up() { return MainTransform().up; }



    public void UpdateUi()
    {
        if (uiDisplay != null)
        { 
            uiDisplay.SetHP(Hitpoints().ToString());
            uiDisplay.SetStamina(stamina);
            uiDisplay.SetWeapons(PrimaryWeapon().timerCooldown.ToString());
        }
    }


    public Transform MainTransform()
    {
        if (mainTransform == null)
        { return transform; }

        return mainTransform;
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


}
