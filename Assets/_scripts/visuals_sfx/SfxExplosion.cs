using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxExplosion : MonoBehaviour
{

    public SFX sfxType;

    public GameObject bulletImpact;
    
    
    public float lifetime;

    private float default_LifeTime = 5;
    
    public string parentName = "PARENT_Explosion";
    private Transform parentTransform;

    //safeguard from an explosion not activating if set to lifetime too short
    private bool canDie = true;

    void Start()
    {
       
    }

    public void Init(SFX _type)
    {
        sfxType = _type;

        lifetime = default_LifeTime;

        transform.parent = null;
        transform.parent = ExplosionParent();

        if (sfxType == SFX.bulletImpact)
        {
            lifetime = 1;
            if (bulletImpact)
            {
                //play on awake particleSystem
                bulletImpact.SetActive(true);
            }
        }

        canDie = true;
    }

    void FixedUpdate()
    {
        if (lifetime != -1 && canDie)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Die();

            }
        }
    }


    public void Die()
    {
        if (ExplosionParent())
        {
            canDie = false;
            transform.parent = ExplosionParent();
            gameObject.SetActive(false);
        }
        else { Destroy(this.gameObject); }
    }


    public Transform ExplosionParent()
    {
        if (parentTransform == null)
        {
            if (parentName.Length < 1)
            {
                parentName = "PARENT_Explosion";// + this.GetType().ToString();
            }

            GameObject findParent = GameObject.Find(parentName);


            if (findParent == null)
            {
                parentTransform = new GameObject(parentName).transform;
            }
            else { parentTransform = findParent.transform; }
        }

        return parentTransform;
    }

}
