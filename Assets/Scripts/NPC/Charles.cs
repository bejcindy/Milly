using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class Charles : NPCControl
{
    public bool charlesSmoking;
    public bool charlesOpenDoor;

    public GameObject charlesPizza;

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
        charlesPizza.SetActive(true);
    }

    public void CharlesAction3()
    {
        noTalkStage = false;
    }

    public void FinishSmoking()
    {
        Debug.Log("finishing smoking");
        //machine.ResetAnimTrigger("Special1");
        machine.SetAnimatorTrigger("Finish");
    }


}
