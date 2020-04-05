using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProcGen : MonoBehaviour
{
  public float scaleRange = 1.1f,spawnRate,rotRange;
  public Renderer myRend;

  public Transform subParts;
  public List<GameObject> spawnableItems;
  public List<Material> colors;
    // Start is called before the first frame update
    void Start()
    {
      RandomizeParts();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RandomizeParts()
    {
      float rnd = Random.Range(0,1);
      if(myRend != null && colors.Count > 0 )
      {
        myRend.material = colors[(int)Random.Range(0,colors.Count)];


      }
      if(subParts != null && subParts.childCount > 0)
      {
            foreach(Transform el in subParts)
            {
              rnd = Random.Range(0.0f,1.0f);
              if(rnd < spawnRate)
              {
                  GameObject clone = Instantiate(spawnableItems[(int)Random.Range(0,spawnableItems.Count)],el.transform.position,el.transform.rotation );

                  clone.transform.Rotate(transform.right * Random.Range(-rotRange,rotRange));

              }

              Destroy(el.gameObject);
            }
      }

    }
}
