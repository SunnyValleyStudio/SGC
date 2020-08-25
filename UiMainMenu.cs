using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainMenu : MonoBehaviour
{
    public Button newGameBtn, resumBtn;
    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        newGameBtn.onClick.AddListener(manager.StartNextScene);
        resumBtn.onClick.AddListener(manager.LoadSavedGame);
        resumBtn.interactable = false;
        if (manager.CheckSavedGameExists())
        {
            resumBtn.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
