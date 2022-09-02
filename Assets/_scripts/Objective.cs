using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class ObjectiveEvent : UnityEvent<InGameEvent>
{

}

public class Objective : Map_POI
{
    public Transform patrolPoint;

    public Color defaultColor, deadColor;
    public MeshRenderer meshRenderer;


    private string SHADER_color = "Color_796B3B6";

    public GameObject explosion;
    public ParticleSystem visual_bulletImpact;


    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;



    // Start is called before the first frame update
    void Start()
    {
        if (Map() && patrolPoint)
        { patrolPoint.parent = Map().transform; }

        if (meshRenderer == null)
        { meshRenderer = GetComponent<MeshRenderer>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitpoints() != -1 && Hitpoints() <= 0)
        {}
    }

    public override void TakeDamage(int _dmg, Bullet _bullet)
    {
        Hitpoints(-_dmg);

        if (Hitpoints() > 0 )
        {
           

            //NOTE: just color is not enough of an indicator [i.e. shape/texture]
            UpdateColor(Color.Lerp(defaultColor, deadColor, PercentHealth()));
        }
        else 
        {
            
            Die();
        }


    }

    public void UpdateColor(Color _color)
    {

        if ( meshRenderer != null)
        {
            meshRenderer.material.SetColor(SHADER_color, Color.Lerp(defaultColor, deadColor, (float)Hitpoints() / (float)STAT_MaxHP()));
        }
    }


    public override void Die()
    {
        GameManager().GetObjectiveEvent().Invoke(InGameEvent.objectiveLost);

        if (explosion != null)
        {
            Instantiate(explosion,transform.position,transform.rotation);
        }

        if (GetCrossHair() != null)
        {
            GetCrossHair().StopTargeting();
        }

        gameObject.SetActive(false); ;
    }

}
