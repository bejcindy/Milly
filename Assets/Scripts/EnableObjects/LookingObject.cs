using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LookingObject : LivableObject
{
    PlayerHolding playerHolding;

    public bool designatedSpot;
    public bool inSpot;
    public bool selected;
    public DialogueSystemTrigger dialogue;
    public bool focusingThis;

    public GameObject[] sameTypePosters;

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
        if (matColorVal > 0)
        {
            base.Update();
            if (focusingThis)
            {
                DataHolder.FocusOnThis(fadeInterval, matColorVal);
                DataHolder.currentFocus = gameObject;
            }
            if (!designatedSpot)
            {
                if (interactable && !playerHolding.inDialogue)
                {
                    playerHolding.AddLookable(gameObject);
                    if (selected)
                    {
                        //DataHolder.FocusOnThis(fadeInterval, matColorVal);
                        //DataHolder.currentFocus = gameObject;
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            activated = true;
                            focusingThis = true;
                            //DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            //DataHolder.currentFocus = gameObject;
                        }
                    }
                    //else if (!selected)
                    //{
                    //    if (DataHolder.currentFocus == gameObject)
                    //    {
                    //        DataHolder.focusing = false;
                    //    }
                    //}

                }
                else
                {
                    activated = false;
                    playerHolding.RemoveLookable(gameObject);
                    //if (DataHolder.currentFocus == gameObject)
                    //{
                    //    DataHolder.focusing = false;
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
                            //gameObject.layer = 12;
                            //DataHolder.FocusOnThis(fadeInterval, matColorVal);
                            //DataHolder.currentFocus = gameObject;
                            focusingThis = true;
                        }
                        activated = true;
                    }
                }
                else
                {
                    //gameObject.layer = 13;
                    if (DataHolder.currentFocus == gameObject)
                    {
                        DataHolder.focusing = false;
                    }
                }
            }

        }
        else
        {
            if (specialEffect != null)
                specialEffect.SetActive(true);
            if (dialogue!=null)
                dialogue.enabled = true;
            //gameObject.layer = 0;
            //gameObject.layer = 13;
            playerHolding.RemoveLookable(gameObject);
            //DataHolder.focusing = false;
            if(sameTypePosters.Length > 0)
            {
                ActivateAll();
            }
        }

        if (selected)
        {
            gameObject.layer = 12;
        }
        else
        {
            gameObject.layer = 0;
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

    void ActivateAll()
    {
        foreach(GameObject obj in sameTypePosters)
        {
            if (obj.GetComponent<LookingObject>())
            {
                LookingObject looking = obj.GetComponent<LookingObject>();
            }

        }
    }

}
