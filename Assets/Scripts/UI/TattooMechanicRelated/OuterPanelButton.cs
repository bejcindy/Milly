using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OuterPanelButton : MonoBehaviour
{
    public PannelController correspondingPanel;

    OuterPanelController parentControl;

    private void Awake()
    {
        parentControl = GetComponentInParent<OuterPanelController>();
    }

    public void OpenPanel()
    {
        parentControl.zoomIn = true;
        parentControl.currentPanel = correspondingPanel;
    }
}
