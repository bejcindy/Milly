using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour,ISaveSystem
{
    public List<GameObject> glasses;
    bool changeLayer;
    
    public void ChangeAllGlassLayers()
    {
        foreach(GameObject glass in glasses)
        {
            glass.layer = 17;
        }
        changeLayer = true;
    }

    public void LoadData(GameData data)
    {
        changeLayer = data.glassChangeLayer;
        if (changeLayer)
            ChangeAllGlassLayers();
    }

    public void SaveData(ref GameData data)
    {
        data.glassChangeLayer = changeLayer;
    }
}
