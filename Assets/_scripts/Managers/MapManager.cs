using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameManager gameManager;
    public Map currentMap;
    public int destination;
    public int fleetlocation;
    public float rangeToChangeMapSegement;
    public Transform currentMapParent;
    public GameObject fleetMarker,activeMapArea; // active map area used for segmenting parts of the map for enemy logic
    public List<GameObject> maps, maplocations; // the world objects, and the ftl menu locations
    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update ()
  {
      if(activeMapArea != null)
      {
        //check the location of the player ship against the map segment to start enabling adjacent npcs
        GameObject playerShip = gameManager.playermanager.GetPlayer();
        if(Vector3.Distance(playerShip.transform.position,activeMapArea.transform.position) > rangeToChangeMapSegement)
        {
          //enable next spot
          activeMapArea = FindClosestArea(playerShip.transform);
          gameManager.npcManager.AlertNpcs(activeMapArea);
        }
      }
	}
  public GameObject GetCurrentArea()
  {
    return activeMapArea;

  }
  public GameObject FindClosestArea(Transform playerShip)
  {
    //find the closest map section
      float dist = rangeToChangeMapSegement * 2;
      GameObject newarea = currentMapParent.GetChild(0).gameObject;
      foreach(Transform go in currentMapParent)
      {
        if(Vector3.Distance(playerShip.position,go.position) < dist)
        {
          newarea = go.gameObject;
          dist = Vector3.Distance(playerShip.position,go.position);
        }

      }
      return newarea;
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
