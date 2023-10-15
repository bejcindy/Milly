using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggedVisibleDetector : MonoBehaviour
{
    public bool rigVisible;
    public bool isVisible;
    Renderer rend;
    bool[] checkBoundVisible;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        checkBoundVisible = new bool[8];
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
        if ((pointOnScreen.x > Screen.width * 0.2f) || (pointOnScreen.x < Screen.width * 0.8f) ||
               (pointOnScreen.y > Screen.height * 0.2f) || (pointOnScreen.y < Screen.height * 0.8f))
        {
            if (GetComponent<Renderer>() && GetComponent<Collider>())
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position - new Vector3(0, 0.1f, 0), GetComponent<Renderer>().bounds.center - (Camera.main.transform.position - new Vector3(0, 0.1f, 0)), out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    //if (hit.collider)
                    //    Debug.Log(gameObject.name+" raycast hit this: "+hit.collider.gameObject.name);
                    if (hit.collider.name != gameObject.name && !hit.collider.CompareTag("Player"))
                    {

                        return false;
                    }

                }
            }
            else if(GetComponent<Renderer>() && !GetComponent<Collider>())
            {
                return true;
            }
            
        }
        else
        {
            int pointsInScreen = 0;
            Vector3 pointA = rend.bounds.min;
            Vector3 pointB = rend.bounds.min + new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointC = rend.bounds.min + new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointD = rend.bounds.min + new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointE = rend.bounds.max - new Vector3(rend.bounds.size.x, 0, 0);
            Vector3 pointF = rend.bounds.max - new Vector3(0, rend.bounds.size.y, 0);
            Vector3 pointG = rend.bounds.max - new Vector3(0, 0, rend.bounds.size.z);
            Vector3 pointH = rend.bounds.max;


            checkBoundVisible[0] = CheckPointInView(pointA);
            checkBoundVisible[1] = CheckPointInView(pointB);
            checkBoundVisible[2] = CheckPointInView(pointC);
            checkBoundVisible[3] = CheckPointInView(pointD);
            checkBoundVisible[4] = CheckPointInView(pointE);
            checkBoundVisible[5] = CheckPointInView(pointF);
            checkBoundVisible[6] = CheckPointInView(pointG);
            checkBoundVisible[7] = CheckPointInView(pointH);

            for (int i = 0; i < checkBoundVisible.Length; i++)
            {
                if (checkBoundVisible[i])
                    pointsInScreen++;
                //else
                //    pointsInScreen--;
            }
            if (pointsInScreen < 2)
            {
                return false;
            }

        }
        
        return true;
    }
    bool CheckPointInView(Vector3 pointPos)
    {
        Vector3 pointOnScreen = Camera.main.WorldToScreenPoint(pointPos);
        if ((pointOnScreen.x < Screen.width * 0.05f) || (pointOnScreen.x > Screen.width * 0.95f) ||
           (pointOnScreen.y < Screen.height * 0.05f) || (pointOnScreen.y > Screen.height * 0.95f))
        {
            return false;
        }
        return true;
    }
}
