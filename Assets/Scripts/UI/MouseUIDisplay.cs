using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MouseUIDisplay : MonoBehaviour
{
    TextMeshProUGUI buttonText;

    //Easy to Remember, no use
    //string[] mouseRelatedStrings = { "LeftClick", "Click", "RightClick", "ScrollDown", "ScrollUp", "Scroll", "Drag" };

    DataHolder dataHolder;
    // Start is called before the first frame update
    void Start()
    {
        dataHolder = ReferenceTool.dataHolder;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        string buttonString = buttonText.text;
        string nonRichTextString = Regex.Replace(buttonString, "<.*?>", string.Empty);
        Image buttonImg = GetComponent<Image>();
        switch (nonRichTextString)
        {
            case "LeftClick":
                buttonImg.sprite = dataHolder.LeftClick;
                buttonText.text = null;
                break;
            case "Click":
                buttonImg.sprite = dataHolder.LeftClick;
                buttonText.text = null;
                break;
            case "RightClick":
                buttonImg.sprite = dataHolder.RightClick;
                buttonText.text = null;
                break;
            case "ScrollDown":
                buttonImg.sprite = dataHolder.ScrollDown;
                buttonText.text = null;
                break;
            case "ScrollUp":
                buttonImg.sprite = dataHolder.ScrollUp;
                buttonText.text = null;
                break;
            case "Scroll":
                buttonImg.sprite = dataHolder.Scroll;
                buttonText.text = null;
                break;
            case "Drag":
                buttonImg.sprite = dataHolder.Drag;
                buttonText.text = null;
                break;
        }
        buttonImg.SetNativeSize();
    }
}
