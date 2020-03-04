using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStar : MonoBehaviour {
    public Vector3 jumpinLocation;
    public bool jumpin;
    public Vector3 lastsafespot; //while jumping in to get that warp effect
    public float jumpinclock;
    public GameObject galactica;
    public int destroyedHangars,destroyedTurrets,destroyedEngines;
    public Transform hangars,turrets,engines;
    //turret type, turret count, fighter rtpe, fighter count, total dmg
    //slowly repair parts, when fighters/turrets are destroyed they have to be rebuilt over time

    // Use this for initialization
    void Start () {

       // transform.position = transform.position * 5;
        lastsafespot  = transform.position ;


    }

	// Update is called once per frame
	void Update () {
        if (jumpin == true)
        {
            JumpIn();
        }


  	}
    public int GetEngineStrength()
    {
      int str = 0;
      foreach(Transform go in engines)
      {
        if(go.GetComponent<ShipPart>().destroyed == false)
        {str++;}
      }
      return str;
    }
    public int GetHangarStrength()
    {
      int str = 0;
      foreach(Transform go in hangars)
      {
        if(go.GetComponent<ShipPart>().destroyed == false)
        {str++;}
      }
      return str;
    }
    public int GetTurretStrength()
    {
      int str = 0;
      foreach(Transform go in turrets)
      {
        if(go.GetComponent<ShipPart>().destroyed == false)
        {str++;}
      }
      return str;
    }

    public void SystemDestroyed()
    {}

    public void SystemDestroyed(string whichSystem)
    {
      if(whichSystem == "hangar"){}
      else if(whichSystem == "turret"){}
      else if(whichSystem == "engine"){}
      else{}
    }



    public void JumpIn()
    {
      transform.position = Vector3.MoveTowards(transform.position,jumpinLocation, 150.0f * Time.deltaTime);
      jumpinclock -= Time.deltaTime;
      if (jumpinclock < 0 )
      {

          lastsafespot = transform.position;
          jumpinclock = 1;

      }
      if ( Vector3.Distance(transform.position, galactica.transform.position) < 2000 || Vector3.Distance(transform.position, jumpinLocation) < 50)

      {

          transform.position = lastsafespot;
          jumpinclock = -1;
          SetHangars(5, 14);
      }
    }

    public void SetHangars(int spawnnumber,int spawnspeed)
    {

        if (jumpin == true)
        {

            // foreach (GameObject el in hangars)
            // {
            //     if (el != null)
            //     {
            //         el.GetComponent<Spawn>().spawnclock = spawnspeed; el.GetComponent<Spawn>().count = spawnnumber; el.GetComponent<Spawn>().spawnspeed = spawnspeed;
            //     }
            // }
            jumpin = false;
        }

    }

    public void OnCollisionEnter(Collision col)
    {

        if (jumpin == true)
        {
            SetHangars(5, 4);
            //jumpin = false;
            transform.position = lastsafespot * 1.2f;
            // GetComponent<Rigidbody>().isKinematic = true;
            jumpin = false;
        }

    }



}
