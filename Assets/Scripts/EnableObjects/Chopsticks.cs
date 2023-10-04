using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopsticks : PickUpObject
{
    Transform otherChopstick;
    public Vector3 chopPickedPos;
    public bool hasFood;

    protected override void Start() 
    {
        base.Start();
        otherChopstick = transform.parent.GetChild(1).transform;
    }

    protected override void Update()
    {
        base.Update();

        if(selected && !thrown)
        {
            otherChopstick.gameObject.layer = 9;
        }
        else
        {
            otherChopstick.gameObject.layer = 0;
        }
    }
}
