using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TattooConnection : MonoBehaviour
{
    public List<RectTransform> relatedTattoos;
    public LivableObject actualObject;
    public NPCControl npcObject;
    public bool activated, related, objTransformed;


    private void Update()
    {
        if(activated && !objTransformed)
        {
            objTransformed = true;
            if(actualObject)
                actualObject.transformed = true;
            if (npcObject)
                npcObject.transformed = true;
        }
    }


}
