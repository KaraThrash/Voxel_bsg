using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIattackpattern : MonoBehaviour {
    public GameObject target;
    public float closedistance;
    public float fardistance;
    private Quaternion targetRotation;
    public int speed;
    public int rotForce = 6;
    private Rigidbody rb;
    public bool flyaway;
    public bool flypast;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GetFarAndComeBack(GameObject targetship)
    {

        //gunCooldown -= Time.deltaTime; 

       // targetRotation = Quaternion.LookRotation(targetship.transform.position - transform.position);

        if (flyaway == true)
        {

            if (flypast == true)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > closedistance)
                {
                  
                    flypast = false;

                }
            }
            else
            {
                targetRotation = Quaternion.LookRotation(transform.position - target.transform.position);
                
            }
           


            if (Vector3.Distance(transform.position, target.transform.position) > fardistance)
            {
                flyaway = false;
                flypast = false;

            }

        }
        else
        {
           targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) < closedistance)
            {
                flyaway = true;
                flypast = true;

            }


        }
       
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        
    }

}
