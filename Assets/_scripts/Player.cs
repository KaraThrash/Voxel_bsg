using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {
  public PlayerControls playerControls;
  public PlayerShipStats playerShipStats;
    public int hp;
    public int shiphp;
    public int money,droppedMoney;
    public bool atDock,inMenu;
    public Text hpText,moneytext,contextButton;
    public GameObject contextButtonUi;
    public GameObject corpseMoneyDrop;
    public GameObject mycamera;
    public GameObject playerSpawn;
    public GameObject myship,viperShip,raptorShip;
    public GameManager gamemanager;
    // Use this for initialization
    void Start () {
      // ChangeShips(0);
        playerControls.SetShipStats(playerShipStats);
	}

	// Update is called once per frame
	void Update () {


          if(Input.GetKeyDown(KeyCode.Y))
          {ChangeShips(0);}

	}

  public void InMapActions()
  {  if(  myship.GetComponent<Rigidbody>().isKinematic == true){  myship.GetComponent<Rigidbody>().isKinematic = false;}
    ControlShip();
    if(atDock == true && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton6)))
    {gamemanager.ActivateMenu();}
  }

  public void InBattleActions()
  {
    if(  myship.GetComponent<Rigidbody>().isKinematic == true){  myship.GetComponent<Rigidbody>().isKinematic = false;}
    ControlShip();
    if(atDock == true && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton6)))
    {gamemanager.ActivateMenu();}
  }

  public void InMenuActions()
  {
    if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.JoystickButton6))
    {gamemanager.ActivateMenu();}
    if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
    {gamemanager.menuManager.ScrollThroughMenu(-1);}
    if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
    {gamemanager.menuManager.ScrollThroughMenu(1);}

    myship.GetComponent<Rigidbody>().isKinematic = true;
  }

  public void ChangeShips(int changeto)
  {
    //TODO: this need a complete overhaul

      playerControls.ChangeShip(playerShipStats,changeto);

  }
  public void ControlShip()
  {
        if(myship.GetComponent<PlayerControls>() != null )
        {
          myship.GetComponent<PlayerControls>().ControlShip();
        }

  }
  public void SetInMenu(bool menuStatus)
  {
      myship.GetComponent<Rigidbody>().isKinematic = false;
    inMenu = menuStatus;
  }
  public void ListenForInput()
  {


  }

  public void NearDock(bool entering)
  {
    atDock = false;
    contextButton.text = "";
    if(entering == true)
    {
      atDock = true;
      // contextButtonUi.active = true;
      contextButton.text = "Tab for Dock Menu";
    }
    else
    {
      //if knocked off the dock, exit the menu
      if(gamemanager.inMenu == true )
      {gamemanager.ActivateMenu();}
      // contextButtonUi.active = false;
      // contextButton.text = "";
    }

  }

  public GameObject GetPlayer()
  {
    return myship;
  }

  public void PickUpCache()
  {
    money += droppedMoney;
    droppedMoney = 0;
  }
    public bool SpendMoney(int cost)
    {

        if (money > cost)
        { money -= cost;
            moneytext.text = money.ToString();
            return true;
        }
        return false;
    }

    // public void SelectFighter(GameObject fighterselected)
    // {
    //     myship = fighterselected;
    //     myship.transform.parent = this.transform;
    //         fighterselected.transform.position = playerSpawn.transform.position;
    //
    //     fighterselected.transform.rotation = playerSpawn.transform.rotation;
    //     myship.GetComponent<ViperControls>().camerasphere = mycamera;
    //     myship.GetComponent<ViperControls>().myplayer = this.gameObject;
    //     mycamera.GetComponent<ThirdPersonCamera>().target = myship;
    //
    // }


    public void vehicletakingdamage(int dmgtaken)
    {
        shiphp = shiphp - dmgtaken;
        if (shiphp <= 0)
        {
          ShipDestroyed();
        }

        SetHPBar();
    }
    public void ShipDestroyed()
    {
      //track total lost money?
      //TODO: lost money buffs missing npcs
          droppedMoney = money;
          money = 0;
          Vector3 dieSpot =   myship.transform.position;
          myship.GetComponent<Rigidbody>().isKinematic = true;
              //drop your held currency to be available to be picked up again
          GameObject clone = Instantiate(corpseMoneyDrop,dieSpot,transform.rotation);
          if(clone.GetComponent<PickUp>() != null)
          {clone.GetComponent<PickUp>().SetAsPlayerCache();}

          //handle the logic of dying before respawning
          gamemanager.PlayerShipDestroyed();

    }

    public void RespawnPlayer()
    {
      myship.GetComponent<Rigidbody>().isKinematic = true;
      myship.transform.position = playerSpawn.transform.position;
      myship.transform.rotation = playerSpawn.transform.rotation;



      myship.GetComponent<Rigidbody>().isKinematic = false;
      //TODO: dont refill health when traveling, right now this is used when moving to a new area
      shiphp = hp;

      playerControls.SetShipStats(playerShipStats);
    }

    public void SetHPBar()
    {

      int count = 0;
      string tempstring = "";
      while (count < shiphp)
      { tempstring += "I"; count++; }
      hpText.text = tempstring;
    }


    public void startnewlevel()
    {
      RespawnPlayer();
        myship.active = true;
        mycamera.GetComponent<ThirdPersonCamera>().enabled = true;
    }


    public void endLevel()
    {
        mycamera.GetComponent<ThirdPersonCamera>().enabled = false;
        if (myship != null)
        { myship.active = false; }



        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void SetPlayerSpawn(GameObject newSpawn)
    {
      playerSpawn = newSpawn;
    }


    public void restartlevel() {
        //for web build
        Application.LoadLevel(Application.loadedLevel);
    }

}
