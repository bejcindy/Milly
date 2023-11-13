using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMOD;
using FMODUnity;

public class LookingObject : LivableObject
{
    PlayerHolding playerHolding;

    public bool designatedSpot;
    public bool inSpot;
    public bool selected;
    public bool posterLinkAct;
    public DialogueSystemTrigger dialogue;
    public bool focusingThis;

    public GameObject[] sameTypePosters;
    bool playedSF;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        if (TryGetComponent<DialogueSystemTrigger>(out DialogueSystemTrigger dia))
        {
            dialogue = dia;
        }
        sameTypePosters = GameObject.FindGameObjectsWithTag(gameObject.tag);
    }

    protected override void Update()
    {
        //Debug.Log(matColorVal);
        base.Update();
        if (matColorVal > 0)
        {
            if (focusingThis)
            {
                DataHolder.FocusOnThis(fadeInterval, matColorVal);
                DataHolder.currentFocus = gameObject;
                if (!playedSF && DataHolder.camBlended && DataHolder.camBlendDone)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Focus", transform.position);
                    
                    playedSF = true;
                }
            }
            if (!designatedSpot)
            {
                if (interactable)
                {
                    playerHolding.AddLookable(gameObject);
                    if (selected)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            activated = true;
                            focusingThis = true;
                        }
                    }

                }
                else
                {
                    playerHolding.RemoveLookable(gameObject);
                    selected = false;
                }
            }

        }
        else
        {
            if (activated)
            {
                if (specialEffect != null)
                    specialEffect.SetActive(true);
                if (dialogue != null && !posterLinkAct)
                    dialogue.enabled = true;
                playerHolding.RemoveLookable(gameObject);
                if (sameTypePosters.Length > 0)
                {
                    ActivateAll();
                }
            }
            else
            {
                if(interactable && !playerHolding.inDialogue)
                {
                    activated = true;
                }
            }

        }

        if (!focusingThis&&!DataHolder.camBlendDone&&!DataHolder.camBlended)
        {
            if (selected)
            {
                gameObject.layer = 9;
            }
            else if (activated)
                gameObject.layer = 17;
            else
            {
                gameObject.layer = 0;
            }
        }

    }

    void OnConversationStart(Transform other)
    {
        playerHolding.inDialogue = true;
    }

    void OnConversationEnd(Transform other)
    {
        playerHolding.inDialogue = false;
    }

    void ActivateAll()
    {
        if(sameTypePosters.Length > 0)
        {
            foreach (GameObject obj in sameTypePosters)
            {
                if (obj.GetComponent<LookingObject>())
                {
                    LookingObject looking = obj.GetComponent<LookingObject>();
                    looking.posterLinkAct = true;
                    looking.TurnOnColor(looking.mat);
                }

            }
        }

    }

}
