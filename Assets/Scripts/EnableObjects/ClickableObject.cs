using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class ClickableObject : LivableObject
{
    Animator animator;
    public bool isGroupControl;
    public BuildingGroupController buildingGroup;

    [Header("Bell Binding")]
    public bool bellBinding;
    public Usable dialogueUsable;
    public FixedCameraObject bindedObject;
    DialogueSystemTrigger dialogue;

    bool iconHidden;
    public EventReference buzzSound;
    bool playedSound;
    protected override void Start()
    {
        base.Start();

        if (TryGetComponent(out Animator anim))
        {
            animator = anim;
        }

        if (TryGetComponent(out DialogueSystemTrigger dia))
        {
            dialogue = dia;
        }
        if (TryGetComponent(out Usable usableCom))
        {
            dialogueUsable = usableCom;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            if (bellBinding && bindedObject.isInteracting && !playerHolding.inDialogue)
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
                            dialogueUsable.OnUseUsable();
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0) && activated)
                    UnpressButton();
            }
            else
                dialogueUsable.enabled = false;
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
        if (!playedSound && !buzzSound.IsNull)
        {
            RuntimeManager.PlayOneShot(buzzSound, transform.position);
            playedSound = true;
        }
    }

    public void UnpressButton()
    {
        animator.ResetTrigger("Pressed");
        animator.SetTrigger("Released");
        Invoke(nameof(ResetAnimTrigger), 0.5f);
        playedSound = false;
    }

    void OnConversationEnd(Transform other)
    {
        if (isGroupControl)
        {
            if (!buildingGroup.activateAll)
            {
                ActivateGroup();
            }
        }
    }
}
