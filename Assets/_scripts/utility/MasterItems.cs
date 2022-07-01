using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterItems 
{

    public static Dictionary<string, Item> masterItemList;


    public static Dictionary<string, Item> MasterItemList()
    {
        if (masterItemList == null)
        {
            masterItemList = new Dictionary<string, Item>();
        }
        return masterItemList;
    }

    public static Item GetItem(string _itemID)
    {
        if (masterItemList.ContainsKey(_itemID))
        {

        }
        return null;
    }


    public static void AddNewItem(Item _item)
    {
        if (MasterItemList().ContainsKey(_item.referenceID) == false)
        {
            MasterItemList().Add(_item.referenceID,_item);
        }
  
    }

}
