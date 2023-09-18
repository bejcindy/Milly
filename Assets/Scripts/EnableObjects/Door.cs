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
    }
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            gameObject.layer = 9;
            if (!interacting)
            {
                knockHint.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    knockHint.SetActive(false);
                    if(eventSoundName != "")
                        FMODUnity.RuntimeManager.PlayOneShot(eventSoundName, player.position);
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
