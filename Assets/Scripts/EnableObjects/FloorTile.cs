using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    Material mat;
    Renderer rend;

    public bool activated;
    public bool firstActivated;

    float matColorVal;
    float fadeInterval;

    public List<GroundDirt> myDirts;
    public List<Transform> myTiles;
    public bool overrideStartSequence;
    bool layerChanged;

    void Start()
    {
        Hugo.totalFloorCount++;
        activated = false;
        rend = GetComponent<Renderer>();
        mat = rend.material;
        matColorVal = 1;
        fadeInterval = 10;
        GetComponentsInChildren<GroundDirt>(myDirts);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !firstActivated)
            TurnOnColor(mat);

        if (AllDirtCleaned())
        {
            activated = true;
        }
        
    }

    bool AllDirtCleaned()
    {
        foreach(GroundDirt dirt in myDirts)
        {
            if (!dirt.cleaned)
            {
                return false;
            }
        }
        return true;
    }




    private void TurnOnColor(Material material)
    {
        if (!layerChanged)
        {
            gameObject.layer = 18;
            matColorVal = 1;
            material.SetFloat("_WhiteDegree", matColorVal);
            foreach (Transform tile in myTiles)
            {
                tile.gameObject.layer = 18;
                Renderer tileRend = tile.GetComponent<Renderer>();
                if (tileRend)
                {
                    tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                }
            }
            layerChanged = true;
        }

        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);

            foreach(Transform tile in myTiles)
            {
                Renderer tileRend = tile.GetComponent<Renderer>();
                if (tileRend)
                {
                    tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                }
            }

        }
        else
        {
            matColorVal = 0;
            firstActivated = true;
            Hugo.totalFloorCleaned++;
            ReferenceTool.broom.TriggerTat();

        }
       
    }
}
