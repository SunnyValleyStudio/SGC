using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    string GetJsonDataToSave();
    void LoadJsonData(string jsonData);
}
