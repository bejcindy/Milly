using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAroundDetecter : MonoBehaviour
{
    public Collider[] triggerAreas;
    public bool[] triggers;
    // Start is called before the first frame update
    void Start()
    {
        triggers = new bool[4];
        for(int i = 0; i < triggerAreas.Length; i++)
        {
            WalkAroundChildTrigger wact = triggerAreas[i].gameObject.AddComponent<WalkAroundChildTrigger>();
            wact.childIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allTrue())
        {
            //Do Things Here and destroy gameobject when finish
            //Debug.Log("allTrue");

        }
    }

    bool allTrue()
    {
        for(int i = 0; i < triggers.Length; i++)
        {
            if (!triggers[i])
                return false;
        }
        return true;
    }
    
}
