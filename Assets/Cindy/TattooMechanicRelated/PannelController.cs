using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class PannelController : MonoBehaviour
{
    public static bool mechanicActivated;
    public bool activated;
    public float fadeSpeed;
    public RectTransform CanvasRect;
    public Image pannelBG;
    public Renderer targetObj;
    public RectTransform currentTattoo;
    public float mouseDragSpeed, scrollSizeSpeed;

    Image[] childImgs;
    public UILineRendererList blackLine, greyLine;

    bool gotPos, noDrag, firstActivated;
    float timer;

    private void Awake()
    {
        childImgs = GetComponentsInChildren<Image>();
        foreach (Image img in childImgs)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }
        blackLine.color = new Color(blackLine.color.r, blackLine.color.g, blackLine.color.b, 0);
        greyLine.color = new Color(greyLine.color.r, greyLine.color.g, greyLine.color.b, 0);
        pannelBG.color = new Color(pannelBG.color.r, pannelBG.color.g, pannelBG.color.b, 0);
        mechanicActivated = false;
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
                GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition - currentTattoo.anchoredPosition;
                currentTattoo.GetComponent<TattooConnection>().activated = true;
                foreach (RectTransform relate in currentTattoo.GetComponent<TattooConnection>().relatedTattoos)
                    relate.GetComponent<TattooConnection>().related = true;
                GetComponent<ConnectionManager>().ActivateLines(currentTattoo.GetComponent<TattooConnection>());
                transform.localScale = new Vector2(1, 1);
                gotPos = true;
                noDrag = true;
            }
            else if (!targetObj || gotPos)
            {
                if (currentTattoo)
                {
                    currentTattoo.GetComponent<Image>().color = FadeInColor(currentTattoo.GetComponent<Image>().color, 1);
                    if (currentTattoo.GetComponent<Image>().color.a == 1)
                    {
                        if (greyLine.color.a != 1)
                            timer += Time.deltaTime;
                        //Debug.Log(timer);
                        if (timer > .5f && timer < 2f)
                        {
                            pannelBG.color = FadeInColor(pannelBG.color, 1);
                        }
                        if (timer > 2f && timer < 3.5f)
                        {
                            if (firstActivated)
                            {
                                foreach (Image img in childImgs)
                                {
                                    if (img.GetComponent<TattooConnection>())
                                    {
                                        if (img.GetComponent<TattooConnection>().activated)
                                            img.color = FadeInColor(img.color, 1);
                                    }
                                }
                                blackLine.color = FadeInColor(blackLine.color, 1);
                            }
                            else
                            {
                                Debug.Log("here");
                                foreach (Image img in childImgs)
                                {
                                    if (img.GetComponent<TattooConnection>())
                                    {
                                        if (!img.GetComponent<TattooConnection>().activated && img.GetComponent<TattooConnection>().related)
                                            img.color = FadeInColor(img.color, .5f);
                                    }
                                }
                                greyLine.color = FadeInColor(greyLine.color, 1);
                            }
                        }
                        if (firstActivated && timer > 3.5f)
                        {
                            foreach (Image img in childImgs)
                            {
                                if (img.GetComponent<TattooConnection>())
                                {
                                    if (!img.GetComponent<TattooConnection>().activated && img.GetComponent<TattooConnection>().related)
                                        img.color = FadeInColor(img.color, .5f);
                                }
                            }
                            greyLine.color = FadeInColor(greyLine.color, 1);
                        }

                    }
                }
                if (!currentTattoo)
                {
                    foreach (Image img in childImgs)
                    {
                        if (img.GetComponent<TattooConnection>())
                        {
                            if (img.GetComponent<TattooConnection>().activated)
                                img.color = FadeInColor(img.color, 1);
                            else if (img.GetComponent<TattooConnection>().related)
                                img.color = FadeInColor(img.color, .5f);
                        }
                    }

                    blackLine.color = FadeInColor(blackLine.color, 1);
                    greyLine.color = FadeInColor(greyLine.color, 1);
                    pannelBG.color = FadeInColor(pannelBG.color, 1);
                }
            }
            if (greyLine.color.a == 1)
            {
                noDrag = false;
                firstActivated = true;
            }

        }
        else
        {
            gotPos = false;
            noDrag = true;
            timer = 0;
            foreach (Image img in childImgs)
            {
                img.color = FadeOutColor(img.color);
            }
            blackLine.color = FadeOutColor(blackLine.color);
            greyLine.color = FadeOutColor(greyLine.color);
            pannelBG.color = FadeOutColor(pannelBG.color);
        }

        if (!noDrag)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * mouseDragSpeed, Input.GetAxis("Mouse Y") * mouseDragSpeed);
                GetComponent<RectTransform>().anchoredPosition += dragAmount;
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                float scrollAmount = Input.mouseScrollDelta.y;
                transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + scrollAmount * scrollSizeSpeed, .5f, 2f), Mathf.Clamp(transform.localScale.y + scrollAmount * scrollSizeSpeed, .5f, 2f));
            }
        }

        if (mechanicActivated)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                activated = !activated;
            }
        }
    }

    Color FadeInColor(Color c, float targetAlpha)
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
