using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrashSoccerScoreBoard : LookingObject
{
    public bool gameActivated;
    public int score;
    public int rank;
    public GarbageScore garBageScore;
    public bool inGameZone;
    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rankText;
    public TextMeshPro boardScoreText;
    public Transform otherScores;
    public int[] scoreList = { 10, 9, 6, 5, 5, 5, 3, 3, 2, 1, 1, 1, 1 };


    bool firstTransformed;
    bool firstTriggerGame;
    bool showedMillyScore;
    bool showedHint;
    bool soccerGameTatTriggered;
    public static bool startedSoccerGame;

    protected override void Start()
    {
        base.Start();
        startedSoccerGame = false;
    }

    protected override void Update()
    {
        base.Update();

        if(rank == 1)
        {
            if (!soccerGameTatTriggered)
            {
                soccerGameTatTriggered = true;
                DialogueManager.StartConversation("SoccerGame/GameComplete");
            }
        }

        if (firstActivated)
        {
            gameActivated = true;
            if (!firstTriggerGame)
            {
                firstTriggerGame = true;
                gameActivated = true;
                inGameZone = true;
            }

        }

        if (inGameZone && !MindPalace.tatMenuOn)
        {
            scorePanel.SetActive(true);
            startedSoccerGame = true;
            CalculateRank();

        }
        else
        {
            scorePanel.SetActive(false);
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
            scoreText.text = score.ToString();
            rankText.text = GetRankText();
            boardScoreText.text = "Milly: " + new string('X', score);
        }

    }


    string GetRankText()
    {
        switch (rank)
        {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return rank.ToString() + "th";
        }
    }



    public void CalculateRank()
    {
        for (int i = 0; i < scoreList.Length; i++)
        {
            if (score > scoreList[i])
            {
                rank = i + 1;
                return;
            }
            else
            {
                continue;
            }
        }
        rank = scoreList.Length + 1;
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

