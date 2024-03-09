using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour,ISaveSystem
{
    Material mat;
    Renderer rend;

    public bool activated;
    public bool firstActivated;

    float matColorVal;
    float fadeInterval;

    public List<GroundDirt> myDirts;
    public List<Transform> myTiles;
    public List<Transform> additionalTiles;
    public bool overrideStartSequence;
    bool layerChanged;
    string id;

    private void Awake()
    {
        if (GetComponent<ObjectID>())
            id = GetComponent<ObjectID>().id;
        else
            Debug.LogError(gameObject.name + " doesn't have ObjectID Component.");

        rend = GetComponent<Renderer>();
        if (rend)
        {
            mat = rend.material;
        }
    }

    void Start()
    {
        Hugo.totalFloorCount++;
        activated = false;
        

        matColorVal = 1;
        fadeInterval = 10;
        GetComponentsInChildren<GroundDirt>(myDirts);
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !firstActivated)
            TurnOnColor(mat);

        if (AllDirtCleaned())
        {
            activated = true;
        }        
    }

    bool AllDirtCleaned()
    {
        foreach(GroundDirt dirt in myDirts)
        {
            if (!dirt.cleaned)
            {
                return false;
            }
        }
        return true;
    }

    private void TurnOnColor(Material material)
    {
        if (!layerChanged)
        {
            gameObject.layer = 18;
            matColorVal = 1;
            if(rend)
                material.SetFloat("_WhiteDegree", matColorVal);
            foreach (Transform tile in myTiles)
            {
                tile.gameObject.layer = 18;
                Renderer tileRend = tile.GetComponent<Renderer>();
                if (tileRend)
                {
                    tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                }
            }

            foreach(Transform tile in additionalTiles)
            {
                foreach(Transform childTile in tile)
                {
                    childTile.gameObject.layer = 18;
                    Renderer tileRend = childTile.GetComponent<Renderer>();
                    if (tileRend)
                    {
                        tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                    }
                }
            }
            layerChanged = true;
        }

        if (matColorVal > 0)
        {
            matColorVal -= 0.1f * fadeInterval * Time.deltaTime;
            if (rend)
            {
                if (material.HasFloat("_WhiteDegree"))
                    material.SetFloat("_WhiteDegree", matColorVal);
            }


            foreach(Transform tile in myTiles)
            {
                Renderer tileRend = tile.GetComponent<Renderer>();
                if (tileRend)
                {
                    tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                }
            }

            foreach (Transform tile in additionalTiles)
            {
                foreach (Transform childTile in tile)
                {
                    Renderer tileRend = childTile.GetComponent<Renderer>();
                    if (tileRend)
                    {
                        tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                    }
                }
            }

        }
        else
        {
            matColorVal = 0;
            firstActivated = true;
            Hugo.totalFloorCleaned++;

        }
       
    }

    public void LoadData(GameData data)
    {
        if (data.floorTileDict.TryGetValue(id, out bool savedActivated))
        {
            activated = savedActivated;
            if (activated)
            {
                matColorVal = 0;
                gameObject.layer = 18;
                if (rend)
                    if (mat.HasFloat("_WhiteDegree"))
                        mat.SetFloat("_WhiteDegree", matColorVal);
                
                foreach (Transform tile in myTiles)
                {
                    tile.gameObject.layer = 18;
                    Renderer tileRend = tile.GetComponent<Renderer>();
                    if (tileRend)
                    {
                        tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                    }
                }

                foreach (Transform tile in additionalTiles)
                {
                    foreach (Transform childTile in tile)
                    {
                        childTile.gameObject.layer = 18;
                        Renderer tileRend = childTile.GetComponent<Renderer>();
                        if (tileRend)
                        {
                            tileRend.material.SetFloat("_WhiteDegree", matColorVal);
                        }
                    }
                }
                firstActivated = true;
            }
        }
    }
    public void SaveData(ref GameData data)
    {
        if (id == null)
            Debug.LogError(gameObject.name + " ID is null.");
        if (id == "")
            Debug.LogError(gameObject.name + " ID is empty.");
        if (data.floorTileDict.ContainsKey(id))
            data.floorTileDict.Remove(id);
        data.floorTileDict.Add(id, activated);
    }
}
