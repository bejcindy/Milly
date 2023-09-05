using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : LivableObject
{
    public string tagName;
    public List<string> tagList;

    public bool stairParent;
    public bool ashTray;

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
        if (ashTray)
        {
            if (other.gameObject.name.Contains("cigarette"))
            {
                activated = true;
            }
        }
    }
}
