using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloria : NPCControl
{
    float waitTime = 3f;
    public Transform chair;
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
        noTalkStage = true;
        destObjects[_counter - 1].GetComponent<Door>().NPCOpenDoor();

        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            StopIdle();
            waitTime = 3f;
        }
    }

    public void GloriaAction3()
    {
        noTalkStage = true;
        currentDialogue.gameObject.SetActive(true);

    }

    public void GloriaAction4()
    {
        noTalkStage = false;
        transform.SetParent(chair);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -1.5f, 0), 1f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.zero), 1f);
    }

}
