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

    foreach(Transform go in enemyparent.transform)
    {
      enemies.Add(go.gameObject);
    }
    AlertNpcs(gameManager.mapManager.GetCurrentArea());
  }
	// Update is called once per frame
	void Update () {

	}
  public void AlertNpcs(GameObject newMapArea)
  {
    foreach(GameObject go in enemies)
    {
      if(go.GetComponent<Enemy>().mapArea == null)
      {
        go.GetComponent<Enemy>().mapArea = gameManager.mapManager.FindClosestArea(go.transform);
      }

      if(go.GetComponent<Enemy>().mapArea != newMapArea)
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
        Destroy(npckilled.gameObject);
    }

    public GameObject GetClosestTarget(Vector3 fromPos)
    {
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

    public float GetDistanceToPlayer(Vector3 fromPos)
    {
      return Vector3.Distance(fromPos,gameManager.playermanager.GetPlayer().transform.position);
    }
}
