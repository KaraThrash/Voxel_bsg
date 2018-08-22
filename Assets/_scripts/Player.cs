using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public int hp;
    public int shiphp;
    public Text hpText;
    public GameObject mycamera;
    public GameObject playerSpawn;
    public GameObject myship;
    public GameManager gamemanager;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
            myship.GetComponent<ViperControls>().camforward = mycamera.GetComponent<ThirdPersonCamera>().myfwdobj;
            myship.GetComponent<ViperControls>().myplayer = this.gameObject;
            mycamera.GetComponent<ThirdPersonCamera>().target = myship;
        }

        int count = 0;
        string tempstring = "";
        while (count < shiphp)
        { tempstring += "I"; count++; }
        hpText.text = tempstring;
    }
    public void restartlevel() {
        //for web build
        Application.LoadLevel(Application.loadedLevel);
    }
}
