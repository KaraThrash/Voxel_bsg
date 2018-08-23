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
        if (Input.GetKeyUp(KeyCode.T)) { addFighter(gameManager.playermanager.myship); }
    }
    public void selectFighter(int whichfighter)
    {
        if (gameManager.playermanager.myship != null)
        { addFighter(gameManager.playermanager.myship); }
        gameManager.playermanager.SelectFighter(fighters[whichfighter]);
        selectedFighter.active = true;
        selectedFighterObj = fighters[whichfighter];
        selectedFighter.GetComponent<Image>().sprite = hangarbuttons[whichfighter].GetComponent<Image>().sprite;
        selectedFighterStats.text = gameManager.playermanager.myship.name;
        fighters.Remove(fighters[whichfighter]);
        SetHangarButtons();
        //hangarbuttons[whichfighter].active = false;
    }
    public void SetHangarButtons()
    {
        int count = 0;
        foreach (GameObject go in hangarbuttons)
        {
            if (count < fighters.Count)
            {
                go.active = true;
                // TODO: go.GetComponent<Button>().image = shiptype

            }
            else { go.active = false; }
            count++;
        }
    }

    public void addFighter(GameObject newfighter)
    {
        if (fighters.Count < hangarbuttons.Count)
        {
            newfighter.active = false;
            newfighter.transform.parent = this.transform;
            newfighter.transform.position = transform.position;
            fighters.Add(newfighter);
            hangarbuttons[fighters.Count - 1].active = true;
            // TODO: go.GetComponent<Button>().image = shiptype

            

        }
    }

}
