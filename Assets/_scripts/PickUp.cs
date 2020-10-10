using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
  public ItemManager itemManager;
    public int type;
    public int value, itemnumber; //if engine speed, if gun attack cooldown
    public bool playerCache,primaryResource;
    public Material[] colors; //green,red,blue,yellow
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void SetWhichItem(ItemManager newItemManager,int newitemnumber)
    {
      itemManager = newItemManager;
        value = 1;
        itemnumber = newitemnumber;
        if(transform.GetChild(0).GetComponent<Renderer>() != null)
        {
          transform.GetChild(0).GetComponent<Renderer>().material = colors[1];
          if (newitemnumber == 7) { transform.GetChild(0).GetComponent<Renderer>().material = colors[0]; }
          if (newitemnumber == 8) { transform.GetChild(0).GetComponent<Renderer>().material = colors[2]; }
         // GetComponent<Renderer>().material = colors[Mathf.Abs(newitemnumber) % 3];
        }


    }
    public void SetAsPlayerCache()
    {
      playerCache = true;
    }
}
