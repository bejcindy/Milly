using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charles : NPCControl
{
    public bool charlesSmoking;

    protected override void Update()
    {
        base.Update();
    }

    public void CharlesAction1()
    {

    }

    public void CharlesAction2()
    {
        Debug.Log("Calling charles 2");
        destObjects[_counter - 1].GetComponent<Door>().NPCOpenDoor();

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
