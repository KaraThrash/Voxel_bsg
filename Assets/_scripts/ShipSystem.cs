﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipSystem : MonoBehaviour
{
    private GameManager gameManager;
    public bool listenForPlayerInput;

    public SystemType systemType;
    public SystemState systemState;

    public Ship ship;
  

    public Rigidbody rbTarget;
    public Buttons listenForButton;
    public float staminaCost;

    [Min(0.01f)]
    public float  STAT_cooldownTime;

    public float  STAT_power;
    public float  STAT_powerSecondary;

    public float  timer_Cooldown;


    protected float lockoutTimer;//for taking damage, being frozen or other circumstances that render this system unusable

    public bool on;

    public KeyCode activateKey,deactivateKey;
    public string activateButton,deactivateButton, axis;
    public Buttons positiveButton,negativeButton;


    public void STAT_CooldownTime(float _val) { STAT_cooldownTime = _val; }
    public float STAT_CooldownTime( ) 
    {
        if (Ship() != null)
        {
            if (Ship().GetEquipment() != null)
            {
                if (systemType == SystemType.armor)
                {
                    return Ship().GetEquipment().GetStats()[Stats.armor];

                }
                else if (systemType == SystemType.engine)
                {
                    return Ship().GetEquipment().GetStats()[Stats.speed];

                }
                else if (systemType == SystemType.weapon)
                {
                    return Ship().GetEquipment().GetStats()[Stats.fireRate];

                }

            }

        }
        return STAT_cooldownTime;
    }

    public bool OnCooldown() { return timer_Cooldown > 0 || timer_Cooldown == -1 ; }

    public void STAT_Power(float _val) { STAT_power = _val; }
    public float STAT_Power() 
    {
        if (Ship() != null)
        {
            if (Ship().GetEquipment() != null)
            {
                if (systemType == SystemType.armor)
                {
                    return Ship().GetEquipment().GetStats()[Stats.armor];

                }
                else if (systemType == SystemType.engine)
                {
                    return Ship().GetEquipment().GetStats()[Stats.speed];

                }
                else if (systemType == SystemType.weapon)
                {
                    return Ship().GetEquipment().GetStats()[Stats.damage];

                }

            }

        }


        return STAT_power;
    }

    public void STAT_PowerSecondary(float _val) { STAT_powerSecondary = _val; }
    public float STAT_PowerSecondary()
    {
        if (Ship() != null)
        {
            if (Ship().GetEquipment() != null)
            {
                if (systemType == SystemType.armor)
                {
                    return Ship().GetEquipment().GetStats()[Stats.armor];

                }
                else if (systemType == SystemType.engine)
                {
                    return Ship().GetEquipment().GetStats()[Stats.mobility];

                }
                else if (systemType == SystemType.weapon)
                {
                    return Ship().GetEquipment().GetStats()[Stats.projectileSpeed];

                }

            }

        }

        return STAT_powerSecondary;
    }

    public void SetActivateKey(KeyCode _key) { activateKey = _key; }
    public void SetActivateButton(string _key) { activateButton = _key; }

    public void SetDectivateKey(KeyCode _key) { deactivateKey = _key; }
    public void SetDectivateButton(string _key) { deactivateButton = _key; }

    public void SetAxis(string _key) { axis = _key; }


    public void LockoutTimer(float _value)
    { lockoutTimer = _value; }
    public float LockoutTimer()
    { return lockoutTimer; }


    // Start is called before the first frame update
    void Start()
    {
        if (Ship() != null && ship.GetComponent<Rigidbody>() != null && GetComponent<Rigidbody>() != null)
        {

           // FixedJoint joint = ship.gameObject.AddComponent<FixedJoint>();
           // joint.connectedBody = GetComponent<Rigidbody>();
        }
        deactivateKey = activateKey;
        deactivateButton = activateButton;

        
    }

    void Update()
    {
       
        if (listenForPlayerInput)
        {
            PlayerInput();
        }

        Act();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Act_Fixed();

        TrackCooldown();

   


    }

    public virtual void TrackCooldown()
    {
        if (timer_Cooldown > 0)
        {
            timer_Cooldown -= Time.deltaTime;
        }
    }

    public virtual void Act()
    {
        if (on && (ship && ship.CanAct()))
        {

        }
    }

    public virtual void Act_Fixed()
    {
        if (on && (ship && ship.CanAct()))
        {
            
        }
    }

    public virtual void PlayerInput()
    {

    }


    public float StaminaCost()
    {

        //if (Ship() != null)
        //{
        //    if (Ship().GetEquipment() != null)
        //    {
        //        if (systemType == SystemType.armor)
        //        {
        //            return Ship().GetEquipment().GetStats()[Stats.armor];

        //        }
        //        else if (systemType == SystemType.engine)
        //        {
        //            return Ship().GetEquipment().GetStats()[Stats.mobility];

        //        }
        //        else if (systemType == SystemType.weapon)
        //        {
        //            return Ship().GetEquipment().GetStats()[Stats.projectileSpeed];

        //        }

        //    }

        //}
        return staminaCost;
    }
    public void StaminaCost(float _cost)
    {
        staminaCost = _cost;
    }

    public virtual void Control(KeyCode _input)
    {
     
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

    public virtual void CollideWithEnviroment(Collision collision)
    {

    }

    public virtual void CollideWithShip(Collision collision)
    {

    }

    public virtual int DamageFromCollision(Collision collision)
    {

        return 0;
    }


    public void RbTarget(Rigidbody _rb) { rbTarget = _rb; }

    public Rigidbody RbTarget() 
    {
        if (rbTarget == null )
        {
            if ( GetComponent<Rigidbody>() != null)
            { rbTarget = GetComponent<Rigidbody>(); }
            else{ rbTarget = this.gameObject.AddComponent<Rigidbody>(); }
        }

        return rbTarget;
    }


    public bool PositiveButtonDown()
    {

        return InputControls.CheckButtonPressed(listenForButton);

    }

    public bool PositiveButtonUp()
    {

        return InputControls.CheckButtonReleased(listenForButton);

    }

    public bool PositiveButton()
    {

        return InputControls.CheckButton(positiveButton);

    }

    public bool NegativeButton()
    {

        return InputControls.CheckButton(negativeButton);

    }

    public bool NegativeButtonDown()
    {

        return InputControls.CheckButtonPressed(negativeButton);

    }

    public Ship Ship() { return ship; }
    public void Ship(Ship _type) { ship = _type; }

    public SystemType GetSystemType() { return systemType; }
    public void SetSystemType(SystemType _type) {  systemType = _type; }

    public SystemState GetSystemState() { return systemState; }
    public void SetSystemState(SystemState _type) { systemState = _type; }






    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }


}
