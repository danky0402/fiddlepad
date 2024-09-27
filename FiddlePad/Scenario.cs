using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zypr.Demo.Utilities;

namespace Zypr.Demo;

public class Scenario
{

    //go to this page for Scenario documentation 
    //https://zypr.app/api/scenario.html

    public static async Task<dynamic> CreateScenarioWithProgressMonitoringAsync(string apiKey,
                                                                                string pPath,                 //make sure you are passing path of Result.json file
                                                                                string poolResourceModel,
                                                                                string? tag = null)
    {
        
        //queue up a simulation request
        var response = await Simulations.CreateScenario(apiKey,
                                                        poolResourceModel,
                                                        tag);

        string scenarioId = response.Id;  //should return a dynamic with Id property which is the scenario id

        //use a do-while loop to monitor progress
        string url  = $"https://api.zypr.app/v1/simulations/{scenarioId}"; 
        
        //set do while loop initial status
        bool completed = false; 

        //clear Results.json file
            string.Empty.ToPrint(pPath, false);    


        //create a timer 
        var watch = Stopwatch.StartNew();   
        
        dynamic obj;

        do
        {
            //call Zypr every 3 seconds
            await Task.Delay(3000);  

            var json = await Simulations.GetScenarioRecord(apiKey, scenarioId); 

            obj = JsonConvert.DeserializeObject<dynamic>(json)!;

            string statusMsg = obj.StatusDescription;
            int statusNbr = (int)obj.StatusNbr;

            TimeSpan ts = watch.Elapsed;
            var et = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds); 
            var progressMsg = $"{statusMsg} for '{scenarioId.Substring(0, 6)}'... {et}";    
            var jObjMsg = new JObject
            {
                { "Message", progressMsg }
            };

            if(statusNbr <= 6)
            {
                //this will append each progress update message in FiddlePad Results.json
                jObjMsg.ToString().ToPrint(pPath);
            }

            else // statusNbr >= 7     see status states reference at: https://zypr.app/api/scenario.html#message-string
            {
               completed = true;
               
               json = await Simulations.GetScenarioRecord(apiKey, scenarioId); 

               //this will overwrite Results.json in FiddlePad                
               json.FormatJson().ToPrint(pPath, false);

            }


        }
        while (!completed);

        return obj;



    }

    
}
