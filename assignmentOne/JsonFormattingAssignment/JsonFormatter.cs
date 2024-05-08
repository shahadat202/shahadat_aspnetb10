using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormattingAssignment
{
    public static class JsonFormatter
    {
        public static string Serialize(object obj)
        {
            StringBuilder stringBuilder = new StringBuilder();
            SerializeValue(obj, stringBuilder);
            return stringBuilder.ToString();
        }

        private static void SerializeValue(object obj, StringBuilder stringBuilder)
        {
            if (obj == null)
            {
                stringBuilder.Append("null");
                return;
            }
            Type type = obj.GetType();

            if (PrimitiveData(type))
            {
                SerializePrimitive(obj, stringBuilder);
            }
            else if (type.IsArray || type.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                SerializeArrayOrList(obj, stringBuilder);
            }
            else if (type.IsClass)
            {
                SerializeObjet(obj, stringBuilder);
            }
        }

        private static void SerializePrimitive(object obj, StringBuilder stringBuilder)
        {
            if (obj is string)
            {
                stringBuilder.Append($"\"{InputString((string)obj)}\"");    
            }
            else if (obj is DateTime)
            {
                stringBuilder.Append($"\"{((DateTime)obj).ToString("yyyy-MM-dd")} \"");
            }
            else
            {
                stringBuilder.Append(obj);
            }
        }

        private static void SerializeArrayOrList(object obj, StringBuilder stringBuilder)
        {
            stringBuilder.Append("[");
            IEnumerable enumerable = (IEnumerable)obj;
            bool isFirst = true;
            foreach (var item in enumerable)
            {
                if (!isFirst)
                {
                    stringBuilder.Append(", ");
                }
                SerializeValue(item, stringBuilder);
                isFirst = false;
            }
            stringBuilder.Append("]");
        }

        private static void SerializeObjet(object obj, StringBuilder stringBuilder)
        {
            stringBuilder.Append("{");
            PropertyInfo[] properties = obj.GetType().GetProperties();  
            for (int i = 0; i < properties.Length; i++) 
            {
                stringBuilder.Append($"\"{properties[i].Name}\":");
                SerializeValue(properties[i].GetValue(obj), stringBuilder);  
                
                if (i < properties.Length - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append("}");
        }

        private static bool PrimitiveData(Type type)
        {
            return type.IsPrimitive || type == typeof(int)
                || type == typeof(double) || type == typeof(decimal)
                || type == typeof(char) || type == typeof(string) || type == typeof(DateTime);
        }

        private static string InputString(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in input)
            {
                //if (c == '"' || c == '\\')
                //{
                //    stringBuilder.Append('\\');
                //}
                stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }
}
