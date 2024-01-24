using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashSoccerScoreBoard : LookingObject
{
    public int score;
    public GarbageScore garBageScore;
    public bool inGameZone;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshPro boardScoreText;
    public Transform otherScores;


    bool firstTransformed;
    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        if (inGameZone)
        {
            scorePanel.SetActive(true);
        }
        else
        {
            scorePanel.SetActive(false);
        }

        if (firstActivated && !firstTransformed)
        {
            foreach (Transform child in otherScores)
            {
                TextMeshPro childText = child.GetComponent<TextMeshPro>();
                ChangeTextColor(childText);
            }
        }
    }
    public void LateUpdate()
    {
        score = garBageScore.score;
        if (inGameZone)
        {
            scoreText.text = "Goals: " + score;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameZone = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inGameZone = false;
        }
    }

    void ChangeTextColor(TextMeshPro text)
    {
        if (text.color.a < 1)
        {
            Color temp = text.color;
            temp.a += 0.1f * Time.deltaTime;
            text.color = temp;
        }
        else
        {
            firstTransformed = true;
        }
    }
}
