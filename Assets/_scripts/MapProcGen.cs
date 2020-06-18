using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapProcGen : MonoBehaviour
{
  public bool randomizeColors,randomizeChildColors;
  public float scaleRange = 1.1f,spawnRate,rotRange,moverange,col0,col1,col2,col3,col4,col5;
  public float rotx,roty,rotz;
  public Renderer myRend;

  public Transform subParts;
  public List<GameObject> spawnableItems;
  public List<Material> colors,colors0,colors1,colors2;
    // Start is called before the first frame update
    void Start()
    {
      // RandomizeParts();
      if(randomizeColors == true)
      {
        if(myRend == null)
        {  myRend = GetComponent<Renderer>();}

        RandomizeColors();
      }

      if(randomizeChildColors == true)
      {


        RandomizeChildColors();
      }

      // if(rotx + roty +rotz != 0)
      // {RandomizeRotation();}

      if(moverange != 0)
      {
        transform.position = new Vector3(transform.position.x + Random.Range(-moverange,moverange),transform.position.y,transform.position.z + Random.Range(-moverange,moverange));
      }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomizeRotation()
    {
        transform.Rotate(Random.Range(0,rotx),Random.Range(0,roty),Random.Range(0,rotz));
    }

    public void RandomizeChildColors()
    {
      Material mat = new Material(Shader.Find("Standard"));
      if(colors.Count > 0)
      {
        mat = colors[Random.Range(0,colors.Count)];

      }else{mat.color = new Color(Random.Range(col0,col1),Random.Range(col2,col3),Random.Range(col4,col5));}

      foreach(Transform go in this.transform)
      {
        if(go.GetComponent<Renderer>() != null)
        {

          go.GetComponent<Renderer>().material = mat;
        }

      }


    }

    public void RandomizeColors()
    {

      Material[] intMaterials = new Material[myRend.materials.Length];
     for(int i=0; i < myRend.materials.Length;i++){
       if(colors.Count > 0)
       {
         intMaterials[i] = colors[Random.Range(0,colors.Count)];

       }
         else
         {
           Material mat = new Material(Shader.Find("Standard"));
           mat.color = new Color(Random.Range(col0,col1),Random.Range(col2,col3),Random.Range(col4,col5));
             intMaterials[i] = mat;
         }

     }
     myRend.materials = intMaterials;

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
