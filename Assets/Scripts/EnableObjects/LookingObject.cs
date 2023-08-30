using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LookingObject : LivableObject
{
    PlayerHolding playerHolding;

    public bool designatedSpot;
    public bool inSpot;
    public DialogueSystemTrigger dialogue;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
    }

    protected override void Update()
    {
        //Debug.Log(matColorVal);
        if (matColorVal > 0)
        {
            base.Update();
            if (!designatedSpot)
            {
                if (interactable)
                {
                    if (!playerHolding.throwing && !playerHolding.inDialogue)
                    {
                        if (!firstActivated)
                        {
                            gameObject.layer = 12;
                            DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            DataHolder.currentFocus = gameObject;
                        }
                        activated = true;
                        if(specialEffect != null)
                            specialEffect.SetActive(true);
                    }

                }
                else
                {
                    gameObject.layer = 0;
                    if (DataHolder.currentFocus == gameObject)
                    {
                        DataHolder.focusing = false;
                    }
                }
            }
            else
            {
                if(inSpot && isVisible)
                {
                    if (!playerHolding.throwing)
                    {
                        if (!firstActivated)
                        {
                            gameObject.layer = 12;
                            DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            DataHolder.currentFocus = gameObject;
                        }
                        activated = true;
                        if (specialEffect != null) 
                            specialEffect.SetActive(true);
                    }
                }
                else
                {
                    gameObject.layer = 13;
                    if (DataHolder.currentFocus == gameObject)
                    {
                        DataHolder.focusing = false;
                    }
                }
            }

        }
        else
        {
            if(dialogue!=null)
                dialogue.enabled = true;
            gameObject.layer = 13;
            DataHolder.focusing = false;
        }

    }

    void OnConversationStart(Transform other)
    {
        player.GetComponent<PlayerHolding>().inDialogue = true;
    }

    void OnConversationEnd(Transform other)
    {
        player.GetComponent<PlayerHolding>().inDialogue = false;
    }

}
