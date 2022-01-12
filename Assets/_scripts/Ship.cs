using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public bool canAct;

    public List<ShipSystem> systems;

    public float stamina, maxStamina, staminaRechargeRate;

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

            RechargeStamina();
            
        }



    }

   


    public void Act()
    {
        if (InputControls.ActionButtonDown())
        {
            Debug.Log("InputControls.RightTrigger() <>" + InputControls.RightTrigger());
            Control(SystemType.engine, 1);
        }

        else if (InputControls.ActionButtonUp())
        {
            Control(SystemType.engine, false);
        }
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
        // round stamina check so that a cost of 0.2 will use 0.1 stamina leaving -0.1
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






    /// UI
    /// 

    public Text staminaBar;

    public void SetStaminaText(string _stamina)
    {
        if (staminaBar != null) { staminaBar.text = stamina.ToString(); }
    }

    ///





}
