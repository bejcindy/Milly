using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] protected bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected float minDist;

    protected Material mat;
    [SerializeField] protected bool activated;
    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;


    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_WhiteDegree");
        matColorVal = 1;
    }

    protected virtual void Update()
    {
        DetectInteractable();
        if (activated)
        {
            TurnOn();
        }
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

    protected virtual void TurnOn()
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            mat.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
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
