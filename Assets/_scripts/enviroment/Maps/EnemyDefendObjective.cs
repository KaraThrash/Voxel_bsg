

public class EnemyDefendObjective : Map
{




    public override void Init()
    {
        GameManager().GetObjectiveEvent().AddListener(ObjectiveEvent);
    }

    public override void ActiveMap()
    {

    }

    public override void ObjectiveEvent(InGameEvent _event)
    {

        if (_event == InGameEvent.objectiveLost)
        {
            foreach (Spawn el in GetSpawnSpots())
            {
                if (el.Map() == this)
                {
                    EnemyManager().SpawnEnemy(el.EnemyPrefab(), el.transform);
                }

                //el.SpawnOne();
            }
        }
    }

}
