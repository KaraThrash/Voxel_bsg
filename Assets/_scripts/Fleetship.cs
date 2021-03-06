﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleetship : MonoBehaviour {
    public GameObject gameManager;
    public FleetManager fleetManager;
    public Transform shipParts;
    public int pop;
    public int food;
    public int fuel;
    public int morale;
    public int totalsubsystems,maxhp; //hp
    public GameObject resourcemanager,rubble;
    public float dieclock;
    public bool hasresources;
    public int value; //points
    public string name;
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
        {

          dieclock -= Time.deltaTime;
            if (dieclock <= 0)
            { ShipDestroyed(); }


        }
	}

  public Transform GetShipParts()
  {
    if(shipParts != null){  return shipParts;}
    return this.transform;
  }

  public Transform GetClosestShipPart(Vector3 fromPos)
  {
      Transform closestTarget = this.transform;
    if(shipParts != null){

      float dist = 9999;
      foreach(Transform go in shipParts)
      {
        if(go.GetComponent<ShipPart>() != null && go.GetComponent<ShipPart>().destroyed == false && Vector3.Distance(fromPos,go.position) < dist)
        {
          dist = Vector3.Distance(fromPos,go.transform.position);
          closestTarget = go;
        }

      }


    }
    return closestTarget;
  }

    public void SystemDestroyed()
    {
        totalsubsystems--;
        if (totalsubsystems <= 0)
        { dieclock = 10.0f;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().angularDrag = 0;
            GetComponent<Rigidbody>().drag = 0;
            GetComponent<Rigidbody>().velocity = Vector3.down * 1.2f;
              GetComponent<Rigidbody>().angularVelocity = (Vector3.down +  Vector3.right) * Random.Range(0.01f, 0.2f);
            if (fleetManager != null) 
            { fleetManager.ShipDestroyed(GetComponent<Fleetship>()); }
            
              if (hasresources == true && resourcemanager != null)
              {
                  resourcemanager.GetComponent<ResourceManager>().ResourceChange(-pop, -food, -fuel, -morale);

              }
              if (value > 0)
              {
                 // gameManager.GetComponent<GameManager>().RaiderDestroyed(value);
              }
        }

    }
    public void ShipDestroyed()
    {
      if(rubble != null){

        GameObject clone = Instantiate(rubble,transform.position,transform.rotation);
        clone.active = true;
      }
        Destroy(this.gameObject);
    }
}
