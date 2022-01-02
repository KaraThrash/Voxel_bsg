using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
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
        
    }

    public virtual void Control(SystemType _system,bool _on) 
    {
    
    }
    public virtual void Control(SystemType _system,float _value) 
    {
        
        foreach (ShipSystem el in systems)
        {
            if (el.SystemType() == _system)
            { }
        }
    }


}
