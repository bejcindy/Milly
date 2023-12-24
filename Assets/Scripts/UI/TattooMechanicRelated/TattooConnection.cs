using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TattooConnection : MonoBehaviour
{
    public List<RectTransform> relatedTattoos;
    public LivableObject actualObject;
    public NPCControl npcObject;
    public bool activated, related, hidden, objTransformed, showCenterTat;
    bool triggeredParent;

    private void Update()
    {
        //if(activated && !objTransformed)
        //{
        //    objTransformed = true;
        //    if(actualObject)
        //        actualObject.transformed = true;
        //    if (npcObject)
        //        npcObject.transformed = true;
        //}
        if (!triggeredParent && activated)
        {
            GetComponentInParent<PannelController>().activated = true;            
            GetComponentInParent<PannelController>().currentTattoo = GetComponent<RectTransform>();
            GetComponentInParent<PannelController>().enabled = true;
            triggeredParent = true;
        }

    }
    public void ShowCenterTattoo()
    {
        showCenterTat = true;
        related = true;
        GetComponentInParent<PannelController>().activated = true;
        GetComponentInParent<PannelController>().currentTattoo = GetComponent<RectTransform>();
        GetComponentInParent<PannelController>().enabled = true;
    }
}
