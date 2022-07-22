
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;


public class GameManager : Manager {
    public static GameManager instance
    {
        get => singleton.instance;
    }

    private static GlobalSingletonGetter<GameManager> singleton =
    new GlobalSingletonGetter<GameManager>(gameObjectName: "GameManager");


    public GameState gameState;
   
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

        DontDestroyOnLoad(this);

        SetGameState(GameState.menu_OutofGame);

        GetObjectiveEvent().AddListener(ObjectiveEvent);
        GetEnemyDeathEvent().AddListener(EnemyDeathEvent);
        GetPlayerDeathEvent().AddListener(PlayerDeathEvent);

        SceneManager.sceneLoaded += StartLevel;

        Player().Ship().GetEquipment().GetStatList().Clear();
    }



    // Update is called once per frame
    void Update () {

        if (GetGameState() == GameState.playing)
        {
            Player().Playing();


        }
        else if (GetGameState() == GameState.playersdead)
        {



        }
        else if (GetGameState() == GameState.menu_OutofGame)
        {



        }

        ListenToInput();


    }

    public void ListenToInput()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
           // GetObjectiveEvent().Invoke(InGameEvent.fleetShipLost);
            GetEnemyDeathEvent().Invoke(EnemyManager().EnemyManager().Enemies()[0]);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GetObjectiveEvent().Invoke(InGameEvent.objectiveLost);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MenuManager().ShowFleetShips();
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ItemManager().DropItem(ItemTypes.currency,1,Player().Ship().MainTransform().position + (Player().Ship().MainTransform().forward * 5));

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager().TravelFromHub(1);
            //GameConstants.TYPE_A = !GameConstants.TYPE_A;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //GameConstants.TYPE_A = !GameConstants.TYPE_A;
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (MenuManager().screen_pause.gameObject.activeSelf)
            {
                MenuManager().DisableMenus();
                cam.SetInMenu(false);
            }
            else
            {
                MenuManager().OpenPauseMenu();
                cam.SetInMenu(true);
            }

           
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
      TimeManager().ResetTheClock();


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





    public  void  StartLevel(Scene scene, LoadSceneMode mode)
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

    public void ForUiButton_ObjectiveEvent(int _event)
    {
        Debug.Log((InGameEvent)_event);
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



    

}
