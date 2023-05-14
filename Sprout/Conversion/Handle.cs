using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sprout;

public static class SP {
    public static dynamic FileHandle(string path) {
        if (!File.Exists(path)) throw new FileNotFoundException($"File {path} does not exist");

        string data = File.ReadAllText(path);
        dynamic output = Parse(data);

        return output;
    }

    public static dynamic Parse(string data) {
        data = data.Trim();

        if (data.StartsWith("{") && data.EndsWith("}"))
            return ParseObject(data.Substring(1, data.Length - 2));
        
        if (data.StartsWith("[") && data.EndsWith("]"))
            return ParseArray(data.Substring(1, data.Length - 2));
        
        return new SPObject();
    }

    public static dynamic ParseValue(string value) {
        if (double.TryParse(value, out double doubleValue)) return doubleValue;
        if (bool.TryParse(value, out bool boolValue)) return boolValue;
        if (value.StartsWith("\"") && value.EndsWith("\"")) return value.Substring(1, value.Length - 2);
        if (value.StartsWith("{") && value.EndsWith("}")) return ParseObject(value.Substring(1, value.Length - 2));
        if (value.StartsWith("[") && value.EndsWith("]")) return ParseArray(value.Substring(1, value.Length - 2));
        if (value == "null") return null;
        
        return value;
    }

    public static SPObject ParseObject(string data) {
        data = data.Trim();

        string[] tokens = SplitString(data, ';');
        SPObject variables = new SPObject();
        
        for (int i = 0; i < tokens.Length; i++) {
            // Handle:
            // <key> = <value (which is "^(.+?)\s*=\s*([\s\S]+)$")>
            Regex keyValueRegex = new Regex(@"^(?<key>[^=]+)=\s*(?<value>[\s\S]+)$", RegexOptions.Multiline);
            Match keyValueMatch = keyValueRegex.Match(tokens[i]);
            
            string key = keyValueMatch.Groups["key"].Value.Trim();
            string value = keyValueMatch.Groups["value"].Value.Trim();

            variables[key] = ParseValue(value);
        }

        return variables;
    }

    public static SPArray ParseArray(string data) {
        data = data.Trim();

        string[] tokens = SplitString(data, ',');
        SPArray variables = new SPArray();
        
        for (int i = 0; i < tokens.Length; i++) {
            variables[i] = ParseValue(tokens[i].Trim());
        }

        return variables;
    }

    private static readonly string arrayPlaceholder = "(SP_ARRAY[{0}])";
    private static readonly string objectPlaceholder = "(SP_OBJECT[{0}])";

    public static string[] SplitString(string data, char c) {
        int arrayIndex = 0;
        int objectIndex = 0;

        Regex arrayObjectRegex = new Regex(@"\[(?:[^\[\]]*|(?<open>\[)|(?<-open>\]))*(?(open)(?!))\]|\{(?:[^{}]*|(?<open>\{)|(?<-open>\}))*(?(open)(?!))\}", RegexOptions.Multiline);

        List<string> arrayMatches = new List<string>();
        List<string> objectMatches = new List<string>();

        data = arrayObjectRegex.Replace(data, match => {
            string matchValue = match.Value;
            if (matchValue.StartsWith("[")) {
                arrayMatches.Add(matchValue);
                return string.Format(arrayPlaceholder, arrayIndex++);
            } else {
                objectMatches.Add(matchValue);
                return string.Format(objectPlaceholder, objectIndex++);
            }
        });

        string[] tokens = data.Split(c);
        List<string> outTokens = new List<string>();

        for (int i = 0; i < tokens.Length; i++) {
            string token = tokens[i];
            token = token.Trim();
            
            for (int j = 0; j < arrayMatches.Count; j++) {
                string placeholder = string.Format(arrayPlaceholder, j);
                token = token.Replace(placeholder, arrayMatches[j]);
            }

            for (int j = 0; j < objectMatches.Count; j++) {
                string placeholder = string.Format(objectPlaceholder, j);
                token = token.Replace(placeholder, objectMatches[j]);
            }

            outTokens.Add(token);
        }

        return outTokens.ToArray();
    }
}