using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour {
    public List<GameObject> AllItems;
    //public List<GameObject> InInventory; // items available to the player, but not equiped or a recent pick up while on mission
    public List<Vector2> equipedItems; 
    public List<Vector2> pickedUpItems; //picked up during a mission
    public List<Vector2> InInventory; //Held items and their count // items available to the player, but not equiped or a recent pick up while on mission
    public List<Button> inventoryselectbuttons,pendingequippedbuttons;
    // public HashSet<GameObject> UniqueInventoryItems;
    // Use this for initialization
    void Awake () {
        DisplayInventory();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //todo: scrollable list
    public void ToggleEquip(int whichitem)
    {
        bool founditem = false;
        int count = 0;

        // buttons are laid out to click and add/remove items from being selected.
        // the button's space on the list goes to that count in the inventory list
        // where the inventory list is a vector2: x being the number from the item master list, and Y being the count
        while (count < equipedItems.Count && founditem == false)
        {
            if (equipedItems[count].x == InInventory[whichitem].x)
            {

                equipedItems[count] = new Vector2(equipedItems[count].x, equipedItems[count].y + 1) ;
                InInventory[whichitem] = new Vector2(InInventory[whichitem].x, InInventory[whichitem].y - 1) ;
                pendingequippedbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)equipedItems[count].x].name + " : " + equipedItems[count].y;
                if (InInventory[whichitem].y <= 0)
                {
                    InInventory.Remove(InInventory[whichitem]);
                    inventoryselectbuttons[whichitem].gameObject.active = false;
                }
                else
                {
                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)InInventory[count].x].name + " : " + InInventory[count].y;
                }
                founditem = true;
            }
            count++;
        }
        if (founditem == false)
        {
            Vector2 newitem = new Vector2(InInventory[whichitem].x, 1);
            equipedItems.Add(newitem);
            pendingequippedbuttons[equipedItems.Count - 1].gameObject.active = true;
            pendingequippedbuttons[equipedItems.Count - 1].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)newitem.x].name + " : " + newitem.y;
            InInventory[whichitem] = new Vector2(InInventory[whichitem].x, InInventory[whichitem].y - 1);
            inventoryselectbuttons[whichitem].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)newitem.x].name + " : " + InInventory[whichitem].y;

        }

    }
    public void DisplayInventory()
    {

        int count = 0;
        foreach (Button inventorybutton in inventoryselectbuttons)
        {
            inventorybutton.gameObject.active = false;
        }
        foreach (Button inventorybutton in pendingequippedbuttons)
        {
            inventorybutton.gameObject.active = false;
        }
        foreach (Vector2 item in InInventory)
        {
            inventoryselectbuttons[count].gameObject.active = true;
            
            inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;
           
            count++;
        }
      


    }
}
