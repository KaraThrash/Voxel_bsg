using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameManager gameManager;
  public int rangeToChangeMapSegement,enemyType, enemyType2;
  public bool mainMap,mapSubSection,fillpatrolparent, fillspawnparent;
  public Transform mapSubSections;
  public Transform enemySpawnLocations, enemySpawnLocations2;
  public Transform patrolLocations;
  public Transform playerStartSpot;
    public List<string> mapevents;
    public float eventInterval, timer;
    public int eventNumber;
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> patspots = new List<Transform>();
        
        if (fillpatrolparent == true)
        {
            FillPatrolParent();
        }
        if (fillspawnparent == true)
        {
            FillSpawnParent();
        }
    }

   

    // Update is called once per frame
    void Update()
    {

    }

    public void TrackMapEvents(GameManager newgameManager)
    {
        gameManager = newgameManager;
        if (eventInterval != -1 && mapevents.Count > 0)
        {
            
            timer -= Time.deltaTime;
            if (timer <= 0)
            { 
                
                timer = eventInterval;
                MapEvent();
            }
        }
    
    }

    public void MapEvent()
    {
        if (eventNumber >= mapevents.Count)
        { eventNumber = 0; }

        if (mapevents[eventNumber] == "spawn one wing")
        {
            Vector3 newpos = enemySpawnLocations.GetChild(0).position;
            int count = 0;
            
            print("SPAWN STUFF");
            while (count < 5)
            {


                SpawnShip(newpos, enemySpawnLocations.GetChild(0).rotation);
                  newpos += (Vector3.up * 3);
                count++;
            }
            
        }
        else if (mapevents[eventNumber] == "spawn all wings")
        {
            foreach (Transform el in enemySpawnLocations)
            {
                Vector3 newpos = el.position;
                int count = 0;

                print("SPAWN STUFF");
                while (count < 5)
                {


                    SpawnShip(newpos,el.rotation);
                    newpos += (Vector3.up * 3);
                    count++;
                }
            }
        
        }

        eventNumber++;
    }

    public void SpawnShip(Vector3 newpos,Quaternion newrot)
    {
        GameObject clone = gameManager.npcManager.SpawnOne(enemyType, newpos, newrot, false, true); //in battle = false , alert = true
        if (mapSubSections.childCount > 0 && mapSubSections.GetChild(0).GetComponent<Map>().patrolLocations != null && clone != null && clone.GetComponent<Enemy>() != null)
        {
            clone.GetComponent<Enemy>().patrolparent = mapSubSections.GetChild(0).GetComponent<Map>().patrolLocations.gameObject;
            clone.GetComponent<Enemy>().SetAlert(true);
            clone.GetComponent<Enemy>().mapArea = mapSubSections.GetChild(0).gameObject;
        }
    }


    public int GetEnemyType()
    {return enemyType;}
    
    public int GetEnemyType2()
    {return enemyType2;}

    public Transform  GetSpawnParent()
    { return enemySpawnLocations; }

    public Transform GetSpawnParent2()
    { return enemySpawnLocations2; }


    public void FillSpawnParent()
    {
        //cycle through the child transform and find anything that is meant to be a spawn point
        List<Transform> spawnspots = new List<Transform>();
        MoveSpawnSpotsToParent(spawnspots, mapSubSections);
        foreach (Transform el in spawnspots)
        {
            if (el.tag == "spawn1")
            { el.parent = enemySpawnLocations2; }
             else
            { el.parent = enemySpawnLocations2; }
                
            el.GetComponent<MeshRenderer>().enabled = false;
        }
   
    }


    public void MoveSpawnSpotsToParent(List<Transform> spawnparent, Transform tocheck)
    {

        foreach (Transform el in tocheck)
        {
            if (el.tag.Substring(0,5) == "spawn") { spawnparent.Add(el); }
            if (el.childCount > 0) { MovePatrolSpotsToParent(spawnparent, el); }

        }


    }


    public void FillPatrolParent()
    {
        //cycle through the child transform and find anything that is meant to be a patrol point
        List<Transform> patspots = new List<Transform>();
        MovePatrolSpotsToParent(patspots, mapSubSections);
        foreach (Transform el in patspots)
        {
            el.parent = patrolLocations;
            el.GetComponent<MeshRenderer>().enabled = false;
        }
    }



    public void MovePatrolSpotsToParent(List<Transform> patrolparent,Transform tocheck)
    {
        
        foreach (Transform el in tocheck)
        {
            if (el.GetComponent<PatrolPoint>() != null) { patrolparent.Add(el); }
            if (el.childCount > 0) { MovePatrolSpotsToParent(patrolparent,el); }

        }
       
       
    }
}
