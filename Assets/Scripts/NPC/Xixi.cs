using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Cinemachine;
using FMOD;

public class Xixi : NPCControl
{
    public EventReference catMeowSF;
    public CinemachineVirtualCamera catCam;
    public Vector3 catFoodPos;
    public Vector3 catFoodRot;
    PlayerLeftHand playerLeftHand;
    string catEatEvent = "event:/NPC/CatEat";

    float waitTime = 3f;
    protected override void Start()
    {
        base.Start();
        talkable = false;
        playerLeftHand = ReferenceTool.playerLeftHand;
    }

    public void XiXiAction0()
    {

    }

    public void XixiAction1()
    {
        talkable = false;
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            StopIdle();
            waitTime = 3f;
        }
            
    }

    public void XixiAction2()
    {
        talkable = true;

    }

    public void XixiAction3()
    {

        if(playerLeftHand.isHolding && playerLeftHand.holdingObj.name.Contains("Cat_can"))
        {
            talkable = true;
        }
        else
        {
            talkable = false;
        }

    }
    
    public void XixiAction4()
    {
        talkable = false;
    }

    public void XixiAction5()
    {

    }
    public void Meow()
    {
        if (!catMeowSF.IsNull)
            RuntimeManager.PlayOneShot(catMeowSF, transform.position);
    }



    public void ChangeIntialActivate()
    {
        colored = true;
        ChangeLayer(17);
    }

    public void ResetXixiCam()
    {
        catCam.m_Priority = 9;
    }

    public void EatCan()
    {
        FMODUnity.RuntimeManager.PlayOneShot(catEatEvent, transform.position);
        anim.SetTrigger("Eat");
    }

    public void MoveCatIntoBox()
    {
        StopIdle();
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
    }




}
