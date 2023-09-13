using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CollisionObject : LivableObject
{
    public string tagName;
    public List<string> tagList;

    public bool stairParent;
    public bool ashTray;
    public bool brokenAC;

    protected override void Start()
    {
        base.Start();
        if (tagName.Contains(","))
        {
            string[] tags = tagName.Split(",");
            for(int i = 0; i < tags.Length; i++)
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
        if (tagList.Contains(collision.gameObject.tag))
        {
            if(!stairParent)
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
                GetComponent<DialogueSystemTrigger>().enabled = true;
            }

            if (GetComponent<BarkOnIdle>())
            {
                GetComponent<BarkOnIdle>().enabled = true;
            }

        }
            
    }

    private void OnTriggerEnter(Collider other)
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
}
