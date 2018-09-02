using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {
    public int totalhp, currenthp;
    public int weaponslots,engineslots,bulleteslots,hullslots,consumableslots;
    public List<int> weaponequipped, enginequipped, bulletequipped, hullequipped, consumablequipped;
    // public int weaponmounts, enginemounts, cargospace;
    //public List<GameObject> mountedweapons,mountedengines,cargo;
    public int type;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Equip(GameObject newitem)
    {
        Debug.Log("equipt" + newitem.ToString());
        
           switch(newitem.GetComponent<PickUp>().type)
        {
            case 0:
                weaponequipped.Add(newitem.GetComponent<PickUp>().itemnumber);
                GetComponent<ViperControls>().guncooldown -= 0.2f;
                break;
            case 1:
                enginequipped.Add(newitem.GetComponent<PickUp>().itemnumber);
                GetComponent<ViperControls>().flySpeed += 20;
                break;
            case 2:
                hullequipped.Add(newitem.GetComponent<PickUp>().itemnumber);
                totalhp += 2; currenthp += 2;
                break;
            default:
                consumablequipped.Add(newitem.GetComponent<PickUp>().itemnumber);
                break;
        }
    }
    public int UnEquip(int whichtype)
    {
       
        int itemremoved = -1;
        switch (whichtype)
        {
            case 0:
                if (weaponequipped.Count > 0)
                {
                    itemremoved = weaponequipped[weaponequipped.Count - 1];
                    weaponequipped.RemoveAt(weaponequipped.Count - 1);
                    GetComponent<ViperControls>().guncooldown += 0.2f;
                }
                break;
            case 1:
                if (enginequipped.Count > 0)
                {
                    itemremoved = enginequipped[enginequipped.Count - 1];
                    enginequipped.RemoveAt(enginequipped.Count - 1);
                    GetComponent<ViperControls>().flySpeed -= 20;
                }
                break;
            case 3:
                if (hullequipped.Count > 0)
                {
                    itemremoved = hullequipped[hullequipped.Count - 1];
                    hullequipped.RemoveAt(hullequipped.Count - 1);
                    totalhp -= 2; currenthp -= 2;
                }
                break;
            default:
                if (consumablequipped.Count > 0)
                {
                    itemremoved = consumablequipped[consumablequipped.Count - 1];
                    consumablequipped.RemoveAt(weaponequipped.Count - 1);
                   }
                break;
        }
        return itemremoved;
    }
   
}
