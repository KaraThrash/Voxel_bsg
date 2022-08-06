using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceManager : Manager {

    public int currency;

    public int pop;
    public int food;
    public int fuel;
    public int morale;
    public int vipers;
    public int raptors;
    public int ftlSpoolTime;//time before the fleet can jump again at no cost


    public Text currencyText;

    public Text vipertext;
    public Text poptext;
    public Text foodtext;
    public Text fueltext;
    public Text moraletext;
    // Use this for initialization
    void Start () {
        GameManager().GetEnemyDeathEvent().AddListener(EnemyDeathEvent);
        UpdateResourceText();
    }

	// Update is called once per frame
	void Update () {
        
	}


    public int GetPoints() { return currency; }
    public void SetCurrency(int _points)
    {
        currency = _points;
        UpdateCurrencyText(currency.ToString());
    }
    public void UpdateSCurrency(int _amount)
    {  
        currency += _amount;
        UpdateCurrencyText(currency.ToString());
    }

    public void UpdateCurrencyText(string _text)
    {
        if (currencyText != null)
        { currencyText.text = _text; }
    }

    public void ResourceChange(ResourceType _resource,int _amount)
    {
        if (_resource == ResourceType.pop)
        { 
            pop += _amount;

        }
        else if (_resource == ResourceType.currency)
        {
            currency += _amount;
        }
        else if (_resource == ResourceType.food)
        {
            food += _amount;
        }
        else if (_resource == ResourceType.fuel)
        {
            fuel += _amount;
        }
        else if (_resource == ResourceType.morale)
        {
            morale += _amount;
        }
        else if (_resource == ResourceType.vipers)
        {
            vipers += _amount;
        }

        UpdateResourceText();
    }

    public void ResourceChange(int popchange,int foodchange,int fuelchange,int moralechage)
    {
        pop += popchange;
        food += foodchange;
        fuel += fuelchange;
        morale += moralechage;
        UpdateResourceText();
   }


    public void UpdateResourceText()
    {
        if (currencyText != null)
        {
            UpdateCurrencyText( currency.ToString());
        }

        if (poptext != null)
        {
            SetTextAsBar(poptext,pop);
        }

        if (foodtext != null)
        {
            SetTextAsBar(foodtext, food);
        }

        if (fueltext != null)
        {
            SetTextAsBar(fueltext, fuel);
        }

        if (moraletext != null)
        {
            SetTextAsBar(moraletext, morale);
        }

        if (vipertext != null)
        {
            SetTextAsBar(vipertext, vipers);
        }

    }


    public void EnemyDeathEvent(Enemy _enemy)
    {
        if (_enemy.Stats() != null)
        { UpdateSCurrency(_enemy.Stats().pointValue); }
        else { UpdateSCurrency(1); }

    }

    public void SetTextAsBar(Text _text, int _amount)
    {
        string count = "";

        while (count.Length < _amount)
        { count += "i"; }

        _text.text = count;
        
    }




}
