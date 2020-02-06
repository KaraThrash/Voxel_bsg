using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalTarget : MonoBehaviour {
    public int hp;
    public GameObject myparent;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        { hp--; }
        if (hp <= 0)
        { myparent.SendMessage("SystemDestroyed",this.gameObject); }


    }

}
