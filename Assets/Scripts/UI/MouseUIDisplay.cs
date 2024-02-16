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
    Vector2 mouseUIScale = new Vector2(.4f, .4f);
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
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.LeftClick;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "Click":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.LeftClick;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "RightClick":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.RightClick;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "ScrollDown":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.ScrollDown;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "ScrollUp":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.ScrollUp;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "Scroll":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.Scroll;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
            case "Drag":
                buttonImg.type = Image.Type.Simple;
                buttonImg.sprite = dataHolder.Drag;
                transform.localScale = mouseUIScale;
                buttonText.text = null;
                break;
        }
        buttonImg.SetNativeSize();
    }
}
