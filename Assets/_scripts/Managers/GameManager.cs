using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Player Player()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        return player;
    }

    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {

          



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
