using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OuterPanelButton : MonoBehaviour
{
    public TattooPanel correspondingPanel;

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
    public void NoDragPanel()
    {
        parentControl.noDrag = true;
    }
    public void CanDragPanel()
    {
        parentControl.noDrag = false;
    }
}
