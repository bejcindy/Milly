using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Felix : NPCControl
{



    protected override void Start()
    {
        base.Start();

    }
    public void FelixAction1()
    {

        //lookCoroutine = StartCoroutine(RotateTowards(destObjects[_counter - 1].transform));
        //if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        //{
        //    Debug.Log("in vincinity");
        //    if(!connectedGroupOne.activateAll)
        //        connectedGroupOne.activateAll = true;
        //}

    }

    public void FelixAction2()
    {

        //if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        //{
        //    if(!connectedGroupTwo.activateAll)
        //        connectedGroupTwo.activateAll = true;
        //}
    }


}
