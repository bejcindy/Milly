using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAroundChildTrigger : MonoBehaviour
{
    public int childIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool triggered = transform.parent.GetComponent<WalkAroundDetecter>().triggers[childIndex];
            if (!triggered)
                transform.parent.GetComponent<WalkAroundDetecter>().triggers[childIndex] = true;
            else
            {
                for(int i=0;i< transform.parent.GetComponent<WalkAroundDetecter>().triggers.Length; i++)
                {
                    if (i != childIndex)
                        transform.parent.GetComponent<WalkAroundDetecter>().triggers[i] = false;
                    else
                        transform.parent.GetComponent<WalkAroundDetecter>().triggers[i] = true;
                }
                
            }

        }
    }
}
