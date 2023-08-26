using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LookingObject : LivableObject
{
    bool interacted;
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
                            //base.FocusOnThis();
                            DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            DataHolder.currentFocus = gameObject;

                            //Debug.Log(gameObject.name + ": focused");
                            interacted = true;
                        }
                        activated = true;
                    }

                }
                else
                {
                    gameObject.layer = 0;
                    if (DataHolder.currentFocus == gameObject)
                    {
                        //    DataHolder.currentFocus = null;
                        DataHolder.focusing = false;
                        //Debug.Log(gameObject.name + ": unfocused");
                    }
                    //if (interacted)
                    //{
                    //    //Unfocus(interacted);
                    //    if (DataHolder.currentFocus == gameObject)
                    //        DataHolder.currentFocus = null;
                    //    //DataHolder.Unfocus(gameObject, interacted, matColorVal);
                    //    Debug.Log(gameObject.name + "unfocusing");
                    //}
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
                            //base.FocusOnThis();
                            DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            DataHolder.currentFocus = gameObject;

                            //Debug.Log(gameObject.name + ": focused");
                            interacted = true;
                        }
                        activated = true;
                    }
                }
                else
                {
                    gameObject.layer = 0;
                    if (DataHolder.currentFocus == gameObject)
                    {
                        //    DataHolder.currentFocus = null;
                        DataHolder.focusing = false;
                        //Debug.Log(gameObject.name + ": unfocused");
                    }
                    //if (interacted)
                    //{
                    //    //Unfocus(interacted);
                    //    if (DataHolder.currentFocus == gameObject)
                    //        DataHolder.currentFocus = null;
                    //    //DataHolder.Unfocus(gameObject, interacted, matColorVal);
                    //    Debug.Log(gameObject.name + "unfocusing");
                    //}
                }
            }

        }
        else
        {
            dialogue.enabled = true;
            //Unfocus(interacted);
            //DataHolder.Unfocus(gameObject, interacted, matColorVal);
            gameObject.layer = 0;
            //Debug.Log(gameObject.name + "uunfocusinig");
            DataHolder.focusing = false;
            gameObject.GetComponent<LookingObject>().enabled = false;
        }

        //if (firstActivated)
        //    gameObject.layer = 0;

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
