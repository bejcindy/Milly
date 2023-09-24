using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TurnOnColor : MonoBehaviour
{
    Renderer[] childrenRends;
    Light[] childrenLights;
    List<Light> originallyOffLights;


    private void OnEnable()
    {
        //if (childrenRends == null)
            childrenRends = GetComponentsInChildren<Renderer>();
        //if (childrenLights == null)
            childrenLights = GetComponentsInChildren<Light>();
        //if (originallyOffLights == null)
            originallyOffLights = new List<Light>();
        foreach (Renderer r in childrenRends)
        {
            Material mat = r.sharedMaterial;
            if(mat.HasProperty("_WhiteDegree"))
                mat.SetFloat("_WhiteDegree", 0);
        }
        foreach(Light l in childrenLights)
        {
            if (!l.gameObject.activeSelf)
            {
                if (!originallyOffLights.Contains(l))
                    originallyOffLights.Add(l);
                l.gameObject.SetActive(true);
            }
        }
            
            //Debug.Log(mat.name);

    }
    private void OnDisable()
    {
        foreach (Renderer r in childrenRends)
        {
            Material mat = r.sharedMaterial;
            if (mat.HasProperty("_WhiteDegree"))
                mat.SetFloat("_WhiteDegree", 1);
            //Debug.Log(mat.name);
        }
        foreach(Light l in originallyOffLights)
        {
            l.gameObject.SetActive(false);
        }
    }
}
