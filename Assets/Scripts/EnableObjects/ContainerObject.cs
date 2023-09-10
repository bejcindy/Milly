using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : CollisionObject
{
    PlayerHolding playerHolding;
    public Vector3 containedPos;

    protected override void Start()
    {
        base.Start();
        playerHolding = player.GetComponent<PlayerHolding>();
    }

    protected override void Update()
    {
        base.Update();


    }

    public bool CheckMatchingObject(GameObject obj)
    {
        if (tagList.Contains(obj.tag)){
            Debug.Log("MATCHING TAG!");
            return true;
        }
        Debug.Log("NOT MATCHING TAG");
        return false;
    }

    public IEnumerator MoveAcceptedObject(Transform obj, float duration)
    {
        obj.SetParent(transform);
        float time = 0;
        Vector3 startPosition = obj.localPosition;
        while (time < duration)
        {
            time += Time.deltaTime/duration;
            obj.localPosition = Vector3.Lerp(startPosition, containedPos, Mathf.SmoothStep(0.0f, 1.0f, time));
            yield return null;
        }
        obj.localPosition = containedPos;
    }




}
