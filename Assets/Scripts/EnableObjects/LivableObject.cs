using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] protected bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool hasParent;
    [SerializeField] protected float minDist;

    protected Material mat;
    [SerializeField] public bool activated;
    [SerializeField] protected bool firstActivated; 
    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;


    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_WhiteDegree");
        matColorVal = 1;
        if (transform.parent != null)
            hasParent = true;
    }

    protected virtual void Update()
    {
        DetectInteractable();
        isVisible = IsInView();
        if (activated)
        {
            if(!firstActivated)
                TurnOnColor(mat);

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

    protected virtual void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
            firstActivated = true;
            if (hasParent)
            {
                GroupMaster groupMaster = transform.parent.GetComponent<GroupMaster>();
                groupMaster.activateAll = true;
            }
        }
        
    }



    protected virtual bool IsInView()
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(GetComponent<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            Debug.Log("Behind: " + gameObject.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < Screen.width * 0.3f) || (pointOnScreen.x > Screen.width * 0.7f) ||
                (pointOnScreen.y < Screen.height * 0.3f) || (pointOnScreen.y > Screen.height * 0.7f))
        {
            Debug.Log("OutOfBounds: " + gameObject.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = gameObject.transform.position - Camera.main.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(Camera.main.transform.position, GetComponent<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.name != gameObject.name)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                Debug.Log(gameObject.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
    }
}
