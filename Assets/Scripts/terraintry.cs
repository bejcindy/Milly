using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class terraintry : MonoBehaviour
{
    //public Camera cam;
    public Terrain t;
    public int posX, posZ;
    float[,,] map;
    public int rad;
    public bool startPainting;
    // Start is called before the first frame update
    void Awake()
    {
        //cam = GetComponent<Camera>();
        //Debug.Log(new Vector2(t.terrainData.alphamapWidth, t.terrainData.alphamapHeight));
        ResetAlphaMask();
        map = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!Input.GetMouseButton(0))
        //    return;
        if (!startPainting)
            return;
        RaycastHit hit;
        //if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        //    return;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.transform.GetComponent<Terrain>())
            {
                //if (Input.GetMouseButton(0))
                //{
                //Debug.Log("pressing");
                ConvertPosition(hit.point);
                Vector2 pixelUV = new Vector2(posZ, posX);
                PaintTerrain(pixelUV, rad);
                //Debug.Log(newCor);
                //}
            }
        }
    }

    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;
        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, 0,
        terrainPosition.z / t.terrainData.size.z);
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    void PaintTerrain(Vector2 pos, int radius)
    {
        //Too Performance Consuming
        //float[,,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 2];
        //float[,,] map = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
        //for (int y = 0; y < t.terrainData.alphamapHeight; y++)
        //{
        //    for (int x = 0; x < t.terrainData.alphamapWidth; x++)
        //    {
        //        Vector2 currentPixel = new Vector2(x, y);
        //        if (Vector2.Distance(pos, currentPixel) <= radius)
        //        {
        //            var frac = 1;
        //            map[x, y, 0] = (float)frac;
        //            map[x, y, 1] = (float)(1 - frac);
        //        }
        //    }
        //}

        //More Continuous Version
        for (int y = (int)pos.y-radius; y < (int)pos.y + radius; y++)
        {
            for (int x = (int)pos.x - radius; x < (int)pos.x + radius; x++)
            {
                Vector2 currentPixel = new Vector2(x, y);
                if (Vector2.Distance(pos, currentPixel) <= radius)
                {
                    var frac = 0;
                    //var frac =  Vector2.Distance(pos, currentPixel) / radius;
                    map[x, y, 0] = (float)frac;
                    map[x, y, 1] = (float)(1 - frac);
                }
            }
        }
        t.terrainData.SetAlphamaps(0, 0, map);
    }
    void ResetAlphaMask()
    {
        float[,,] map = t.terrainData.GetAlphamaps(0, 0, t.terrainData.alphamapWidth, t.terrainData.alphamapHeight);
        for (int y = 0; y < t.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < t.terrainData.alphamapWidth; x++)
            {

                    var frac = 1;
                    map[x, y, 0] = (float)frac;
                    map[x, y, 1] = (float)(1 - frac);


            }
        }
        t.terrainData.SetAlphamaps(0, 0, map);
    }
    void TerrainPaint()
    {
        float[,,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 2];

        // For each point on the alphamap...
        for (int y = 0; y < t.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < t.terrainData.alphamapWidth; x++)
            {
                // Get the normalized terrain coordinate that
                // corresponds to the point.
                float normX = x * 1.0f / (t.terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (t.terrainData.alphamapHeight - 1);

                // Get the steepness value at the normalized coordinate.
                var angle = t.terrainData.GetSteepness(normX, normY);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                var frac = angle / 90.0;
                map[x, y, 0] = (float)frac;
                map[x, y, 1] = (float)(1 - frac);
            }
        }
        t.terrainData.SetAlphamaps(0, 0, map);
    }

}
