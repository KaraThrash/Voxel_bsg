using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Actor {


    public ItemTypes itemType;

    public int type;
    public int quantity; //if engine speed, if gun attack cooldown
    public bool playerCache,primaryResource;
    public bool onMap;

    public MeshRenderer render;
    public GameObject mainWorldObject;

    public Material[] colors; //green,red,blue,yellow


    public void Init(ItemTypes _itemType, int _quantity)
    {
        itemType = _itemType;
        quantity = _quantity;

        onMap = true;

        if (render)
        {
            render.material = colors[(int)Random.Range(0, colors.Length)];
        }
        else
        {
            if (transform.GetChild(0).GetComponent<Renderer>() != null)
            {
                transform.GetChild(0).GetComponent<Renderer>().material = colors[(int)Random.Range(0,colors.Length)];
            }
        }
        if (mainWorldObject)
        {
            mainWorldObject.SetActive(true);
        }



    }


    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void Update () {

	}



    public override void ProcessCollisionEnter(Collision collision)
    {
        GetPickedUp();
    }

    public void GetPickedUp()
    {
        onMap = false;
       transform.parent =  GameManager().ItemManager().Parent_Pickup();

        if (mainWorldObject)
        {
            mainWorldObject.SetActive(false);
        }
        else { gameObject.SetActive(false); }

    }


    public void SetWhichItem(ItemTypes _itemType,int _quantity)
    {
        itemType = _itemType;
        quantity = _quantity;

        if (render)
        {
            render.material = colors[0];
        }
        else 
        {
            if (transform.GetChild(0).GetComponent<Renderer>() != null)
            {
                transform.GetChild(0).GetComponent<Renderer>().material = colors[1];
            }
        }

        


    }



    public void SetAsPlayerCache()
    {
      playerCache = true;
    }
}
