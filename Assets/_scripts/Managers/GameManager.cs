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
    public int killstoadvance;
    public int raidersdestroyed;
    public int currentwave;
    public GameObject spwaner;
    public GameObject basestarSpawner;
    public GameObject enemyparent;
    public GameObject bulletparent;
    public GameObject hubmenu;
    public List<GameObject> basestarSpawners;
    public Text mapArea;
    public Text raidersdestroyedtext;
    public Text jumptimertext;
    public Text wavenumber;
    public float nextwaveclock; //after jump until the wave starts
    public float timetojumpclock; //after goal for the wave is achieved and the wave is ready to end
    public GameObject ftljumpimage;
    //public GameObject npcManager;
    public bool onmenuscreen;


    // Use this for initialization
    void Start () {
        wavenumber.text = "1";
        raidersdestroyedtext.text = "1";
     
    }
	
	// Update is called once per frame
	void Update () {

        if (nextwaveclock != -1)
        {
            nextwaveclock -= Time.deltaTime;
            if (nextwaveclock <= 0)
            {
                nextwaveclock = -1;
             
            }
        }

        if (timetojumpclock != -1)
        {
            if (timetojumpclock > 2)
            { jumptimertext.text = "Prepare to Jump"; }
            else { jumptimertext.text = "Jump imminent"; }

            timetojumpclock -= Time.deltaTime;

            if (timetojumpclock <= 0)
            {
                jumptimertext.text = "";
                timetojumpclock = -1;
                
            }
        }


    }

    public void TravelFromHub()
    {
        if (mapManager.destination != -1 && playermanager.myship != null)
        {
            mapArea.text = mapManager.destination.ToString();
            mapManager.MoveToNewArea();
            playermanager.myship.transform.position = Vector3.zero;
            playermanager.myship.transform.eulerAngles = Vector3.zero;
            hubmenu.active = false;
            playermanager.startnewlevel();
        }
    }
    public void TravelToHub()
    {
        npcManager.enemyparent.active = false; //TODO enemy parent objects based on map. THis needs to always clean up enemies
            mapArea.text = mapManager.destination.ToString();
            mapManager.ReturnToHub();
            if ( playermanager.myship != null)
            {
            hangarManager.addFighter(playermanager.myship);

            }
        playermanager.myship = null;
        //playermanager.endLevel();
            hubmenu.active = true;
            
        
    }
}
