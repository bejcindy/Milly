using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charles : NPCControl
{
    public bool charlesSmoking;

    protected override void Start()
    {
        base.Start();
    }

    public void CharlesAction1()
    {
        if (!charlesSmoking)
        {
            machine.SetAnimatorTrigger("Smoke");
            charlesSmoking = true;
        }

    }

    public void CharlesAction2()
    {

    }

    public void CharlesAction3()
    {

    }
}
