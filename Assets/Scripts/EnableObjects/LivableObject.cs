using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LivableObject : MonoBehaviour
{
    protected Transform player;
    [SerializeField] protected bool interactable;
    [SerializeField] protected bool isVisible;
    [SerializeField] protected bool chainControl;
    [SerializeField] protected float minDist;


    protected Material mat;
    [SerializeField] protected Renderer rend;
    [SerializeField] public bool activated;
    [SerializeField] public bool firstActivated; 
    [SerializeField] protected float matColorVal;
    [SerializeField] protected float fadeInterval;

    [Header("Effects")]
    [SerializeField] protected GameObject specialEffect;
    [SerializeField] protected GameObject postProcessingVolume;

    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        postProcessingVolume = GameObject.Find("GlowVolume");
        if (GetComponent<Renderer>() != null)
        {
            rend = GetComponent<Renderer>();
        }
        mat = rend.material;
        mat.EnableKeyword("_WhiteDegree");
        matColorVal = 1;
    }

    protected virtual void Update()
    {
        DetectInteractable();
        matColorVal = mat.GetFloat("_WhiteDegree");
        isVisible = IsInView();
        if (activated)
        {
            if(!firstActivated)
                TurnOnColor(mat);
            if (chainControl)
                GetComponent<GroupMaster>().activateAll = true;
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
            if (specialEffect != null)
                specialEffect.SetActive(true);
        }


    }

    float minBlur = .1f;
    float maxBlur = .75f;
    float focusDist = .75f;


    protected virtual bool IsInView()
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            //Debug.Log("Behind: " + gameObject.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < Screen.width * 0.3f) || (pointOnScreen.x > Screen.width * 0.7f) ||
                (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
        {
            //Debug.Log("OutOfBounds: " + gameObject.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = gameObject.transform.position - Camera.main.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(Camera.main.transform.position, rend.bounds.center, out hit))
        {
            if (hit.transform.name != gameObject.name && hit.transform.name != "Player" && !hit.transform.name.Contains("Trigger")
                && hit.transform.gameObject.layer !=6)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                //Debug.Log(gameObject.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
    }
}
