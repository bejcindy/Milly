using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Bottle : PickUpObject
{

    [Foldout("Bottle")]
    Transform body;

    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.TRASH;
        body = transform.GetChild(0);
        rend = body.GetComponent<Renderer>();
    }

    protected override void Update()
    {
        base.Update();


        if (selected && !thrown)
            rend.gameObject.layer = 9;
        else if (inHand)
            rend.gameObject.layer = 7;
        else if (activated)
            rend.gameObject.layer = 17;
        else 
            rend.gameObject.layer = 0;
    }

}
