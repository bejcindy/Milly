using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{
    float waitTime = 3f;
    public Transform chair;

    bool gloriaOpenDoor;
    protected override void Start()
    {
        base.Start();
        overrideNoControl = true;
        talkable = true;
    }

    public void GloriaAction1()
    {
        stopIdleAfterConvo = true;
    }
     
    public void GloriaAction2()
    {
        stopIdleAfterConvo = true;
        noLookInConvo = true;
    }

    public void GloriaAction3()
    {

        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);
        noLookInConvo = true;

    }

    public void GloriaAction4()
    {
        noLookInConvo = true;
        noMoveAfterTalk = true;
        remainInAnim = true;

    }

    public void GloriaAction5()
    {
        transform.SetParent(chair);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.4f, -0.6f, 0.2f), 1f);
        Vector3 sitRotation = new Vector3(0, 180, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(sitRotation), 1f);

    }

}
