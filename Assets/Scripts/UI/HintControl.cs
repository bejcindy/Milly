using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintControl : MonoBehaviour
{
    public GameObject hintText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHintText(string newText)
    {
        hintText.GetComponent<TextMeshProUGUI>().text = newText;
    }
}
