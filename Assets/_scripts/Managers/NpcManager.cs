using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour {
    public List<GameObject> enemies,npcs,fleetNpcs;
    public GameObject enemyparent;
    public GameManager gameManager;
    // Use this for initialization
    void Start () {

	}
  void Awake()
  {
    //NOTE: this is for testing/ devpurposes.
    enemies.Clear();
    foreach(Transform go in enemyparent.transform)
    {
      enemies.Add(go.gameObject);
    }
    AlertNpcs(gameManager.mapManager.GetCurrentArea());
  }
	// Update is called once per frame
	void Update () {

	}
  public void CleanUpEnemies()
  {
    foreach(GameObject go in enemies)
    {
      if(go != null)
      {
        Destroy(go);
      }

    }
    enemies.Clear();
  }

  public void SpawnEnemiesForNewMap(Map currentMap)
  {
    int count = 0;
    //iterate through the main maps sub areas, and then iterate through the sub pieces spawn locations
    foreach(Transform go in currentMap.mapSubSections)
    {
      count = 0;
      while(count < go.GetComponent<Map>().enemySpawnLocations.childCount)
      {
        GameObject clone = Instantiate(npcs[0], go.GetComponent<Map>().enemySpawnLocations.GetChild(count).position, go.GetComponent<Map>().enemySpawnLocations.GetChild(count).rotation) as GameObject;
        clone.transform.parent = enemyparent.transform;
        enemies.Add(clone);
        clone.GetComponent<Enemy>().ResetToNeutral(GetComponent<NpcManager>());
        clone.GetComponent<Enemy>().patrolparent = go.GetComponent<Map>().patrolLocations.gameObject;//.GetChild(count);
        clone.GetComponent<Enemy>().SetAlert(false);
        count++;
      }
    }

  }
  public void AlertNpcs(GameObject newMapArea)
  {
    foreach(GameObject go in enemies)
    {
      if(go.GetComponent<Enemy>().mapArea == null)
      {
        go.GetComponent<Enemy>().mapArea = gameManager.mapManager.FindClosestArea(go.transform);
      }

      if(go.GetComponent<Enemy>().mapArea != null &&  go.GetComponent<Enemy>().mapArea != newMapArea)
      {go.GetComponent<Enemy>().SetAlert(false);}
      else{go.GetComponent<Enemy>().SetAlert(true);}
    }

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

    public void NPCkilled(Enemy npckilled)
    {
      //remove this enemy from the list
        enemies.Remove(npckilled.gameObject);
        //generate points for the player
        gameManager.playerManager.SpendMoney(-npckilled.value) ;

        //save the spot where the enemy died and spawn an item drop at that Location
        //NOTE: destroy the enemy first so it doesnt collide and make the drop act erratic
        Vector3 npcDieSpot = npckilled.transform.position;
        int npckilledItemType = npckilled.itemheldtype;
        Destroy(npckilled.gameObject);
        gameManager.itemManager.ItemDrop(npcDieSpot,npckilledItemType);

    }

    public GameObject GetClosestTarget(Vector3 fromPos)
    {
        //NOTE: for prototype only target the player
      return gameManager.playerManager.myship;

      if(fleetNpcs.Count <= 0){return null;}
      GameObject closestTarget = fleetNpcs[0];
      float dist = Vector3.Distance(fromPos,closestTarget.transform.position);
      foreach(GameObject go in fleetNpcs)
      {
        if(Vector3.Distance(fromPos,closestTarget.transform.position) < dist)
        {
          dist = Vector3.Distance(fromPos,go.transform.position);
          closestTarget = go;
        }

      }
      return closestTarget;
    }
    public GameObject GetClosestEnemy(GameObject fromObject)
    {
      if( enemies.Count <= 0){return fromObject;}
      float currentDistance = 9999;
      GameObject closestEnemy = enemies[0];
      foreach(GameObject go in enemies)
      {
        if(Vector3.Distance(go.transform.position,fromObject.transform.position) < currentDistance)
        {
          currentDistance = Vector3.Distance(go.transform.position,fromObject.transform.position);
          closestEnemy = go;

        }

      }
      return closestEnemy;
    }

    public GameObject GetClosestUnConscriptedEnemy(GameObject fromObject)
    {
      if( enemies.Count <= 0){return fromObject;}
      float currentDistance = 9999;
      GameObject closestEnemy = fromObject;
      foreach(GameObject go in enemies)
      {
        if(go.GetComponent<Enemy>().conscriptable == true && go.GetComponent<Enemy>().squadLeader == null){
            if(Vector3.Distance(go.transform.position,fromObject.transform.position) < currentDistance)
            {
              currentDistance = Vector3.Distance(go.transform.position,fromObject.transform.position);
              closestEnemy = go;

            }
          }

      }
      return closestEnemy;
    }

    public GameObject GetPlayerShip()
    {
      return gameManager.playerManager.myship;
    }


    public float GetDistanceToPlayer(Vector3 fromPos)
    {
      return Vector3.Distance(fromPos,gameManager.playerManager.GetPlayer().transform.position);
    }
}
