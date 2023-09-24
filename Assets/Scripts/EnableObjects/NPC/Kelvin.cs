using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelvin : NPCControl
{
    public BuildingGroupController connectedGroupOne;
    public BuildingGroupController connectedGroupTwo;
    public BuildingGroupController connectedGroupThree;

    protected override void Start()
    {
        base.Start();
        machine.initialState = ChooseInitialState('F');
    }

    public void Kelvin1Action()
    {

        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            Debug.Log("in vincinity");
            if (!connectedGroupOne.activateAll)
                connectedGroupOne.activateAll = true;
        }

    }

    public void Kelvin2Action()
    {

        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            if (!connectedGroupTwo.activateAll)
                connectedGroupTwo.activateAll = true;
        }
    }

    public void KelvinAction3()
    {
        if (Vector3.Distance(transform.position, player.position) < npcVincinity)
        {
            if (!connectedGroupThree.activateAll)
                connectedGroupThree.activateAll = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Kelvin1")
        {
            Debug.Log("runnin dest 1 rotate");
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(connectedGroupOne.transform));
        }

        if (other.gameObject.name == "Kelvin2")
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(connectedGroupTwo.transform));
        }

        if (other.gameObject.name == "Kelvin3")
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = StartCoroutine(RotateTowards(connectedGroupThree.transform));
        }
    }
}
