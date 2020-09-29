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
        filePath = Application.persistentDataPath + "/savedgame1.json";
    }

    private void Start()
    {
        itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();

    }

    public void SaveObjects()
    {
        List<string> classNames = new List<string>();
        List<string> classData = new List<string>();
        foreach (var item in itemsToSave)
        {
            var data = item.GetJsonDataToSave();
            classNames.Add(item.GetType().ToString());
            classData.Add(data);
        }
        var dataToSave = new SavedData
        {
            classNames = classNames,
            classData = classData
        };

        var jsonString = JsonUtility.ToJson(dataToSave);

        System.IO.File.WriteAllText(filePath, jsonString);
        Debug.Log(filePath);
    }

    public IEnumerator LoadSavedDataCoroutine(Action OnFinishedLoading)
    {
        if (CheckSavedDataExists())
        {
            yield return new WaitForSecondsRealtime(2);
            var jsonSavedData = System.IO.File.ReadAllText(filePath);
            SavedData savedData = JsonUtility.FromJson<SavedData>(jsonSavedData);

            foreach (var item in itemsToSave)
            {
                var className = item.GetType().ToString();
                if (savedData.classNames.Contains(className))
                {
                    item.LoadJsonData(savedData.classData[savedData.classNames.IndexOf(className)]);
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

    [Serializable]
    struct SavedData
    {
        public List<string> classNames, classData;
    }
}
