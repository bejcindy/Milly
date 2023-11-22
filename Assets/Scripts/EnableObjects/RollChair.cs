using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollChair : FixedCameraObject
{
    public Transform chairPos;

    protected override void Start()
    {
        base.Start();
        moveCam = true;
    }
    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            GetComponent<Collider>().enabled = false;
            transform.SetParent(chairPos);
            transform.localPosition = Vector3.zero;
        }
    }
}
