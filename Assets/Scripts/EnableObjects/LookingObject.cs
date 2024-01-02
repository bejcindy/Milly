using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMOD;
using FMODUnity;
using VInspector;

public class LookingObject : LivableObject
{
    PlayerMovement pm;
    private bool posterLinkAct;
    private DialogueSystemTrigger dialogue;
    private GameObject[] sameTypePosters;
    bool playedSF;
    string lookSound = "event:/Sound Effects/Poster_Look";

    [Foldout("Looking")]
    public bool selected;
    public bool focusingThis;



    [Foldout("Special Looking")]
    public bool designatedSpot;
    public bool inSpot;

    protected override void Start()
    {
        base.Start();
        pm = ReferenceTool.playerMovement;
        if (TryGetComponent<DialogueSystemTrigger>(out DialogueSystemTrigger dia))
        {
            dialogue = dia;
        }
        sameTypePosters = GameObject.FindGameObjectsWithTag(gameObject.tag);
        lookingType = true;
    }

    protected override void Update()
    {
        //Debug.Log(matColorVal);
        base.Update();
        if (matColorVal > 0)
        {
            if (focusingThis)
            {
                //DataHolder.currentFocus = gameObject;
                DataHolder.FocusOnThis(matColorVal);
                if (!playedSF && DataHolder.camBlended && DataHolder.camBlendDone)
                {
                    RuntimeManager.PlayOneShot("event:/Sound Effects/Focus", transform.position);
                    
                    playedSF = true;
                }
            }
            if (!designatedSpot)
            {
                if (interactable && !MainTattooMenu.tatMenuOn)
                {
                    playerHolding.AddLookable(gameObject);
                    if (selected)
                    {
                        if (Input.GetKeyDown(KeyCode.Space) && !focusingThis)
                        {
                            if(gameObject.tag.Contains("Poster"))
                                RuntimeManager.PlayOneShot(lookSound, transform.position);

                            activated = true;
                            focusingThis = true;
                            FocusOnObject();
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
            focusingThis = false;
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
            else if (activated || transformed)
                gameObject.layer = 17;
            else
            {
                gameObject.layer = 0;
            }
        }

    }

    public void FocusOnObject()
    {
        DataHolder.currentFocus = this.gameObject;
        DataHolder.focusCinemachine.LookAt = gameObject.transform;
        playerCam.LookAt = gameObject.transform;
        DataHolder.pov.m_HorizontalAxis.m_MaxSpeed = 0f;
        DataHolder.pov.m_VerticalAxis.m_MaxSpeed = 0f;

        DataHolder.focusCinemachine.m_Priority = playerCam.m_Priority + 1;
        gameObject.layer = 12;
        playerHolding.looking = true;
        pm.enabled = false;
        playerHolding.HideLookingHint();

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
