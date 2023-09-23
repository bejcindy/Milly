using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Felix : NPCControl
{
    public BuildingGroupController connectedGroupOne;
    public BuildingGroupController connectedGroupTwo;



    public void Felix1Action()
    {

        //animation state
        //StartCoroutine(RotateTowards(connectedGroupOne.transform));
        //changes dialogue but maybe this is in idle to talking

        //activates stuff underconditions 

        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            Debug.Log("in vincinity");
            connectedGroupOne.activateAll = true;
        }

    }

    public void Felix2Action()
    {
        Debug.Log("2 is happening too");
        SetAnimatorTrigger("Stop");
        //StartCoroutine(RotateTowards(connectedGroupTwo.transform));

        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            connectedGroupTwo.activateAll = true;
        }
    }
}
