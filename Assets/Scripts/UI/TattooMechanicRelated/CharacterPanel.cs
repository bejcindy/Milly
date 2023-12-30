using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : TattooPanel
{
    GraphicRaycaster graphRay;
    public Transform currentPanel;

    bool replacePanel;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
    }

    protected override void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponentInParent<GraphicRaycaster>().enabled = true;
        canvasGroup.alpha = AlphaBasedOnScale(1);

        if(canvasGroup.alpha == 1)
        {
            if (!replacePanel)
            {
                mainTattooMenu.ChangePanel();
                replacePanel = true;
            }


        }

        if (!noDrag)
        {
            DragPanel();
        }
    }

    public void ResetLocalVar()
    {
        replacePanel = false;
    }



    float AlphaBasedOnScale(float maxA)
    {
        float alpha = Mathf.Lerp(0, maxA, Mathf.InverseLerp(.5f, .2f, currentPanel.localScale.x));
        return alpha;
    }

    protected override void DragPanel()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * mouseDragSpeed, Input.GetAxis("Mouse Y") * mouseDragSpeed);
            panelTransform.anchoredPosition += dragAmount;
        }
    }


}
