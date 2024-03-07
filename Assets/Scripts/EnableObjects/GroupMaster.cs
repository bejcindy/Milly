using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMaster : MonoBehaviour,ISaveSystem
{
    public List<Transform> chainTriggers;
    public bool activateAll;
    public float groupColorVal;
    public float fadeInterval;

    public bool designatedGroup;
    public bool childToChildren;
    public bool buildingControl;
    public bool delayedIgnition;

    public Transform delayedGroup;
    public BuildingGroupController bgc;

    string id;

    void Awake()
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
            if(groupColorVal > 0)
            {
                groupColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            }
            else
            {
                groupColorVal = 0;
                enabled = false;
            }
            ActivateAll();
        }
    }

    void ActivateAll()
    {
        if (designatedGroup)
        {
            foreach (Transform child in chainTriggers)
            {
                ActivateGroup(child);
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
            }
        }

        if (childToChildren)
        {
            foreach(Transform child in transform.parent)
            {
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
                if (child.GetComponent<Renderer>()!=null && child.GetComponent<SpriteRenderer>() == null)
                    ActivateGroup(child);
            }
        }

        if (buildingControl)
        {
            bgc.activateAll = true; 
        }

        if (delayedIgnition)
        {
            StartCoroutine(DelayedActivate());
        }
    }

    IEnumerator DelayedActivate()
    {
        foreach(Transform child in chainTriggers)
        {
            child.GetComponent<LivableObject>().activated = true;
            child.gameObject.layer = 17;
            yield return new WaitForSeconds(1.0f);
        }
    }

    void ActivateGroup(Transform child)
    {
        
        if (child.GetComponent<LivableObject>() == null)
        {
            if(child.GetComponent<Renderer>()!= null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") > 0)
                    {
                        TurnOnColor(childMat);
                    }
                }

                if (child.GetComponent<Renderer>().materials.Length > 1)
                {
                    foreach(Material mat in child.GetComponent<Renderer>().materials)
                    {
                        if (mat.HasProperty("_WhiteDegree")){
                            mat.EnableKeyword("_WhiteDegree");
                            TurnOnColor(mat);
                        }
                    }
                }
            }
        }
        else
        {
            if (!child.GetComponent<LivableObject>().activated)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Material childMat = child.GetComponent<Renderer>().material;
                    if (childMat.HasProperty("_WhiteDegree"))
                    {
                        childMat.EnableKeyword("_WhiteDegree");
                        if (childMat.GetFloat("_WhiteDegree") > 0)
                        {
                            TurnOnColor(childMat);
                        }
                    }
                    child.GetComponent<LivableObject>().activated = true;
                }

            }
        }
    }

    void TurnOnColor(Material material)
    {
        material.SetFloat("_WhiteDegree", groupColorVal);

    }

    void LoadActivateAll()
    {
        if (designatedGroup)
        {
            foreach (Transform child in chainTriggers)
            {
                LoadActivateGroup(child);
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
            }
        }

        if (childToChildren)
        {
            foreach (Transform child in transform.parent)
            {
                if (child.gameObject.layer == 6 || child.gameObject.layer == 18)
                    child.gameObject.layer = 18;
                else
                    child.gameObject.layer = 17;
                if (child.GetComponent<Renderer>() != null && child.GetComponent<SpriteRenderer>() == null)
                    LoadActivateGroup(child);
            }
        }

        if (buildingControl)
        {
            bgc.activateAll = true;
        }

        if (delayedIgnition)
        {
            foreach (Transform child in chainTriggers)
            {
                child.GetComponent<LivableObject>().activated = true;
                child.gameObject.layer = 17;
                if (child.GetComponent<Renderer>() != null)
                {
                    Material childMat = child.GetComponent<Renderer>().material;
                    if (childMat.HasProperty("_WhiteDegree"))
                    {
                        childMat.EnableKeyword("_WhiteDegree");
                        if (childMat.GetFloat("_WhiteDegree") > 0)
                        {
                            childMat.SetFloat("_WhiteDegree", 0);
                        }
                    }

                    if (child.GetComponent<Renderer>().materials.Length > 1)
                    {
                        foreach (Material mat in child.GetComponent<Renderer>().materials)
                        {
                            if (mat.HasProperty("_WhiteDegree"))
                            {
                                mat.EnableKeyword("_WhiteDegree");
                                mat.SetFloat("_WhiteDegree", 0);
                            }
                        }
                    }
                }
            }
        }
    }

    void LoadActivateGroup(Transform child)
    {

        if (child.GetComponent<LivableObject>() == null)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                Material childMat = child.GetComponent<Renderer>().material;
                if (childMat.HasProperty("_WhiteDegree"))
                {
                    childMat.EnableKeyword("_WhiteDegree");
                    if (childMat.GetFloat("_WhiteDegree") > 0)
                    {
                        childMat.SetFloat("_WhiteDegree", 0);
                    }
                }

                if (child.GetComponent<Renderer>().materials.Length > 1)
                {
                    foreach (Material mat in child.GetComponent<Renderer>().materials)
                    {
                        if (mat.HasProperty("_WhiteDegree"))
                        {
                            mat.EnableKeyword("_WhiteDegree");
                            mat.SetFloat("_WhiteDegree", 0);
                        }
                    }
                }
            }
        }
        else
        {
            if (!child.GetComponent<LivableObject>().activated)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Material childMat = child.GetComponent<Renderer>().material;
                    if (childMat.HasProperty("_WhiteDegree"))
                    {
                        childMat.EnableKeyword("_WhiteDegree");
                        if (childMat.GetFloat("_WhiteDegree") > 0)
                        {
                            childMat.SetFloat("_WhiteDegree", 0);
                        }
                    }
                    child.GetComponent<LivableObject>().activated = true;
                }

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
                groupColorVal = 0;
                enabled = false;
                LoadActivateAll();
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (id == null)
            Debug.LogError(gameObject.name + " ID is null.");
        if (id == "")
            Debug.LogError(gameObject.name + " ID is empty.");
        if (data.groupMasterDict.ContainsKey(id))
        {
            data.groupMasterDict.Remove(id);
        }
        data.groupMasterDict.Add(id, activateAll);
    }
}
