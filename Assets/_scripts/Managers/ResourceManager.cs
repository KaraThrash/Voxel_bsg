using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceManager : MonoBehaviour {
    public int pop;
    public int food;
    public int fuel;
    public int morale;
    public int vipers;
    public int raptors;
    public Text poptext;
    public Text foodtext;
    public Text fueltext;
    public Text moraletext;
    // Use this for initialization
    void Start () {
        poptext.text = pop.ToString();
        foodtext.text = food.ToString();
        fueltext.text = fuel.ToString();
        moraletext.text = morale.ToString();
    }

	// Update is called once per frame
	void Update () {

	}


    public void ResourceChange(int popchange,int foodchange,int fuelchange,int moralechage)
    {
        pop += popchange;
        food += foodchange;
        fuel += fuelchange;
        morale += moralechage;
    poptext.text = pop.ToString();
  foodtext.text = food.ToString();
    fueltext.text = fuel.ToString();
    moraletext.text = morale.ToString();
   }
}
