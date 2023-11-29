using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;

public class ScreenShotGrabber : MonoBehaviour
{
    public string path;
    public string prefix;
    public int sizeMultiplier = 1;
    public void TakeScreenShot()
    {
        string realPath;
        realPath = path;
        if (prefix != null)
            realPath += "/"+prefix + " ";
        else
            realPath += "/screenshot ";
        //path += System.Guid.NewGuid().ToString() + ".png";
        string date = System.DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "-");
        realPath += date + ".png";

        ScreenCapture.CaptureScreenshot(realPath, sizeMultiplier);
    }
}
