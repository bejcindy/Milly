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
    Vector2 mouseUIScale = new Vector2(.3f, .3f);
    Image buttonImg;

    // Start is called before the first frame update
    void Start()
    {
        dataHolder = ReferenceTool.dataHolder;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        string buttonString = buttonText.text;
        string nonRichTextString = Regex.Replace(buttonString, "<.*?>", string.Empty);
        buttonImg = GetComponent<Image>();
        switch (nonRichTextString)
        {
            case "LeftClick":                
                buttonImg.sprite = dataHolder.LeftClick;
                ChangeValues();
                break;
            case "Click":                
                buttonImg.sprite = dataHolder.LeftClick;
                ChangeValues();
                break;
            case "RightClick":
                buttonImg.sprite = dataHolder.RightClick;
                ChangeValues();
                break;
            case "ScrollDown":
                buttonImg.sprite = dataHolder.ScrollDown;
                ChangeValues();
                break;
            case "ScrollUp":
                buttonImg.sprite = dataHolder.ScrollUp;
                ChangeValues();
                break;
            case "Scroll":
                buttonImg.sprite = dataHolder.Scroll;
                ChangeValues();
                break;
            case "Drag":
                buttonImg.sprite = dataHolder.Drag;
                ChangeValues();
                break;
        }
        buttonImg.SetNativeSize();
    }

    void ChangeValues()
    {
        buttonImg.type = Image.Type.Simple;
        transform.localScale = mouseUIScale;
        buttonText.text = null;
        transform.parent.GetComponent<HorizontalLayoutGroup>().padding.top = 0;
        transform.parent.GetComponent<HorizontalLayoutGroup>().padding.bottom = 0;
    }
}
