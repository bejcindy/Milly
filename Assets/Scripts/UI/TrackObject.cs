using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackObject : MonoBehaviour
{
    public Sprite sprite;
    public GameObject trackThis;
    Image objectUI;
    RectTransform objectUIRect;
    RectTransform CanvasRect;
    // Start is called before the first frame update
    void Start()
    {
        objectUI = GetComponent<Image>();
        objectUI.sprite = sprite;
        objectUI.SetNativeSize();
        objectUIRect = GetComponent<RectTransform>();
        CanvasRect = transform.parent.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(trackThis.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        objectUIRect.anchoredPosition = WorldObject_ScreenPosition;
    }
}
