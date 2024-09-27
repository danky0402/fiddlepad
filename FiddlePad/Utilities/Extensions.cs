using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zypr.Demo.Utilities;

public static class Extensions
{
    public static string ToPrint(this string content, string path, bool append = true)
    {
        Helpers.PrintResults(path, content, append);
        
        return content;     // returning content allows for chaining extension methods (fluent methods)
                            // that receive content as string variable 
    }
    
    public static string ToSave(this string content, string path, string fileName)
    {
        Helpers.SaveResults(path, fileName, content);
        return content;
    }

    public static string ToDatabase(this string content, string connectionString)
    {
        //write a specific helper to save to a database 
        return content;
    }

    public static string FormatJson(this string content)
    {
        var obj = JsonConvert.DeserializeObject<dynamic>(content)!;
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public static string ToPivot(this string json)
    {
        
        if(json.TryParseJson(out JArray result))
        {   
            var jArr = JArray.Parse(json);
            return Helpers.PivotTransform(jArr);
        }
        return string.Empty; 
    }

    public static string ToTSV(this string json)
    {
        
        if(json.TryParseJson(out JObject result))
        {   
            var jObj = JObject.Parse(json);
            return Helpers.FlatFileTransform(jObj);
        }
        return string.Empty; 
    }
    public static string ToCSV(this string json)
    {
        
        if(json.TryParseJson(out JObject result))
        {   
            var jObj = JObject.Parse(json);
            return Helpers.FlatFileTransform(jObj, false);
        }
        return string.Empty; 
    }

    public static bool TryParseJson<T>(this string value, out T result)
    {
        bool success = true;
        var settings = new JsonSerializerSettings
        {
            Error = (sender, args) => { 
                        success = false;
                        args.ErrorContext.Handled = true;
                     },

            MissingMemberHandling = MissingMemberHandling.Error
        };
        result = JsonConvert.DeserializeObject<T>(value, settings)!;
        return success;
    }
}
