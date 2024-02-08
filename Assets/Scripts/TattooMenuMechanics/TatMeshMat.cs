using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatMeshMat : MonoBehaviour
{

    public Material burnMat;
    public Material ditherMat;


    public void SwitchDitherMat()
    {
        GetComponent<Renderer>().material = ditherMat;
    }

    public void SwitchBurnMat()
    {
        GetComponent<Renderer>().material = burnMat;
    }
}
