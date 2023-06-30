using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMaster : MonoBehaviour
{
    public List<LivableObject> objects;
    public bool activateAll;
    public float groupColorVal;
    public float fadeInterval;
    // Start is called before the first frame update
    void Start()
    {
        foreach(LivableObject livableObject in GetComponentsInChildren<LivableObject>())
        {
            objects.Add(livableObject);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (activateAll)
        {
            ActivateAll();
        }
    }

    void ActivateAll()
    {
        foreach(Transform child in transform)
        {
            Material childMat = child.GetComponent<Renderer>().material;
            childMat.EnableKeyword("_WhiteDegree");
            TurnOnColor(childMat);
        }
    }

    void TurnOnColor(Material material)
    {
        if (groupColorVal > 0)
        {
            groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            material.SetFloat("_WhiteDegree", groupColorVal);
        }
        else
        {
            groupColorVal = 0;
        }

    }
}
