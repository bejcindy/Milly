using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    public SerializableDictionary<string, LivableValues> livableDict;
    public SerializableDictionary<string, bool> doorDict;

    public SerializableDictionary<string, Vector3> pickupDict;
    public SerializableDictionary<string, bool> catFoodDict;
    public SerializableDictionary<string, bool> pizzaDict;

    public SerializableDictionary<string, bool> groupMasterDict;
    public SerializableDictionary<string, bool> buildingGroupControllerDict;


    public SerializableDictionary<string, NPCData> npcStage;
    public SerializableDictionary<string, bool> onOffState;

    //Default values when there is no saved data
    public GameData()
    {
        playerPosition = new Vector3(-35.5f, 7.5f, 18.5f);
        pickupDict = new SerializableDictionary<string, Vector3>();
        livableDict = new SerializableDictionary<string, LivableValues>();
        groupMasterDict = new SerializableDictionary<string, bool>();
        buildingGroupControllerDict = new SerializableDictionary<string, bool>();
        doorDict = new SerializableDictionary<string, bool>();
        pizzaDict = new SerializableDictionary<string, bool>();
        catFoodDict = new SerializableDictionary<string, bool>();
        npcStage = new SerializableDictionary<string, NPCData>();
    }
    
}

[System.Serializable]
public class LivableValues
{
    public bool activated;
    public bool transformed;
    public LivableValues(bool _activated, bool _transformed)
    {
        activated = _activated;
        transformed = _transformed;
    }
}




