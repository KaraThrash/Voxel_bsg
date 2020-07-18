using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
  public int rangeToChangeMapSegement,enemyType, enemyType2;
  public bool mapSubSection;
  public Transform mapSubSections;
  public Transform enemySpawnLocations, enemySpawnLocations2;
  public Transform patrolLocations;
  public Transform playerStartSpot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GetEnemyType()
    {return enemyType;}
    
    public int GetEnemyType2()
    {return enemyType2;}
}
