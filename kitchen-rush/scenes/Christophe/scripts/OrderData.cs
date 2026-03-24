using System.Collections.Generic;

public class OrderData
{
    public List<IngredientType> RequiredIngredients = new List<IngredientType>();

    public string GetDisplayText()
    {
        string text = "";

        foreach (var ing in RequiredIngredients)
        {
            text += ing.ToString() + "\n";
        }

        return text;
    }
}