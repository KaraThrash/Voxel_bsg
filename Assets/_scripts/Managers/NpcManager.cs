using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour {
    public List<GameObject> npcs;
    public GameObject enemyparent;
    public GameManager gameManager;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SpawnOne(int whichone,Vector3 where, Quaternion rot)
    {
        GameObject clone = Instantiate(npcs[whichone], where, rot) as GameObject;
        clone.transform.parent = enemyparent.transform;
    }
    public GameObject SpawnNewController(int whichone, Vector3 where, Quaternion rot)
    {
        GameObject clone = Instantiate(npcs[whichone], where, rot) as GameObject;
       
        return clone;
    }

    public void NPCkilled(int moneyvalue)
    {
        gameManager.itemManager.SpendMoney(-moneyvalue) ;
    }

}
