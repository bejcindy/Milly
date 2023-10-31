using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public GameObject hallwayCutColored;
    public GameObject hallwayCutMono;
    public GameObject skyCut;
    public GameObject entranceCut;
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
            if(!hallwayCutColored.activeSelf)
                hallwayCutColored.SetActive(true);
            else
            {
                hallwayCutColored.GetComponent<PlayableDirector>().Play();
            }
            hallwayCutMono.GetComponent<PlayableDirector>().Stop();
            skyCut.GetComponent<PlayableDirector>().Stop();
            entranceCut.GetComponent<PlayableDirector>().Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!hallwayCutMono.activeSelf)
                hallwayCutMono.SetActive(true);
            else
                hallwayCutMono.GetComponent<PlayableDirector>().Play();

            hallwayCutColored.GetComponent<PlayableDirector>().Stop();
            skyCut.GetComponent<PlayableDirector>().Stop();
            entranceCut.GetComponent<PlayableDirector>().Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!skyCut.activeSelf)
                skyCut.SetActive(true);
            else
                skyCut.GetComponent<PlayableDirector>().Play();

            hallwayCutColored.GetComponent<PlayableDirector>().Stop();
            hallwayCutMono.GetComponent<PlayableDirector>().Stop();
            entranceCut.GetComponent<PlayableDirector>().Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!entranceCut.activeSelf)
                entranceCut.SetActive(true);
            else
                entranceCut.GetComponent<PlayableDirector>().Play();

            hallwayCutMono.GetComponent<PlayableDirector>().Stop();
            hallwayCutColored.GetComponent<PlayableDirector>().Stop();
            skyCut.GetComponent<PlayableDirector>().Stop();
        }
    }
}
