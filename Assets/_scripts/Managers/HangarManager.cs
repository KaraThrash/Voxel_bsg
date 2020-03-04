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
        if (Input.GetKeyUp(KeyCode.T)) { addFighter(gameManager.playerManager.myship); }
    }
    public void selectFighter(int whichfighter)
    {
        if (gameManager.playerManager.myship != null)
        { addFighter(gameManager.playerManager.myship); }

        gameManager.playerManager.SelectFighter(fighters[whichfighter]);
        selectedFighter.active = true; //image displaying the selected fighter
        selectedFighter.transform.GetChild(0).GetComponent<Text>().text = hangarbuttons[whichfighter].transform.GetChild(1).GetComponent<Text>().text;
        selectedFighterObj = fighters[whichfighter];
        selectedFighter.GetComponent<Image>().sprite = hangarbuttons[whichfighter].GetComponent<Image>().sprite;
        //selectedFighter.GetComponent<Image>().color = hangarbuttons[whichfighter].GetComponent<Image>().color;
       // selectedFighterStats.text = gameManager.playerManager.myship.name;
        fighters.Remove(fighters[whichfighter]);
        SetHangarButtons();
        gameManager.itemManager.ToggleHangarDisplay(1,2,3,3,3);
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
                go.transform.GetChild(1).GetComponent<Text>().text = fighters[count].GetComponent<Fighter>().currenthp.ToString() + " / " + fighters[count].GetComponent<Fighter>().totalhp.ToString();
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
            hangarbuttons[fighters.Count - 1].transform.GetChild(1).GetComponent<Text>().text = newfighter.GetComponent<Fighter>().currenthp.ToString() + " / " + newfighter.GetComponent<Fighter>().totalhp.ToString();
            // TODO: go.GetComponent<Button>().image = shiptype



        }
    }
    public void RepairFighter(int whichfighter)
    {
        GameObject fighterToRepair;
        if (whichfighter == -1)
        {
            fighterToRepair = gameManager.playerManager.myship;

        }
        else
        {
            fighterToRepair = fighters[whichfighter];
        }

            if (fighterToRepair.GetComponent<Fighter>().currenthp < fighterToRepair.GetComponent<Fighter>().totalhp)
        {
            if (gameManager.itemManager.SpendMoney(1))
            {


                fighterToRepair.GetComponent<Fighter>().currenthp++;
                if (whichfighter == -1)
                { selectedFighter.transform.GetChild(0).GetComponent<Text>().text = fighterToRepair.GetComponent<Fighter>().currenthp.ToString() + " / " + fighterToRepair.GetComponent<Fighter>().totalhp.ToString(); }
                else
                { hangarbuttons[whichfighter].transform.GetChild(1).GetComponent<Text>().text = fighterToRepair.GetComponent<Fighter>().currenthp.ToString() + " / " + fighterToRepair.GetComponent<Fighter>().totalhp.ToString(); }

            }
        }

    }
}
