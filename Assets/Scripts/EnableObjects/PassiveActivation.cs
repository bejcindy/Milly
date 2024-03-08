using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveActivation : MonoBehaviour,ISaveSystem
{
    float matColorVal;
    float fadeInterval;
    public bool activated;
    bool hasGroupControl;
    GroupMaster groupControl;

    Material mat;
    string id;

    void Awake()
    {
        if (GetComponent<ObjectID>())
            id = GetComponent<ObjectID>().id;
        else
            Debug.LogError(gameObject.name + " doesn't have ObjectID Component.");
        mat = GetComponent<Renderer>().material;
    }

    void Start()
    {
        //matColorVal = 1;
        fadeInterval = 10;
        //mat.SetFloat("_WhiteDegree", matColorVal);
        if (GetComponent<GroupMaster>() != null)
        {
            hasGroupControl = true;
            groupControl = GetComponent<GroupMaster>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activated && !StartSequence.noControl)
        {
            TurnOnColor(mat);
            if (hasGroupControl)
            {
                groupControl.activateAll = true;
            }
        }
    }

    protected virtual void TurnOnColor(Material material)
    {
        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (material.HasFloat("_WhiteDegree"))
                material.SetFloat("_WhiteDegree", matColorVal);
        }
        else
        {
            matColorVal = 0;
            activated = false;
            this.enabled = false;
        }
    }

    public void LoadData(GameData data)
    {
        if (data.passiveActivationDict.TryGetValue(id, out bool savedActivated))
        {
            activated = savedActivated;
            if (activated)
            {
                matColorVal = 0;
                if (mat.HasFloat("_WhiteDegree"))
                    mat.SetFloat("_WhiteDegree", matColorVal);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (id == null)
            Debug.LogError(gameObject.name + " ID is null.");
        if (id == "")
            Debug.LogError(gameObject.name + " ID is empty.");
        if (data.passiveActivationDict.ContainsKey(id))
            data.passiveActivationDict.Remove(id);
        data.passiveActivationDict.Add(id, activated);
    }
}
