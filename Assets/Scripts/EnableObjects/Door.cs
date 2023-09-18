using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : LivableObject
{
    DialogueSystemTrigger dialogue;
    public GameObject knockHint;
    public bool interacting;

    string eventSoundName;


    protected override void Start()
    {
        base.Start();
        knockHint = GameObject.Find("QTEPanel").transform.GetChild(5).gameObject;
        dialogue = GetComponent<DialogueSystemTrigger>();
    }
    protected override void Update()
    {
        base.Update();
        if (interactable && !activated)
        {
            gameObject.layer = 9;
            if (!interacting)
            {
                knockHint.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    knockHint.SetActive(false);
                    dialogue.enabled = true;
                    interacting = true;
                    activated = true;
                }
            }
        }
        else
        {
            knockHint.SetActive(false);
            gameObject.layer = 0;
        }
    }
}
