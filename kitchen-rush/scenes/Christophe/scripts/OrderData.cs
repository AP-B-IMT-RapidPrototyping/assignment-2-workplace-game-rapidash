using System.Collections.Generic;

public class OrderData
{
    public List<IngredientType> RequiredIngredients = new List<IngredientType>();

    public float SpawnTime; // 🔥 wanneer order gemaakt is

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