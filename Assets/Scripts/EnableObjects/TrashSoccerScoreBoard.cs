using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashSoccerScoreBoard : LookingObject
{
    public bool gameActivated;
    public int score;
    public GarbageScore garBageScore;
    public bool inGameZone;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshPro boardScoreText;
    public Transform otherScores;


    bool firstTransformed;
    bool showedMillyScore;
    bool showedHint;
    public static bool startedSoccerGame;

    protected override void Start()
    {
        base.Start();
        startedSoccerGame = false;
    }

    protected override void Update()
    {
        base.Update();
        if (inGameZone)
        {
            scorePanel.SetActive(true);
            startedSoccerGame = true;
            //if (!showedHint && !MindPalace.hideHint)
            //{
            //    DataHolder.ShowHint(DataHolder.hints.soccerHint);
            //    if (Input.GetKeyDown(KeyCode.Q))
            //    {
            //        DataHolder.HideHint(DataHolder.hints.soccerHint);
            //        showedHint = true;
            //    }
            //}

        }
        else
        {
            scorePanel.SetActive(false);
            //DataHolder.HideHint(DataHolder.hints.soccerHint);
        }

        if (focusingThis)
        {
            scoreText.gameObject.layer = 13;
            foreach (Transform t in otherScores)
            {
                t.gameObject.layer = 13;
            }
        }
        else
        {
            if (activated)
            {
                if (scoreText.gameObject.layer != 17)
                {
                    scoreText.gameObject.layer = 17;
                    foreach (Transform t in otherScores)
                    {
                        t.gameObject.layer = 17;
                    }
                }

            }
        }

        if (firstActivated && !firstTransformed)
        {
            foreach (Transform child in otherScores)
            {
                TextMeshPro childText = child.GetComponent<TextMeshPro>();
                ChangeTextColor(childText);
            }
        }
        //milly score trigger mechanic
        if (!showedMillyScore && garBageScore.score > 0)
        {
            ChangeTextColor(boardScoreText);
            if (boardScoreText.color.a == 1)
                showedMillyScore = true;
        }

    }
    public void LateUpdate()
    {
        score = garBageScore.score;
        if (inGameZone)
        {
            scoreText.text = "Goals: " + score;
            boardScoreText.text = "Milly: " + new string('X', score);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(gameActivated)
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

    public void ActivateSoccerGame()
    {
        gameActivated = true;
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
            text.gameObject.layer = 17;
            firstTransformed = true;
        }
    }
}
