using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFleetshipButton :MonoBehaviour
{
    public Text text_shipname;
    public List<Image> img_resource;
    public Text text_resourceName; 
    public Text text_total; 
    public Text text_modify; 
    public Text text_production;
    public Text text_storage;


    public void SetShipNameText(string _name)
    {
        if (text_shipname) { text_shipname.text = _name; }
    }

    public void SetResourceImage(Sprite _image,string _name)
    {
        if (text_resourceName)
        { text_resourceName.text = _name; }

        foreach (Image el in img_resource)
        {
            el.sprite = _image;
        }
    }

    public void SetResourceText(float _total,float _modify,float _production)
    {
        if (text_total) { text_total.text = _total.ToString(); }
        if (text_modify) { text_modify.text = _modify.ToString(); }
        if (text_production) { text_production.text = _production.ToString(); }
    }

    public void SetStorageText( float _storage)
    {
        if (text_storage) { text_storage.text = _storage.ToString(); }
    }

}
