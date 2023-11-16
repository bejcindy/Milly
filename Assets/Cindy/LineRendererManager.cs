using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineRendererManager : MonoBehaviour
{
    public UILineRendererList UILRL;
    //public List<RectTransform> tatts;
    public List<Vector2> pts;
    // Start is called before the first frame update
    void Start()
    {
        UILRL.Points = pts;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
