using Flurl.Http;
using Newtonsoft.Json;

namespace Zypr.Demo;

public class User()
{

    public static async Task<string>  CreateTrialUser(string firstName,
                                                      string emailAddress,
                                                      string organizationName)
    {

        var trialUserRecord = new TrialUserRecord(firstName,
                                                  emailAddress,
                                                 organizationName);

        var json = JsonConvert.SerializeObject(trialUserRecord);                                         

        
        
        string url = $"https://api.zypr.app/v1/users/registration";

        var response = await url.AllowAnyHttpStatus()
                                .WithHeader("Content-Type","application/json")
                                .PostStringAsync(json); 
        
        
        var msg = await response.ResponseMessage.Content.ReadAsStringAsync();
        
        return msg;                
    }

    public static async Task<string>  ConfirmTrialUserRegistration(string emailAddress,
                                                                   string activationCode)
    {

        
        string url = $"https://api.zypr.app/v1/users/{activationCode}/registration?email={emailAddress}";

        var response = await url.AllowAnyHttpStatus()
                                .WithHeader("Content-Type","application/json")
                                .PostAsync(); 

        //the response is an unformatted json object
        var json = await response.ResponseMessage.Content.ReadAsStringAsync();
        
        if(json != null)
        {
            //this simply formats json  
            dynamic obj = JsonConvert.DeserializeObject(json)!;  //parse into a c# object
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        return json!;


    }


    //Note:  The user's api key is not returned because the calling this endpoint
    //       requires the user's api key (i.e., the user knows their api key)
    //       If the api key is forgotten or compromised, you will need to  
    //       request a new api key, which follows the same request/confirmation 
    //       process as creating a new user
    public static async Task<UserRecord>  GetUserRecordAsync(string apiKey)
    {
        
        string url = $"https://api.zypr.app/v1/users/info";
    


        var response = await url.WithHeader("Content-Type", "application/json")    //using FLURL library
                                .WithHeader("x-api-key", apiKey)
                                .AllowAnyHttpStatus()
                                .GetAsync();

        var json = await response.ResponseMessage.Content.ReadAsStringAsync();

        var userRecord = JsonConvert.DeserializeObject<UserRecord>(json);

        return userRecord!;
    }

    public class TrialUserRecord
    {
        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; }   = string.Empty;
        
        public string Organization { get; set; } = string.Empty;

        public TrialUserRecord(){}

        public TrialUserRecord(string firstName,
                               string emailAddress,
                               string organizationName)
        {
            FirstName = firstName;
            Email = emailAddress;
            Organization = organizationName;

        }

    }


    public class UserRecord
    {
        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; }   = string.Empty;
            
        public string Role { get; set; } = string.Empty;   

        public string Status { get; set; } = string.Empty;
       
        public string Organization { get; set; } = string.Empty;

        public string ApiExpirationDate { get; set; } = string.Empty;

        public int DaysRemaining { get; set; } 

        public DateTime Created {get; set;}

        public DateTime Timestamp { get; set;}  //timestamp of when this record was retrieved

        public UserRecord(){}   //empty constructor required for JSON deserialization
        
    }

}
