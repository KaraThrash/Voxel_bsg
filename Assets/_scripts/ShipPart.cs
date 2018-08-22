using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour {
    public GameObject myship;
    public int hp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        { TakeDamage(11); }
	}
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        { myship.SendMessage("SystemDestroyed"); Destroy(this.gameObject); }
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bullet")
        { TakeDamage(1); }
     





    }

}
