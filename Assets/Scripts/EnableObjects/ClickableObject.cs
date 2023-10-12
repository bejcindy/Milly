using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickableObject : LivableObject
{
    Animator animator;
    public bool groupControl;
    public BuildingGroupController buildingGroup;

    [Header("Bell Binding")]
    public bool bellBinding;
    public Usable dialogueUsable;
    public FixedCameraObject bindedObject;
    DialogueSystemTrigger dialogue;

    bool iconHidden;
    PlayerHolding playerHolding;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
        if(TryGetComponent<Animator>(out Animator anim))
        {
            animator = anim;
        }

        if(TryGetComponent<DialogueSystemTrigger>(out DialogueSystemTrigger dia))
        {
            dialogue = dia;
        }
        if(TryGetComponent<Usable>(out Usable usableCom))
        {
            dialogueUsable = usableCom;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            
            if(bellBinding && bindedObject.isInteracting && !playerHolding.inDialogue)
            {
                dialogueUsable.enabled = true;
                if (Input.GetMouseButton(0))
                {
                    RaycastHit raycastHit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out raycastHit, 6f))
                    {
                        if (raycastHit.transform.name == transform.name)
                        {
                            activated = true;
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0) && activated)
                {

                    UnpressButton();
                }
            }
            else
            {
                dialogueUsable.enabled = false;
            }

        }
    }

    void ResetAnimTrigger()
    {
        animator.ResetTrigger("Released");
    }

    public void ActivateGroup()
    {
        buildingGroup.activateAll = true;
    }

    public void PressButton()
    {
        animator.SetTrigger("Pressed");
    }

    public void UnpressButton()
    {
        animator.ResetTrigger("Pressed");
        animator.SetTrigger("Released");
        Invoke(nameof(ResetAnimTrigger), 0.5f);
    }

    void OnConversationEnd(Transform other)
    {
        if(gameObject.name.Contains("button"))
        {
            Debug.Log("Conversation over");
        }
        if (groupControl)
        {
            if (!buildingGroup.activateAll)
            {
                ActivateGroup();
            }
        }
    }
}
