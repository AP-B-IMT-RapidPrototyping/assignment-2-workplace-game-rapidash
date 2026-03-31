using System.Collections.Generic;

public class OrderData
{
    public int Id;

    public List<IngredientType> RequiredIngredients = new List<IngredientType>();
    public float SpawnTime;

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