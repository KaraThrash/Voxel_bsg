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
  public FtlImageFade imageFade;




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
      StartNewMap();
    }

public void StartNewMap()
{
  //clean up enemies cvurrently spawned
  npcManager.CleanUpEnemies();
  //get new map

  //spawn enemies for each designated spawn location
  npcManager.SpawnEnemiesForNewMap(mapManager.currentMap.enemySpawnLocations);
  //reset player hp

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
