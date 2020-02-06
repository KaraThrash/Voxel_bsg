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




    //
    // public Player playermanager;
    // public MapManager mapManager;
    // public HangarManager hangarManager;
    // public NpcManager npcManager;
    // public ItemManager itemManager;
    // public int killstoadvance;
    // public int raidersdestroyed;
    // public int currentwave;
    // public GameObject spwaner;
    // public GameObject basestarSpawner;
    // public GameObject enemyparent;
    // public GameObject bulletparent;
    // public GameObject hubmenu;
    // public List<GameObject> basestarSpawners;
    // public Text mapArea;
    // public Text raidersdestroyedtext;
    // public Text jumptimertext;
    // public Text wavenumber;
    // public float nextwaveclock; //after jump until the wave starts
    // public float timetojumpclock; //after goal for the wave is achieved and the wave is ready to end
    // public GameObject ftljumpimage;
    // //public GameObject npcManager;
    // public bool onmenuscreen;


    // Use this for initialization
    void Start () {

    }

	// Update is called once per frame
	void Update () {



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
