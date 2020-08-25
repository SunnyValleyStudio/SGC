using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCrafting : MonoBehaviour
{
    public Action onCraftButtonClicked;
    public Action<int> onRecipeClicked;

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

    private void OnRecipeClicked(int id)
    {
        if (recipeUiElementDictionary.ContainsKey(id))
        {
            onRecipeClicked.Invoke(id);
        }
    }

    public void ShowInventoryFull()
    {
        var element = Instantiate(inventoryFullText, Vector3.zero, Quaternion.identity, ingredientElementsPanel.transform);
    }

    public void AddIngredient(string ingredientName, Sprite ingredientSprite, int ingredientCOunt, bool enoughItems)
    {
        var element = Instantiate(ingredientPrefab, Vector3.zero, Quaternion.identity, ingredientElementsPanel.transform);
        var ingredientHelper = element.GetComponent<IngredientItemElement>();
        ingredientHelper.SetItemUI(ingredientName, ingredientSprite, ingredientCOunt, enoughItems);
    }

    public List<int> PrepareRecipeItems(List<RecipeSO> listOfRecipes)
    {
        ClearUI();
        recipeUiElementDictionary.Clear();
        List<int> recipeUiIdList = new List<int>();
        foreach (var item in listOfRecipes)
        {
            var element = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity, recipesItemPanel.transform);
            var recipeHelper = element.GetComponent<RecipeItemHelper>();
            recipeHelper.OnClickEvent += OnRecipeClicked;
            recipeUiElementDictionary.Add(element.GetInstanceID(), recipeHelper);
            recipeHelper.SetItemUI(item.recipeName, item.GEtOutcomeSprite());

            recipeUiIdList.Add(element.GetInstanceID());
        }
        return recipeUiIdList;
    }
}
