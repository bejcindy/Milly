using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatObject : PickUpObject
{
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.FOOD;
    }
}
