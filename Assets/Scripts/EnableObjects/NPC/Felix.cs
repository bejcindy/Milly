using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Felix : NPCControl
{
    public BuildingGroupController connectedGroupOne;
    public BuildingGroupController connectedGroupTwo;



    protected override void Start()
    {
        base.Start();
        machine.initialState = ChooseInitialState('F');
    }
    public void Felix1Action()
    {


        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            Debug.Log("in vincinity");
            if(!connectedGroupOne.activateAll)
                connectedGroupOne.activateAll = true;
        }

    }

    public void Felix2Action()
    {

        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            if(!connectedGroupTwo.activateAll)
                connectedGroupTwo.activateAll = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Felix1")
        {
            Debug.Log("runnin dest 1 rotate");
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(connectedGroupOne.transform));
        }

        if(other.gameObject.name == "Felix2")
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(connectedGroupTwo.transform));
        }
    }
}
