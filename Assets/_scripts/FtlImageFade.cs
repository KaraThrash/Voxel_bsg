using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FtlImageFade : MonoBehaviour {
    public Color colorStart ;
    public Color colorEnd ;
    public Renderer fadematerial;
    public bool fade;
    // Use this for initialization
    void Start()
    {
        colorStart = GetComponent<RawImage>().color;
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
            this.GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, colorEnd, 2.0f * Time.deltaTime);

           
            // fadematerial.material.color.a -= 0.1f;
        }

    }
    public void StartFade()
    {
        GetComponent<RawImage>().color = colorStart;
    }
}
