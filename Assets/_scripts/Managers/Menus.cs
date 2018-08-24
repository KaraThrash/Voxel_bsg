using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour {
    public GameObject hubparentmenu;
    public List<GameObject> hubsubmenus;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void HubMenuChange(int whichmenu)
    {
        foreach (GameObject go in hubsubmenus)
        { go.active = false; }
        hubsubmenus[whichmenu].active = true;
        //switch (whichmenu)
        //{
        //    case 0:

        //        break;
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    default:
        //        break;
        //}
    }
}
