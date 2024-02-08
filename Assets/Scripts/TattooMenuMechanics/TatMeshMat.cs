using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatMeshMat : MonoBehaviour
{

    public Material burnMat;
    public Material ditherMat;
    
    
    Material mat;

    public void SwitchDitherMat()
    {
        GetComponent<Renderer>().material = ditherMat;
        GetComponent<Renderer>().material.SetFloat("_DitherThreshold", 1);
        GetComponent<Renderer>().material.SetFloat("_WhiteDegree", 0);
    }

    public void SwitchBurnMat()
    {
        GetComponent<Renderer>().material = burnMat;
        GetComponent<Renderer>().material.SetFloat("_WhiteDegree", 0);
    }
}
