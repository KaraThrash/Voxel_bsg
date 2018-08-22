using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dradis : MonoBehaviour {
    public int sensortarget;
    public int mytype;//for when others target this
    public bool radar; // radar == true when this is detecting false when it is for being targeted
    public GameObject myship;
    public GameObject target;
    public int maxrange;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > maxrange)
            { target = null; }
            
        }
	}

    public void OnTriggerEnter(Collider col)
    {
        if (target == null)
        {if (col.gameObject.tag == "dradistarget" )
                if (col.GetComponent<Dradis>().mytype == sensortarget)
                { target = col.GetComponent<Dradis>().myship; }
                
        }
    }

}
