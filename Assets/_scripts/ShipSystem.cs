using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{
    public bool listenForPlayerInput;
    public SystemType systemType;
    public SystemState systemState;

    public Ship ship;

    public Rigidbody rbTarget;
    public Buttons listenForButton;
    public float staminaCost;

    public float power;

    private float lockoutTimer;//for taking damage, being frozen or other circumstances that render this system unusable

    public bool on;

    public KeyCode activateKey,deactivateKey;
    public string activateButton,deactivateButton, axis;
    public string positiveButton,negativeButton;




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
        if (ship != null && ship.GetComponent<Rigidbody>() != null && GetComponent<Rigidbody>() != null)
        {

           // FixedJoint joint = ship.gameObject.AddComponent<FixedJoint>();
           // joint.connectedBody = GetComponent<Rigidbody>();
        }
        deactivateKey = activateKey;
        deactivateButton = activateButton;
    }

    // Update is called once per frame
    void Update()
    {
        Act();

        if (listenForPlayerInput)
        {
            PlayerInput();
        }


    }



    public virtual void Act()
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

    public bool PositiveButton()
    {

        return InputControls.CheckButton(listenForButton);

    }

    public Ship Ship() { return ship; }
    public void Ship(Ship _type) { ship = _type; }

    public SystemType GetSystemType() { return systemType; }
    public void SetSystemType(SystemType _type) {  systemType = _type; }

    public SystemState GetSystemState() { return systemState; }
    public void SetSystemState(SystemState _type) { systemState = _type; }
}
