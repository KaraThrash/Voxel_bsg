using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FtlImageFade : MonoBehaviour {
    public Color colorStart ;
    public Color colorEnd ;
    public Color ftl,takedmg ;
    public Renderer fadematerial;
    public bool fade;
    public float fadeSpeed;
    // Use this for initialization
    void Start()
    {
        // colorStart = GetComponent<RawImage>().color;

        colorEnd = colorStart;

        colorEnd.a = 0;
        //fadematerial = GetComponent<Renderer>();
        // colorEnd.a = 0;
      //  StartFade();
        this.GetComponent<RawImage>().color = colorEnd;
    }

    // Update is called once per frame
    void Update()
    {
        //fadematerial.material.color != colorEnd ||
        if (this.GetComponent<RawImage>().color.a != 0)
        {
          if(fadeSpeed == 0){fadeSpeed = 1;}
            this.GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, colorEnd, fadeSpeed * Time.deltaTime);


            // fadematerial.material.color.a -= 0.1f;
        }

    }
    public void StartFade()
    {
        GetComponent<RawImage>().color = colorStart;
    }
}
