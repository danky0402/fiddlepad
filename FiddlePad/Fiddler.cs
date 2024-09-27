using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Zypr.Demo.Utilities;

namespace Zypr.Demo;

public class Fiddler
{
    private string _apikey = string.Empty;
    private string _pPath =  string.Empty;      //print path
    private string _sPath = string.Empty;       //save path
    
    //  use this is append to a file name to make it unique down to the minute - to seconds ("yyyy-MM-dd-HH-mm-ss")
    //  cast a DateTime variable type to a variable string with a specific format
    //  read more here: https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

    private string _timestamp = $"-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}"; 
    
    public Fiddler(string apiKey,
                   string printPath,
                   string savePath)
    {
        _apikey = apiKey;
        _pPath = Path.Combine(printPath, "Results.json");
        _sPath = savePath;
    }


    public async Task Execute()
    {
       
    //  CHECK STATUS of Zypr.app site - no api key is required
        
            
            string msg = await HealthCheck.Execute();
            msg.ToPrint(_pPath, false);                              //print to default Results.json
            msg.ToSave(_sPath, $"HeathCheck{_timestamp}.json");      //save file to default Content folder 

            //alternatively, this can be chained
            
            (await HealthCheck.Execute())
                              .ToPrint(_pPath, false)
                              .ToSave(_sPath, $"HeathCheck{_timestamp}.json");  
                
            Debugger.Break();
         
    //  REGISTER AS TRIAL USER

        // var msg = await User.CreateTrialUser("first name",
        //                                      "your email address",
        //                                      "organization name");

        // msg.ToPrint(_pPath, false);
        // An email is sent to your email address with an activation code which is required for the next step

    //  CONFIRM REGISTERED USER

            // var msg = await User.ConfirmTrialUserRegistration("email addressdaniel994@mediacombb.net",
            //                                                   "activation code");
            
            // msg.ToPrint(_pPath).ToSave(); 
            
            // Note: This contains your new api key.  It is not mailed to you.
            //       Add your api key to the ZyprApiKey property in the settings


    //All the scripts that follow require an api key to execute 

    //  GET MY USER RECORD 
        
            var userRecord = await User.GetUserRecordAsync(_apikey);

            var userJson = JsonConvert.SerializeObject(userRecord);     //returning c# UserRecord
            userJson.ToString()!.ToPrint(_pPath, false);
            
            

    //  GET LIST of STARTER MODELS 

             var lst = await Starters.GetListOfAllStarterModels(_apikey);
             string json = JsonConvert.SerializeObject(lst, Formatting.Indented);
             json.ToPrint(_pPath).ToSave(_sPath, $"StarterModels{_timestamp}.json");

             

    //  GET A STARTER MODEL
      
            dynamic starterModel = await Starters.GetStartModel(_apikey,
                                                               "P01",
                                                               "1.0");

            //  Note: response is a c# dynamic type  
            
            string jsonPoolModel  = JsonConvert.SerializeObject(starterModel, Formatting.Indented);
            jsonPoolModel.ToPrint(_pPath, false);

            

    //  RUN A SIMULATION  (a simulation produces a Scenario object)


            //  This is an example of a custom script that is saved in a different file in the FiddlePad project.
            //  A Scenario class is created with the static method below that combines multiple steps:  
            //      a) request a simulation be executed
            //      b) monitor the request process by polling the simulation's status
            //      c) print the status to Results.json
            //      d) check status for completion  
            //      e) at completion, get finalized scenario record
            //      f) print finalized scenario to Results.json 
            //      g) return finalized scenario object as dynamic type

            dynamic scenario = await Scenario.CreateScenarioWithProgressMonitoringAsync(_apikey,
                                                                                        _pPath,
                                                                                        jsonPoolModel);
            
            //  with a tag that can be applied to any simulation to group simulations
            
            // dynamic scenario = await Scenario.CreateScenarioWithProgressMonitoringAsync(_apikey,
            //                                                                             _pPath,
            //                                                                             jsonPoolModel,
            //                                                                             "CollectionName");

            //set  Scenario's id as c# property
            
            string scenarioId = scenario.Id;
            
            //when serializing a dynamic type object, make sure the it is serialized into a string type  
            
            json = JsonConvert.SerializeObject(scenario, Formatting.Indented);
            
            string filename = $"scenario-p01{_timestamp}.json";
            json.ToPrint(_pPath, false ).ToSave(_sPath, filename);


    // DELETE SCENARIO 

            await Simulations.DeleteScenarioByIdAsync(_apikey,
                                                      scenarioId);
            
            // //check for record
            json = await Simulations.GetScenarioRecord(_apikey,
                                                       scenarioId);

            json.ToPrint(_pPath, false);

            
           
    // DELETE SCENARIOS
            
            //await Simulations.DeleteScenarioByTagAsync(_apikey,
            //                                           "CollectionName");


    // TRANSFORM SCENARIO RESULT into CALENDARIZED FLAT FILE to import or copy into Excel 

            //read saved scenario file
            var fullName = Path.Combine(_sPath, filename);

            var prmJson = Helpers.ReadJsonFile(fullName);
            
            if(prmJson != string.Empty)
            {
                JObject jObj = JObject.Parse(prmJson);
                
                var hardwareInv = jObj["CalendarizedForecast"]!["HardwareInventory"]!.ToString();

                string pivot = hardwareInv.ToPivot();
                
                string tsv = pivot.ToTSV();

                tsv.ToPrint(_pPath, false);
                
                tsv.ToSave(_sPath, $"HardwareForecast{_timestamp}.txt");

                Debugger.Break();  //hovering over "tsv" variable shows the result.
                                   //select all, copy and then paste into excel

            //alternatively, chain the methods                   
            
                // hardwareInv.ToPivot()
                //            .ToTSV()
                //            .ToPrint(_pPath, false)
                //            .ToSave(_sPath, $"HardwareForecast{_timestamp}.txt");

            }
    }

}

