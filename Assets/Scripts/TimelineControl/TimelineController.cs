using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    public GameObject hallwayCutColored;
    public GameObject hallwayCutMono;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CutsceneSwitch();
    }

    public void CutsceneSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hallwayCutColored.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hallwayCutMono.SetActive(true);
        }
    }
}
