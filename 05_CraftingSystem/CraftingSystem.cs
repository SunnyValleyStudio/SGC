using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public Action<RecipeSO> onCraftItemRequest;
    public Func<string, int, bool> onCheckResourceAvailability;
    public Func<bool> onCheckInventoryFull;

    public List<RecipeSO> craftingRecipes;
    List<int> recipeUiIdList = new List<int>();

    public UiCrafting uiCrafting;

    int currentRecipeUiId = -1;

    private void Start()
    {
        uiCrafting = GetComponent<UiCrafting>();
        uiCrafting.onRecipeClicked += RecipeClickedHandler;
        uiCrafting.onCraftButtonClicked += CraftRecipeHandler;
        uiCrafting.BlockCraftButton();
    }

    public void ToggleCraftingUI(bool saveLastViewedRecipe = false)
    {
        uiCrafting.ToggleUI();
        if (saveLastViewedRecipe == false)
        {
            currentRecipeUiId = -1;
        }
        if(currentRecipeUiId!= -1)
        {
            RecheckIngredients();
        }
        if (uiCrafting.Visible)
        {
            recipeUiIdList = uiCrafting.PrepareRecipeItems(craftingRecipes);
        }
        
    }

    public void RecheckIngredients()
    {
        if(currentRecipeUiId != -1)
        {
            RecipeClickedHandler(currentRecipeUiId);
        }
    }

    private void CraftRecipeHandler()
    {
        var recipeIndex = recipeUiIdList.IndexOf(currentRecipeUiId);
        var recipe = craftingRecipes[recipeIndex];
        onCraftItemRequest.Invoke(recipe);
    }

    private void RecipeClickedHandler(int id)
    {
        currentRecipeUiId = id;
        uiCrafting.ClearIngredients();
        var recipeIndex = recipeUiIdList.IndexOf(currentRecipeUiId);
        var recipe = craftingRecipes[recipeIndex];
        var ingredientsIdCountDict = recipe.GetIngredientsIdValueDict();

        bool blockCraftButton = false;
        foreach (var key in ingredientsIdCountDict.Keys)
        {
            bool enoughItemFlag = onCheckResourceAvailability.Invoke(key, ingredientsIdCountDict[key]);
            if(blockCraftButton == false)
            {
                blockCraftButton = !enoughItemFlag;
            }
            uiCrafting.AddIngredient(ItemDataManager.instance.GetItemName(key), ItemDataManager.instance.GetItemSprite(key), ingredientsIdCountDict[key], enoughItemFlag); 
        }

        uiCrafting.ShowIngredientsUI();
        if (blockCraftButton)
        {
            uiCrafting.BlockCraftButton();
        }
        else
        {
            uiCrafting.UnblockCraftButton();
        }
        if (onCheckInventoryFull.Invoke())
        {
            uiCrafting.ShowInventoryFull();
        }
    }
}
