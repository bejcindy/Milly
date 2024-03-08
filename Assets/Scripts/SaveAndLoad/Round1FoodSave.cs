using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round1FoodSave : MonoBehaviour,ISaveSystem
{
    public void LoadData(GameData data)
    {
        gameObject.SetActive(data.round1FoodActive);
    }
    public void SaveData(ref GameData data)
    {
        data.round1FoodActive = gameObject.activeSelf;
    }
}
