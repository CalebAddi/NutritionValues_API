using System;
using RestSharp;
using System.Collections.Generic;

public class NutritionClient
{
    private readonly RestClient _client;
    private const string api_url = "https://api.nutritionix.com/v1_1/";
    private const string api_key = "T5296e92bfb4c4e4188499014eac885f9"; 

    public NutritionClient()
    {
        _client = new RestClient(api_url);
    }

    public NutritionInfo GetNutritionalInfo(string food_name)
    {
        var request = new RestRequest($"search/{food_name}");
        request.AddParameter("results", "0:1");
        request.AddParameter("fields", "item_name, item_id, brand_name, nf_calories, nf_total_fat, nf_cholesterol, nf_sodium, nf_total_carbohydrate, nf_protein, nf_dietary_fiber");
        request.AddParameter("appKey", api_key);

        var resp = _client.Execute<NutritionResponse>(request);

        if (resp.IsSuccessful)
        {
            var nutritionResponse = resp.Data;
            if (nutritionResponse != null && nutritionResponse.Hits.Count > 0)
            {
                var hit = nutritionResponse.Hits[0];
                var fields = hit.Fields;
                var item_name = fields.item_name;
                var brand_name = fields.brand_name;
                var calories = fields.nf_calories;
                var total_fat = fields.nf_total_fat;
                var cholesterol = fields.nf_cholesterol;
                var sodium = fields.nf_sodium;
                var total_carbohydrate = fields.nf_total_carbohydrate;
                var protein = fields.nf_protein;
                var fiber = fields.nf_dietary_fiber;

                return new NutritionInfo
                {
                    ItemName = item_name,
                    BrandName = brand_name,
                    Calories = calories,
                    TotalFat = total_fat,
                    Cholesterol = cholesterol,
                    Sodium = sodium,
                    TotalCarbohydrate = total_carbohydrate,
                    Protein = protein,
                    Fiber = fiber
                };
            }
            else
            {
                return null;
            }
        }
        else
        {
            // handle request error
            return null;
        }
    }
}

public class NutritionInfo
{
    public string ItemName { get; set; }
    public string BrandName { get; set; }
    public int Calories { get; set; }
    public float TotalFat { get; set; }
    public int Cholesterol { get; set; }
    public int Sodium { get; set; }
    public float TotalCarbohydrate { get; set; }
    public float Protein { get; set; }
    public float Fiber { get; set; }
}


public class Fields
{
    public string item_name { get; set; }
    public string item_id { get; set; }
    public string brand_name { get; set; }
    public int nf_calories { get; set; }
    public float nf_total_fat { get; set; }
    public int nf_cholesterol { get; set; }
    public int nf_sodium { get; set; }
    public float nf_total_carbohydrate { get; set; }
    public float nf_protein { get; set; }
    public float nf_dietary_fiber { get; set; }
}

public class Hit
{
    public Fields Fields { get; set; }
}

public class NutritionResponse
{
    public List<Hit> Hits { get; set; }
}

// public class Nutrition
// {
//     public int Calories { get; set; }
//     public float TotalFat { get; set; }
//     public int Cholesterol { get; set; }
//     public int Sodium { get; set; }
//     public float TotalCarbohydrate { get; set; }
//     public float Protein { get; set; }
//     public float Fiber { get; set; }
// }