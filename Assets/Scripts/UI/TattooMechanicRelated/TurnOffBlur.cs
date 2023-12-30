using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffBlur : MonoBehaviour
{
    public GameObject blurCanvas;
    public void TurnItOff()
    {
        blurCanvas.SetActive(false);
    }
    public void TurnItOn()
    {
        blurCanvas.SetActive(true);
    }
}
