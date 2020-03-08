using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

public FleetManager fleetManager;
  public Player playerManager;
  public EnemyFleet enemyFleetManager;
  public Menus menuManager;
  public MapManager mapManager;
  public HangarManager hangarManager;
  public NpcManager npcManager;
  public ItemManager itemManager;
  public WorldTime timeManager;
  public InfoDisplay infoManager;
  public ThirdPersonCamera cam;
  public FtlImageFade imageFade;

  public GameObject dockMenu;
  public bool inMenu,inBattle;


    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {

    if(Input.GetKeyDown(KeyCode.P))
    {
      StartNewMap();
    }

    }
    public void PlayerShipDestroyed()
    {
      imageFade.StartFade();
      playerManager.RespawnPlayer();

      //NOTE: nothing should respawn on player death
      //since the player can immediately dock if they want to respawn stuff so they can still easily farm
      // StartNewMap();
    }

    public bool GetInBattle()
    {return inBattle;}


    public void StartNewMap()
    {
      //clean up enemies cvurrently spawned
      npcManager.CleanUpEnemies();
      //get new map

      //spawn enemies for each designated spawn location
      npcManager.SpawnEnemiesForNewMap(mapManager.GetCurrentMap());
      //reset player hp

    }

    public void ActivateMenu()
    {
     //when opening the menu the manager returns true, false if it is closing the menu
     //unlock or lock the mouse based on the menu state
     inMenu = menuManager.OpenMenu();

      cam.SetInMenu(  inMenu);
      timeManager.SetInMenu(inMenu);
      //set the player controller so that when in menu the ship isnt flying around
     playerManager.SetInMenu(inMenu);
     //TODO: think about pausing or not
    }

    //rest at a bonfire
    public void Dock(Transform dock,Transform dockSpawnSpot)
    {
      //for transitions block the player view and slowly return it to make transportation from locations and reseting the map less jarring
      imageFade.StartFade();
      playerManager.SetPlayerSpawn(dockSpawnSpot.gameObject);
      playerManager.RespawnPlayer();
      StartNewMap();


    }

    public void JumpFleet()
    {

      enemyFleetManager.baseStar.gameObject.active = false;
      mapManager.MoveToNewArea(-2);
      mapManager.GetCurrentMap();
      // playerManager.SetPlayerSpawn(  mapManager.GetCurrentMap().playerStartSpot.gameObject);
      playerManager.startnewlevel();
      StartNewMap();
      ActivateMenu();
    }

    public void TravelFromHub(int dest)
    {
        if (dest != -1 && playerManager.myship != null)
        {
            mapManager.MoveToNewArea(dest);
            mapManager.GetCurrentMap();
            playerManager.SetPlayerSpawn(  mapManager.GetCurrentMap().playerStartSpot.gameObject);
            playerManager.startnewlevel();
            StartNewMap();
            ActivateMenu();
        }
    }

    public void TravelToHub()
    {

        npcManager.enemyparent.active = false; //TODO enemy parent objects based on map. THis needs to always clean up enemies
            mapManager.ReturnToHub();

    }
}
