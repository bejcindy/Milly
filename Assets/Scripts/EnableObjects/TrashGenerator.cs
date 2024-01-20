using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    public bool generate;
    public GameObject[] trashPrefabs;
    public Transform[] trashLocations;
    public Transform selectedLocation;
    public int trashCount;
    public int stage;
    bool locationChosen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            if (!locationChosen)
            {
                locationChosen = true;
                SelectLocation();
            }

            GenerateTrash();
        }
    }

    void SelectLocation()
    {
        selectedLocation = trashLocations[stage];
    }

    void GenerateTrash()
    {
        selectedLocation.GetComponent<BoxCollider>().enabled = true;
        trashCount = Random.Range(2, 5);
        for(int i = 0; i < trashCount; i++)
        {
            int index = Random.Range(0, trashPrefabs.Length);
            float x = Random.Range(0.5f, 1f);
            float y = Random.Range(0.5f, 1f);
            float z = Random.Range(0.5f, 1f);
            Vector3 spawnPos = new Vector3(x, y, z);
            Instantiate(trashPrefabs[index], selectedLocation.position+spawnPos, Random.rotation, selectedLocation);
        }
        generate = false;
        locationChosen = false;
        selectedLocation.GetComponent<BoxCollider>().enabled = false;
        if (stage < trashLocations.Length - 1)
            stage++;
        else
            stage = 0;
    }
}
