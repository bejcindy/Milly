using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hugo : NPCControl 
{
    public GameObject broom;
    public Transform broomPlacePos;
    public TrashSoccerScoreBoard scoreBoard;

    public GameObject cigButtConvo;
    public CharacterTattoo willowTat;
    protected override void Start()
    {
        base.Start();

        noMoveAfterTalk = true;
        noLookInConvo = true;
    }

    public void HugoAction1()
    {

        currentDialogue.gameObject.SetActive(true);
    }

    public void HugoAction2() { }

    public void HugoAction3()
    {
        if (inConversation)
        {
            npcActivated = true;
        }
        scoreBoard.enabled = true;
        noMoveAfterTalk = false;
    }

    public void HugoAction4()
    {
        talkable = true;
    }

    public void TriggerWillowTattoo()
    {
        willowTat.triggered = true;
    }

    public void HugoDropBroom()
    {
        broom.transform.SetParent(null);
        broom.transform.position = broomPlacePos.position + new Vector3(0, 1, 0);
        broom.GetComponent<Rigidbody>().isKinematic = false;
        broom.transform.rotation = Quaternion.identity;
    }

    protected override void OnConversationEnd(Transform other)
    {
        base.OnConversationEnd(other);

        if(_counter == 3)
        {
            ActivateTattooMenu();
        }
        if(_counter == 4)
        {
            TriggerWillowTattoo();
        }
    }

    protected override void OnTriggerEnter(Collider other) 
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("CigButt"))
        {
            if(!cigButtConvo.activeSelf)
            {
                cigButtConvo.SetActive(true);
            }
        }
    }


}
