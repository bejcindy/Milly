using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hugo : NPCControl 
{
    public GameObject broom;
    public Transform broomPlacePos;
    public TrashSoccerScoreBoard scoreBoard;
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
        talkable = false;
    }

    public void HugoAction3()
    {
        if (inConversation)
        {
            npcActivated = true;
        }
        scoreBoard.enabled = true;
        broom.transform.SetParent(null);
        broom.transform.position = broomPlacePos.position + new Vector3(0, 1, 0);
        broom.GetComponent<Rigidbody>().isKinematic = false;
        broom.transform.rotation = Quaternion.identity;
        noMoveAfterTalk = false;
    }

    public void HugoAction4()
    {
        talkable = true;
    }


}
