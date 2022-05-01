using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject tospawn;
    public GameObject toWatch;

    public GameObject enemyparent;


    public int count;
    public float countmod;
    public float spawnclock;
    public float spawnspeed;
    public bool randomspawn;

    void Start () {

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (toWatch == null && tospawn != null)
        {
            toWatch =  SpawnOne();
            toWatch.SetActive(true);
        }

        if (spawnclock != -1 && count > 0)
        {
            spawnclock -= Time.deltaTime;
            if (spawnclock <= 0)
            {

                SpawnOne();
                
                if (randomspawn == true)
                { spawnclock = Random.Range(20.0f,150.0f); }
                else { spawnclock = spawnspeed; }
            }
        }
	}
    public GameObject SpawnOne( )
    {
        count--;
        if (count <= 0) { spawnclock = -1; }

        GameObject clone = Instantiate(tospawn, transform.position, transform.rotation) as GameObject;
        clone.transform.parent = enemyparent.transform;
        return clone;
    }
    

}
