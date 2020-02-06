using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public int hp;
    public int shiphp;
    public int money;
    public Text hpText,moneytext;
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
            //myship.transform.position = playerSpawn.transform.position;
            //shiphp = 5;
            //myship.GetComponent<Rigidbody>().velocity = Vector3.zero; myship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //myship.transform.rotation = playerSpawn.transform.rotation;
            Destroy(myship);
            myship =  gamemanager.npcManager.GetComponent<NpcManager>().SpawnNewController(2, playerSpawn.transform.position,playerSpawn.transform.rotation);
            myship.GetComponent<ViperControls>().camerasphere = mycamera;
            myship.GetComponent<ViperControls>().myplayer = this.gameObject;
            mycamera.GetComponent<ThirdPersonCamera>().target = myship;
        }


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
