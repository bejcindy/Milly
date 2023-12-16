using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineRendererManager : MonoBehaviour
{
    public UILineRendererList UILRL;
    public UILineRendererList GreyUILRL;
    //public List<RectTransform> tatts;
    public List<Vector2> pts;
    public List<Vector2> greyPts;
    // Start is called before the first frame update
    void Start()
    {
        //UILRL.Points = pts;
    }

    // Update is called once per frame
    void Update()
    {
        UILRL.Points = pts;
        GreyUILRL.Points = greyPts;
        if (pts.Count == 0)
            UILRL.enabled = false;
        else
            UILRL.enabled = true;
        if (greyPts.Count == 0)
            GreyUILRL.enabled = false;
        else
            GreyUILRL.enabled = true;
    }
}
