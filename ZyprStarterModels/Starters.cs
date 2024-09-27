using Newtonsoft.Json;
using Flurl.Http;


namespace Zypr.Demo
{
    public class Starters
    {

        public static async Task<List<PoolModelSummary>> GetListOfAllStarterModels(string apiKey)
        {
            string url = "https://api.zypr.app/v1/pool-models/search";


            var response = await url.WithHeader("Content-Type", "application/json")
                                    .WithHeader("x-api-key", apiKey)
                                    .AllowAnyHttpStatus()
                                    .GetAsync();
                        
            var msg = await response.ResponseMessage.Content.ReadAsStringAsync();

            //  This is using the POCO class PoolModelSummary to convert json response
            //  into a c# object

            var poolModels = JsonConvert.DeserializeObject<List<PoolModelSummary>>(msg);

            return poolModels!;    

        } 

        public class PoolModelSummary
        {
            public string Code {get; set;} = string.Empty;

            public string Version  {get; set;} = string.Empty;

            public string Description {get; set;} = string.Empty;

            public PoolModelSummary() {}        //empty constructor required for json converting

        }


        public static async Task<dynamic> GetStartModel(string apiKey,
                                                        string code,
                                                        string version)
        {
            string url = $"https://api.zypr.app/v1/pool-models/search?code={code}&version={version}";

            var response = await url.WithHeader("Content-Type", "application/json")
                                    .WithHeader("x-api-key", apiKey)
                                    .AllowAnyHttpStatus()
                                    .GetAsync();
            
            var msg = await response.ResponseMessage.Content.ReadAsStringAsync();

            //  Pool Resource Model (PRM) is very large, so I skipped defining a POCO (plain old class object) and used dynamic 
            //  Returning a dynamic type means the return object is c# object, but because it is dynamic "intellisense" 
            //  will not be able to show available c# properties like POCO PRM class would describe
            
            //  Alternatively, the returned PRM could be left as json like this: "return msg";
            //  And like dynamic type, you will need to refer to Zypr documentation to manipulate the PRM object 
            //  https://zypr.app/api/pool-resource-model.html

            var poolModel = JsonConvert.DeserializeObject<dynamic>(msg);
            return poolModel!;
        }

        

        

    }
}
