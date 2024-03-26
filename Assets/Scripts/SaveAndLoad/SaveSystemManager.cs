using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveSystemManager : MonoBehaviour
{
    [Header("File Storage Data Config")]
    [SerializeField] string fileName;
    public bool newGame;

    GameData gameData;
    List<ISaveSystem> saveSystemObjs;
    FileDataHandler dataHandler;

    public static SaveSystemManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one SaveSystemManager in the scene.");
        }
        instance = this;
        //dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        //saveSystemObjs = FindAllSaveSystemObjs();
        //LoadGame();
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveSystemObjs = FindAllSaveSystemObjs();
#if UNITY_EDITOR
        if (!newGame)
            LoadGame();
        else
            NewGame();
#else
        LoadGame();
#endif              
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.L))
            LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        Debug.Log("New Game");
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        gameData = dataHandler.Load();
        Debug.Log("Load Game");
        //If there is no data to be loaded, initialize a new game
        if (gameData == null)
        {
            Debug.Log("No saves found, initializing a new game.");
            NewGame();
        }
        //Push the loaded data to corresponding scripts
        foreach (ISaveSystem saveSystemObj in saveSystemObjs)
        {
            saveSystemObj.LoadData(gameData);
        }
        //Debug.Log("Loading player pos" + gameData.playerPosition);
    }
    public void SaveGame()
    {
        //Pass the data to other scripts so they can update it
        foreach (ISaveSystem saveSystemObj in saveSystemObjs)
        {
            saveSystemObj.SaveData(ref gameData);
        }

        //Save the data to a file using the data handler
        dataHandler.Save(gameData);
        Debug.Log("SaveGame");
    }

    List<ISaveSystem> FindAllSaveSystemObjs()
    {
        IEnumerable<ISaveSystem> saveSystemObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveSystem>();
        return new List<ISaveSystem>(saveSystemObjs);
    }
}
