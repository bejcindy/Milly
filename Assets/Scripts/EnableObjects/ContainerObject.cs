using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : CollisionObject
{
    public Vector3 containedPos;
    public bool table;

    protected override void Update()
    {
        if(!table)
            base.Update();
    }

    public bool CheckMatchingObject(GameObject obj)
    {
        if (tagList.Contains(obj.tag))
        {
            return true;
        }
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
        activated = true;
    }
}
