using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour {
    public GameObject myship,destroyedIndicator,damagedIndicator,healthyIndicator,itemDrop;
    //TODO: assign subsystems to be disabled when this is damaged/destroyed
    public int hp,maxhp,itemvalue,itemnumber;
    public string partType;//turret,engine,hangar,etc
    public bool destroyed,cannotBeHit,holdsPrimaryResource;
    public float dieTime = -1, dieTimer;
	// Use this for initialization
	void Start () {
        dieTimer = -1;

    }

	// Update is called once per frame
	void Update () {

        // if (Input.GetKeyDown(KeyCode.P))
        // { TakeDamage(11); }
        if (dieTimer != -1)
        {
            dieTimer -= Time.deltaTime;
            if (dieTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }


	}
    public void TakeDamage(int dmg)
    {
      if(destroyed == false){
        hp -= dmg;
          if (hp <= 0)
          {
            Die();
          }
            else
            {
                if (hp <= maxhp * 0.5f)
                {
                    if (damagedIndicator != null && healthyIndicator.active == true) { healthyIndicator.active = false; damagedIndicator.active = true; }
                }
            }
      }
    }
    public void Die()
    {
      destroyed = true;
        //if the part should be removed from play have it die after a time, if it should remain jave the die time be -1 so it doesnt track
        dieTimer = dieTime;

        //set the ship part to indicate that it is destroyed
        if (healthyIndicator != null) { healthyIndicator.active = false; }
        if (damagedIndicator != null ) { damagedIndicator.active = false; }
        if (destroyedIndicator != null) { destroyedIndicator.active = true; }

        if (itemDrop != null)
        {
            GameObject clone = Instantiate(itemDrop, transform.position, transform.rotation);
            if (clone.GetComponent<PickUp>() != null)
            {
               // clone.GetComponent<PickUp>().SetWhichItem(itemnumber , itemvalue, holdsPrimaryResource);
            }
        }

        if (myship == null) {
            //if this has an item drop leave it when it dies, and set it to the correct values
            
            return;
        }

      if(myship.GetComponent<ShipPart>() != null)
      {
        //set the ship part to indicate that it is destroyed
         
        myship.GetComponent<ShipPart>().SubSystemDestroyed();

      }
      else
      {
        myship.SendMessage("SystemDestroyed");
      if(destroyedIndicator != null){  destroyedIndicator.active = true;}else{this.gameObject.active = false;}
        // Destroy(this.gameObject);
      }

    }
    public void SubSystemDestroyed()
    {
      TakeDamage(1);
    }

    public void OnTriggerEnter(Collider col)
    {

        if (cannotBeHit == false && (col.gameObject.tag == "Bullet" || col.gameObject.tag == "BulletEnemy"))
        { TakeDamage(1); }






    }

}
