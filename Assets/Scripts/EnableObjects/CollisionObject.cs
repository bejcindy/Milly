using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using FMODUnity;

public class CollisionObject : LivableObject
{
    public string tagName;
    public List<string> tagList;

    public bool stairParent;
    public bool brokenAC;
    public string eventName;

    public bool assistant;
    public EventReference collisionSound;
    protected override void Start()
    {
        base.Start();
        if (tagName.Contains(","))
        {
            string[] tags = tagName.Split(",");
            for (int i = 0; i < tags.Length; i++)
            {
                tagList.Add(tags[i]);
            }
        }
        else
        {
            tagList.Add(tagName);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //stop the annoying collision sound from lanterns bumping into the roof
        if (gameObject.name.Contains("lantern") && collision.gameObject.name.Contains("roof"))
            return;

        if(!StartSequence.noControl || overrideStartSequence)
        {
            if (tagList.Contains(collision.gameObject.tag) || (collision.gameObject.GetComponent<PickUpObject>() && collision.gameObject.GetComponent<PickUpObject>().thrownByPlayer))
            {
                if (!stairParent)
                    activated = true;
                else
                {
                    Material parentMat = transform.parent.GetComponent<Renderer>().material;
                    parentMat.EnableKeyword("_WhiteDegree");
                    TurnOnColor(parentMat);
                }

                if (gameObject.CompareTag("Ladder"))
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                }

                if (brokenAC)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    if (GetComponent<DialogueSystemTrigger>())
                        GetComponent<DialogueSystemTrigger>().enabled = true;
                }
            }
        }

        if (!collisionSound.IsNull && DataHolder.canMakeSound)
            RuntimeManager.PlayOneShot(collisionSound, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!StartSequence.noControl || overrideStartSequence)
        {
            if (!assistant)
            {
                if (other.CompareTag("Player"))
                {
                    activated = true;
                }
                if (tagList.Contains(other.gameObject.tag))
                {
                    activated = true;
                }
            }
            else
            {
                transform.parent.GetComponent<LivableObject>().activated = true;
            }
        }


    }
}
