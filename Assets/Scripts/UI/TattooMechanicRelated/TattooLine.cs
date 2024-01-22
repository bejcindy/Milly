using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class TattooLine : MonoBehaviour
{
    public Tattoo startTat;
    public Tattoo endTat;
    public TattooPanel myPanel;
    public bool centerLine;
    public bool halfActivated;
    public bool activated;
    [SerializeField] float halfOnColor;
    public VisualEffect vfx;
    bool playedBlackVFX, playedGreyVFX, VFXPlaying;
    Image lineImage;
    CenterTattoo centerTat;
    float timer;
    void Start()
    {
        myPanel = transform.parent.GetComponent<TattooPanel>();
        centerTat = myPanel.centerTat;
        lineImage = GetComponent<Image>();
        lineImage.color = new Color(0, 0, 0, 0);
    }


    void Update()
    {
        if (centerLine)
        {
            if (centerTat.forceActivate)
            {
                if (endTat.activated)
                {
                    ShowFullLine();
                }
            }
            if (centerTat.activated)
            {
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

            if (timer < .5f)
            {
                if (!playedGreyVFX)
                {
                    vfx.SetVector4("MainColor", new Color(150,150, 150, 0));
                    vfx.Play();
                    VFXPlaying = true;
                    playedGreyVFX = true;
                }
                timer += Time.deltaTime;
            }
            else if (timer < 1f)
            {
                if (VFXPlaying)
                {
                    vfx.Stop();
                    VFXPlaying = false;
                }
                timer += Time.deltaTime;
            }
            else
            {
                lineImage.color = Tattoo.FadeInColor(lineImage.color, halfOnColor);

            }
        }
        else
        {
            halfActivated = true;
            timer = 0;
            if (centerLine)
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
            if (timer < .5f)
            {
                if (!playedBlackVFX)
                {
                    vfx.SetVector4("MainColor", Color.black);
                    vfx.Play();
                    VFXPlaying = true;
                    playedBlackVFX = true;
                }
                timer += Time.deltaTime;
            }
            else if (timer < 1f)
            {
                if (VFXPlaying)
                {
                    vfx.Stop();
                    VFXPlaying = false;
                }
                timer += Time.deltaTime;
            }
            else
            {
                lineImage.color = Tattoo.FadeInColor(lineImage.color, 1);
            }
        }
        else
        {
            activated = true;
            timer = 0;
            if (centerLine)
                endTat.isShown = true;
            else
            {
                startTat.isShown = true;
                endTat.isShown = true;
            }
        }

    }
}
