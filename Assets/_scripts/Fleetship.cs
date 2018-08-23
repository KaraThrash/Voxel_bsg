using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleetship : MonoBehaviour {
    public GameObject gameManager;
    public int pop;
    public int food;
    public int fuel;
    public int morale;
    public int totalsubsystems; //hp
    public GameObject resourcemanager;
    public float dieclock;
    public bool hasresources;
    public int value; //points
    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager");
        if (hasresources == true)
        {
            resourcemanager.GetComponent<ResourceManager>().ResourceChange(pop, food, fuel, morale);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (dieclock != -1)
        { dieclock -= Time.deltaTime;
            if (dieclock <= 0)
            { ShipDestroyed(); }
        }
	}

    public void SystemDestroyed()
    {
        totalsubsystems--;
        if (totalsubsystems <= 0)
        { dieclock = 10.0f;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().angularDrag = 0;
            GetComponent<Rigidbody>().drag = 0;
            GetComponent<Rigidbody>().velocity = Vector3.down * 5.0f;
        }

    }
    public void ShipDestroyed()
    {
        if (hasresources == true)
        {
            resourcemanager.GetComponent<ResourceManager>().ResourceChange(-pop, -food, -fuel, -morale);
        }
        if (value > 0)
        {
           // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
        }
        Destroy(this.gameObject);
    }
}
