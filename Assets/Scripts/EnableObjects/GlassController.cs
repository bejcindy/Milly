using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    public List<GameObject> glasses;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAllGlassLayers()
    {
        foreach(GameObject glass in glasses)
        {
            glass.layer = 17;
        }
    }
}
