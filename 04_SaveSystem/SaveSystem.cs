using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    IEnumerable<ISavable> itemsToSave;
    string filePath;

    private void Awake()
    {
        filePath = System.IO.Path.Combine(Application.persistentDataPath, "/savedgame1.json");
    }

    private void Start()
    {
        itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

    }

    public void SaveObjects()
    {
        Dictionary<string, string> dataDictionary = new Dictionary<string, string>();
        foreach (var item in itemsToSave)
        {
            var data = item.GetJsonDataToSave();
            dataDictionary.Add(item.GetType().ToString(), data);
        }

        var jsonString = JsonConvert.SerializeObject(dataDictionary);

        System.IO.File.WriteAllText(filePath, jsonString);
        Debug.Log(filePath);
    }

    public IEnumerator LoadSavedDataCoroutine(Action OnFinishedLoading)
    {
        if (CheckSavedDataExists())
        {
            yield return new WaitForSecondsRealtime(2);
            var jsonSavedData = System.IO.File.ReadAllText(filePath);
            Dictionary<string, string> dataDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSavedData);
            foreach (var item in itemsToSave)
            {
                var key = item.GetType().ToString();
                if (dataDictionary.ContainsKey(key))
                {
                    item.LoadJsonData(dataDictionary[key]);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
            OnFinishedLoading?.Invoke();
        }
    }

    public bool CheckSavedDataExists()
    {
        return System.IO.File.Exists(filePath);
    }
}
