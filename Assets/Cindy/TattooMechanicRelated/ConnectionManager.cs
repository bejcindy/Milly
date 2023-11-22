using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    public float avoidRadius;
    List<RectTransform> tattoos;
    List<List<RectTransform>> connections;
    List<RectTransform> connectionList;
    public List<Vector2> positions;
    public LineRendererManager LRM;
    bool[] conditionStorage;

    private void Awake()
    {
        tattoos = new List<RectTransform>();
        connections = new List<List<RectTransform>>();
        foreach (RectTransform child in transform)
        {
            if (child.GetComponent<TattooConnection>())
            {
                tattoos.Add(child);
                connections.Add(child.GetComponent<TattooConnection>().relatedTattoos);
            }
        }

        connectionList = new List<RectTransform>();
        positions = new List<Vector2>();
        //LRM.pts = new List<Vector2>(connectionList.Count);
        for (int i = 0; i < tattoos.Count; i++)
        {
            if (connections[i].Count != 0)
            {
                for (int j = 0; j < connections[i].Count; j++)
                {
                    if (i == 0)
                    {
                        connectionList.Add(tattoos[i]);
                        connectionList.Add(connections[i][j]);
                        //tattoos side of avoidance
                        Vector2 direction = (connections[i][j].anchoredPosition - tattoos[i].anchoredPosition).normalized;
                        //LRM.pts.Add(tattoos[i].anchoredPosition + direction * avoidRadius);
                        //LRM.pts.Add(connections[i][j].anchoredPosition - direction * avoidRadius);
                        positions.Add(tattoos[i].anchoredPosition);
                        positions.Add(connections[i][j].anchoredPosition);
                    }
                    else
                    {
                        if (connectionList.Contains(tattoos[i]))
                        {
                            int previous = connectionList.IndexOf(tattoos[i]) - 1;
                            int next = previous + 2;
                            if (connectionList[previous] != connections[i][j] && connectionList[next] != connections[i][j])
                            {
                                connectionList.Add(tattoos[i]);
                                connectionList.Add(connections[i][j]);
                                Vector2 direction = (connections[i][j].anchoredPosition - tattoos[i].anchoredPosition).normalized;
                                //LRM.pts.Add(tattoos[i].anchoredPosition + direction * avoidRadius);
                                //LRM.pts.Add(connections[i][j].anchoredPosition - direction * avoidRadius);
                                positions.Add(tattoos[i].anchoredPosition);
                                positions.Add(connections[i][j].anchoredPosition);
                            }
                        }
                    }
                }
            }
        }
        conditionStorage = new bool[positions.Count];
    }

    List<Vector2> referenceList;
    List<Vector2> greyRefList;

    public void ActivateLines(TattooConnection tattooConnection)
    {
        Vector2 currentPos = tattooConnection.GetComponent<RectTransform>().anchoredPosition;
        referenceList = new List<Vector2>();
        greyRefList = new List<Vector2>();
        for(int i = 0; i < connectionList.Count; i++)
        {
            if (connectionList[i].GetComponent<TattooConnection>().activated)
                conditionStorage[i] = true;
        }
        for (int i = 0; i < conditionStorage.Length; i += 2)
        {
            //if both true, solid black line
            if (conditionStorage[i] && conditionStorage[i + 1])
            {
                Vector2 direction = (positions[i + 1] - positions[i]).normalized;
                referenceList.Add(positions[i] + direction * avoidRadius);
                referenceList.Add(positions[i + 1] - direction * avoidRadius);
            }
            //if one is true, grey line
            else if ((conditionStorage[i] && !conditionStorage[i + 1]) || (!conditionStorage[i] && conditionStorage[i + 1]))
            {
                Vector2 direction = (positions[i + 1] - positions[i]).normalized;
                greyRefList.Add(positions[i] + direction * avoidRadius);
                greyRefList.Add(positions[i + 1] - direction * avoidRadius);
            }

        }
        LRM.pts = referenceList;
        LRM.greyPts = greyRefList;
    }


}
