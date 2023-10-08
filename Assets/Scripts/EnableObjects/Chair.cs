using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : FixedCameraObject
{
    public GameObject AkiConfrontation;
    protected override void Update()
    {
        base.Update();

        if (MainQuestState.firstGloriaTalk && isInteracting)
            AkiConfrontation.SetActive(true);

        if (isInteracting)
            playerHolding.atTable = true;
        else
            playerHolding.atTable = false;
    }
}
