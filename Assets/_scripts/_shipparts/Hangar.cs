using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : Actor
{

    public GameObject enemyPrefab;
    public Transform locationToSpawn;

    public float timeToSpawn;
    private float timer_Spawn;

    public Transform SpawnLocation()
    {
        if (locationToSpawn != null)
        { return locationToSpawn; }

        return transform;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHealth > 0 && timeToSpawn != 0 && enemyPrefab != null )
        { TrackTimeToSpawn(); }
        
    }

    public void TrackTimeToSpawn()
    {
        timer_Spawn += Time.deltaTime;
        if (timer_Spawn >= timeToSpawn)
        {
            timer_Spawn = 0;
            GameManager().EnemyManager().SpawnEnemy(enemyPrefab, SpawnLocation());

             
        }
    }


}
