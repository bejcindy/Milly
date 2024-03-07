using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Destination : ScriptableObject
{
    public int talkTimes;
    public string idleTrigger;
    public string talkTrigger;
    public bool talkLockCam;
    public bool lookInTalk;

}
