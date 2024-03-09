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

    //NPC
    public SerializableDictionary<string, NPCData> npcStage;
    public SerializableDictionary<string, bool> onOffState;
    public SerializableDictionary<string, bool> floorTileDict;
    public SerializableDictionary<string, bool> groundDirtDict;

    //Tattoo
    public SerializableDictionary<string, bool> tatSpaceDict;
    public SerializableDictionary<string, TattooData> tattooDict;
    public SerializableDictionary<string, TattooMenuData> tattooMenuDict;
    public SerializableDictionary<string, TatCharData> tatCharDict;

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
        doorDict = new SerializableDictionary<string, bool>();
        pizzaDict = new SerializableDictionary<string, bool>();
        catFoodDict = new SerializableDictionary<string, bool>();

        
        floorTileDict = new SerializableDictionary<string, bool>();
        groundDirtDict = new SerializableDictionary<string, bool>();

        passiveActivationDict = new SerializableDictionary<string, bool>();

        npcStage = new();
        onOffState = new();
        tatSpaceDict = new();
        tattooDict = new();
        tattooMenuDict = new();
        tatCharDict = new();
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

[System.Serializable]
public class TattooData
{
    public bool isDiscovered;
    public bool isActivated;
    public TattooData(bool _isDiscovered, bool _isActivated)
    {
        isDiscovered = _isDiscovered;
        isActivated = _isActivated;
    }
}

[System.Serializable]
public class TattooMenuData
{
    public bool isDiscovered;
    public bool isFinished;

    public TattooMenuData(bool _isDiscovered, bool _isFinished)
    {
        isDiscovered= _isDiscovered;
        isFinished= _isFinished;
    }
}

[System.Serializable]
public class TatCharData
{
    public int stage;
    public bool stageColored;

    public TatCharData(int stage, bool stageColored)
    {
        this.stage = stage;
        this.stageColored = stageColored;
    }
}







