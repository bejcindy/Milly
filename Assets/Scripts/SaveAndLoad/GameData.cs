using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameData
{
    public bool round1FoodActive;
    public bool glassChangeLayer;
    public bool katieDollDiaDone;

    public Vector3 playerPosition;

    public SerializableDictionary<string, LivableValues> livableDict;
    public SerializableDictionary<string, bool> doorDict;
    public SerializableDictionary<string, bool> lidDict;

    public SerializableDictionary<string, bool> lookingDict;

    public SerializableDictionary<string, Vector3> pickupDict;
    public SerializableDictionary<string, bool> catFoodDict;
    public SerializableDictionary<string, bool> pizzaDict;

    public SerializableDictionary<string, bool> groupMasterDict;
    public SerializableDictionary<string, bool> buildingGroupControllerDict;

    public SerializableDictionary<string, bool> floorTileDict;
    public SerializableDictionary<string, bool> groundDirtDict;

    public SerializableDictionary<string, bool> passiveActivationDict;

    //Default values when there is no saved data
    public GameData()
    {
        playerPosition = new Vector3(-35.5f, 7.5f, 18.5f);
        
        livableDict = new SerializableDictionary<string, LivableValues>();
        doorDict = new SerializableDictionary<string, bool>();
        lidDict = new SerializableDictionary<string, bool>();

        lookingDict = new SerializableDictionary<string, bool>();

        pickupDict = new SerializableDictionary<string, Vector3>();
        catFoodDict = new SerializableDictionary<string, bool>();
        pizzaDict = new SerializableDictionary<string, bool>();

        groupMasterDict = new SerializableDictionary<string, bool>();
        buildingGroupControllerDict = new SerializableDictionary<string, bool>();
        
        floorTileDict = new SerializableDictionary<string, bool>();
        groundDirtDict = new SerializableDictionary<string, bool>();

        passiveActivationDict = new SerializableDictionary<string, bool>();
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

