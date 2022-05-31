using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    List<SpawnSpot> enemyStartPositions; //space that can be moved through
    List<Transform> open; //space that can be moved through
    List<Transform>  poi;


    public Map map;
    public Transform parent_startPositions;

    public Transform watchThis;
    public float mapRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public float Radius() { return mapRadius; }

    public List<SpawnSpot> GetStartPositions()
    {
        if (enemyStartPositions == null)
        {
            enemyStartPositions = new List<SpawnSpot>();

            if (parent_startPositions != null)
            {
                foreach (Transform el in parent_startPositions)
                {
                    if (el.GetComponent<SpawnSpot>())
                    { enemyStartPositions.Add(el.GetComponent<SpawnSpot>()); }
                }
            }
        }

        return enemyStartPositions;
    }



}
