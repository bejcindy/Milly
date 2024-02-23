using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HintColorControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Image>())
        {
            StartCoroutine(ChangeImageColor(GetComponent<Image>()));
        }
        else
        {
            StartCoroutine(ChangeImageColor(transform.GetChild(0).GetComponent<Image>()));
            StartCoroutine(ChangeTextColor(transform.GetChild(1).GetComponent<TextMeshProUGUI>()));
            StartCoroutine(ChangeTextColor(transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ChangeColor(Color c)
    {
        yield return new WaitForSeconds(.5f);
        c = new Color(c.r, c.g, c.b, 1);
    }
    IEnumerator ChangeImageColor(Image i)
    {
        yield return new WaitForSeconds(.1f);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    }
    IEnumerator ChangeTextColor(TextMeshProUGUI t)
    {
        yield return new WaitForSeconds(.1f);
        t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
    }
}
