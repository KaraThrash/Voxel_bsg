using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGlow : MonoBehaviour
{
    public List<Material> colors;
    public int currentColor;
    public float timeToChange,timer;
    public Renderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (colors.Count == 0 || myRenderer == null) { return; }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timeToChange;
            currentColor++;
            if (currentColor >= colors.Count)
            { currentColor = 0; }
            myRenderer.material = colors[currentColor];
        }
    }
}
