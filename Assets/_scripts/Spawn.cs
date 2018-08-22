using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public int mynpcfromlist;
    public GameObject tospawn;
    public GameObject gm;
    public GameObject enemyparent;
    public int count;
    public float countmod;
    public float spawnclock;
    public float spawnspeed;
    public bool haschildren;
    public bool randomspawn;
    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager");
        enemyparent = GameObject.Find("EnemyParent");
        
        //while (count > 0)
        //{
        //    countmod = count * 35;
        //    Instantiate(tospawn, new Vector3(transform.position.x + countmod, transform.position.y, transform.position.z), transform.rotation);
        //    Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y, transform.position.z + countmod), transform.rotation);
        //    Instantiate(tospawn, new Vector3(transform.position.x - countmod, transform.position.y, transform.position.z), transform.rotation);
        //    Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y, transform.position.z - countmod), transform.rotation);
        //    count--;
        //}
    }
	
	// Update is called once per frame
	void Update () {
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
    public void SpawnOne( )
    {
        count--;
        if (count <= 0) { spawnclock = -1; }
        //     Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);


        

        if (haschildren == true)
        {
           // gm.GetComponent<GameManager>().npcManager.GetComponent<NpcManager>().SpawnOne(mynpcfromlist, transform.GetChild(count % 5).position, transform.rotation);
            GameObject clone = Instantiate(tospawn, transform.GetChild(count % transform.childCount).position * 5, transform.rotation) as GameObject;
            clone.transform.parent = enemyparent.transform;
            //  GameObject clone = Instantiate(tospawn, transform.GetChild(count % 5).position, transform.rotation) as GameObject;
            // clone.transform.parent = enemyparent.transform;
        }
        else
        {
           // gm.GetComponent<GameManager>().npcManager.GetComponent<NpcManager>().SpawnOne(mynpcfromlist, transform.position, transform.rotation);
            GameObject clone = Instantiate(tospawn, transform.position, transform.rotation) as GameObject;
            clone.transform.parent = enemyparent.transform;
        }
    }
    public void SpawnStuff(int newcount)
    {
        count = newcount;
        while (count > 0)
        {
            if (countmod % 2 == 0)
            {
                countmod = count * 33;
            }
            else {
                countmod = count * -33;
            }
            if (count % 2 == 0)
            {
                
                Instantiate(tospawn, new Vector3(transform.position.x + countmod, transform.position.y, transform.position.z), transform.rotation);
            }
            else
            { Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y + countmod, transform.position.z), transform.rotation); }

          
            //Instantiate(tospawn, new Vector3(transform.position.x + countmod, transform.position.y, transform.position.z), transform.rotation);
            //Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y, transform.position.z + countmod), transform.rotation);
            //Instantiate(tospawn, new Vector3(transform.position.x - countmod, transform.position.y, transform.position.z), transform.rotation);
            //Instantiate(tospawn, new Vector3(transform.position.x, transform.position.y, transform.position.z - countmod), transform.rotation);
            count--;
        }
    }

}
