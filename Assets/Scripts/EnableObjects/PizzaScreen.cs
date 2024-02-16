using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PizzaScreen : MonoBehaviour
{
    public SpriteRenderer currentImage;
    SpriteRenderer previousImage;
    public Light lidLight;
    public float lightIntensity;

    bool showingLight;
    public bool showing;
    public bool finished;
    public int stage;

    float alpha;
    float lutDuration = 60f;
    bool lutBack;
    // Start is called before the first frame update
    void Start()
    {
        currentImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
        alpha = 0;
        showingLight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (showingLight)
        {
            if(lidLight.intensity < lightIntensity)
            {
                lidLight.intensity += Time.deltaTime * .1f;
            }
            else
            {
                showingLight = false;
            }
        }

        if (showing)
        {
            ShowSlide();
        }

        if(finished)
        {

            Invoke(nameof(FadeLastImage), 15f);
            if (lutDuration > 0)
            {
                lutDuration -= Time.deltaTime;
            }
            else
            {
                if (!lutBack)
                {
                    lutBack = true;
                    StartCoroutine(ReferenceTool.postProcessingAdjust.LerpToDefaultColor());
                }

            }
            

        }

    }

    public void ShowSlide()
    {
        if (currentImage.color.a < 1)
        {
            alpha += Time.deltaTime;
            Color currentAlpha = currentImage.color;
            currentAlpha.a = alpha;
            currentImage.color = currentAlpha;

            if (previousImage != null)
            {
                Color previousAlpha = previousImage.color;
                previousAlpha.a = 1-alpha;
                previousImage.color = previousAlpha;
            }
        }
        else
        {
            previousImage = currentImage;
            alpha = 0;
            showing = false;
            if(stage < transform.childCount-1)
            {
                stage++;
                currentImage = transform.GetChild(stage).GetComponent<SpriteRenderer>();
            }
            else
            {
                finished = true;
            }
        }
    }

    void FadeLastImage()
    {
        if (currentImage.color.a > 0)
        {
            alpha += Time.deltaTime;
            Color currentAlpha = currentImage.color;
            currentAlpha.a = 1-alpha;
            currentImage.color = currentAlpha;
        }


    }


}
