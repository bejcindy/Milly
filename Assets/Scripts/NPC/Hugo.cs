using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hugo : NPCControl 
{
    public GameObject broom;
    public Transform broomPlacePos;
    protected override void Start()
    {
        base.Start();

        noMoveAfterTalk = true;
        noLookInConvo = true;
    }

    public void HugoAction1()
    {
        noMoveAfterTalk = true;
        currentDialogue.gameObject.SetActive((true));

    }

    public void HugoAction2()
    {

    }

    public void HugoAction3()
    {
        broom.transform.SetParent(null);
        broom.transform.position = broomPlacePos.position + new Vector3(0, 1, 0);
        broom.GetComponent<Rigidbody>().isKinematic = false;
        broom.transform.rotation = Quaternion.identity;
        Invoke(nameof(StopIdle), 1f);
    }

    public void HugoAction4()
    {
        talkable = true;
    }


}
