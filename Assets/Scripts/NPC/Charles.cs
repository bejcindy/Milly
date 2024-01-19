using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class Charles : NPCControl
{
    public bool charlesSmoking;
    public bool charlesOpenDoor;

    float waitTime = 3f;

    protected override void Start()
    {
        base.Start();

    }
    protected override void Update()
    {
        base.Update();
    }

    public void CharlesAction1()
    {

        talkable = true;

    }

    public void CharlesAction2()
    {
        noTalkStage = true;
        if(charlesOpenDoor)
            destObjects[_counter - 1].GetComponent<Door>().NPCOpenDoor();

        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            StopIdle();
            waitTime = 3f;
        }

    }

    public void CharlesAction3()
    {
        noTalkStage = false;
        gameObject.SetActive(false);
    }

    public void FinishSmoking()
    {
        Debug.Log("finishing smoking");
        //machine.ResetAnimTrigger("Special1");
        machine.SetAnimatorTrigger("Finish");
    }


}
