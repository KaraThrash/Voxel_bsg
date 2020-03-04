using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour {
  public GameManager gameManager;
    public GameObject hubparentmenu,hangarMenu,fleetMenu,mapmenu,ftlmenu,enemyFleetMenu;

    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
  public bool OpenMenu()
  {

    //enable menu objects on the canvas if they are currently disabled
    //or turn them off if already in menu
    if(hubparentmenu.active == false)
    {
       hubparentmenu.active = true;
        HubMenuChange("hangar");
        return true;
      }
      else
      {
    hubparentmenu.active = false;
    return false;
    }

  }
    public void HubMenuChange(string whichmenu)
    {

        hangarMenu.active = false;
        fleetMenu.active = false;
        mapmenu.active = false;
        ftlmenu.active = false;
        enemyFleetMenu.active = false;
        switch (whichmenu)
        {
            case "hangar":
                hangarMenu.active = true;
            break;
            case "fleet":
                fleetMenu.active = true;
                gameManager.fleetManager.UpdateInfo();
            break;
            case "map":
                mapmenu.active = true;
            break;
            case "ftl":
                ftlmenu.active = true;
            break;
            case "enemyfleet":
                enemyFleetMenu.active = true;
                gameManager.enemyFleetManager.UpdateInfo();
            break;
           default:
              hangarMenu.active = true;
             break;
        }
    }


    public void FTLTravelMenu(int dest)
    {
      gameManager.TravelFromHub(dest);

    }
}
