using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GameData
{
    public bool round1FoodActive;
    public bool glassChangeLayer;
    public bool katieDollDiaDone;

    public bool soccerDumpsterOn;
    public int soccerScore;

    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public SerializableDictionary<string, LivableValues> livableDict;
    public SerializableDictionary<string, bool> doorDict;
    public SerializableDictionary<string, bool> lidDict;
    public SerializableDictionary<string, bool> windowDict;
    public SerializableDictionary<string, bool> benchDict;

    public SerializableDictionary<string, bool> lookingDict;

    public SerializableDictionary<string, PickUpValues> pickupDict;
    public SerializableDictionary<string, bool> catFoodDict;
    public SerializableDictionary<string, bool> pizzaDict;
    public SerializableDictionary<string, int> cigDict;

    public SerializableDictionary<string, bool> groupMasterDict;
    public SerializableDictionary<string, bool> buildingGroupControllerDict;


    public SerializableDictionary<string, NPCData> npcStage;
    public SerializableDictionary<string, bool> onOffState;

    public SerializableDictionary<string, bool> floorTileDict;
    public SerializableDictionary<string, bool> groundDirtDict;

    public SerializableDictionary<string, bool> passiveActivationDict;

    public GameObject vinylOnRecordPlayer;
    public SerializableDictionary<string, GameObject> vinylHolderDict;
    public SerializableDictionary<string, bool> vinylDict;

    //Default values when there is no saved data
    public GameData()
    {
        playerPosition = new Vector3(-35.5f, 7.5f, 18.5f);
        playerRotation = new Quaternion(0, 0.866f, 0, 0.5f);

        livableDict = new SerializableDictionary<string, LivableValues>();
        doorDict = new SerializableDictionary<string, bool>();
        lidDict = new SerializableDictionary<string, bool>();
        windowDict = new SerializableDictionary<string, bool>();
        benchDict = new SerializableDictionary<string, bool>();

        lookingDict = new SerializableDictionary<string, bool>();

        pickupDict = new SerializableDictionary<string, PickUpValues>();
        catFoodDict = new SerializableDictionary<string, bool>();
        pizzaDict = new SerializableDictionary<string, bool>();
        cigDict = new SerializableDictionary<string, int>();

        groupMasterDict = new SerializableDictionary<string, bool>();
        buildingGroupControllerDict = new SerializableDictionary<string, bool>();
        
        npcStage = new SerializableDictionary<string, NPCData>();
        
        floorTileDict = new SerializableDictionary<string, bool>();
        groundDirtDict = new SerializableDictionary<string, bool>();

        passiveActivationDict = new SerializableDictionary<string, bool>();

        vinylHolderDict = new SerializableDictionary<string, GameObject>();
        vinylDict = new SerializableDictionary<string, bool>();
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
public class PickUpValues
{
    public Vector3 pos;
    public bool dumped;
    public PhysicMaterial physicMat;
    public PickUpValues(Vector3 position, bool _dumped, PhysicMaterial _physicMat)
    {
        pos = position;
        dumped = _dumped;
        physicMat = _physicMat;
    }
}
