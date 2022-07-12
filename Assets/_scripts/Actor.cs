using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private GameManager gameManager;
    public bool isPlayer = false;
    [SerializeField]

    private int maxHP = 10;
    public bool canAct;
    public int currentHealth;


    public Transform mainTransform;
    public Rigidbody rb;
    public Crosshair crosshair;

    private Map currentMap;

    


    public GameManager GameManager()
    {
        if (gameManager == null)
        { gameManager = FindObjectOfType<GameManager>(); }

        return gameManager;
    }





    public void OnCollisionEnter(Collision collision)
    {
        ProcessCollisionEnter(collision);
    }

    public void OnTriggerEnter(Collider collision)
    {
        ProcessTriggerEnter(collision);
    }

    public virtual void ProcessCollisionEnter(Collision collision)
    {

    }

    public virtual void ProcessTriggerEnter(Collider collision)
    {

    }







    public Crosshair GetCrossHair()
    {
        //TODO: Centralize prefabs that would need to be spawned if they dont already exist
        return crosshair;
    }

    public Map Map()
    {
        if (currentMap == null)
        {
            currentMap = FindObjectOfType<Map>();
        }

        return currentMap;
    }


    public virtual void TakeDamage(int _dmg)
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
            if (isPlayer)
            {
                GameManager().MenuManager().Set_PlayerHitPointsText(currentHealth);
            }
        }

    }

    public int Hitpoints()
    { return currentHealth; }

    public int STAT_MaxHP()
    { return maxHP; }

    public float PercentHealth()
    {
        float current = Hitpoints();
        float max = Mathf.Clamp(STAT_MaxHP(), 1, 100);

        return current / max;
    }


    public virtual void Die()
    {

        Destroy(gameObject);
    }


    public Transform MainTransform()
    {
        if (mainTransform == null)
        { return transform; }

        return mainTransform;
    }

    public Rigidbody RB()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        return rb;
    }


}
