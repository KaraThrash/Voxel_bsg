using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStar : MonoBehaviour {
    public Vector3 jumpinLocation;
    public List<GameObject> hangars;
    public bool jumpin;
    public Vector3 lastsafespot; //while jumping in to get that warp effect
    public float jumpinclock;
    public GameObject galactica;

    //turret type, turret count, fighter rtpe, fighter count, total dmg
    //slowly repair parts, when fighters/turrets are destroyed they have to be rebuilt over time

    // Use this for initialization
    void Start () {
        galactica = GameObject.Find("Battlestar");
        transform.rotation = galactica.transform.rotation;
       // transform.position = transform.position * 5;
        jumpin = true;
        lastsafespot  = transform.position ;


    }

	// Update is called once per frame
	void Update () {
        if (jumpin == true)
        {
            JumpIn();
        }


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

            foreach (GameObject el in hangars)
            {
                if (el != null)
                {
                    el.GetComponent<Spawn>().spawnclock = spawnspeed; el.GetComponent<Spawn>().count = spawnnumber; el.GetComponent<Spawn>().spawnspeed = spawnspeed;
                }
            }
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

    //public void SystemDestroyed(GameObject whichsystem)
    //{
    //    hangars.Remove(whichsystem);
    //}

}
