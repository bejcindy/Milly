using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("DisplayText")]
public class DisplayText : MonoBehaviour
{

    public float delay = .05f;
    public float disappearDelay = 2f;
    public float delayBetweenTexts = 2f;
   
    //public bool show;
    string currentText = "";

    public string txt;

    //For Testing
    //string[] dialogueTest = { "A:aaaaa", "A:ababababa", "B:bbbbb" };
    //int[] dialogueTestInt = { 2, 1 };
    //public GameObject textBox2;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>().text;
        GetComponent<TextMeshProUGUI>().text = currentText;
    }

    // Update is called once per frame
    void Update()
    {
        //For Testing
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(ShowDialogue(dialogueTestInt, dialogueTest, textBox2, true));
        //}
    }

    public IEnumerator ShowOneText(string textToShow)
    {
        txt = textToShow;
        GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < txt.Length + 1; i++)
        {
            currentText = txt.Substring(0, i);
            GetComponent<TextMeshProUGUI>().text = currentText;
            Debug.Log("typing");
            yield return new WaitForSeconds(delay);
        }
        
        yield return new WaitForSeconds(disappearDelay);
        GetComponent<TextMeshProUGUI>().text = "";
    }

    public IEnumerator ShowText(int parts, string[] textToShow)
    {
        
        for (int j = 0; j < parts; j++)
        {
            txt = textToShow[j];
            GetComponent<TextMeshProUGUI>().text = "";
            for (int i = 0; i < txt.Length + 1; i++)
            {
                currentText = txt.Substring(0, i);
                GetComponent<TextMeshProUGUI>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delayBetweenTexts);
        }
        yield return new WaitForSeconds(disappearDelay);
        GetComponent<TextMeshProUGUI>().text = "";
    }

    public IEnumerator ShowDialogue(int[] separateParts, string[] dialogue, GameObject targetTextBox, bool MCSpeakFirst)
    {
        TextMeshProUGUI NPCBox = targetTextBox.GetComponent<TextMeshProUGUI>();
        NPCBox.text = "";
        bool MCSpeaking = MCSpeakFirst;
        TextMeshProUGUI activeBox;
        int dialogueCounter=0;
        for(int i = 0; i < separateParts.Length; i++)
        {
            if (MCSpeaking)
                activeBox = GetComponent<TextMeshProUGUI>();
            else
                activeBox = NPCBox;
            for(int j = 0; j < separateParts[i]; j++)
            {
                txt = dialogue[dialogueCounter];
                
                activeBox.text = "";
                for (int k = 0; k < txt.Length + 1; k++)
                {
                    currentText = txt.Substring(0, k);
                    activeBox.text = currentText;
                    yield return new WaitForSeconds(delay);
                }
                dialogueCounter++;
                yield return new WaitForSeconds(delayBetweenTexts);
            }
            MCSpeaking = !MCSpeaking;
            yield return new WaitForSeconds(delayBetweenTexts);
        }
        yield return new WaitForSeconds(disappearDelay);
        GetComponent<TextMeshProUGUI>().text = "";
        NPCBox.text = "";
    }
}
