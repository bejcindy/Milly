using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.UI;
public class CenterTattoo : MonoBehaviour
{
    public TattooPanel myPanel;
    Image lineImage;
    Image fullImage;
    bool firstActivated;
    [Foldout("Activation")]
    public bool noCharTat;
    public bool activated;
    public bool forceActivate;
    public bool transformed;
    public bool colorOn;
    bool focused;


    [Foldout("References")]

    RectTransform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        myPanel = transform.parent.GetComponent<TattooPanel>();
        myTransform = GetComponent<RectTransform>();
        lineImage = transform.GetChild(0).GetComponent<Image>();
        fullImage = GetComponent<Image>();

        lineImage.color = new Color(0, 0, 0, 0);
        fullImage.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (forceActivate)
        {
            if (!focused)
            {
                focused = true;
                myPanel.FocusOnTattoo(myTransform);
            }

            if(lineImage.color.a < 1)
                lineImage.color = Tattoo.FadeInColor(lineImage.color, 1);
            else
            {
                colorOn = true;
            }

            if (noCharTat)
                activated = true;
        }

        if (activated && !transformed)
        {
            if (!firstActivated)
            {
                firstActivated = true;
                myPanel.FocusOnTattoo(myTransform);

            }
            if (lineImage.color.a < 1)
                lineImage.color = Tattoo.FadeInColor(lineImage.color, 1);
            else
            {
                colorOn = true;
            }
        }

        if (transformed)
        {
            forceActivate = false;
            activated = false;
            if (lineImage.color.a > 0)
                lineImage.color = Tattoo.FadeOutColor(lineImage.color);
            else
            {
                if (fullImage.color.a < 1)
                {
                    fullImage.color = Tattoo.FadeInColor(fullImage.color, 1);
                    myPanel.noDrag = true;
                }
                else
                {
                    myPanel.noDrag = false;
                    this.enabled = false;
                }
            }
        }
    }


}
