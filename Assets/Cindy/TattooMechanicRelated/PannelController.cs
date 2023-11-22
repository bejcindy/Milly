using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PannelController : MonoBehaviour
{
    public Renderer targetObj;
    public bool activated;
    public float fadeSpeed;
    public RectTransform CanvasRect;
    public Image pannelBG;
    public RectTransform currentTattoo;

    Image[] childImgs;
    UILineRendererList lines;
    bool gotPos,noDrag;
    

    private void Awake()
    {
        childImgs = GetComponentsInChildren<Image>();
        lines = GetComponentInChildren<UILineRendererList>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (targetObj && currentTattoo && !gotPos)
            {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(targetObj.bounds.center);
                Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
                GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition-currentTattoo.anchoredPosition;
                currentTattoo.GetComponent<TattooConnection>().activated = true;
                foreach (RectTransform relate in currentTattoo.GetComponent<TattooConnection>().relatedTattoos)
                    relate.GetComponent<TattooConnection>().related = true;
                GetComponent<ConnectionManager>().ActivateLines(currentTattoo.GetComponent<TattooConnection>());
                gotPos = true;
                noDrag = true;
            }
            else if (!targetObj || gotPos)
            {
                if (currentTattoo)
                    currentTattoo.GetComponent<Image>().color= FadeInColor(currentTattoo.GetComponent<Image>().color, 1);
                if (currentTattoo.GetComponent<Image>().color.a == 1 || !currentTattoo)
                {
                    foreach (Image img in childImgs)
                    {
                        //img.color = FadeInColor(img.color);
                        if (img.GetComponent<TattooConnection>())
                        {
                            if (img.GetComponent<TattooConnection>().activated)
                                img.color = FadeInColor(img.color, 1);
                            else if (img.GetComponent<TattooConnection>().related)
                                img.color = FadeInColor(img.color, .5f);
                        }
                    }
                    lines.color = FadeInColor(lines.color, 1);
                    pannelBG.color = FadeInColor(pannelBG.color, 1);
                }
            }else if (pannelBG.color.a == 1)
            {
                
            }

        }
        else
        {
            gotPos = false;
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
            }
            lines.color = FadeOutColor(lines.color);
            pannelBG.color = FadeOutColor(pannelBG.color);
        }
    }

    Color FadeInColor(Color c,float targetAlpha)
    {

        if (c.a < targetAlpha)
            c += new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, targetAlpha);
        return c;
    }

    Color FadeOutColor(Color c)
    {

        if (c.a > 0)
            c -= new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, 0);
        return c;
    }

}
