using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : ContainerObject
{
    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) < minDist)
            interactable = true;
    }
}
 