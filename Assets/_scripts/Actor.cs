using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int currentHealth;
    private Map currentMap;


    public Map Map()
    {
        if (currentMap == null)
        {
            currentMap = FindObjectOfType<Map>();
        }

        return currentMap;
    }


    public void TakeDamage(int _dmg)
    {
        //apply dmg reduc, external forces, other talents etc

        Hitpoints(-_dmg);



    }




    public void Hitpoints(int _change)
    {
        if (currentHealth != -1)
        {
            currentHealth += _change;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //NOTE: for enemies die when it needs to 'make decision' to create the dying animation

            }
        }

    }

    public int Hitpoints()
    { return currentHealth; }


    public void Die()
    {

        Destroy(gameObject);
    }

}
