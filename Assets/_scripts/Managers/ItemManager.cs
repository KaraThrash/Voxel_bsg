﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour {
    public int money, fuel;
    public Text fueltext, moneytext;
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
        if (gameManager.hangarManager.selectedFighterObj != null)
        {
            Fighter selectedFighter = gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>();
            switch (typedisplayed)
            {
                case 0:
                    if (selectedFighter.weaponequipped.Count < gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponslots)
                    {
                        // gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Add(whichitem);
                        selectedFighter.Equip(AllItems[(int)gunsInInventory[whichitem].x]);
                        gunbuttons[selectedFighter.weaponequipped.Count - 1].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)gunsInInventory[whichitem].x].name;
                        gunsInInventory[whichitem] = new Vector2(gunsInInventory[whichitem].x, gunsInInventory[whichitem].y - 1);
                        if (gunsInInventory[whichitem].y <= 0)
                        {
                            gunsInInventory.Remove(gunsInInventory[whichitem]);
                        }
                    }

                    break;
                case 1:
                    if (selectedFighter.enginequipped.Count < gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().engineslots)
                    {
                        // gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Add(whichitem);
                        selectedFighter.Equip(AllItems[(int)enginesInInventory[whichitem].x]);
                        enginebuttons[selectedFighter.enginequipped.Count - 1].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)enginesInInventory[whichitem].x].name;

                        enginesInInventory[whichitem] = new Vector2(enginesInInventory[whichitem].x, enginesInInventory[whichitem].y - 1);
                        if (enginesInInventory[whichitem].y <= 0)
                        {
                            enginesInInventory.Remove(enginesInInventory[whichitem]);
                        }


                    }
                    break;
                case 2:


                    break;
                case 3:
                    if (selectedFighter.hullequipped.Count < gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().hullslots)
                    {
                        // gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>().weaponequipped.Add(whichitem);
                        selectedFighter.Equip(AllItems[(int)hullsInInventory[whichitem].x]);
                        hullbuttons[selectedFighter.hullequipped.Count - 1].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)hullsInInventory[whichitem].x].name;

                        hullsInInventory[whichitem] = new Vector2(hullsInInventory[whichitem].x, hullsInInventory[whichitem].y - 1);
                        if (hullsInInventory[whichitem].y <= 0)
                        {
                            hullsInInventory.Remove(hullsInInventory[whichitem]);
                        }

                    }
                    break;

                default:

                    break;

            }
        }
        showtypeavailable(typedisplayed);
    }
    public void ToggleUnequip(int whichtype)
    {
        if (gameManager.hangarManager.selectedFighterObj != null)
        {
            Fighter selectedFighter = gameManager.hangarManager.selectedFighterObj.GetComponent<Fighter>();
            int whichitem = selectedFighter.UnEquip(whichtype);
            if (whichitem != -1)
            {


                int count = 0;
                bool founditem = false;
                switch (whichtype)
                {
                    case 0:
                        while (count < gunsInInventory.Count)
                        {
                            if (gunsInInventory[count].x == whichitem)
                            {
                                gunsInInventory[count] = new Vector2(gunsInInventory[count].x, gunsInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { gunsInInventory.Add(new Vector2(whichitem, 1)); }
                        gunbuttons[selectedFighter.weaponequipped.Count].transform.GetChild(0).GetComponent<Text>().text = "Gun Slot";
                        break;
                    case 1:
                        while (count < enginesInInventory.Count)
                        {
                            if (enginesInInventory[count].x == whichitem)
                            {
                                enginesInInventory[count] = new Vector2(enginesInInventory[count].x, enginesInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { enginesInInventory.Add(new Vector2(whichitem, 1)); }
                        enginebuttons[selectedFighter.enginequipped.Count].transform.GetChild(0).GetComponent<Text>().text = "Engine Slot";
                        break;
                    case 2:

                        break;
                    case 3:
                        while (count < hullsInInventory.Count)
                        {
                            if (hullsInInventory[count].x == whichitem)
                            {
                                hullsInInventory[count] = new Vector2(hullsInInventory[count].x, hullsInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { gunsInInventory.Add(new Vector2(whichitem, 1)); }
                        hullbuttons[selectedFighter.hullequipped.Count].transform.GetChild(0).GetComponent<Text>().text = "Hull Slot";
                        break;

                    default:

                        break;

                }
            }
        }
    }
    public void ItemPickUp(GameObject whichitem)
    {
        
           

            int whichtype = AllItems[whichitem.GetComponent<PickUp>().itemnumber].GetComponent<PickUp>().type;

                int count = 0;
                bool founditem = false;
                switch (whichtype)
                {
                    case 0:
                        while (count < gunsInInventory.Count)
                        {
                            if (gunsInInventory[count].x == whichitem.GetComponent<PickUp>().itemnumber)
                            {
                                gunsInInventory[count] = new Vector2(gunsInInventory[count].x, gunsInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { gunsInInventory.Add(new Vector2(whichitem.GetComponent<PickUp>().itemnumber, 1)); }
                        
                        break;
                    case 1:
                        while (count < enginesInInventory.Count)
                        {
                            if (enginesInInventory[count].x == whichitem.GetComponent<PickUp>().itemnumber)
                            {
                                enginesInInventory[count] = new Vector2(enginesInInventory[count].x, enginesInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { enginesInInventory.Add(new Vector2(whichitem.GetComponent<PickUp>().itemnumber, 1)); }
                       
                        break;
                    case 2:

                        break;
                    case 3:
                        while (count < hullsInInventory.Count)
                        {
                            if (hullsInInventory[count].x == whichitem.GetComponent<PickUp>().itemnumber)
                            {
                                hullsInInventory[count] = new Vector2(hullsInInventory[count].x, hullsInInventory[count].y + 1);
                                founditem = true;
                            }
                            count++;

                        }
                        if (founditem == false) { gunsInInventory.Add(new Vector2(whichitem.GetComponent<PickUp>().itemnumber, 1)); }
                    
                        break;
                case -2://money
                money += whichitem.GetComponent<PickUp>().value;

                break;
                case -3://fuel
                    fuel += whichitem.GetComponent<PickUp>().value;

                    break;
                default:

                        break;

                }
            
        
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
            while (count2 < selectedFighter.weaponslots)
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
        while (count2 < selectedFighter.engineslots)
        {
            enginebuttons[count2].gameObject.active = true;
            if (selectedFighter.enginequipped.Count > count2)
            {
                enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.enginequipped[count2]].name;
            }
            else { enginebuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "engine slot"; }
            count2++;
        }
        count2 = 0;
        while (count2 < selectedFighter.hullslots)
        {
            hullbuttons[count2].gameObject.active = true;
            if (selectedFighter.hullequipped.Count > count2)
            {
                hullbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = AllItems[selectedFighter.hullequipped[count2]].name;
            }
            else { hullbuttons[count2].transform.GetChild(0).GetComponent<Text>().text = "hull slot"; }
            count2++;
        }
        count2 = 0;
        while (count2 < selectedFighter.bulleteslots)
        {
            bulletbuttons[count2].gameObject.active = true;
            count2++;
        }
        count2 = 0;
        while (count2 < selectedFighter.consumableslots)
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
                if(gunsInInventory.Count != 0) { 
                foreach (Vector2 item in gunsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                }

                break;
            case 1:
                if (enginesInInventory.Count != 0)
                {
                    foreach (Vector2 item in enginesInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                }
                break;
            case 3:
                if (hullsInInventory.Count != 0)
                {
                    foreach (Vector2 item in hullsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                }
                break;
            case 2:
                if (bulletsInInventory.Count != 0)
                {
                    foreach (Vector2 item in bulletsInInventory)
                {
                    inventoryselectbuttons[count].gameObject.active = true;

                    inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                    count++;
                }
                }
                break;

            default:
                     if (consumablesInInventory.Count != 0)
                    {
                        foreach (Vector2 item in consumablesInInventory)
                        {
                            inventoryselectbuttons[count].gameObject.active = true;

                            inventoryselectbuttons[count].transform.GetChild(0).GetComponent<Text>().text = AllItems[(int)item.x].name + " : " + item.y;

                            count++;
                        } }
                break;

        }

    }

    public void showfuelandMoney()
    {
        string tempstring = "";
        while (tempstring.Length < fuel)
        { tempstring += "I"; }
        fueltext.text = tempstring;
        moneytext.text = money.ToString();
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

    public bool SpendMoney(int cost)
    {

        if (money >= cost)
        {
            money -= cost;
            moneytext.text = money.ToString();
            return true;
        }
        return false;
    }
}
