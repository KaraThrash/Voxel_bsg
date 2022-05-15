
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager instance
    {
        get => singleton.instance;
    }

    private static GlobalSingletonGetter<GameManager> singleton =
    new GlobalSingletonGetter<GameManager>(gameObjectName: "GameManager");


    public Player player;
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
    public bool inMenu,inBattle,inMap;

    public ObjectiveEvent event_Objective;

    public Player Player()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        return player;
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


    // Use this for initialization
    void Start () {

        GetObjectiveEvent().AddListener(ObjectiveEvent);

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
      
    }

 


}
