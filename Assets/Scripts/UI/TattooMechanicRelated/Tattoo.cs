using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using TMPro;

public class Tattoo : MonoBehaviour
{
    [Foldout("Activation")]
    public bool isCenter;
    public bool isHinted;
    public bool activated;
    public bool hasRelated;
    public bool isShown;
    public bool hidden;

    [Foldout("Related")]
    public List<Tattoo> relatedTats;
    public RectTransform tatTransform;
    public static float fadeSpeed;

    TattooPanel myPanel;
    CenterTattoo centerTat;
    TextMeshProUGUI hintedVersion;
    Image tatImage;
    bool firstActivated;

    private void Awake()
    {
        Tattoo.fadeSpeed = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        hintedVersion = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        myPanel = transform.parent.GetComponent<TattooPanel>();
        tatTransform = GetComponent<RectTransform>();
        tatImage = GetComponent<Image>();
        centerTat = myPanel.centerTat;
        tatImage.color = new Color(0, 0, 0, 0);
        hintedVersion.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ShowTattooImage();
        if (activated && !firstActivated)
        {
            myPanel.panelOn = true;
            myPanel.enabled = true;
        }
    }

    void ShowTattooImage()
    {
        if(isHinted && !isShown)
        {
            if (hintedVersion.color.a < 1)
            {
                hintedVersion.color = FadeInColor(hintedVersion.color, 1);
                myPanel.noDrag = true;
            }
            else
                myPanel.noDrag = false;
        }
        else if(isShown)
        {
            if (!firstActivated)
            {
                if (!myPanel.lerping)
                    StartCoroutine(myPanel.LerpPosition(-tatTransform.anchoredPosition, 1f));
                firstActivated = true;
            }

            if (hintedVersion.color.a >0)
                hintedVersion.color = FadeOutColor(hintedVersion.color);
            else
            {
                if (tatImage.color.a != 1)
                    tatImage.color = FadeInColor(tatImage.color, 1);
            }

        }

    }

    public static Color FadeInColor(Color c, float targetAlpha)
    {

        if (c.a < targetAlpha)
            c += new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, targetAlpha);
        return c;
    }

    public static Color FadeOutColor(Color c)
    {

        if (c.a > 0)
            c -= new Color(0, 0, 0, fadeSpeed) * Time.deltaTime;
        else
            c = new Color(c.r, c.g, c.b, 0);
        return c;
    }


}
