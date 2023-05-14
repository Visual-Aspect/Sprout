using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sprout;

public class Variable {
    public string Name { get; set; }
    public dynamic Value { get; set; }

    public Variable(string name, dynamic value) {
        Name = name;
        Value = value;
    }

    public override string ToString() {
        return Name + ": " + Value;
    }
}

public class SPObject {
    public List<Variable> Variables { get; set; }

    public SPObject(List<Variable> variables) {
        Variables = variables;
    }

    public SPObject() {
        Variables = new List<Variable>();
    }

    public dynamic? GetVariable(string name) {
        foreach (Variable variable in Variables) {
            if (variable.Name == name) return variable.Value;
        }

        return null;
    }

    public void SetVariable(string name, dynamic value) {
        foreach (Variable variable in Variables) {
            if (variable.Name == name) {
                variable.Value = value;
                return;
            }
        }

        Variables.Add(new Variable(name, value));
    }

    public void RemoveVariable(string name) {
        foreach (Variable variable in Variables) {
            if (variable.Name == name) {
                Variables.Remove(variable);
                return;
            }
        }
    }

    public void ClearVariables() {
        Variables.Clear();
    }

    public override string ToString() {
        string output = "{\n";

        foreach (Variable variable in Variables) {
            output += "    " + variable.Name + ": " + variable.Value + "\n";
        }

        output += "}";

        return output;
    }
}

public class SPArray {
    public List<dynamic> Values { get; set; }

    public SPArray(List<dynamic> values) {
        Values = values;
    }

    public void Add(dynamic value) {
        Values.Add(value);
    }

    public void Remove(dynamic value) {
        Values.Remove(value);
    }

    public void Clear() {
        Values.Clear();
    }
}

public static class SP {
    public static dynamic? FileHandle(string path) {
        if (!File.Exists(path)) return null;

        string data = File.ReadAllText(path);

        dynamic? output = Parse(data);

        if (output is SPObject spObject) {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            
            foreach (Variable variable in spObject.Variables) {
                dictionary.Add(variable.Name, variable.Value);
            }

            return dictionary;
        } else if (output is SPArray) {
            Dictionary<int, object> dictionary = new Dictionary<int, object>();

            for (int i = 0; i < output.Values.Count; i++) {
                dictionary.Add(i, output.Values[i]);
            }

            return dictionary;
        }

        return null;
    }

    public static dynamic? Parse(string data) {
        data = data.Trim();

        if (data.StartsWith("{") && data.EndsWith("}")) {
            return ParseObject(data.Substring(1, data.Length - 2));
        } else if (data.StartsWith("[") && data.EndsWith("]")) {
            return ParseArray(data.Substring(1, data.Length - 2));
        }
        
        return null;
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

            void currentTo(dynamic value) {
               variables.SetVariable(key, value);
               //Console.WriteLine("Setting " + key + " to " + value + " (" + value.GetType() + ")");
            }

            if (double.TryParse(value, out double doubleValue)) {
                currentTo(doubleValue);
            } else if (bool.TryParse(value, out bool boolValue)) {
                currentTo(boolValue);
            } else if (value.StartsWith("\"") && value.EndsWith("\"")) {
                currentTo(value.Substring(1, value.Length - 2));
            } else if (value.StartsWith("{") && value.EndsWith("}")) {
                currentTo(ParseObject(value.Substring(1, value.Length - 2)) ?? new SPObject(new List<Variable>()));
            } else if (value.StartsWith("[") && value.EndsWith("]")) {
                currentTo(ParseArray(value.Substring(1, value.Length - 2)) ?? new SPArray(new List<dynamic>()));
            } else {
                currentTo(value);
            }
        }

        return variables;
    }

    public static dynamic? ParseArray(string data) {
        return null;
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