using Flurl.Http;
using Newtonsoft.Json;

namespace Zypr.Demo
{
    public class HealthCheck
    {
        public static async Task<string> Execute()
        {
           
            //An api key is not required to call health check.
            //Health check is provided by Zypr and used by the hosting service 
            //that hosts Zypr to continuously check Zypr's availability every few seconds.
            //Ravello is notified on failure.


            string url = $"https://api.zypr.app/healthcheck";

    
            var response = await url.AllowAnyHttpStatus()                          //using FLURL library
                                    .WithHeader("Content-Type","application/json")
                                    .GetAsync();                         

            
            
            var msg = new HealthCheckResponse(response.StatusCode,
                                             (response.StatusCode == 200)          //if 200
                                                ? "Zypr is available"              //then this is the ok response message 
                                                : "Not the response expected");    //this is the not ok response message

            
           return JsonConvert.SerializeObject(msg);
            
        }

        public class HealthCheckResponse
        {
            public int StatusCode {get; set;}
            
            public string Message {get; set; } = string.Empty;

            public HealthCheckResponse(int statusCode,
                                    string message)
            {
                StatusCode = statusCode;
                Message = message;
            }
        }
    }
}