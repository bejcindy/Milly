using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("DisplayText")]
public class DisplayText : MonoBehaviour
{

    public float delay = .05f;
    public float disappearDelay = 2f;
    //public bool show;
    string currentText = "";

    public string txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>().text;
        GetComponent<TextMeshProUGUI>().text = currentText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowText()
    {
        GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < txt.Length+1; i++)
        {
            currentText = txt.Substring(0, i);
            GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(disappearDelay);
        GetComponent<TextMeshProUGUI>().text = "";
    }
}
