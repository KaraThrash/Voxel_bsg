using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectiveEvent : UnityEvent<InGameEvent>
{

}

public class Objective : Actor
{
    public Transform patrolPoint;

    public Color defaultColor, deadColor;
    public MeshRenderer meshRenderer;


    private string SHADER_Color = "Color_796B3B6"; 

    

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

    public override void TakeDamage(int _dmg)
    {
        Hitpoints(-_dmg);

        if (Hitpoints() > 0 && meshRenderer != null)
        {
            meshRenderer.material.SetColor(SHADER_Color, Color.Lerp(defaultColor, deadColor, (float)Hitpoints() / (float)MaxHP()));
        }
        else 
        {
            GameManager.instance.GetObjectiveEvent().Invoke(InGameEvent.objectiveLost);
            Die();
        }


    }


}
