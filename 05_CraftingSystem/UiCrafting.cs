using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCrafting : MonoBehaviour
{
    public Action onCraftButtonClicked;

    private Dictionary<int, RecipeItemHelper> recipeUiElementDictionary = new Dictionary<int, RecipeItemHelper>();

    public GameObject craftingPanel, recipesItemPanel, ingredientsMainPanel, ingredientElementsPanel, inventoryFullText;
    public GameObject recipePrefab, ingredientPrefab;
    public Button craftBtn;

    public bool Visible { get => craftingPanel.activeSelf; }

    private void Start()
    {
        ClearUI();
        ingredientsMainPanel.SetActive(false);
        craftingPanel.SetActive(false);
        craftBtn.onClick.AddListener(() => onCraftButtonClicked.Invoke());
    }

    private void ClearUI()
    {
        foreach (Transform child in recipesItemPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowIngredientsUI()
    {
        ingredientsMainPanel.SetActive(true);
    }

    public void ClearIngredients()
    {
        foreach (Transform child in ingredientElementsPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ToggleUI()
    {
        if(craftingPanel.activeSelf == false)
        {
            craftingPanel.SetActive(true);
        }
        else
        {
            ingredientsMainPanel.SetActive(false);
            craftingPanel.SetActive(false);
        }
    }

    public void BlockCraftButton()
    {
        craftBtn.interactable = false;
    }


    public void UnblockCraftButton()
    {
        craftBtn.interactable = true;
    }
}
