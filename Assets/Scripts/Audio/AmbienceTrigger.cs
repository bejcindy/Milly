using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using FMOD;

public class AmbienceTrigger : MonoBehaviour
{

    [SerializeField]
    private EventReference SelectAudio;
    private EventInstance Audio;
    public float Volume;
    public bool playing;
    // Start is called before the first frame update
    void Start()
    {
        Audio = RuntimeManager.CreateInstance(SelectAudio);
        Volume = 0;
        Audio.setParameterByName("AmbienceVolume", Volume);
        RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (Volume < 1)
            {
                Volume += 0.5f* Time.deltaTime; 
                Audio.setParameterByName("AmbienceVolume", Volume);
            }
        }
        else
        {
            if (Volume > 0)
            {
                Volume -= 0.5f * Time.deltaTime;
                Audio.setParameterByName("AmbienceVolume", Volume);
            }
            else
            {
                Audio.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playing = true;
            Audio.start();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playing = false;
        }
    }
}
