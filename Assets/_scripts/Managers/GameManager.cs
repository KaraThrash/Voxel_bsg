using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  public Player playermanager;
  public MapManager mapManager;
  public HangarManager hangarManager;
  public NpcManager npcManager;
  public ItemManager itemManager;
  public ThirdPersonCamera cam;
  public FtlImageFade imageFade;

  public GameObject dockMenu;



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
      playermanager.RespawnPlayer();

      //NOTE: nothing should respawn on player death
      //since the player can immediately dock if they want to respawn stuff so they can still easily farm
      // StartNewMap();
    }




    public void StartNewMap()
    {
      //clean up enemies cvurrently spawned
      npcManager.CleanUpEnemies();
      //get new map

      //spawn enemies for each designated spawn location
      npcManager.SpawnEnemiesForNewMap(mapManager.GetCurrentMap());
      //reset player hp

    }

    public void ActivateMenu(int whichMenu)
    {
        // dockMenu.active = false;
      switch(whichMenu)
      {
        case 0:

        break;
        case 1: //at bonfire menu
        if(dockMenu.active == true){
          dockMenu.active = false;
          cam.SetInMenu(false);
        }
        else{dockMenu.active = true;
          //set the camera to not lock the cursor when in a menu
          cam.SetInMenu(true);
        }

        break;
        default:
        break;
      }

    }

    //rest at a bonfire
    public void Dock(Transform dock,Transform dockSpawnSpot)
    {
      //for transitions block the player view and slowly return it to make transportation from locations and reseting the map less jarring
      imageFade.StartFade();
      playermanager.SetPlayerSpawn(dockSpawnSpot.gameObject);
      playermanager.RespawnPlayer();
      StartNewMap();


    }
    public void TravelFromHub()
    {
        if (mapManager.destination != -1 && playermanager.myship != null)
        {
            mapManager.MoveToNewArea();
            playermanager.myship.transform.position = Vector3.zero;
            playermanager.myship.transform.eulerAngles = Vector3.zero;
            playermanager.startnewlevel();
        }
    }

    public void TravelToHub()
    {

        npcManager.enemyparent.active = false; //TODO enemy parent objects based on map. THis needs to always clean up enemies
            mapManager.ReturnToHub();

    }
}
