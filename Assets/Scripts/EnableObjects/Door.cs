using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : LivableObject
{
    DialogueSystemTrigger dialogue;
    public GameObject leftKnockHint;
    public GameObject rightKnockHint;
    public bool interacting;

    string eventSoundName;
    PlayerHolding playerHolding;


    protected override void Start()
    {
        base.Start();
        dialogue = GetComponent<DialogueSystemTrigger>();
        playerHolding = player.GetComponent<PlayerHolding>();
    }
    protected override void Update()
    {
        base.Update();
        if (interactable && !activated)
        {
            gameObject.layer = 9;
            if (!interacting)
            {
                if (playerHolding.GetLeftHand())
                {
                    leftKnockHint.SetActive(true);
                }
                else if (playerHolding.GetRightHand())
                {
                    rightKnockHint.SetActive(true);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    leftKnockHint.SetActive(false);
                    rightKnockHint.SetActive(false);
                    dialogue.enabled = true;
                    interacting = true;
                    activated = true;
                }
            }
        }
        else
        {
            leftKnockHint.SetActive(false);
            rightKnockHint.SetActive(false);
            gameObject.layer = 0;
        }
    }
}
