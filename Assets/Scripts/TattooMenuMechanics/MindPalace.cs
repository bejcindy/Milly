using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPalace : MonoBehaviour
{
    public static bool tatMenuOn;
    [TextArea]
    public string hint;

    // Start is called before the first frame update
    void Start()
    {
        tatMenuOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DataHolder.ShowHint(hint);
        
    }
}
