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
    public int ftlSpoolTime;//time before the fleet can jump again at no cost
    public Text poptext;
    public Text foodtext;
    public Text fueltext;
    public Text moraletext;
    // Use this for initialization
    void Start () {
        UpdateResourceText();
    }

	// Update is called once per frame
	void Update () {

	}

    public void UpdateResourceText()
    {
      poptext.text = pop.ToString();
      foodtext.text = food.ToString();
      fueltext.text = fuel.ToString();
      moraletext.text = morale.ToString();
    }

    public void ResourceChange(int popchange,int foodchange,int fuelchange,int moralechage)
    {
        pop += popchange;
        food += foodchange;
        fuel += fuelchange;
        morale += moralechage;
        UpdateResourceText();
   }
}
