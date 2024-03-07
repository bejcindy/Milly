using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using PixelCrushers.DialogueSystem;

public class Chair : FixedCameraObject
{
    [SerializeField]
    bool sit;

    protected override void Start()
    {
        base.Start();
        dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            playerHolding.atTable = true;
            if (!sit)
            {
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Chair_Pull", transform.position);
                sit = true;

            }
        }
        else
        {
            playerHolding.atTable = false;
        }
    }
}
