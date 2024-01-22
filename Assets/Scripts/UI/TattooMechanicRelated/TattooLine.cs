using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TattooLine : MonoBehaviour
{
    public Tattoo startTat;
    public Tattoo endTat;
    public TattooPanel myPanel;
    public bool centerLine;
    public bool halfActivated;
    public bool activated;

    [SerializeField] float halfOnColor;
    Image lineImage;
    CenterTattoo centerTat;
    void Start()
    {
        myPanel=transform.parent.GetComponent<TattooPanel>();
        centerTat = myPanel.centerTat;
        lineImage = GetComponent<Image>();
        lineImage.color = new Color(0, 0, 0, 0);
    }


    void Update()
    {
        if(centerLine)
        {
            if (centerTat.forceActivate)
            {
                if (endTat.activated)
                {
                    ShowFullLine();
                }
            }
            if(centerTat.activated){
                if (endTat.activated)
                {
                    ShowFullLine();
                }
                else
                {
                    ShowHalfLine();
                }
            }

        }
        else
        {
            if (startTat.activated || endTat.activated)
            {
                if (startTat.isShown && endTat.isShown)
                {
                    ShowFullLine();
                }
                else if ((startTat.isHinted || endTat.isHinted) && (!startTat.hidden && !endTat.hidden))
                {
                    ShowHalfLine();
                    if (startTat.activated)
                        startTat.isShown = true;
                    if (endTat.activated)
                        endTat.isShown = true;
                }

            }
        }


    }


    void ShowHalfLine()
    {

        if (lineImage.color.a != halfOnColor)
        {
            myPanel.mainTattooMenu.lerping = true;
            lineImage.color = Tattoo.FadeInColor(lineImage.color, halfOnColor);
        }
        else
        {
            halfActivated = true;
            if(centerLine)
                endTat.isHinted = true;
            else
            {
                startTat.isHinted = true;
                endTat.isHinted = true;
            }
        }

    }

    void ShowFullLine()
    {
        if (lineImage.color.a != 1)
        {
            myPanel.mainTattooMenu.lerping = true;
            lineImage.color = Tattoo.FadeInColor(lineImage.color, 1);
        }
        else
        {
            activated = true;
            if(centerLine)
                endTat.isShown = true;
            else
            {
                startTat.isShown = true;
                endTat.isShown = true;
            }
        }

    }
}
