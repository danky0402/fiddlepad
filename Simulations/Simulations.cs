
using Newtonsoft.Json;
using Flurl.Http;

namespace Zypr.Demo;

public class Simulations
{

    public static async Task<dynamic> CreateScenario(string apiKey,
                                                     string poolResourceModel,
                                                     string? tag = null)
    {
        
        string url = (tag == null) ?  "https://api.zypr.app/v1/simulations"
                                   : $"https://api.zypr.app/v1/simulations?tag={tag}";


        var response = await url.AllowAnyHttpStatus()                              //using FLURL library
                                .WithHeader("x-api-key", apiKey)
                                .WithHeader("Content-Type","application/json")
                                .PostStringAsync(poolResourceModel);

        var content = await response.ResponseMessage.Content.ReadAsStringAsync();

        //using dynamic object to create pool resource model c# object on-the-fly 
        //(i.e., without creating a complete POCO Scenario class)
        //you can view the scope of the Scenario class graph size by using 
        //the graph output button on the Results.json file when it contains a completed scenario  
        
        var obj = JsonConvert.DeserializeObject<dynamic>(content);                      
        
        return obj!;

    }


    public static async Task<string> GetScenarioRecord(string apiKey,
                                                        string scenarioId)
    {
        string url = $"https://api.zypr.app/v1/simulations/{scenarioId}";

        var response = await url.WithHeader("Content-Type", "application/json")                                 
                                .WithHeader("x-api-key", apiKey)
                                .AllowAnyHttpStatus()
                                .GetAsync();

        //you might need to check the response.StatusCode 
       
        var content = await response.ResponseMessage.Content.ReadAsStringAsync();

        return content;

    }

    public static async Task<string> DeleteScenarioByIdAsync(string apiKey,
                                                    string scenarioId)
    {

        string url = $"https://api.zypr.app/v1/simulations?id={scenarioId}";

        var response = await url.WithHeader("Content-Type", "application/json")           
                                .WithHeader("x-api-key", apiKey)
                                .AllowAnyHttpStatus()
                                .DeleteAsync();
                            
        var msg = await response.ResponseMessage.Content.ReadAsStringAsync();

        return msg;

    }
    public static async Task<string> DeleteScenarioByTagAsync(string apiKey,
                                                              string tag)
    {

        string url = $"https://api.zypr.app/v1/simulations?tag={tag}";

        var response = await url.WithHeader("Content-Type", "application/json")           
                                .WithHeader("x-api-key", apiKey)
                                .AllowAnyHttpStatus()
                                .DeleteAsync();
                            
        var msg = await response.ResponseMessage.Content.ReadAsStringAsync();

        return msg;

    }

}
