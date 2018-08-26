using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour {
    public GameManager gameManager;
    public List<GameObject> AllItems;
    //public List<GameObject> InInventory; // items available to the player, but not equiped or a recent pick up while on mission
    public List<Vector2> equipedItems; 
    public List<Vector2> pickedUpItems; //picked up during a mission
    //public List<Vector2> InInventory; //Held items and their count // items available to the player, but not equiped or a recent pick up while on mission
    public List<Vector2> gunsInInventory, enginesInInventory, hullsInInventory, bulletsInInventory, consumablesInInventory;

    public List<Button> inventoryselectbuttons,pendingequippedbuttons;
    public List<Button> gunbuttons,enginebuttons,hullbuttons,bulletbuttons,consumablebuttons;
    public int typedisplayed;
    // public HashSet<GameObject> UniqueInventoryItems;
    // Use this for initialization
    void Awake () {
        //DisplayInventory();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //depreceated: toggle stacks
    //public void ToggleEquip(int whichitem)
    //{
    //    bool founditem = false;
    //    int count = 0;

    //    // buttons are laid out to click and add/remove items from being selected.
    //    // the button's space on the list goes to that count in the inventory list
    //    // where the inventory list is a vector2: x being the number from the item master list, and Y being the count
    //    List<Vector2> InInventory = gunsInInventory;
    //    InInventory.AddRange(enginesInInventory);
    //    InInventory.AddRange(hullsInInventory);
    //    InInventory.AddRange(bulletsInInventory);
    //    InInventory.AddRange(consumablesInInventory);



    //    while (count < equipedItems.Count && founditem == false)
    //    {
    //        if (equipedItems[count].x == InInventory[whichitem].x)
    //        {

    //            equipedItems[count] = new Vector2(equipedItems[count].x, equipedItems[count].y + 1) ;
    //            InInventory[whichitem] = new Vector2(InInventory[whichitem].x, InInventory[whichitem].y - 1) ;
    //            pendingequippedbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)equipedItems[count].x].name + " : " + equipedItems[count].y;
    //            if (InInventory[whichitem].y <= 0)
    //            {
    //                InInventory.Remove(InInventory[whichitem]);
    //                inventoryselectbuttons[whichitem].gameObject.active = false;
    //            }
    //            else
    //            {
    //                inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)InInventory[count].x].name + " : " + InInventory[count].y;
    //            }
    //            founditem = true;
    //        }
    //        count++;
    //    }
    //    if (founditem == false)
    //    {
    //        Vector2 newitem = new Vector2(InInventory[whichitem].x, 1);
    //        equipedItems.Add(newitem);
    //        pendingequippedbuttons[equipedItems.Count - 1].gameObject.active = true;
    //        pendingequippedbuttons[equipedItems.Count - 1].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)newitem.x].name + " : " + newitem.y;
    //        InInventory[whichitem] = new Vector2(InInventory[whichitem].x, InInventory[whichitem].y - 1);
    //        inventoryselectbuttons[whichitem].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)newitem.x].name + " : " + InInventory[whichitem].y;

    //    }

    //}

    public void ToggleEquip(int whichitem)
    {
        Debug.Log("toggle equipt" + typedisplayed.ToString());
        switch (typedisplayed)
        {
            case 0:
                if (gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Count < gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponslots)
                {
                    // gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Add(whichitem);
                    gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().Equip(AllItems[(int)gunsInInventory[whichitem].x]);
                    gunbuttons[whichitem].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)gunsInInventory[whichitem].x].name;
                    gunsInInventory.Remove(gunsInInventory[whichitem]);
                }

                break;
            case 1:
                if (gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Count < gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponslots)
                {
                    // gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Add(whichitem);
                    gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().Equip(AllItems[(int)enginesInInventory[whichitem].x]);
                    enginebuttons[whichitem].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)enginesInInventory[whichitem].x].name;
                    enginesInInventory.Remove(enginesInInventory[whichitem]);
                }
                break;
            case 2:
              
                break;
            case 3:
             
                break;

            default:
               
                break;

        }

        showtypeavailable(typedisplayed);
    }

    public void ToggleHangarDisplay(int weaponslots,int engineslots,int hullslots,int bulletslots,int consumableslots)
    {

    
        List<Button> shipslots = new List<Button>();
        shipslots.AddRange(gunbuttons);
        shipslots.AddRange(enginebuttons);
        shipslots.AddRange(hullbuttons);
        shipslots.AddRange(bulletbuttons);
        shipslots.AddRange(consumablebuttons);
        foreach (Button inventorybutton in shipslots)
        {
            inventorybutton.gameObject.active = false;
        }
        Fighter selectedFighter = gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>();

        int count2 = 0;
            while (count2 < weaponslots)
            {
                gunbuttons[count2].gameObject.active = true;
            if (selectedFighter.weaponequipped.Count > count2)
            {
                gunbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.weaponequipped[count2]].name;
            }
            else { gunbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "weapon slot"; }
                count2++;
            }
            count2 = 0;
        while (count2 < engineslots)
        {
            enginebuttons[count2].gameObject.active = true;
            if (selectedFighter.weaponequipped.Count > count2)
            {
                enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.enginequipped[count2]].name;
            }
            else { enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "engine slot"; }
            count2++;
        }
        count2 = 0;
        while (count2 < hullslots)
        {
            hullbuttons[count2].gameObject.active = true;
            count2++;
        }
        count2 = 0;
        while (count2 < bulletslots)
        {
            bulletbuttons[count2].gameObject.active = true;
            count2++;
        }
        count2 = 0;
        while (count2 < consumableslots)
        {
            consumablebuttons[count2].gameObject.active = true;
            count2++;
        }
        count2 = 0;
      
       

    }
    public void showtypeavailable(int whattype)
    {
        //item type of ship slot to equip
        foreach (Button inventorybutton in inventoryselectbuttons)
        {
            inventorybutton.gameObject.active = false;
        }
        int count = 0;
        typedisplayed = whattype;
        switch (whattype)
        {
            case 0:
                foreach (Vector2 item in gunsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }

                break;
            case 1:
                foreach (Vector2 item in enginesInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                break;
            case 2:
                foreach (Vector2 item in hullsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                break;
            case 3:
                foreach (Vector2 item in bulletsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                break;

            default:
                foreach (Vector2 item in consumablesInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                break;

        }

    }


    public void DisplayInventory()
    {
        List<Vector2> InInventory = new List<Vector2>();
        InInventory.AddRange(gunsInInventory);
        InInventory.AddRange(enginesInInventory);
        InInventory.AddRange(hullsInInventory);
        InInventory.AddRange(bulletsInInventory);
        InInventory.AddRange(consumablesInInventory);
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
