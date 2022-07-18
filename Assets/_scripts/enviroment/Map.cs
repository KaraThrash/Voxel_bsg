using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : Manager
{
    [SerializeField]
    Transform gravityCenter;
    [SerializeField]
    Transform playerSpawn;

    public GameObject prefab_enemy;

    public Transform parent_PatrolPoints;

    public MapChunk currentChunk;
    private MapChunk previousChunk; //stagger disabling chunks to make changing over smoother and less noticable to the player

    public EnviromentType enviroment;

    public float gravityStrength;
    public float mapRadius;
 



    protected List<Map_POI> poi;



    public Vector3 GetNextPatrolPoint(int _point)
    {
        if (parent_PatrolPoints != null)
        {
            if (parent_PatrolPoints.childCount > 0 && _point >= 0)
            {
                return parent_PatrolPoints.GetChild(_point % parent_PatrolPoints.childCount).position;
            }
        }

        return Vector3.zero;
    }

    public List<Map_POI> GetPOIList()
    {
        if (poi == null || poi.Count == 0)
        {
            poi = new List<Map_POI>();

            foreach (Map_POI el in FindObjectsOfType<Map_POI>())
            {
                poi.Add(el);
            }

           
        }
            return poi;
    }


    public void Start()
    {
        if (GameManager())
        { 
            Init();

        }
    }

    public void FixedUpdate()
    {
        if (GameManager())
        {
            ActiveMap();

        }
        
    }


    public virtual void Init()
    {
        poi = new List<Map_POI>();

        foreach (Map_POI el in FindObjectsOfType<Map_POI>())
        {
            poi.Add(el);
        }

        GameManager().GetObjectiveEvent().AddListener(GameEventListener);
        
    }

    public virtual void ActiveMap()
    { 
    
    }


    public void EnterNewChunk()
    {
        if (PreviousChunk())
        { }

        PreviousChunk(CurrentChunk());


        foreach (SpawnSpot el in CurrentChunk().GetStartPositions())
        {
            GameManager().EnemyManager().SpawnEnemy(prefab_enemy, el.transform);
        }
    }

    public void EnterNewChunk(MapChunk _chunk)
    {
        if (PreviousChunk())
        { }

        PreviousChunk(CurrentChunk());

        CurrentChunk(_chunk);

        foreach (SpawnSpot el in CurrentChunk().GetStartPositions())
        {
            GameManager().EnemyManager().SpawnEnemy(el.prefab_Enemy, el.transform);
        }
    }


    public virtual void GameEventListener(InGameEvent _event)
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




    public bool OutOfBounds(Vector3 _actor)
    {

        Transform pointToCheckAgainst = gravityCenter;
        float dist = mapRadius;

        if (CurrentChunk())
        { 
            pointToCheckAgainst = CurrentChunk().transform;
            dist = CurrentChunk().Radius();
        }

        if (pointToCheckAgainst)
        {
            if (Vector3.Distance(_actor, pointToCheckAgainst.position) >= dist)
            {
                return true;
            }
        }
        else
        {
            if (_actor.magnitude > dist)
            { return true; }
        }

        

        return false;
    }


    public MapChunk CurrentChunk()
    {
        return currentChunk;
    }

    public void CurrentChunk(MapChunk _chunk)
    {
        currentChunk = _chunk;
    }

    public MapChunk PreviousChunk()
    {
        return previousChunk;
    }

    public void PreviousChunk(MapChunk _chunk)
    {
        previousChunk = _chunk;
    }


    public Vector3 GetPlayerSpawnPosition()
    {
        if (playerSpawn == null)
        {
            return transform.position;

        }

        return playerSpawn.position;
    }






    public Vector3 CenterOfMap()
    {
        if (gravityCenter)
        { return gravityCenter.position; }

        return transform.position;
    }


    public Vector3 Gravity()
    {
        //in the absence of a relative obj direct gravity from the origin
        if (gravityCenter)
        {
            return -(gravityCenter.position).normalized * gravityStrength;
        }

        return Vector3.down * 9.81f;
    }

    public Vector3 Gravity(Transform _obj)
    {
        //only the central object of the map excerts gravity [or nothing does: e.g. space]
        if (gravityCenter)
        {
            return (_obj.position - gravityCenter.position).normalized * gravityStrength;
        }

        return Vector3.down * 9.81f;
    }


    public void Enviroment(EnviromentType _env) { enviroment = _env; }
    public EnviromentType Enviroment( ) { return enviroment ; }




}
