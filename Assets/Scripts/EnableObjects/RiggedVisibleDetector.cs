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
        //if (rigVisible)
        //{
        //    isVisible = IsInView();
        //}
        //else
        //{
        //    isVisible = false;
        //}
        isVisible = rigVisible;
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
            if (gameObject.name.Contains("pizza"))
                Debug.Log("Behind: " + gameObject.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < Screen.width * 0.2) || (pointOnScreen.x > Screen.width * 0.8f) ||
                (pointOnScreen.y < Screen.height * 0.2f) || (pointOnScreen.y > Screen.height * 0.8f))
        {
            if (gameObject.name.Contains("pizza"))
                Debug.Log("OutOfBounds: " + gameObject.name);
            return false;
        }

        //RaycastHit hit;
        //Vector3 heading = gameObject.transform.position - Camera.main.transform.position;
        //Vector3 direction = heading.normalized;// / heading.magnitude;

        //if (Physics.Linecast(Camera.main.transform.position, transform.position, out hit))
        //{
        //    if (hit.transform.name != gameObject.name && !hit.transform.name.Contains("Player"))
        //    {
        //        /* -->
        //        Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
        //        Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
        //        */
        //        if(gameObject.name.Contains("Charles"))
        //            Debug.Log(gameObject.name + " occluded by " + hit.transform.name);
        //        return false;
        //    }
        //}
        return true;
    }
}
