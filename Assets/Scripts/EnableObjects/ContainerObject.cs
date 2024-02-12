using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : LivableObject
{
    public string tagName;
    public List<string> tagList;
    public Vector3 containedPos;
    public bool table;

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

    protected override void Update()
    {
        if(!table)
            base.Update();
    }

    public bool CheckMatchingObject(GameObject obj)
    {
        if (isVisible)
        {
            if (tagList.Contains(obj.tag))
            {
                return true;
            }
            return false;
        }
        else
            return false;
    }

    public IEnumerator MoveAcceptedObject(Transform obj, float duration)
    {
        obj.SetParent(transform);
        float time = 0;
        Vector3 startPosition = obj.localPosition;
        Quaternion startRotation = obj.localRotation;
        while (time < duration)
        {
            time += Time.deltaTime/duration;
            obj.localPosition = Vector3.Lerp(startPosition, containedPos, Mathf.SmoothStep(0.0f, 1.0f, time));
            obj.localRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(Vector3.zero), time / duration);
            yield return null;
        }
        obj.localPosition = containedPos;
        obj.GetComponent<PickUpObject>().inHand = false;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        if (obj.GetComponent<Cigarette>())
            obj.GetComponent<Cigarette>().enabled = false;

        if(!StartSequence.noControl || overrideStartSequence)
            activated = true;
    }


}
