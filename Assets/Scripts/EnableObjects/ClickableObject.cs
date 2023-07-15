using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : LivableObject
{
    Animator animator;
    public bool groupControl;
    public BuildingGroupController buildingGroup;
    protected override void Start()
    {
        base.Start();
        if(TryGetComponent<Animator>(out Animator anim))
        {
            animator = anim;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            Cursor.lockState = CursorLockMode.None;
            if (Input.GetMouseButton(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 6f))
                {
                    if (raycastHit.transform.name == transform.name)
                    {
                        activated = true;
                        animator.SetTrigger("Pressed");
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && activated)
            {
                if (groupControl)
                {
                    if (!buildingGroup.activateAll)
                    {
                        ActivateGroup();
                    }
                }

                animator.ResetTrigger("Pressed");
                animator.SetTrigger("Released");
                Invoke(nameof(ResetAnimTrigger), 0.5f);
            }
        }
    }

    void ResetAnimTrigger()
    {
        animator.ResetTrigger("Released");
    }

    void ActivateGroup()
    {
        buildingGroup.activateAll = true;
    }
}
