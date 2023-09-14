using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RevertOriginalMaterials : MonoBehaviour
{
    Renderer[] All_Objects;
    void Awake()
    {
        All_Objects = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        
      }
}