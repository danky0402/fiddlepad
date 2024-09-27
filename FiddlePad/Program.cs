//this is MS document page for dotnet CLI   https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet

///alternative entry methods    // static void Main() { }
// static int Main() { }
// static void Main(string[] args) { }
// static int Main(string[] args) { }
// static async Task Main() { }
// static async Task<int> Main() { }
// static async Task Main(string[] args) { }
// static async Task<int> Main(string[] args) { }


using Zypr.Demo.Utilities;

namespace Zypr.Demo {

    public class Program()
    {
        
        static async Task Main() 
        {
            //This is the entry  point of the FiddlerPad program and where 
            //the properties in the settings.json file are used to initialize
            //key variables (e.g., your api key)

            //The setings.json file is always assumed to be located in the
            //root directory FiddlePad project.

            var apiKey = Helpers.GetApiKey();     
            
            //These two value in settings.json can remain blank and will default to 
            //  1) the project root direct to "print" into the "Results.json" file
            //  2) the Content folder to "save" output 

            //A value is only required if you want to change where you save output from Zypr (e.g., onto a file server
            //that is backed up)

            var print = Helpers.SetValue("PrintDirectory");          
            var save = Helpers.SetValue("SaveDirectory");


            //Initialize a fiddler object
            
            //This takes you into the fiddler object where you can write/modify/arrange code to use the Zypr's API endpoints. 
            //An API endpoint is a specific URL
            //Here is an example of Zypr's API for "Users", but two different endpoints 
            // 1) This is to create your registration 
            //      https://api.zypr.app/v1/users/registration 

            // 2) This is to confirm your registation after receiving by email your activation code
            //      https://api.zypr.app/v1/users/{activationCode}/registration?{querystring}  


                var fiddler = new Fiddler(apiKey,
                                          print,
                                          save);        

                await fiddler.Execute();
        }
        
    }
}