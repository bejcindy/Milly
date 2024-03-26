using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsePanelController : MonoBehaviour
{
    bool originalBypassThrow;

    private void OnEnable()
    {
        originalBypassThrow = ReferenceTool.playerLeftHand.bypassThrow;
        ReferenceTool.playerLeftHand.bypassThrow = true;
    }
    private void OnDisable()
    {
        ReferenceTool.playerLeftHand.bypassThrow = originalBypassThrow;
    }
}
