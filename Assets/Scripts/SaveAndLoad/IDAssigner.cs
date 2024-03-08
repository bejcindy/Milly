using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;


public class IDAssigner : MonoBehaviour
{
    [Foldout("Object Active Control")]
    public List<GameObject> controlledObjects = new();

    //private void OnEnable()
    //{
    //    GenerateGuid();
    //}
    [Button]
    void AssignLivableObjectID()
    {
        LivableObject[] livables = FindObjectsOfType<LivableObject>();
        foreach (LivableObject livable in livables)
        {
            if (!livable.gameObject.GetComponent<ObjectID>())
            {
                livable.gameObject.AddComponent<ObjectID>();
                livable.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                livable.gameObject.GetComponent<ObjectID>().GenerateGuid();
                //Debug.Log("1");
            }
        }
    }

    [Button]
    void AssignNPCID()
    {
        NPCControl[] characters = FindObjectsOfType<NPCControl>();
        foreach (NPCControl character in characters)
        {
            if (!character.gameObject.GetComponent<ObjectID>()) 
            {
                character.gameObject.AddComponent<ObjectID>();
                character.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                character.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
        }
    }

    [Button]
    void AssignGroupMasterID()
    {
        GroupMaster[] controllers = FindObjectsOfType<GroupMaster>();
        foreach (GroupMaster controller in controllers)
        {
            if (!controller.gameObject.GetComponent<ObjectID>())
            {
                controller.gameObject.AddComponent<ObjectID>();
                controller.gameObject.GetComponent<ObjectID>().GenerateGuid(); 
            }
            else
            {
                controller.gameObject.GetComponent<ObjectID>().GenerateGuid();
                //Debug.Log("1");
            }
        }
    }

    [Button]
    void AssignBuildingGroupControllerID()
    {
        BuildingGroupController[] controllers = FindObjectsOfType<BuildingGroupController>();
        foreach (BuildingGroupController controller in controllers)
        {
            if (!controller.gameObject.GetComponent<ObjectID>())
            {
                controller.gameObject.AddComponent<ObjectID>();
                controller.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                controller.gameObject.GetComponent<ObjectID>().GenerateGuid();
                //Debug.Log("1");
            }
        }
    }

    [Button]
    void AssignFloorTileID()
    {
        FloorTile[] tiles = FindObjectsOfType<FloorTile>();
        foreach (FloorTile tile in tiles)
        {
            if (!tile.gameObject.GetComponent<ObjectID>())
            {
                tile.gameObject.AddComponent<ObjectID>();
                tile.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                tile.gameObject.GetComponent<ObjectID>().GenerateGuid();
                //Debug.Log("1");
            }
        }
    }

    [Button]
    void AssignOnOffID()
    {
        foreach(GameObject obj in controlledObjects)
        {
            if (!obj.GetComponent<ObjectID>())
            {
                obj.gameObject.AddComponent<ObjectID>();
                obj.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                obj.GetComponent<ObjectID>().GenerateGuid();
            }
        }
    }
    [Button]
    void AssignGroundDirtID()
    {
        GroundDirt[] dirts = FindObjectsOfType<GroundDirt>();
        foreach (GroundDirt dirt in dirts)
        {
            if (!dirt.gameObject.GetComponent<ObjectID>())
            {
                dirt.gameObject.AddComponent<ObjectID>();
                dirt.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                dirt.gameObject.GetComponent<ObjectID>().GenerateGuid();
                //Debug.Log("1"); 
            }
        }
    }
    [Button]
    void AssignPassiveActivationID()
    {
        PassiveActivation[] passives = FindObjectsOfType<PassiveActivation>();
        foreach (PassiveActivation passive in passives)
        {
            if (!passive.gameObject.GetComponent<ObjectID>())
            {
                passive.gameObject.AddComponent<ObjectID>();
                passive.gameObject.GetComponent<ObjectID>().GenerateGuid();
            }
            else
            {
                passive.gameObject.GetComponent<ObjectID>().GenerateGuid(); 
                //Debug.Log("1"); 
            }
        }
    }
}
