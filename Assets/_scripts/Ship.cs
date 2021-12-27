using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int hp;
    public float speedMain, speedAdjust, speedTurn, speedTurnAdjust;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public virtual void ShipInput(ShipSystem _system,bool _on)
    { 
    
    
    }

    public virtual void ShipInput(ShipSystem _system, float _val)
    {


    }
}

