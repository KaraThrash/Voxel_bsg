using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public int destination;
    public int fleetlocation;
    public List<GameObject> maps;
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
        maps[destination].active = true;
        destination = -1;
    }
}
