using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingObject : LivableObject
{

    protected override void Update()
    {
        base.Update();
        if (IsInView(gameObject))
        {
            activated = true;
        }
        else
        {
            activated = false;
        }
    }

    private bool IsInView(GameObject toCheck)
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(GetComponent<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < Screen.width*0.3f) || (pointOnScreen.x > Screen.width*0.7f) ||
                (pointOnScreen.y < Screen.height*0.3f) || (pointOnScreen.y > Screen.height*0.7f))
        {
            Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - Camera.main.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(Camera.main.transform.position, GetComponent<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.name != toCheck.name)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
    }
}
