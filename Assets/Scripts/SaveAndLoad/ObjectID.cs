using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectID : MonoBehaviour
{
    public string id;

    public void GenerateGuid()
    {        
        if (id == "")
            id = System.Guid.NewGuid().ToString();        
    }
}
