using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Akihito : NPCControl 
{
    public Transform akiLantern;

    public void ActivateAki()
    {
        onHoldChar = true;
        ChangeLayer(17);
    }

    public void AkihitoAction1()
    {
        noMoveAfterTalk = true;
        noTalkInWalk = true;
    }

    public void LanternFall()
    {
        akiLantern.GetComponent<HingeJoint>().breakForce = 0;
        akiLantern.GetComponent<HingeJoint>().connectedBody = null;
        akiLantern.GetComponent<CollisionObject>().enabled = false;
        akiLantern.GetComponent<GroupMaster>().enabled = false;
        akiLantern.GetComponent<PickUpObject>().enabled = true;
    }



}
