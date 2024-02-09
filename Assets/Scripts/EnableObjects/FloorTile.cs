using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    Material mat;
    Renderer rend;

    bool activated;
    bool firstActivated;

    float matColorVal;
    float fadeInterval;

    public GroundDirt[] myDirts;
    public bool overrideStartSequence;
    bool layerChanged;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        matColorVal = 1;
        fadeInterval = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !firstActivated)
            TurnOnColor(mat);
        
    }




    private void TurnOnColor(Material material)
    {
        if (!layerChanged)
        {
            //gameObject.layer = 18;
            layerChanged = true;
        }

        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);

        }
        else
        {
            matColorVal = 0;
            firstActivated = true;

        }
       
    }
}
