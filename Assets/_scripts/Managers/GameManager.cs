
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance
    {
        get => singleton.instance;
    }

    private static GlobalSingletonGetter<GameManager> singleton =
    new GlobalSingletonGetter<GameManager>(gameObjectName: "GameManager");


    public GameState gameState;
    public Player player;
    public EnemyManager enemyManager;
   // public PlayerManager playerManager;
    public Menus menuManager;
    public MapManager mapManager;
    public NpcManager npcManager;
    public ItemManager itemManager;
    public WorldTime timeManager;
    public ThirdPersonCamera cam;
    public FtlImageFade imageFade;
    public GameObject dockMenu;
    public ScrollingText scrollingText;
    public Text contextText;

    public ObjectiveEvent event_Objective;
    public EnemyEvent event_EnemyDeath;
    public UnityEvent event_PlayerDeath;

    


 


    // Use this for initialization
    void Start () {

        //DontDestroyOnLoad(this);

        GetObjectiveEvent().AddListener(ObjectiveEvent);
        GetPlayerDeathEvent().AddListener(PlayerDeathEvent);

        SceneManager.sceneLoaded += StartLevel;
    }



    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.M) )
        {
            GetObjectiveEvent().Invoke(InGameEvent.fleetShipLost);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GetObjectiveEvent().Invoke(InGameEvent.objectiveLost);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TravelFromHub(1);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Player().InitForLevel();
            Player().Ship().transform.position = MapManager().GetMap().GetPlayerSpawnPosition();
            MenuManager().StartInGame();

            MapManager().GetMap().EnterNewChunk();

            EnemyManager().StartLevel();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameConstants.typeA = !GameConstants.typeA;
        }


        if (GetGameState() == GameState.playing)
        {
            Player().Playing();


        }
        else if (GetGameState() == GameState.playersdead)
        {



        }




    }






    public void PlayerShipDestroyed()
    {
      imageFade.StartFade();
     
    }



    public void StartNewMap()
    {
     
      //reset player ship rotation to the docking ship
      cam.ResetCameraAngle();
    }




    //rest at a bonfire
    public void Dock(Transform dock,Transform dockSpawnSpot)
    {
    

    }

    public void ResetTheClock()
    {
      timeManager.ResetTheClock();


    }
    public void EnemyAttack(bool autoresolve)
    {
     

    }

    public void JumpFleet()
    {

    }

    //can be called from the ftl menu screen after a target is selected
    public void TravelFromHub(int dest)
    {
        SceneManager.LoadScene(dest);
        SetGameState(GameState.playing);



    }

    public void SpendExtraLife()
    {
        //deduct a ship from the player's inventory
        //move the player back to spawn, reset their hitpoints
        Player().InitForLevel();
        Player().Ship().transform.position = MapManager().GetMap().GetPlayerSpawnPosition();
        MenuManager().StartInGame();
        SetGameState(GameState.playing);
        EnemyManager().StartLevel();
    }


    public void StartLevel(Scene scene, LoadSceneMode mode)
    {
        if (GetGameState() == GameState.playing)
        {
            Player().InitForLevel();
            Player().Ship().transform.position = MapManager().GetMap().GetPlayerSpawnPosition();
            MenuManager().StartInGame();

            MapManager().GetMap().EnterNewChunk();

            EnemyManager().StartLevel();


        }
        
        
    }








    public void EnemyDeathEvent(Enemy _enemy)
    {
        Debug.Log("EnemyDeathEvent: ");
        
    }

    public EnemyEvent GetEnemyDeathEvent()
    {
        if (event_EnemyDeath == null)
        {
            event_EnemyDeath = new EnemyEvent();

        }
        return event_EnemyDeath;
    }

    public void PlayerDeathEvent( )
    {
        Debug.Log("PlayerDeathEvent: " );
        //decide to spend a life or restart or return to firelink
        SetGameState(GameState.playersdead);
        MenuManager().PlayerDeath();
    }
    public UnityEvent GetPlayerDeathEvent()
    {
        if (event_PlayerDeath == null)
        {
            event_PlayerDeath = new UnityEvent();

        }
        return event_PlayerDeath;
    }

    public void ObjectiveEvent(InGameEvent _event)
    {
        Debug.Log(_event);
        //throw new NotImplementedException();

    }
    public UnityEvent<InGameEvent> GetObjectiveEvent()
    {
        if (event_Objective == null)
        {
            event_Objective = new ObjectiveEvent();
        }

        return event_Objective;
    }







    public void SetGameState(GameState _state)
    { gameState = _state; }
    public GameState GetGameState()
    { return gameState; }

    public Player Player()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        return player;
    }

    public EnemyManager EnemyManager()
    {
        if (enemyManager == null)
        {
            enemyManager = FindObjectOfType<EnemyManager>();
        }
        return enemyManager;
    }

    public Menus MenuManager()
    {
        if (menuManager == null)
        {
            menuManager = FindObjectOfType<Menus>();
        }
        return menuManager;
    }

    public MapManager MapManager()
    {
        if (mapManager == null)
        {
            mapManager = FindObjectOfType<MapManager>();
        }
        return mapManager;
    }

}
