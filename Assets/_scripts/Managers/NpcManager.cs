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

  public void SpawnEnemiesForNewMap(Transform spawnAreas)
  {

    foreach(Transform go in spawnAreas)
    {
      GameObject clone = Instantiate(npcs[0], go.position, go.rotation) as GameObject;
      clone.transform.parent = enemyparent.transform;
      enemies.Add(clone);
      clone.GetComponent<Enemy>().ResetToNeutral(GetComponent<NpcManager>());
      clone.GetComponent<Enemy>().SetAlert(false);
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
        enemies.Remove(npckilled.gameObject);
        gameManager.playermanager.SpendMoney(-npckilled.value) ;

        Vector3 npcDieSpot = npckilled.transform.position;
        int npckilledItemType = npckilled.itemheldtype;
        Destroy(npckilled.gameObject);
        gameManager.itemManager.ItemDrop(npcDieSpot,npckilledItemType);

    }

    public GameObject GetClosestTarget(Vector3 fromPos)
    {

      return gameManager.playermanager.myship;

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

    public GameObject GetPlayerShip()
    {
      return gameManager.playermanager.myship;
    }


    public float GetDistanceToPlayer(Vector3 fromPos)
    {
      return Vector3.Distance(fromPos,gameManager.playermanager.GetPlayer().transform.position);
    }
}
