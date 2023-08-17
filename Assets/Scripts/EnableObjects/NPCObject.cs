using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : LivableObject
{
    public BoxCollider convoTrigger;

    [Header("Object Activation")]
    public bool objectOriented;
    public LivableObject npcObject;


    [Header("Looking Activation")]
    public bool lookingOriented;
    public LookingObject npcLooking;

    protected override void Start()
    {
        base.Start();
        if (lookingOriented)
            npcLooking = GetComponent<LookingObject>();

        convoTrigger = GetComponent<BoxCollider>();
    }


    protected override void Update()
    {
        base.Update();
        if (npcObject.activated)
        {
            activated = true;
        }

        if (lookingOriented)
        {
            if (npcLooking.activated)
                activated = true;
        }
    }


    public void ActivateConvoTrigger()
    {
        convoTrigger.enabled = true;
    }

    public void DeactivateConvoTrigger()
    {
        convoTrigger.enabled = false;
    }
}
