using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Cinemachine;

public class Xixi : NPCControl
{
    public EventReference catMeowSF;
    public CinemachineVirtualCamera catCam;

    float waitTime = 3f;
    protected override void Start()
    {
        base.Start();
        talkable = true;
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
        noTalkInWalk = false;
    }

    public void XixiAction3()
    {
        GetComponent<BoxCollider>().enabled = true;
        noTalkInWalk = true;
    }
    public void Meow()
    {
        if (!catMeowSF.IsNull)
            RuntimeManager.PlayOneShot(catMeowSF, transform.position);
    }

    protected override void OnConversationStart(Transform other)
    {
        base.OnConversationStart(other);
        catCam.m_Priority = 10;
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
        initialActivated = true;
        ChangeLayer(17);
    }

    public void ResetXixiCam()
    {
        catCam.m_Priority = 9;
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
