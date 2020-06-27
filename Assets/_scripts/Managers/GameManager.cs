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
  public AttackManager attackManager;
  public ThirdPersonCamera cam;
  public FtlImageFade imageFade;

  public GameObject dockMenu;
  public bool inMenu,inBattle,inMap;


    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {

          if(inMenu == true){playerManager.InMenuActions();}
          else if(inBattle == true){playerManager.InBattleActions();}
          else if(inMap == true){playerManager.InMapActions();}
          else{}



          if(Input.GetKeyDown(KeyCode.P))
          {
            StartNewMap();
          }

    }


    public void PlayerShipDestroyed()
    {
      imageFade.StartFade();
            //have enemies that were targeting the player stop
      npcManager.PlayerDie();
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

      //fleet ships and base stars are only around during the battles, regular map levels are just the player
      if(inBattle == true)
      {
        StartBattle();
      }else
      {

        fleetManager.fleetShips.gameObject.active = false;
        fleetManager.galactica.gameObject.active = false;
        enemyFleetManager.baseStar.gameObject.active = false;
        mapManager.StartNewMap(playerManager.myship.transform);
      }
      //reset player ship rotation to the docking ship
      cam.ResetCameraAngle();
    }

    public void StartBattle()
    {
      fleetManager.fleetShips.gameObject.active = true;
      fleetManager.galactica.gameObject.active = true;
      enemyFleetManager.baseStar.gameObject.active = true;
      enemyFleetManager.StartFleetBattle();
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
    public void ResetTheClock()
    {
      timeManager.ResetTheClock();


    }
    public void EnemyAttack(bool autoresolve)
    {
      if(autoresolve == true)
      {attackManager.AutoResolve(fleetManager,enemyFleetManager);}
      else
      {//if not auto resolve then the player is joining the battle
          TravelFromHub(attackManager.GetBattleMap());

      }

    }

    public void JumpFleet()
    {

      enemyFleetManager.baseStar.gameObject.active = false;
      TravelFromHub(0);
      // mapManager.MoveToNewArea(0); //jump is to firelink //a safe location
      // mapManager.GetCurrentMap();
      // // playerManager.SetPlayerSpawn(  mapManager.GetCurrentMap().playerStartSpot.gameObject);
      // playerManager.startnewlevel();
      // StartNewMap();
      // ActivateMenu();
        ResetTheClock();
    }

    //can be called from the ftl menu screen after a target is selected
    public void TravelFromHub(int dest)
    {
        if (playerManager.myship != null)
        {
            mapManager.MoveToNewArea(dest);
            mapManager.GetCurrentMap();
            playerManager.SetPlayerSpawn(  mapManager.GetCurrentMap().playerStartSpot.gameObject);
            playerManager.startnewlevel();
            if(dest < 0){inBattle = true;}
            else{inBattle = false;}
            StartNewMap();
            ActivateMenu();
        }
    }


}
