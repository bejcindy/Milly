using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAroundDetecter : LivableObject
{
    public Collider[] triggerAreas;
    public bool[] triggers;
    public bool poleLight;
    public Light streetLight;
    public GameObject lightBulb;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        triggers = new bool[4];
        for(int i = 0; i < triggerAreas.Length; i++)
        {
            WalkAroundChildTrigger wact = triggerAreas[i].gameObject.AddComponent<WalkAroundChildTrigger>();
            wact.childIndex = i;
        }
        if(poleLight)
            lightBulb = transform.GetChild(5).gameObject;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (allTrue())
        {
            activated = true;

        }
        if (matColorVal <= 0)
        {
            if(streetLight!=null)
                streetLight.enabled = true;
            if (lightBulb != null)
                lightBulb.SetActive(true);
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
