using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace JsonDeserialization
{
    public static class JsonFormatter
    {
        public static T Deserialize<T>(string jsonString)
        {
            var jsonObject = ParseJsonObject(jsonString);
            return (T)DeserializeObject(typeof(T), jsonObject);
        }

        private static Dictionary<string, object> ParseJsonObject(string jsonString)
        {
            jsonString = jsonString.Trim();

            if (jsonString.StartsWith("{") && jsonString.EndsWith("}"))
            {
                jsonString = jsonString.Substring(1, jsonString.Length - 2).Trim();    
            }
            else
            {
                throw new FormatException("Invalid JSON format. JSON object must and end with curly braces.");
            }
            var jsonObject = new Dictionary<string, object>();

            var keyVlauePairs = jsonString.Split(',')
                                    .Select(part => part.Split(':', 2))
                                    .Select(pair => new
                                    {
                                        Key = pair[0].Trim().Trim('"'),
                                        Value = pair[1].Trim()
                                    });
            
            foreach (var pair in keyVlauePairs)
            {
                jsonObject[pair.Key] = ParseValue(pair.Value);
            }
            return jsonObject;
        }

        private static object ParseValue(string value)
        {
            value = value.Trim();

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                return value.Substring(1, value.Length - 2);
            }
            else if (value == "null")
            {
                return null;
            }
            else if (value == "true")
            {
                return true;
            }
            else if (value == "false")
            {
                return false;
            }
            else if (value.Contains('.') || value.Contains('e') || value.Contains('E'))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (value.StartsWith("[") && value.EndsWith("]"))
            {
                var arrayElements = value.Substring(1, value.Length - 2)
                                        .Split(',')
                                        .Select(element => ParseValue(element.Trim()))
                                        .ToArray();

                return arrayElements;
            }
            else if (value.StartsWith("{") && value.EndsWith("}"))
            {
                return ParseJsonObject(value);
            }
            else
            {
                throw new FormatException($"Unsupported JSON value: {value}");
            }
        }

        private static object DeserializeObject(Type targetType, Dictionary<string, object> jsonObject)
        {
            var obj = Activator.CreateInstance(targetType);

            foreach (var property in targetType.GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = jsonObject.ContainsKey(propertyName) ? jsonObject[propertyName] : null;

                if (propertyValue != null)
                {
                    var propertyType = property.PropertyType;

                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var elementType = propertyType.GetGenericArguments()[0];    
                        var listType = typeof(List<>).MakeGenericType(elementType);
                        var list = (IList)Activator.CreateInstance(listType);
                    
                        foreach (var item in (object[])propertyValue)
                        {
                            list.Add(DeserializeObject(elementType, (Dictionary<string, object>)item));  
                        }

                        property.SetValue(obj, list);
                    }
                    else if (propertyType.IsClass)
                    {
                        property.SetValue(obj, DeserializeObject(propertyType, (Dictionary<string, object>)propertyValue));
                    }
                    else
                    {
                        property.SetValue(obj, Convert.ChangeType(propertyValue, targetType));    
                    }
                }
            }
            return obj;
        }
    }
}
