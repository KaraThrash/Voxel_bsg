using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpot : MonoBehaviour
{
  public GameObject prefab_Enemy;
    private MeshRenderer meshRenderer;
  

    // Start is called before the first frame update
    void Start()
    {
        DisableMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableMesh()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; ;
        }
    }
}
