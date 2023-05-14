using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sprout;

public class SPObject {
    public Dictionary<string, dynamic> Variables = new Dictionary<string, dynamic>();
    
    public dynamic this[string key] {
        get {
            if (Variables.ContainsKey(key)) return Variables[key];
            else return null;
        } set {
            Variables[key] = value;
        }
    }

    public override string ToString() {
        return ToString(0);
    }

    public string ToString(int indent) {
        string indentString = new string(' ', indent * 4);
        string output = "{\n";

        foreach (KeyValuePair<string, dynamic> variable in Variables) {
            string key = variable.Key;
            dynamic value = variable.Value;

            if (value is SPObject) {
                output += $"{indentString}    {key} = {((SPObject)value).ToString(indent + 1)};\n";
            } else if (value is SPArray) {
                output += $"{indentString}    {key} = {((SPArray)value).ToString(indent + 1)};\n";
            } else {
                output += $"{indentString}    {key} = {value};\n";
            }
        }

        output += $"{indentString}}}";

        return output;
    }
}
