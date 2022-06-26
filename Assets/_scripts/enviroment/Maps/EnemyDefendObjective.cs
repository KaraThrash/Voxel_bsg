

public class EnemyDefendObjective : Map
{




    public override void Init()
    {
        GameManager().GetObjectiveEvent().AddListener(GameEventListener);
    }

    public override void ActiveMap()
    {

    }

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
    }

}
