using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSystem : MonoBehaviour
{
    public SystemType systemType;

    public Ship ship;

    public float power;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Thruster();
    }


    public void Thruster()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * power * Time.deltaTime);
    }





    public SystemType SystemType() { return systemType; }
    public void SystemType(SystemType _type) {  systemType = _type; }
}
