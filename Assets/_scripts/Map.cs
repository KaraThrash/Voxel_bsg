using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public int rangeToChangeMapSegement,enemyType, enemyType2;
  public bool mapSubSection,fillpatrolparent;
  public Transform mapSubSections;
  public Transform enemySpawnLocations, enemySpawnLocations2;
  public Transform patrolLocations;
  public Transform playerStartSpot;
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> patspots = new List<Transform>();
        if (fillpatrolparent == true)
        { MovePatrolSpotsToParent(patspots, mapSubSections);
            foreach (Transform el in patspots)
            {
                el.parent = patrolLocations;
                el.GetComponent<MeshRenderer>().enabled = false;
            }
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


    public void MovePatrolSpotsToParent(List<Transform> patrolparent,Transform tocheck)
    {
        
        foreach (Transform el in tocheck)
        {
            if (el.GetComponent<PatrolPoint>() != null) { patrolparent.Add(el); }
            if (el.childCount > 0) { MovePatrolSpotsToParent(patrolparent,el); }

        }
       
       
    }
}
