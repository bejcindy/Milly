using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OuterPanelButton : MonoBehaviour
{
    public TattooPanel correspondingPanel;
    public CharacterPanel parentControl;

    private void Awake()
    {

    }

    public void OpenPanel()
    {
        parentControl.ChoosePanel(correspondingPanel);
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
