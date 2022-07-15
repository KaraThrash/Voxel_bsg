using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL_DestroyObjectives : Map
{
    public int interval_spawnEvent;
    private float timer_spawnEvent;

    public int maxEnemyOnScreen;
    public int enemyPerWave;

    public int interval_timeBetweenSpawn;
    private float timer_timeBetweenSpawn;


    public int count_enemyLeftToSpawn;

    public int tracker_enemySpawned;
    public int tracker_objDestroyed;


    public override void Init()
    {
        poi = new List<Map_POI>();

        foreach (Map_POI el in FindObjectsOfType<Map_POI>())
        {
            poi.Add(el);
        }

        GameManager().GetObjectiveEvent().AddListener(GameEventListener);
        if (interval_spawnEvent > 0)
        {
            
            timer_spawnEvent = interval_spawnEvent;
            
        }
    }


    public override void ActiveMap()
    {
        if ( maxEnemyOnScreen > EnemyManager().EnemyParent().childCount)
        {
            if (count_enemyLeftToSpawn > 0)
            {
                timer_timeBetweenSpawn -= Time.deltaTime;


                if (timer_timeBetweenSpawn <= 0)
                {

                    Spawn();
                }


            }
            else
            {
                if (interval_spawnEvent > 0)
                {
                    timer_spawnEvent -= Time.deltaTime;
                    if (timer_spawnEvent <= 0)
                    {
                        timer_spawnEvent = interval_spawnEvent;
                    }
                }
            }

            


        }


    }


    public void Spawn()
    {

        foreach (Map_POI el in GetPOIList())
        {
            if (el.Map() == this && el.isSpawnPoint)
            {
                SubID newsubid = SubID.none;
                if (count_enemyLeftToSpawn % 3 == 0)
                { newsubid = SubID.A; }
                else if (count_enemyLeftToSpawn % 2 == 0)
                { newsubid = SubID.B; }
                else 
                { newsubid = SubID.C; }
                EnemyManager().SpawnEnemy(prefab_enemy, el.transform, newsubid);
                timer_timeBetweenSpawn = interval_timeBetweenSpawn;
                count_enemyLeftToSpawn--;
                return;
            }

            if (timer_timeBetweenSpawn > 0)
            { break; }


            //el.SpawnOne();
        }
    }




    public override void GameEventListener(InGameEvent _event)
    {

        if (_event == InGameEvent.objectiveLost)
        {
            count_enemyLeftToSpawn = enemyPerWave ;
            timer_timeBetweenSpawn = interval_timeBetweenSpawn;
            Spawn();

        }
        else if (_event == InGameEvent.enemyKilled)
        {
            foreach (Map_POI el in GetPOIList())
            {
                if (el.Map() == this && el.isSpawnPoint)
                {
                    EnemyManager().SpawnEnemy(prefab_enemy, el.transform);
                }

                //el.SpawnOne();
            }
        }


    }

}
