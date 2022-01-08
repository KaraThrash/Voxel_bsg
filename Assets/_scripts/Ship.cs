using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public bool canAct;

    public List<ShipSystem> systems;

    // Start is called before the first frame update
    void Start()
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
            

            if (el.SystemType() == _system)
            {

            }
        }
    }
    public virtual void Control(SystemType _system, float _value)
    {

        foreach (ShipSystem el in systems)
        {
            el.Activate();

            if (el.SystemType() == _system)
            {

            }
        }
    }

    public void CanAct(bool _on)
    {canAct = _on;}

    public bool CanAct()
    { return canAct; }
}
