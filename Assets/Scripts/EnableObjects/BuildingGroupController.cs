using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGroupController : MonoBehaviour,ISaveSystem
{
    public bool activateAll;
    public bool groupControl;
    public float groupColorVal;
    public float fadeInterval;

    public BuildingGroupController bgc;
    public BuildingGroupController targetBgc;

    public CharacterTattoo myTat;

    string id;

    private void Awake()
    {
        if (GetComponent<ObjectID>())
            id = GetComponent<ObjectID>().id;
        else
            Debug.LogError(gameObject.name + " doesn't have ObjectID Component.");
    }

    void Start()
    {
        groupColorVal = 1;
    }

    void Update()
    {
        if (activateAll)
        {
            gameObject.layer = 17;
            if (groupColorVal > 0)
                groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            else
            {
                if (myTat)
                {
                    myTat.triggered = true;
                }
                groupColorVal = 0;
                activateAll = false;
                enabled = false;
            }

            ActivateAll(transform);

            if (groupControl)
            {
                if (bgc.activateAll)
                {
                    targetBgc.activateAll = true;
                }
            }
        }
    }

    public void Activate()
    {
        activateAll = true;
    }

    void ActivateAll(Transform obj)
    {
        if(obj.GetComponent<Renderer>()!= null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") >= 0)
                    TurnOnColor(mat);
            }
        }

        if (obj.GetComponent<LivableObject>() != null)
            obj.GetComponent<LivableObject>().activated = true;

        foreach(Transform child in obj)
        {
            child.gameObject.layer = 17;
            if (child.GetComponent<LivableObject>() != null)
                child.GetComponent<LivableObject>().activated = true;

            if (child.childCount <= 0 && child.GetComponent<Renderer>()!=null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") >= 0)
                        TurnOnColor(childMat);
                }
            }
            else
            {
                ActivateAll(child);
            }
        }
    }

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", groupColorVal);
    }

    void LoadActivateAll(Transform obj)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Material mat = obj.GetComponent<Renderer>().material;
            if (mat.HasProperty("_WhiteDegree"))
            {
                mat.EnableKeyword("_WhiteDegree");
                if (mat.GetFloat("_WhiteDegree") >= 0)
                    mat.SetFloat("_WhiteDegree", 0);
            }
        }

        if (obj.GetComponent<LivableObject>() != null)
            obj.GetComponent<LivableObject>().activated = true;

        foreach (Transform child in obj)
        {
            child.gameObject.layer = 17;
            if (child.GetComponent<LivableObject>() != null)
                child.GetComponent<LivableObject>().activated = true;

            if (child.childCount <= 0 && child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") >= 0)
                        childMat.SetFloat("_WhiteDegree", 0);
                }
            }
            else
            {
                LoadActivateAll(child);
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data.groupMasterDict.TryGetValue(id, out bool activate))
        {
            activateAll = activate;
            if (activateAll)
            {
                gameObject.layer = 17;                
                groupColorVal = 0;
                activateAll = false;
                enabled = false;                

                LoadActivateAll(transform);                
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (id == null)
            Debug.LogError(gameObject.name + " ID is null.");
        if (id == "")
            Debug.LogError(gameObject.name + " ID is empty.");
        if (data.buildingGroupControllerDict.ContainsKey(id))
        {
            data.buildingGroupControllerDict.Remove(id);
        }
        data.buildingGroupControllerDict.Add(id, activateAll);
    }
}
