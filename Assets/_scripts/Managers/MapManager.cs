using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameManager gameManager;
    public Map currentMap,fireLinkMap; //main map for post fleet jumps
    public int destination;
    public int fleetlocation;
    public float rangeToChangeMapSegement;
    public Transform currentMapParent;
    public GameObject fleetMarker,activeMapArea; // active map area used for segmenting parts of the map for enemy logic
    public List<GameObject> maps, battlemaps,maplocations; // the world objects, and the ftl menu locations
    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update ()
  {
      if(activeMapArea != null)
      {
        //check the location of the player ship against the map segment to start enabling adjacent npcs
        GameObject playerShip = gameManager.playerManager.GetPlayer();
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
  public Map GetCurrentMap()
  {
    return currentMap;

  }
  public GameObject FindClosestArea(Transform playerShip)
  {
    //find the closest map section
      float dist = rangeToChangeMapSegement * 2;
      GameObject newarea = currentMap.mapSubSections.GetChild(0).gameObject;
      foreach(Transform go in currentMap.mapSubSections)
      {
        if(Vector3.Distance(playerShip.position,go.position) < dist)
        {
          newarea = go.gameObject;
          dist = Vector3.Distance(playerShip.position,go.position);
        }

      }

      //check to see if the new piece has a difference range to change over to the next one
      if(newarea.GetComponent<Map>().rangeToChangeMapSegement > 0){rangeToChangeMapSegement = newarea.GetComponent<Map>().rangeToChangeMapSegement;}
      else{rangeToChangeMapSegement = 100;}

      return newarea;
  }
    public void SetDestination(int target)
    {
        // destination = target;
    }
    public void MoveToNewArea(int newArea)
    {
        fireLinkMap.gameObject.active = false;

        foreach (GameObject go in maps)
        { go.active = false; }
        foreach (GameObject go in battlemaps)
        { go.active = false; }
        //firelink map is trhe post jump fleet location
        if(newArea == 0)
        {
          fireLinkMap.gameObject.active = true;
          currentMap = fireLinkMap;
        }
        else  if(newArea < 0)
        {
          if(battlemaps.Count > Mathf.Abs(newArea))
          {
            battlemaps[Mathf.Abs(newArea)].active = true;
            currentMap = battlemaps[Mathf.Abs(newArea)].GetComponent<Map>();
          }

        }else
        {

          if(maps.Count > newArea)
          {
            maps[newArea].active = true;
            currentMap = maps[newArea].GetComponent<Map>();
          }

        }



    }


    public void ReturnToHub()
    {
       //  foreach (GameObject go in maps)
       //  { go.active = false; }
       // // fleetMarker.transform.position = maplocations[destination].transform.position;
       // // maps[destination].active = true;
       //  destination = -1;
    }
}
