using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public int destination;
    public int fleetlocation;
    public GameObject fleetMarker;
    public List<GameObject> maps, maplocations; // the world objects, and the ftl menu locations
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetDestination(int target)
    {
        destination = target;
    }
    public void MoveToNewArea()
    {
        foreach (GameObject go in maps)
        { go.active = false; }
        fleetMarker.transform.position = maplocations[destination].transform.position;
        maps[destination].active = true;
        destination = -1;
    }
    public void ReturnToHub()
    {
        foreach (GameObject go in maps)
        { go.active = false; }
       // fleetMarker.transform.position = maplocations[destination].transform.position;
       // maps[destination].active = true;
        destination = -1;
    }
}
