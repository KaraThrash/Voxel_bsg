using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public int hp;
    public int shiphp;
    public int money,droppedMoney;
    public Text hpText,moneytext;
    public GameObject corpseMoneyDrop;
    public GameObject mycamera;
    public GameObject playerSpawn;
    public GameObject myship;
    public GameManager gamemanager;
    // Use this for initialization
    void Start () {
      // Item newitem = new Item{name = "red"};
	}

	// Update is called once per frame
	void Update () {

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
    public void SelectFighter(GameObject fighterselected)
    {
        myship = fighterselected;
        myship.transform.parent = this.transform;
            fighterselected.transform.position = playerSpawn.transform.position;

        fighterselected.transform.rotation = playerSpawn.transform.rotation;
        myship.GetComponent<ViperControls>().camerasphere = mycamera;
        myship.GetComponent<ViperControls>().myplayer = this.gameObject;
        mycamera.GetComponent<ThirdPersonCamera>().target = myship;

    }
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
          droppedMoney = money;
          money = 0;

          gamemanager.PlayerShipDestroyed();

    }
    public void RespawnPlayer()
    {
      Vector3 dieSpot =   myship.transform.position;
      myship.GetComponent<Rigidbody>().isKinematic = true;
      myship.transform.position = playerSpawn.transform.position;
      myship.transform.rotation = playerSpawn.transform.rotation;
      //drop your held currency to be available to be picked up again

      GameObject clone = Instantiate(corpseMoneyDrop,dieSpot,transform.rotation);
      if(clone.GetComponent<PickUp>() != null)
      {clone.GetComponent<PickUp>().SetAsPlayerCache();}


      myship.GetComponent<Rigidbody>().isKinematic = false;
      shiphp = hp;
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
        myship.active = true;
        mycamera.GetComponent<ThirdPersonCamera>().enabled = true;
    }
    public void endLevel()
    {
        mycamera.GetComponent<ThirdPersonCamera>().enabled = false;
        if (myship != null)
        { myship.active = false; }
        gamemanager.TravelToHub();


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void restartlevel() {
        //for web build
        Application.LoadLevel(Application.loadedLevel);
    }
}
