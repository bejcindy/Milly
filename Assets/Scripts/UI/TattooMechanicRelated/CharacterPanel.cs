using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : TattooPanel
{
    GraphicRaycaster graphRay;
    public Transform currentPanel;


    bool replacePanel;
    bool changingPanel;
    TattooPanel changeToPanel;
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
        if(!changingPanel)
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

        if (changingPanel)
        {
            PanelTransition();  
        }
    }

    public void ResetLocalVar()
    {
        replacePanel = false;
    }

    public void ChoosePanel(TattooPanel panel)
    {
        changingPanel = true;
        changeToPanel = panel;
    }

    void PanelTransition()
    {
        if(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = FadeOut(canvasGroup.alpha);
            mainTattooMenu.activePanel = changeToPanel;
        }
        else
        {
            changingPanel = false;
            mainTattooMenu.TurnOnActivePanel();
            gameObject.SetActive(false);
        }

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


    float FadeOut(float x)
    {
        if (x > 0)
            x -= 1f * Time.deltaTime;
        else
        {
            x = 0;
            mainTattooMenu.TurnOnActivePanel();
            gameObject.SetActive(false);
        }

        return x;
    }

}
