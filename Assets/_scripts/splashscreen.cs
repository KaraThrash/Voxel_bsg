using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashscreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Application.LoadLevel("ForMobile");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartGame()
    {
        Application.LoadLevel("game");
    }
}
