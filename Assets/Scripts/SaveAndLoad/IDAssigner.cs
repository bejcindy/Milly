using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;


public class IDAssigner : MonoBehaviour
{

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
}
