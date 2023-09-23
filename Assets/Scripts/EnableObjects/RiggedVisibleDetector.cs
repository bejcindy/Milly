using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggedVisibleDetector : MonoBehaviour
{
    public bool rigVisible;
    public bool isVisible;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigVisible)
        {
            isVisible = IsInView();
        }
        else
        {
            isVisible = false;
        }
        //isVisible = rigVisible;
    }

    private void OnBecameVisible()
    {
        rigVisible = true;
    }

    private void OnBecameInvisible()
    {
        rigVisible=false;
    }


    protected virtual bool IsInView()
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(rend.bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {

            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < Screen.width * 0.2) || (pointOnScreen.x > Screen.width * 0.8f) ||
                (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
        {

            return false;
        }

        //RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position - new Vector3(0, 0.1f, 0), GetComponent<Renderer>().bounds.center - (Camera.main.transform.position - new Vector3(0, 0.1f, 0)), out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        //{
        //    //if (hit.collider)
        //    //    Debug.Log(gameObject.name+" raycast hit this: "+hit.collider.gameObject.name);
        //    if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
        //    {
        //        return false;
        //    }

        //}
        return true;
    }
}
