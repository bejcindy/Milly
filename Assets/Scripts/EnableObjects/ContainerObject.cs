using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : LivableObject
{
    public bool foodContainer;

    private void OnTriggerEnter(Collider other)
    {
        if (foodContainer)
        {
            if (other.name.Contains("pizza"))
            {
                activated = true;
            }
        }
    }


}
