

using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zypr.Demo.Utilities
{
    public class Helpers
    {
        /// <summary>
        /// Looks to settings.json in project root directory for ZyprApiKey property
        /// </summary>
        /// <returns></returns>
        public static string GetApiKey()            
        {
           
            string projectRoot = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
            string fileName = Path.Combine(projectRoot, "settings.json");

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);    
                if(json != string.Empty)
                {
                    JObject jObj = JObject.Parse(json);  //assume root is not a Json Array     
                    
                    if(jObj.ContainsKey("ZyprApiKey"))
                    {
                        return jObj["ZyprApiKey"]!.ToString();
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Sets Value property based on Key.  Assumes settings.json file is in project root directory.  
        /// Uses setting.json file to override default ToPrint and ToSave directory location. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SetValue(string key)       //assumes settings.json file is in project root directory    
        {
            string projectRoot = Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName;
            string settingsFile = Path.Combine(projectRoot, "settings.json");

            if (File.Exists(settingsFile))
            {
                string json = File.ReadAllText(settingsFile);    
                if(json != string.Empty)
                {
                    JObject jObj = JObject.Parse(json);     //assume root is not a Json Array     
                    
                    //
                    if(jObj.ContainsKey(key) && jObj[key]!.ToString() != string.Empty)  //KeyValue pair - check key exists and, if so, does value exist?
                    {
                        return jObj[key]!.ToString();
                    }
                    else
                    {
                        
                        if(key == "PrintDirectory")
                        {
                           return projectRoot;     

                        }     
                        
                        if(key == "SaveDirectory")
                        {
                            return Path.Combine(projectRoot, "Content");
                        }

                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Invoked by ToPrint extension method
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="content"></param>
        /// <param name="append"></param>
        public static void PrintResults(string fullName, string content, bool append = true)
        {
            //always prints to file name: Results.json
            string _content = File.Exists(fullName) ? content   
                                                    : "Results.json file doesn't exist";
            
            // if(!append)
            // {
            //     //truncate the file to eliminate existing content
            //     File.WriteAllText(fullName, string.Empty);
            // }
            
            if(_content == string.Empty)
            {
                using StreamWriter sw0 = new StreamWriter(fullName,  append);
                sw0.WriteLine(string.Empty);
                return;    
            }

            //test if content can be parsed into a Jobject or JArray
            if( _content.TryParseJson(out JObject result0) || _content.TryParseJson(out JArray result1))
            {   
                //content is json, so format it so it's easier to read
                _content = _content.FormatJson();
            }

            using StreamWriter sw1 = new StreamWriter(fullName,  append);
            sw1.Write(_content);
            sw1.WriteLine(string.Empty);
           
        }

        /// <summary>
        /// Invoked by ToSave extension method 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
            public static void SaveResults(string path, string fileName, string content)
        {
            var fullName = Path.Combine(path,fileName);

            File.WriteAllText(fullName, content);
            
        }
        
    
        public static string PivotTransform(JArray jArr)
        {
            //This converts row-oriented json data into column-oriented output

            //this is the new object that will hold the json pivot
            JObject jObj = new JObject();
            

            //build first column property names (keys) and
            //an array to hold all values
            //include first entry into value array
            
            foreach(JProperty prop in jArr[0].Cast<JProperty>())  //first JObject only
            {
                //adding initial property names to new JObject and values  
                jObj.Add(new JProperty(prop.Name, new JArray(prop.Value)));
            }
            

            //populate columns 2 through n
            foreach(JObject obj in jArr.Children().Skip(1).Cast<JObject>())
            {
                foreach(JProperty prop in obj.Children().Cast<JProperty>())
                {
                    //the exclamation sign "!" is indicating to the compiler the prop.Value is expected NOT TO be null (otherwise it will throw an error) 
                    (jObj[prop.Name] as JArray)!.Add(prop.Value);
                }
            }

            return jObj.ToString();
        }

        public static string FlatFileTransform(JObject jObj, bool isTsv = true)
        {
            string separator = (isTsv) ? "\t"
                                       : ",";

            StringBuilder sb = new StringBuilder();

            foreach(JProperty prop in jObj.Children().Cast<JProperty>())
            {
                var row = prop.Name + separator + string.Join(separator, (prop.Value as JArray)!); 
                sb.AppendLine(row);
            }

            return sb.ToString();

        }


        public static string ReadJsonFile(string fullName)
        {
             if(!File.Exists(fullName))
             {
                return string.Empty;
             }

             return File.ReadAllText(fullName);
        }
        
    }

    
}   





        // public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath)
        // {
        //     var directory = new DirectoryInfo( currentPath ?? Directory.GetCurrentDirectory());
        //     while (directory != null && !directory.GetFiles("*.sln").Any())
        //     {
        //         directory = directory.Parent;
        //     }
        //     return directory!;
        // }


        // public static async Task<string> ReadPoolResourceModel(string fullName)
        // {
        //     if (File.Exists(fullName))
        //     {
        //         return await File.ReadAllTextAsync(fullName);
        //     }

        //     return string.Empty;

        // }
        // public static async Task<bool> WritePoolResourceModel(string path, string fileName, string poolModel)
        // {
        //     try
        //     {
        //         string fullName = Path.Combine(path, fileName);

        //         //create a new file or overwrite
        //         await File.WriteAllTextAsync(fullName, poolModel.FormatJson());
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         var err = ex.Message;    
        //         return false;
        //     }
        // }

       
        


     
