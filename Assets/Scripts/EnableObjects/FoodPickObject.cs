using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickObject : MonoBehaviour
{
    public bool selected;
    PlayerLeftHand leftHand;
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
            gameObject.layer = 9;
        else
            gameObject.layer = 0;
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


}
