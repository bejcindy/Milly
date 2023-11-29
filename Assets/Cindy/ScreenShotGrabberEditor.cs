using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ScreenShotGrabber))]
public class ScreenShotGrabberEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScreenShotGrabber grabber = (ScreenShotGrabber)target;
        grabber.path = EditorGUILayout.TextField("Folder Path", grabber.path);
        grabber.prefix = EditorGUILayout.TextField("Prefix for File Name", grabber.prefix);
        //grabber.sizeMultiplier = EditorGUILayout.IntField("Size Multiplier", grabber.sizeMultiplier);
        grabber.sizeMultiplier = EditorGUILayout.IntSlider("Size Multiplier", grabber.sizeMultiplier, 1, 10);
        if (GUILayout.Button("Take Screenshot"))
        {
            grabber.TakeScreenShot();
        }
    }
}