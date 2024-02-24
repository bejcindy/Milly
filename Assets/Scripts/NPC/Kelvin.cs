using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelvin : NPCControl
{
    protected override void Start()
    {
        base.Start();
        talkable = true;
        matColorVal = 1;
    }

    public void KelvinAction1()
    {
        noCameraLock = true;
    }

    public void KelvinAction2()
    {

    }

    public void KelvinAction3()
    {
        noTalkStage = true;
        gameObject.SetActive(false);
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);
    }
}
