using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WalkAroundDetecter : LivableObject
{
    public Collider[] triggerAreas;
    public bool[] triggers;
    public bool poleLight;
    public Light streetLight;
    public GameObject lightBulb;
    public EventReference lightSound;
    bool soundPlayed;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        triggers = new bool[4];
        for (int i = 0; i < triggerAreas.Length; i++)
        {
            WalkAroundChildTrigger wact = triggerAreas[i].gameObject.AddComponent<WalkAroundChildTrigger>();
            wact.childIndex = i;
        }
        if (poleLight)
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
            if (streetLight && !soundPlayed)
            {
                streetLight.enabled = true;
                if (!lightSound.IsNull)
                    RuntimeManager.PlayOneShot(lightSound, lightBulb.transform.position);
                soundPlayed = true;
            }
            if (lightBulb && !soundPlayed)
            {
                lightBulb.SetActive(true);
                //if (!lightSound.IsNull)
                //    RuntimeManager.PlayOneShot(lightSound, lightBulb.transform.position);
                soundPlayed = true;
            }
        }
    }

    bool allTrue()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            if (!triggers[i])
                return false;
        }
        return true;
    }

}
