using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkTrigger : MonoBehaviour
{
    public bool npcTrigger;

    public bool npcActivated;

    LivableObject activateTrigger;
    public BarkOnIdle bark;
    DialogueSystemTrigger hitDialogue;
    // Start is called before the first frame update
    void Start()
    {
        if (!npcTrigger)
        {
            bark = GetComponent<BarkOnIdle>();
            activateTrigger = GetComponent<LivableObject>();
        }
        else
        {
            hitDialogue = GetComponent<DialogueSystemTrigger>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Throwable"))
        {
            if (!npcTrigger)
            {
                activateTrigger.activated = true;
                bark.enabled = true;
            }
            else
            {
                hitDialogue.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {
            if (!npcTrigger)
            {
                activateTrigger.activated = true;
                bark.enabled = true;
            }
            else
            {
                hitDialogue.enabled = true;
                npcActivated = true;
            }
        }
    }
}
