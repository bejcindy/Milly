using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Chair : FixedCameraObject
{
    [SerializeField]
    
    bool stand, sit;
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
                stand = false;
            }
        }
        else
        {
            playerHolding.atTable = false;
            if (!stand)
            {
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Chair_Pull", transform.position);
                sit = false;
                stand = true;
            }
        }
    }
}
