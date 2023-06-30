using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAlive : MonoBehaviour
{
    public Material mat;
    public bool activated;
    public float matColorVal;
    public float fadeInterval;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_WhiteDegree");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if(matColorVal > 0)
            {
                matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
                mat.SetFloat("_WhiteDegree", matColorVal);
            }
            else
            {
                matColorVal = 0;
            }
        }
    }


    public void Activate()
    {
        if(matColorVal > 0)
            activated = true;
    }
}
