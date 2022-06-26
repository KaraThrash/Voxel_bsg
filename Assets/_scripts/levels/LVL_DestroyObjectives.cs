using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL_DestroyObjectives : Map
{
    


    [SerializeField]
    private List<Transform> spawnLocations;
    private List<Transform> patrolPoints;


    public override void GameEventListener(InGameEvent _event)
    {

        if (_event == InGameEvent.objectiveLost)
        {
            foreach (Map_POI el in GetPOIList())
            {
                if (el.Map() == this)
                {
                    EnemyManager().SpawnEnemy(prefab_enemy, el.transform);
                }

                //el.SpawnOne();
            }
        }
        else if (_event == InGameEvent.enemyKilled)
        {
            foreach (Map_POI el in GetPOIList())
            {
                if (el.Map() == this && el.spawnSpot)
                {
                    EnemyManager().SpawnEnemy(prefab_enemy, el.transform);
                }

                //el.SpawnOne();
            }
        }


    }

}
