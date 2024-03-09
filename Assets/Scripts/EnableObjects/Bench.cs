using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : FixedCameraObject 
{
    public float sitTime;
    public bool npcBound;
    public NPCControl npcControl;
    public bool thoughtDone;
    
    protected override void Update()
    {
        base.Update();
        if (isInteracting && !thoughtDone)
        {
            DetectSitThought();
        }
        
    }

    void DetectSitThought()
    {
        if (sitTime > 0)
        {
            sitTime -= Time.deltaTime;
        }
        else
        {
            if (!npcBound)
            {
                GetComponent<DialogueSystemTrigger>().enabled = true;
            }

            thoughtDone = true;
        }
    }

    public override void LoadData(GameData data)
    {
        base.LoadData(data);
        if (data.benchDict.TryGetValue(id, out bool savedThoughtDone))        
            thoughtDone = savedThoughtDone;        
    }

    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);
        if (data.benchDict.ContainsKey(id))
            data.benchDict.Remove(id);        
        data.benchDict.Add(id, thoughtDone);
    }
}
