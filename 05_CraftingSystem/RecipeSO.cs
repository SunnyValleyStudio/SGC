using SVS.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewRecipeData", menuName ="Crafting/Recipe")]
public class RecipeSO : ScriptableObject, IInventoryItem
{
    public string recipeName;
    public ItemSO outcome;
    [Range(1,100)]
    public int outcomeQuantity = 1;
    public List<RecipeIngredients> ingredientsRequired;

    public string ID => outcome.ID;

    public int Count => outcomeQuantity;

    public bool IsStackable => outcome.isStackable;

    public int StackLimit => outcome.stackLimit;

    public Dictionary<string, int> GetIngredientsIdValueDict()
    {
        Dictionary<string, int> ingredientsDict = new Dictionary<string, int>();
        foreach (var item in ingredientsRequired)
        {
            ingredientsDict.Add(item.ingredient.ID, item.count);
        }
        return ingredientsDict;
    }

    public Sprite GEtOutcomeSprite()
    {
        return outcome.imageSprite;
    }
}

[Serializable]
public struct RecipeIngredients
{
    public ItemSO ingredient;
    public int count;
}
