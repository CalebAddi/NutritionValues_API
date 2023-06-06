using RestSharp;
using System.Collections.Generic;

public class NutritionixClient
{
    private readonly RestClient _client;
    private const string api_url = "https://api.nutritionix.com/v1_1/";
    private const string api_key = "TEMP TEXT FOR NOW"; // temp text until my nutritionix dev account is accepted

    public NutritionixClient()
    {
        _client = new RestClient(api_url);
    }

    public NutritionInfo GetNutritionalInfo(string food_name)
    {
        var request = new RestRequest($"search/{food_name}");
        request.AddParameter("results", "0:1");
        request.AddParameter("fields", "item_name, item_id, brand_name, nf_calories, nf_total_fat");
        request.AddParameter("appKey", api_key);

        var resp = _client.Get<NutritionResponse>(request);

        if (resp.IsSuccessful && resp.Data.Hits.Count > 0)
        {
            var food_item = resp.Data.Hits[0].Fields;
            var item_name = food_item.ItemName;
            var brand_name = food_item.BrandName;
            var calories = food_item.NutritionFacts.Calories;
            var total_fat = food_item.NutritionFacts.TotalFat;

            return new NutritionInfo
            {
                ItemName = item_name,
                BrandName = brand_name,
                Calories = calories,
                TotalFat = total_fat
            };
        }
        else
        {
            return null;
        }
    }
}