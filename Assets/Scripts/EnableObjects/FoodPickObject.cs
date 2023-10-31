using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FoodPickObject : MonoBehaviour
{
    public bool selected;
    public TableController myTable;
    PlayerLeftHand leftHand;

    public Vector3 inChopRot;
    public bool picked;
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

        if (selected && !picked)
        {
            gameObject.layer = 9;
            leftHand.selectedFood = this.transform;
        }
        else if(myTable.tableControlOn && !picked)
        {
            gameObject.layer = 17;
        }
        else if(picked)
        {
            gameObject.layer = 7;
            if(leftHand.selectedFood == this.transform)
            {
                RuntimeManager.PlayOneShot("event:/Sound Effects/ObjectInteraction/Chopsticks_PickFood", transform.position);
                leftHand.selectedFood = null;
            }
        }
        else
        {
            gameObject.layer = 0;
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
