using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public int rangeToChangeMapSegement,enemyType, enemyType2;
  public bool mapSubSection,fillpatrolparent, fillspawnparent;
  public Transform mapSubSections;
  public Transform enemySpawnLocations, enemySpawnLocations2;
  public Transform patrolLocations;
  public Transform playerStartSpot;
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
    public int GetEnemyType()
    {return enemyType;}
    
    public int GetEnemyType2()
    {return enemyType2;}


    public void FillSpawnParent()
    {
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
