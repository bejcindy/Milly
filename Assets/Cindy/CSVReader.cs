using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    int columns = 3;
    public TextAsset csvFile;

    [System.Serializable]
    public class csvContent
    {
        public string Object;
        public string Index;
        public string TextContent;
    }
    [System.Serializable]
    public class ObjectList
    {
        public csvContent[] content;
    }

    public ObjectList myOL = new ObjectList();

    private void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = csvFile.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        //string[] data = csvFile.text.Split(",", System.StringSplitOptions.None);
        foreach (string s in data)
        {
            Debug.Log(s);
        }
        int tableSize = data.Length / columns - 1;
        myOL.content = new csvContent[tableSize];
        for(int i = 0; i < tableSize; i++)
        {
            myOL.content[i] = new csvContent();
            myOL.content[i].Object = data[columns * (i + 1)];
            myOL.content[i].Index = data[columns * (i + 1) + 1];
            myOL.content[i].TextContent = data[columns * (i + 1) + 2];
        }
    }
}
