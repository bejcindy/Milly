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

    }

    public void CharlesAction2()
    {

    }

    public void CharlesAction3()
    {

    }

    public void FinishSmoking()
    {
        Debug.Log("finishing smoking");
        machine.ResetAnimTrigger("Special1");
        machine.SetAnimatorTrigger("Finish");
    }
}
