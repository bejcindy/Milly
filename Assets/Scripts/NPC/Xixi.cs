using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Cinemachine;

public class Xixi : NPCControl
{
    public EventReference catMeowSF;
    public CinemachineVirtualCamera catCam;
    public Vector3 catFoodPos;
    public Vector3 catFoodRot;
    PlayerLeftHand playerLeftHand;

    float waitTime = 3f;
    protected override void Start()
    {
        base.Start();
        talkable = true;
        playerLeftHand = ReferenceTool.playerLeftHand;
    }

    public void XixiAction1()
    {
        noTalkStage = true;
        noTalkInWalk = false;
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            StopIdle();
            waitTime = 3f;
        }
            
    }

    public void XixiAction2()
    {
        noTalkStage = false;
        noLookInConvo = true;

    }

    public void XixiAction3()
    {
        remainInAnim = true;
        noMoveAfterTalk = true;
        noTalkInWalk = false;
        noLookInConvo = false;

        if(playerLeftHand.isHolding && playerLeftHand.holdingObj.name.Contains("Cat_can"))
        {
            noTalkStage = false;
        }
        else
        {
            noTalkStage = true;
        }

    }
    public void Meow()
    {
        if (!catMeowSF.IsNull)
            RuntimeManager.PlayOneShot(catMeowSF, transform.position);
    }


    protected override void OnConversationEnd(Transform other)
    {
        inConversation = false;
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);
        currentDialogue.gameObject.SetActive(false);
        noTalkInWalk = true;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void ChangeIntialActivate()
    {
        transformed = true;
        ChangeLayer(17);
    }

    public void ResetXixiCam()
    {
        catCam.m_Priority = 9;
    }

    public void EatCan()
    {
        anim.SetTrigger("Eat");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!npcActivated)
                npcActivated = true;
            ChangeLayer(17);
            StartTalking();
        }
    }


}
