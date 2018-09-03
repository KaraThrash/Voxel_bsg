using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
    public int type;
    public int value, itemnumber; //if engine speed, if gun attack cooldown
    public Material[] colors; //green,red,blue,yellow
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetWhichItem(int newitemnumber)
    {
        value = 1;
        itemnumber = newitemnumber;
        GetComponent<Renderer>().material = colors[1];
        if (newitemnumber == 7) { GetComponent<Renderer>().material = colors[0]; }
        if (newitemnumber == 8) { GetComponent<Renderer>().material = colors[2]; }
       // GetComponent<Renderer>().material = colors[Mathf.Abs(newitemnumber) % 3];

    }

}
