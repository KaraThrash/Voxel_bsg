using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HangarManager : MonoBehaviour {
    public GameManager gameManager;
    public List<GameObject> fighters;
    public List<GameObject> hangarbuttons;
    public GameObject selectedFighter, selectedFighterObj,defaultfighter;

    public Text selectedFighterStats;
    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {
        // if (Input.GetKeyUp(KeyCode.T)) { addFighter(gameManager.playerManager.myship); }
    }

}
