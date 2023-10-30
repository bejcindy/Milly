using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineActivate : MonoBehaviour
{
    public BuildingGroupController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller.activateAll = true;
    }
     
    // Update is called once per frame
    void Update()
    {
        
    }
}
