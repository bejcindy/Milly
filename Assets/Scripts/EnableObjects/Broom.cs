using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : PickUpObject
{
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.TRASH;
    }

    protected override void Update()
    {
        base.Update();
        LayerDetection();
    }

    void LayerDetection()
    {
        if (selected && !thrown)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 9;
            }
        }
        else if (inHand)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 7;
            }
        }
        else if (activated)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 0;
            }
        }
    }
}
