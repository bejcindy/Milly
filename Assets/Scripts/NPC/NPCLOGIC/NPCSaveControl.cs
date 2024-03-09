using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NPCSaveControl : MonoBehaviour, ISaveSystem
{
    public Transform npcListRef;
    public List<NPCControl> characters;
    public List<GameObject> tempChars;
    public static Dictionary<NPCControl, NPCData> npcActiveDict = new();




    void Awake()
    {

        foreach(NPCControl character in characters)
        {
            npcActiveDict.Add(character, new NPCData(character._counter, character.gameObject.activeSelf, character.colored));
        }
    }



    public virtual void SaveData(ref GameData data)
    {
        foreach(NPCControl character in characters)
        {
            string charID = character.GetComponent<ObjectID>().id;

            if (String.IsNullOrEmpty(charID))
                Debug.LogError(gameObject.name + " ID is null.");

            if (data.npcStage.ContainsKey(charID))
                data.npcStage.Remove(charID);
            data.npcStage.Add(charID, npcActiveDict[character]);
        }

        foreach(GameObject temp in tempChars)
        {
            string charID = temp.GetComponent<ObjectID>().id;

            if (String.IsNullOrEmpty(charID))
                Debug.LogError(gameObject.name + " ID is null.");

            if (data.onOffState.ContainsKey(charID))
                data.onOffState.Remove(charID);
            data.onOffState.Add(charID, temp.activeSelf);

        }
    }

    public virtual void LoadData(GameData data)
    {
        foreach(NPCControl character in characters)
        {
            string charID = character.GetComponent<ObjectID>().id;

            if (data.npcStage.TryGetValue(charID, out NPCData values))
            {
                character._counter = values.stage;
                character.gameObject.SetActive(values.activeNPC);
                character.colored = values.colored;
            }
        }

        foreach(GameObject temp in tempChars)
        {
            string charID = temp.GetComponent<ObjectID>().id;
            if (data.onOffState.TryGetValue(charID, out bool activeState))
            {
                gameObject.SetActive(activeState);
            }
        }

    }


}


[System.Serializable]
public class NPCData
{
    public int stage;
    public bool activeNPC;
    public bool colored;

    public NPCData(int counter, bool activeNPC, bool isColored)
    {
        stage = counter;
        this.activeNPC = activeNPC;
        this.colored = isColored;
    }
}
