using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TurnOnColor : MonoBehaviour
{
    Renderer[] childrenRends;


    private void OnEnable()
    {
        if (childrenRends == null)
            childrenRends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in childrenRends)
        {
            Material mat = r.sharedMaterial;
            mat.SetFloat("_WhiteDegree", 0);
        }

            
            //Debug.Log(mat.name);

    }
    private void OnDisable()
    {
        foreach (Renderer r in childrenRends)
        {
            Material mat = r.sharedMaterial;
            mat.SetFloat("_WhiteDegree", 1);
            //Debug.Log(mat.name);
        }
    }
}
