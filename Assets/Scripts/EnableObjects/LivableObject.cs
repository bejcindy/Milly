using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] protected bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected float minDist;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DetectInteractable();
    }

    protected virtual void DetectInteractable()
    {
        if (Vector3.Distance(transform.position, player.position) <= minDist)
        {
            if (isVisible)
                interactable = true;
            else
                interactable = false;
        }
        else
        {
            interactable = false;
        }
    }



    protected virtual void OnBecameVisible()
    {
        isVisible = true;
    }

    protected virtual void OnBecameInvisible()
    {
        isVisible=false;
    }
}
