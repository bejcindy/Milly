using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.VisualScripting;

public class GarbageScore : MonoBehaviour
{
    public bool inGameZone;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    [SerializeField]
    int score;
    public Material dumpsterMat;
    float matColorVal = 1f;
    float fadeInterval = 10f;
    bool firstActivated,firstTrashIn;
    // Start is called before the first frame update
    void Start()
    {
        dumpsterMat.SetFloat("_WhiteDegree", matColorVal);
    }

    // Update is called once per frame
    void Update()
    {
        if (score > 0)
            firstTrashIn = true;
        if (!firstActivated && firstTrashIn)
            TurnOnColor(dumpsterMat);

        if (inGameZone)
        {
            scorePanel.SetActive(true);
        }
        else
        {
            scorePanel.SetActive(false);
        }

    }

    public void LateUpdate()
    {
        if (inGameZone)
        {
            scoreText.text = "Score: " + score;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickUpObject>())
        {
            score++;
        }
        if (other.CompareTag("Player"))
        {
            inGameZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PickUpObject>())
        {
            score--;
        }
        if (other.CompareTag("Player"))
        {
            inGameZone = false;
        }
    }
    void TurnOnColor(Material material)
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
            firstActivated = true;
        }
    }


}
