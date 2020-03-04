using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour {
    public GameObject myship,destroyedIndicator,damagedIndicator,healthyIndicator;
    //TODO: assign subsystems to be disabled when this is damaged/destroyed
    public int hp;
    public string partType;//turret,engine,hangar,etc
    public bool destroyed;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

        // if (Input.GetKeyDown(KeyCode.P))
        // { TakeDamage(11); }


	}
    public void TakeDamage(int dmg)
    {
      if(destroyed == false){
        hp -= dmg;
          if (hp <= 0)
          {
            Die();
          }
      }
    }
    public void Die()
    {
      destroyed = true;
      if(myship.GetComponent<BaseStar>() != null)
      {
        //set the ship part to indicate that it is destroyed
          if(healthyIndicator != null){   healthyIndicator.active = false;}
        if(damagedIndicator != null){  destroyedIndicator.active = true;}

        //if this is on the basestar update the enemy fleet stats
        myship.GetComponent<BaseStar>().SystemDestroyed(partType);
      }
      else
      {
        myship.SendMessage("SystemDestroyed");
        Destroy(this.gameObject);
      }

    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Bullet")
        { TakeDamage(1); }






    }

}
