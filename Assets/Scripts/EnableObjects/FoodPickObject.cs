using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickObject : MonoBehaviour
{
    public bool selected;
    PlayerLeftHand leftHand;

    public Vector3 inChopRot;
    // Start is called before the first frame update
    void Start()
    {
        leftHand = GameObject.Find("Player").GetComponent<PlayerLeftHand>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHand.chopAiming)
        {
            DetectChopPick();
        }
        else
        {
            selected = false;
        }

        if (selected)
        {
            gameObject.layer = 9;
            leftHand.selectedFood = this.transform;
        }

        else
        {
            gameObject.layer = 0;
            if(leftHand.selectedFood == this.transform)
            {
                leftHand.selectedFood = null;
            }
        }

    }

    public void DetectChopPick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == this.name)
            {
                selected = true;
            }
            else
            {
                selected = false;
            }
        }
    }

    public IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;

    }

    public IEnumerator LerpRotation(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.localRotation;
        while (time < duration)
        {
            transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endValue;
    }


}
