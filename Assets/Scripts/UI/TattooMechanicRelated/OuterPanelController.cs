using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OuterPanelController : MonoBehaviour
{
    public static bool mechanicActivated;
    public bool takeOver;
    public bool zoomIn;
    public PannelController currentPanel;
    [Header("Corresponding")]
    public PannelController[] panels;
    public RectTransform[] imgRects;
    Image[] childImgs;
    bool resetChild;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (PannelController panel in panels)
            panel.parentControl = this;
        childImgs = GetComponentsInChildren<Image>();
        foreach (Image img in childImgs)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        mechanicActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (takeOver && currentPanel && !zoomIn)
        {
            //when zoom out child panel & fade in this panel
            //foreach (Image img in childImgs)
            //    img.color = AlphaBasedOnScale(img.color, currentPanel.transform, 1);
            for(int i = 0; i < panels.Length; i++)
            {
                if (panels[i].activatedOnce)
                {
                    imgRects[i].GetComponent<Image>().color = AlphaBasedOnScale(imgRects[i].GetComponent<Image>().color, currentPanel.transform, 1);
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = true;
                }
                else
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = false;
            }
            RectTransform matchingImg = imgRects[System.Array.IndexOf(panels, currentPanel)];
            GetComponent<RectTransform>().anchoredPosition = currentPanel.GetComponent<RectTransform>().anchoredPosition - matchingImg.anchoredPosition;
            ReferenceTool.pauseMenu.inTattoo = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (currentPanel.transform.localScale.x == .2f)
            {
                currentPanel.enabled = false;
                currentPanel.activated = false;
            }
            resetChild = false;
        }
        else if(takeOver && currentPanel && zoomIn)
        {
            if (!resetChild)
            {
                RectTransform matchingImg = imgRects[System.Array.IndexOf(panels, currentPanel)];
                currentPanel.transform.localScale = new Vector2(.2f, .2f);
                currentPanel.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + matchingImg.anchoredPosition;
                currentPanel.enabled = true;
                currentPanel.activated = true;
                currentPanel.noDrag = true;
                resetChild = true;
                ReferenceTool.pauseMenu.inTattoo = false;
            }
            //foreach (Image img in childImgs)
            //    img.color = AlphaBasedOnScale(img.color, currentPanel.transform, 1);
            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i].activatedOnce)
                {
                    imgRects[i].GetComponent<Image>().color = AlphaBasedOnScale(imgRects[i].GetComponent<Image>().color, currentPanel.transform, 1);
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = true;
                }
                else
                    imgRects[i].GetComponent<OuterPanelButton>().enabled = false;
            }
            currentPanel.transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + .1f*Time.deltaTime, .2f, 1f), Mathf.Clamp(transform.localScale.y + .1f*Time.deltaTime, .2f, 1f));
            if (currentPanel.transform.localScale.x == 1f)
            {
                currentPanel.noDrag = false;
                zoomIn = false;
                takeOver = false;
            }
        }
        else
        {
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
                if (img.GetComponent<OuterPanelButton>())
                    img.GetComponent<OuterPanelButton>().enabled = false;
            }
            ReferenceTool.pauseMenu.inTattoo = false;
        }
    }

    Color AlphaBasedOnScale(Color c,Transform t, float maxA)
    {
        float alpha = Mathf.Lerp(0, maxA, Mathf.InverseLerp(.5f, .2f, t.localScale.x));
        c = new Color(c.r, c.g, c.b, alpha);
        return c;
    }

    Color FadeOutColor(Color c)
    {

        if (c.a > 0)
            c -= new Color(0, 0, 0, 1f) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, 0);
        return c;
    }
}


